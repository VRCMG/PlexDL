﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PlexDL.Player
{
    /// <summary>
    /// A class that is used to group together the Media methods and properties of the PlexDL.Player.Player class.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [CLSCompliant(true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class Media : HideObjectMembers
    {
        #region Fields (Media Class)

        private Player _base;

        // Media album art information
        private Image _tagImage;

        private DirectoryInfo _directoryInfo;
        private string[] _searchKeyWords = { "*front*", "*cover*" }; // , "*albumart*large*" };
        private string[] _searchExtensions = { ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".tiff" };

        // Media chapter information
        private const string ROOT_ATOM_TYPES = "ftyp,moov,mdat,pdin,moof,mfra,stts,stsc,stsz,meta,free,skip";

        private byte[] MOOV_ATOM = { (byte)'m', (byte)'o', (byte)'o', (byte)'v' };
        private byte[] TRAK_ATOM = { (byte)'t', (byte)'r', (byte)'a', (byte)'k' };
        private byte[] TREF_ATOM = { (byte)'t', (byte)'r', (byte)'e', (byte)'f' };
        private byte[] CHAP_ATOM = { (byte)'c', (byte)'h', (byte)'a', (byte)'p' };
        private byte[] TKHD_ATOM = { (byte)'t', (byte)'k', (byte)'h', (byte)'d' };
        private byte[] MDIA_ATOM = { (byte)'m', (byte)'d', (byte)'i', (byte)'a' };
        private byte[] MINF_ATOM = { (byte)'m', (byte)'i', (byte)'n', (byte)'f' };
        private byte[] STBL_ATOM = { (byte)'s', (byte)'t', (byte)'b', (byte)'l' };
        private byte[] STTS_ATOM = { (byte)'s', (byte)'t', (byte)'t', (byte)'s' };
        private byte[] STCO_ATOM = { (byte)'s', (byte)'t', (byte)'c', (byte)'o' };
        private byte[] UDTA_ATOM = { (byte)'u', (byte)'d', (byte)'t', (byte)'a' };
        private byte[] CHPL_ATOM = { (byte)'c', (byte)'h', (byte)'p', (byte)'l' };

        private FileStream _reader;
        private long _fileLength;
        private long _atomEnd;
        private long _moovStart;
        private long _moovEnd;

        #endregion Fields (Media Class)

        internal Media(Player player)
        {
            _base = player;
        }

        /// <summary>
        ///  Gets a value indicating the source type of the playing media, such as a file or webcam.
        /// </summary>
        public MediaSourceType SourceType
        {
            get
            {
                _base._lastError = Player.NO_ERROR;
                MediaSourceType source = MediaSourceType.None;

                if (_base._playing) source = _base.AV_GetSourceType();
                return source;
            }
        }

        /// <summary>
        /// Gets the natural length (duration) of the playing media. See also: Player.Media.Duration and Player.Media.GetDuration.
        /// </summary>
        public TimeSpan Length
        {
            get
            {
                _base._lastError = Player.NO_ERROR;
                if (!_base._playing || !_base._fileMode) return TimeSpan.Zero;
                return TimeSpan.FromTicks(_base._mediaLength);
            }
        }

        /// <summary>
        /// Gets the duration of the playing media from the (adjustable) start time to the stop time. See also: Player.Media.Length and Player.Media.GetDuration.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                _base._lastError = Player.NO_ERROR;
                if (!_base._playing || !_base._fileMode) return TimeSpan.Zero;
                return _base._stopTime == 0 ? TimeSpan.FromTicks(_base._mediaLength - _base._startTime) : TimeSpan.FromTicks(_base._stopTime - _base._startTime);
            }
        }

        /// <summary>
        /// Returns the duration of the specified part of the playing media. See also: Player.Media.Length and Player.Media.Duration.
        /// </summary>
        /// <param name="part">Specifies the part of the playing media whose duration is to be obtained.</param>
        public TimeSpan GetDuration(MediaPart part)
        {
            _base._lastError = Player.NO_ERROR;

            if (!_base._playing || !_base._fileMode) return TimeSpan.Zero;

            switch (part)
            {
                case MediaPart.BeginToEnd:
                    return TimeSpan.FromTicks(_base._mediaLength);
                //break;

                case MediaPart.StartToStop:
                    return _base._stopTime == 0 ? TimeSpan.FromTicks(_base._mediaLength - _base._startTime) : TimeSpan.FromTicks(_base._stopTime - _base._startTime);
                //break;

                case MediaPart.FromStart:
                    return TimeSpan.FromTicks(_base.PositionX - _base._startTime);
                //break;

                case MediaPart.ToEnd:
                    return TimeSpan.FromTicks(_base._mediaLength - _base.PositionX);
                //break;

                case MediaPart.ToStop:
                    return _base._stopTime == 0 ? TimeSpan.FromTicks(_base._mediaLength - _base.PositionX) : TimeSpan.FromTicks(_base._stopTime - _base.PositionX);
                //break;

                //case MediaLength.FromBegin:
                default:
                    return (TimeSpan.FromTicks(_base.PositionX));
                //break;
            }
        }

        /// <summary>
        /// Returns the specified part of the file name or device name of the playing media.
        /// </summary>
        /// <param name="part">Specifies the part of the name to return.</param>
        public string GetName(MediaName part)
        {
            string mediaName = string.Empty;
            _base._lastError = Player.NO_ERROR;

            if (!_base._fileMode && !_base._liveStreamMode)
            {
                if (part == MediaName.FileName || part == MediaName.FileNameWithoutExtension) mediaName = _base._fileName;
            }
            else if (_base._playing)
            {
                try
                {
                    switch (part)
                    {
                        case MediaName.FileName:
                            mediaName = Path.GetFileName(_base._fileName);
                            break;

                        case MediaName.DirectoryName:
                            mediaName = Path.GetDirectoryName(_base._fileName);
                            break;

                        case MediaName.PathRoot:
                            mediaName = Path.GetPathRoot(_base._fileName);
                            break;

                        case MediaName.Extension:
                            mediaName = Path.GetExtension(_base._fileName);
                            break;

                        case MediaName.FileNameWithoutExtension:
                            mediaName = Path.GetFileNameWithoutExtension(_base._fileName);
                            break;

                        default: // case MediaName.FullPath:
                            mediaName = _base._fileName;
                            break;
                    }
                }
                catch (Exception e) { _base._lastError = (HResult)Marshal.GetHRForException(e); }
            }
            return mediaName;
        }

        /// <summary>
        /// Gets the number of audio tracks in the playing media. See also: Player.Media.GetAudioTracks.
        /// </summary>
        public int AudioTrackCount
        {
            get
            {
                _base._lastError = Player.NO_ERROR;
                return _base._audioTrackCount;
            }
        }

        /// <summary>
        /// Returns a list of the audio tracks in the playing media. Returns null if no audio tracks are present. See also: Player.Media.AudioTrackCount.
        /// </summary>
        public AudioTrack[] GetAudioTracks()
        {
            _base._lastError = Player.NO_ERROR;

            AudioTrack[] tracks = null;
            int count = _base._audioTrackCount;
            if (count > 0)
            {
                tracks = new AudioTrack[count];
                for (int i = 0; i < count; i++)
                {
                    AudioTrack track = new AudioTrack();
                    track._mediaType = _base._audioTracks[i].MediaType;
                    track._name = _base._audioTracks[i].Name;
                    track._language = _base._audioTracks[i].Language;
                    track._channelCount = _base._audioTracks[i].ChannelCount;
                    track._samplerate = _base._audioTracks[i].Samplerate;
                    track._bitdepth = _base._audioTracks[i].Bitdepth;
                    track._bitrate = _base._audioTracks[i].Bitrate;
                    tracks[i] = track;
                }
            }
            return tracks;
        }

        /// <summary>
        /// Gets the number of video tracks in the playing media. See also: Player.Media.GetVideoTracks.
        /// </summary>
        public int VideoTrackCount
        {
            get
            {
                _base._lastError = Player.NO_ERROR;
                return _base._videoTrackCount;
            }
        }

        /// <summary>
        /// Returns a list of the video tracks in the playing media. Returns null if no video tracks are present. See also: Player.Media.VideoTrackCount.
        /// </summary>
        public VideoTrack[] GetVideoTracks()
        {
            _base._lastError = Player.NO_ERROR;

            VideoTrack[] tracks = null;
            int count = _base._videoTrackCount;
            if (count > 0)
            {
                tracks = new VideoTrack[count];
                for (int i = 0; i < count; i++)
                {
                    VideoTrack track = new VideoTrack();
                    track._mediaType = _base._videoTracks[i].MediaType;
                    track._name = _base._videoTracks[i].Name;
                    track._language = _base._videoTracks[i].Language;
                    track._frameRate = _base._videoTracks[i].FrameRate;
                    track._width = _base._videoTracks[i].SourceWidth;
                    track._height = _base._videoTracks[i].SourceHeight;
                    tracks[i] = track;
                }
            }
            return tracks;
        }

        /// <summary>
        /// Gets the original size of the video image of the playing media.
        /// </summary>
        public Size VideoSourceSize
        {
            get
            {
                _base._lastError = Player.NO_ERROR;
                return _base._hasVideo ? _base._videoSourceSize : Size.Empty;
            }
        }

        /// <summary>
        /// Gets the video frame rate of the playing media, in frames per second.
        /// </summary>
        public float VideoFrameRate
        {
            get
            {
                _base._lastError = Player.NO_ERROR;
                return _base._hasVideo ? _base._videoFrameRate : 0;
            }
        }

        /// <summary>
        /// Gets or sets the (repeat) start time of the playing media. The start time can also be set with the Player.Play method. See also: Player.Media.StopTime.
        /// </summary>
        public TimeSpan StartTime
        {
            get
            {
                _base._lastError = Player.NO_ERROR;
                return TimeSpan.FromTicks(_base._startTime);
            }
            set
            {
                if (!_base._playing || !_base._fileMode)
                {
                    _base._lastError = HResult.MF_E_NOT_AVAILABLE;
                    return;
                }

                _base._lastError = Player.NO_ERROR;
                long newStart = value.Ticks;

                if (_base._startTime == newStart) return;
                if ((_base._stopTime != 0 && newStart >= _base._stopTime) || newStart >= _base._mediaLength)
                {
                    _base._lastError = HResult.MF_E_OUT_OF_RANGE;
                    return;
                }

                _base._startTime = newStart;
                if (newStart > _base.PositionX) _base.PositionX = newStart;
                if (_base._mediaStartStopTimeChanged != null) _base._mediaStartStopTimeChanged(_base, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the (repeat) stop time of the playing media. The stop time can also be set with the Player.Play method.
        /// TimeSpan.Zero or 00:00:00 indicates the natural end of the media. See also: Player.Media.StartTime.
        /// </summary>
        public TimeSpan StopTime
        {
            get
            {
                _base._lastError = Player.NO_ERROR;
                return TimeSpan.FromTicks(_base._stopTime);
            }
            set
            {
                if (!_base._playing || !_base._fileMode)
                {
                    _base._lastError = HResult.MF_E_NOT_AVAILABLE;
                    return;
                }

                _base._lastError = Player.NO_ERROR;
                long newStop = value.Ticks;

                if (_base._stopTime == newStop) return;
                if ((newStop != 0 && newStop < _base._startTime) || newStop >= _base._mediaLength)
                {
                    _base._lastError = HResult.MF_E_OUT_OF_RANGE;
                    return;
                }

                _base._stopTime = newStop;
                _base.AV_UpdateTopology();
                if (_base._mediaStartStopTimeChanged != null) _base._mediaStartStopTimeChanged(_base, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Returns metadata (media information such as title and artist name) of the playing media (if available).
        /// </summary>
        public Metadata GetMetadata()
        {
            return GetMetadata(ImageSource.MediaOrFolder);
        }

        /// <summary>
        /// Returns metadata (media information such as title and artist name) of the playing media (if available).
        /// </summary>
        /// <param name="imageSource">A value indicating whether and where an image related to the media should be obtained.</param>
        public Metadata GetMetadata(ImageSource imageSource)
        {
            if (_base._playing)
            {
                if (_base._fileMode)
                {
                    return GetMetadata(_base._fileName, imageSource);
                }
                else
                {
                    _base._lastError = Player.NO_ERROR;

                    Metadata data = new Metadata();
                    data._album = GetName(MediaName.FullPath);
                    data._title = GetName(MediaName.FileNameWithoutExtension);
                    if (_base._liveStreamMode && !string.IsNullOrWhiteSpace(data._title) && data._title.Length > 1)
                    {
                        data._title = char.ToUpper(data._title[0]) + data._title.Substring(1);
                    }
                    return data;
                }
            }
            else
            {
                _base._lastError = HResult.MF_E_NOT_AVAILABLE;
                return new Metadata();
            }
        }

        /// <summary>
        /// Returns metadata (media information such as title and artist name) of the playing media (if available).
        /// </summary>
        /// <param name="fileName">The path and name of the file whose metadata is to be obtained.</param>
        /// <param name="imageSource">A value indicating whether and where an image related to the media should be obtained.</param>
        /// <returns></returns>
        public Metadata GetMetadata(string fileName, ImageSource imageSource)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                _base._lastError = HResult.E_INVALIDARG;
                return new Metadata();
            }

            Metadata tagInfo;
            _base._lastError = Player.NO_ERROR;

            try
            {
                if (!new Uri(fileName).IsFile)
                {
                    tagInfo = new Metadata();
                    {
                        try
                        {
                            tagInfo._title = Path.GetFileNameWithoutExtension(fileName);
                            tagInfo._album = fileName;
                        }
                        catch { /* ignore */ }
                    }
                    return tagInfo;
                }
            }
            catch { /* ignore */ }

            tagInfo = GetMediaTags(fileName, imageSource);

            try
            {
                // Get info from file path
                if (tagInfo._artist == null || tagInfo._artist.Length == 0) tagInfo._artist = Path.GetFileName(Path.GetDirectoryName(fileName));
                if (tagInfo._title == null || tagInfo._title.Length == 0) tagInfo._title = Path.GetFileNameWithoutExtension(fileName);

                // Get album art image (with certain values of 'imageSource')
                if (imageSource == ImageSource.FolderOrMedia || imageSource == ImageSource.FolderOnly || (imageSource == ImageSource.MediaOrFolder && tagInfo._image == null))
                {
                    GetMediaImage(fileName);
                    if (_tagImage != null) // null image not to replace image retrieved from media file with FolderOrMedia
                    {
                        tagInfo._image = _tagImage;
                        _tagImage = null;
                    }
                }
            }
            catch (Exception e) { _base._lastError = (HResult)Marshal.GetHRForException(e); }

            return tagInfo;
        }

        private Metadata GetMediaTags(string fileName, ImageSource imageSource)
        {
            Metadata tagInfo = new Metadata();
            IMFSourceResolver sourceResolver;
            IMFMediaSource mediaSource = null;
            IPropertyStore propStore = null;
            PropVariant propVariant = new PropVariant();

            HResult result = MFExtern.MFCreateSourceResolver(out sourceResolver);
            if (result == Player.NO_ERROR)
            {
                try
                {
                    MFObjectType objectType;
                    object source;

                    result = sourceResolver.CreateObjectFromURL(fileName, MFResolution.MediaSource, null, out objectType, out source);
                    if (result == Player.NO_ERROR)
                    {
                        mediaSource = (IMFMediaSource)source;

                        object store;
                        result = MFExtern.MFGetService(mediaSource, MFServices.MF_PROPERTY_HANDLER_SERVICE, typeof(IPropertyStore).GUID, out store);
                        if (result == Player.NO_ERROR)
                        {
                            propStore = (IPropertyStore)store;

                            // Artist
                            result = propStore.GetValue(PropertyKeys.PKEY_Music_Artist, propVariant);
                            tagInfo._artist = propVariant.GetString();
                            if (string.IsNullOrWhiteSpace(tagInfo._artist))
                            {
                                propStore.GetValue(PropertyKeys.PKEY_Music_AlbumArtist, propVariant);
                                tagInfo._artist = propVariant.GetString();
                            }

                            // Title
                            propStore.GetValue(PropertyKeys.PKEY_Title, propVariant);
                            tagInfo._title = propVariant.GetString();

                            // Album
                            propStore.GetValue(PropertyKeys.PKEY_Music_AlbumTitle, propVariant);
                            tagInfo._album = propVariant.GetString();

                            // Genre
                            propStore.GetValue(PropertyKeys.PKEY_Music_Genre, propVariant);
                            tagInfo._genre = propVariant.GetString();

                            // Duration
                            propStore.GetValue(PropertyKeys.PKEY_Media_Duration, propVariant);
                            tagInfo._duration = TimeSpan.FromTicks((long)propVariant.GetULong());

                            // TrackNumber
                            propStore.GetValue(PropertyKeys.PKEY_Music_TrackNumber, propVariant);
                            tagInfo._trackNumber = (int)propVariant.GetUInt();

                            // Year
                            propStore.GetValue(PropertyKeys.PKEY_Media_Year, propVariant);
                            tagInfo._year = propVariant.GetUInt().ToString();

                            // Image
                            if (imageSource != ImageSource.None && imageSource != ImageSource.FolderOnly)
                            {
                                propStore.GetValue(PropertyKeys.PKEY_ThumbnailStream, propVariant);
                                if (propVariant.ptr != null)
                                {
                                    IStream stream = (IStream)Marshal.GetObjectForIUnknown(propVariant.ptr);

                                    System.Runtime.InteropServices.ComTypes.STATSTG streamInfo;
                                    stream.Stat(out streamInfo, STATFLAG.NoName);

                                    int streamSize = (int)streamInfo.cbSize;
                                    if (streamSize > 0)
                                    {
                                        byte[] buffer = new byte[streamSize];
                                        stream.Read(buffer, streamSize, IntPtr.Zero);

                                        MemoryStream ms = new MemoryStream(buffer, false);
                                        Image image = Image.FromStream(ms);

                                        tagInfo._image = new Bitmap(image);

                                        image.Dispose();
                                        ms.Dispose();
                                    }

                                    Marshal.ReleaseComObject(streamInfo);
                                    Marshal.ReleaseComObject(stream);
                                }
                            }
                        }
                    }
                }
                //catch (Exception e) { result = (HResult)Marshal.GetHRForException(e); }
                catch { /* ignore */ }
            }

            if (sourceResolver != null) Marshal.ReleaseComObject(sourceResolver);
            if (mediaSource != null) Marshal.ReleaseComObject(mediaSource);
            if (propStore != null) Marshal.ReleaseComObject(propStore);
            propVariant.Dispose();

            return tagInfo;
        }

        // Get media information image help function
        private void GetMediaImage(string fileName)
        {
            _directoryInfo = new DirectoryInfo(Path.GetDirectoryName(fileName));
            string searchFileName = Path.GetFileNameWithoutExtension(fileName);
            string searchDirectoryName = _directoryInfo.Name;

            // 1. search using the file name
            if (!SearchMediaImage(searchFileName))
            {
                // 2. search using the directory name
                if (!SearchMediaImage(searchDirectoryName))
                {
                    // 3. search using keywords
                    int i = 0;
                    bool result;
                    do result = SearchMediaImage(_searchKeyWords[i++]);
                    while (!result && i < _searchKeyWords.Length);

                    if (!result)
                    {
                        // 4. find largest file
                        SearchMediaImage("*");
                    }
                }
            }
            _directoryInfo = null;
        }

        // Get media image help function
        private bool SearchMediaImage(string searchName)
        {
            if (searchName.EndsWith(@":\")) return false; // root directory - no folder name (_searchDirectoryName, for example C:\)

            string imageFile = string.Empty;
            long length = 0;
            bool found = false;

            for (int i = 0; i < _searchExtensions.Length; i++)
            {
                FileInfo[] filesFound = _directoryInfo.GetFiles(searchName + _searchExtensions[i]);

                if (filesFound.Length > 0)
                {
                    for (int j = 0; j < filesFound.Length; j++)
                    {
                        if (filesFound[j].Length > length)
                        {
                            length = filesFound[j].Length;
                            imageFile = filesFound[j].FullName;
                            found = true;
                        }
                    }
                }
            }
            if (found) _tagImage = Image.FromFile(imageFile);
            return found;
        }

        /*
            Thanks to Zeugma440, https://github.com/Zeugma440/atldotnet/wiki/Focus-on-Chapter-metadata
            A great help to defeat the uggly Apple chapter beast.
        */

        /// <summary>
        /// Returns chapter information of the playing media (if available).
        /// </summary>
        /// <param name="appleChapters">When this method returns, contains the chapter information of the media stored in the Apple format or null.</param>
        /// <param name="neroChapters">When this method returns, contains the chapter information of the media stored the Nero format or null.</param>
        public int GetChapters(out MediaChapter[] appleChapters, out MediaChapter[] neroChapters)
        {
            if (_base._playing) return GetChapters(_base._fileName, out appleChapters, out neroChapters);

            appleChapters = null;
            neroChapters = null;

            _base._lastError = HResult.MF_E_NOT_AVAILABLE;
            return (int)_base._lastError;
        }

        /// <summary>
        /// Returns chapter information of the specified media file (if available).
        /// </summary>
        /// <param name="fileName">The path and name of the file whose chapter information is to be obtained.</param>
        /// <param name="appleChapters">When this method returns, contains the chapter information of the media stored in the Apple format or null.</param>
        /// <param name="neroChapters">When this method returns, contains the chapter information of the media stored in the Nero format or null.</param>
        public int GetChapters(string fileName, out MediaChapter[] appleChapters, out MediaChapter[] neroChapters)
        {
            appleChapters = null;
            neroChapters = null;

            if (string.IsNullOrWhiteSpace(fileName)) _base._lastError = HResult.E_INVALIDARG;
            else if (!File.Exists(fileName)) _base._lastError = HResult.ERROR_FILE_NOT_FOUND;
            else
            {
                try
                {
                    byte[] buffer = new byte[16];

                    // check length and if first atom type is valid
                    _reader = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    _reader.Read(buffer, 0, 8);
                    if (_reader.Length > 1024 && (ROOT_ATOM_TYPES.IndexOf(Encoding.ASCII.GetString(new byte[] { buffer[4], buffer[5], buffer[6], buffer[7] }), StringComparison.Ordinal) >= 0))
                    {
                        _base._lastError = Player.NO_ERROR;
                    }
                    else _base._lastError = HResult.MF_E_NOT_AVAILABLE;
                }
                catch (Exception e) { _base._lastError = (HResult)Marshal.GetHRForException(e); }
            }

            if (_base._lastError == Player.NO_ERROR)
            {
                _fileLength = _reader.Length;
                _reader.Position = 0;

                appleChapters = GetAppleChapters();
                if (_moovStart != 0) neroChapters = GetNeroChapters();
            }

            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }

            return (int)_base._lastError;
        }

        private MediaChapter[] GetAppleChapters()
        {
            MediaChapter[] appleChapters = null;
            byte[] buffer = new byte[256];

            try
            {
                long index = FindAtom(MOOV_ATOM, 0, _fileLength);
                if (index > 0)
                {
                    bool found = false;

                    _moovStart = index;
                    _moovEnd = _atomEnd;
                    long moovIndex = index;
                    long moovEnd = _atomEnd;

                    long oldIndex;
                    long oldEnd;

                    int trackCounter = 0;
                    int trackNumber = 0;

                    while (!found && index < moovEnd)
                    {
                        oldEnd = _atomEnd;

                        // walk the "moov" atom
                        index = FindAtom(TRAK_ATOM, index, _atomEnd);
                        if (index > 0)
                        {
                            oldIndex = _atomEnd;
                            trackCounter++;

                            // walk the "trak" atom
                            index = FindAtom(TREF_ATOM, index, _atomEnd);
                            if (index > 0)
                            {
                                index = FindAtom(CHAP_ATOM, index, _atomEnd);
                                if (index > 0)
                                {
                                    _reader.Read(buffer, 0, 4);
                                    trackNumber = buffer[0] << 24 | buffer[1] << 16 | buffer[2] << 8 | buffer[3];
                                    found = true;

                                    index = oldIndex;
                                    _reader.Position = index;
                                    _atomEnd = oldEnd;

                                    break; // break while
                                }
                            }
                            index = oldIndex;
                            _reader.Position = index;
                            _atomEnd = oldEnd;
                        }
                        else // no more trak atoms - break not really necessary (?)
                        {
                            break;
                        }
                    }

                    if (found)
                    {
                        // get the chapters track
                        int count = trackNumber - trackCounter;
                        if (count < 0)
                        {
                            count = trackNumber;
                            index = moovIndex;
                            _reader.Position = index;
                            _atomEnd = _moovEnd;
                        }
                        for (int i = 0; i < count && index > 0; i++)
                        {
                            index = FindAtom(TRAK_ATOM, index, _atomEnd);
                            if (i < count - 1)
                            {
                                index = _atomEnd;
                                _reader.Position = index;
                                _atomEnd = _moovEnd;
                            }
                        }

                        if (index > 0)
                        {
                            // walk the "trak" atom
                            oldIndex = index;
                            oldEnd = _atomEnd;
                            index = FindAtom(TKHD_ATOM, index, _atomEnd);
                            if (index > 0)
                            {
                                index = oldIndex;
                                _reader.Position = index;
                                _atomEnd = oldEnd;
                                index = FindAtom(MDIA_ATOM, index, _atomEnd);
                                if (index > 0)
                                {
                                    oldIndex = index;

                                    // get time scale
                                    index += 20;
                                    _reader.Position = index;
                                    _reader.Read(buffer, 0, 4);
                                    int timeScale = buffer[0] << 24 | buffer[1] << 16 | buffer[2] << 8 | buffer[3];
                                    if (timeScale == 0) timeScale = 1;

                                    index = oldIndex;
                                    _reader.Position = index;
                                    index = FindAtom(MINF_ATOM, index, _atomEnd);
                                    if (index > 0)
                                    {
                                        index = FindAtom(STBL_ATOM, index, _atomEnd);
                                        if (index > 0)
                                        {
                                            oldIndex = index;
                                            oldEnd = _atomEnd;
                                            index = FindAtom(STTS_ATOM, index, _atomEnd);
                                            if (index > 0)
                                            {
                                                // get chapter start times (durations)
                                                index += 4;
                                                _reader.Position = index;
                                                _reader.Read(buffer, 0, 4);
                                                int chapterCount = buffer[0] << 24 | buffer[1] << 16 | buffer[2] << 8 | buffer[3];

                                                if (chapterCount > 0)
                                                {
                                                    int startTimeCounter = chapterCount;
                                                    int startTimeIndex = 1; // first one is zero
                                                    int startTime = 0;

                                                    int[] startTimes = new int[startTimeCounter];
                                                    while (startTimeCounter > 1)
                                                    {
                                                        _reader.Read(buffer, 0, 8);
                                                        int sampleCount = buffer[0] << 24 | buffer[1] << 16 | buffer[2] << 8 | buffer[3];
                                                        startTime += (buffer[4] << 24 | buffer[5] << 16 | buffer[6] << 8 | buffer[7]) / timeScale;
                                                        for (int i = 0; i < sampleCount; i++)
                                                        {
                                                            startTimes[startTimeIndex++] = startTime;
                                                        }
                                                        startTimeCounter -= sampleCount;
                                                    }

                                                    index = oldIndex;
                                                    _reader.Position = index;
                                                    _atomEnd = oldEnd;
                                                    index = FindAtom(STCO_ATOM, index, _atomEnd);
                                                    if (index > 0)
                                                    {
                                                        // get chapter titles
                                                        index += 4;
                                                        _reader.Position = index;
                                                        _reader.Read(buffer, 0, 4);
                                                        int entries = buffer[0] << 24 | buffer[1] << 16 | buffer[2] << 8 | buffer[3];
                                                        if (entries == chapterCount)
                                                        {
                                                            appleChapters = new MediaChapter[chapterCount];
                                                            for (int i = 0; i < chapterCount; i++)
                                                            {
                                                                _reader.Read(buffer, 0, 4);
                                                                int offset1 = buffer[0] << 24 | buffer[1] << 16 | buffer[2] << 8 | buffer[3];

                                                                index = _reader.Position;
                                                                _reader.Position = offset1;

                                                                _reader.Read(buffer, 0, 2);
                                                                int len = buffer[0] << 8 | buffer[1];

                                                                _reader.Read(buffer, 0, len);
                                                                appleChapters[i] = new MediaChapter();
                                                                appleChapters[i]._title = Encoding.ASCII.GetString(buffer, 0, len);
                                                                appleChapters[i]._startTime = TimeSpan.FromSeconds(startTimes[i]);

                                                                _reader.Position = index;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                appleChapters = null;
            }
            return appleChapters;
        }

        private MediaChapter[] GetNeroChapters()
        {
            MediaChapter[] neroChapters = null;
            byte[] buffer = new byte[256];

            long index = _moovStart; // retrieved at GetAppleChapters
            _reader.Position = index;
            long moovEnd = _moovEnd;
            _atomEnd = moovEnd;

            try
            {
                while (index < moovEnd)
                {
                    long oldIndex;
                    long oldEnd = _atomEnd;

                    index = FindAtom(UDTA_ATOM, index, _atomEnd);
                    if (index > 0)
                    {
                        oldIndex = _atomEnd;
                        index = FindAtom(CHPL_ATOM, index, _atomEnd);
                        if (index > 0)
                        {
                            index += 5;
                            _reader.Position = index;
                            _reader.Read(buffer, 0, 4);
                            int count = buffer[0] << 24 | buffer[1] << 16 | buffer[2] << 8 | buffer[3];
                            neroChapters = new MediaChapter[count];
                            int length;

                            for (int i = 0; i < count; i++)
                            {
                                _reader.Read(buffer, 0, 9);

                                neroChapters[i] = new MediaChapter();
                                neroChapters[i]._startTime = TimeSpan.FromTicks(((long)(buffer[0] << 24 | buffer[1] << 16 | buffer[2] << 8 | buffer[3]) << 32)
                                                                                | ((buffer[4] << 24 | buffer[5] << 16 | buffer[6] << 8 | buffer[7]) & 0xffffffff));
                                length = buffer[8];
                                _reader.Read(buffer, 0, length);
                                neroChapters[i]._title = Encoding.ASCII.GetString(buffer, 0, length);
                            }
                            break; // chapters found and done
                        }
                        else // chapters not found, check for more udta atoms
                        {
                            index = oldIndex;
                            _reader.Position = index;
                            _atomEnd = oldEnd;
                        }
                    }
                    else // no more udta atoms - chapters not present and done
                    {
                        break;
                    }
                }
            }
            catch
            {
                neroChapters = null;
            }
            return neroChapters;
        }

        private long FindAtom(byte[] type, long startIndex, long endIndex)
        {
            long index = startIndex;
            long end = endIndex - 8;
            byte[] buffer = new byte[16];

            while (index < end)
            {
                _reader.Read(buffer, 0, 8);
                long atomSize = buffer[0] << 24 | buffer[1] << 16 | buffer[2] << 8 | buffer[3];
                if (atomSize < 2)
                {
                    if (atomSize == 0) atomSize = _fileLength - index;
                    else // size == 1
                    {
                        _reader.Read(buffer, 8, 8);
                        atomSize = ((long)((buffer[8] << 24) | (buffer[9] << 16) | (buffer[10] << 8) | buffer[11]) << 32)
                                   | ((buffer[12] << 24 | buffer[13] << 16 | buffer[14] << 8 | buffer[15]) & 0xffffffff);
                    }
                }

                if (buffer[4] == type[0] && buffer[5] == type[1] && buffer[6] == type[2] && buffer[7] == type[3])
                {
                    _atomEnd = index + atomSize;
                    return _reader.Position; // found
                }

                index += atomSize;
                _reader.Position = index;
            }
            return 0; // not found
        }
    }
}