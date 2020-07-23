# PlexDL
### Plex Downloader/Streamer written in C#

* Utilises modified csharp-plex-api by ammmze for server detection - [GitHub Repo](https://github.com/ammmze/csharp-plex-api)
* Utilises Google's Material Design Icons - [Official Site](https://material.io/icons)
* Utilises Newtonsoft.Json by JamesNK - [GitHub Repo](https://github.com/JamesNK/Newtonsoft.Json)
* Utilises AltoHttp by aalitor for Web Downloads - [GitHub Repo](https://github.com/aalitor/AltoHttp)
* Utilises PVS.MediaPlayer by Peter Vegter - [CodeProject Article](https://www.codeproject.com/Articles/109714/PVS-MediaPlayer-Audio-and-Video-Player-Library)
* Utilises WinFormAnimation by falahati - [Github Repo](https://github.com/falahati/WinFormAnimation/)
* Utilises CircularProgressBar by falahati - [GitHub Repo](https://github.com/falahati/CircularProgressBar/)

### What does PlexDL do?
Many different things!

PlexDL uses a Plex Media Server's ability to serve XML API requests, and from the data returned, PlexDL gathers information and displays it in various gridviews to make it easier for you to enjoy your content. It supports the following and more:
- Viewing Metadata about any media type currently supported. To do this, just head over to `Content->Metadata` on the main toolbar.
- Streaming your media. PlexDL utilises a really cool media library known as PVS.MediaPlayer. It allows playback of supported files via the Windows Media Foundation (WMF), and the best part? It's possible to create a custom GUI and skin it how you want. That's what we've done for you in PlexDL, but if you're not wanting to use PVS, you can use VLC or your default browser.
- Downloading your media. PlexDL is named like this for a reason! We've created a specialised framework to allow authorised Plex downloads. Even if you're not a server owner, you can still download if you have an account or an account token.
- Exporting media profiles. You can export a .pmxml file which will store infirmation about your content. At any time (and given your token is still valid), you can import this back into the `Content->Metadata` window and stream it without even logging in!

**Important Notes**

- PVS.MediaPlayer can play some codecs (any that are native to WMF), but the diversity of the container specification makes it unsafe (code-wise) to implement it at this point. Hence, PlexDL will not allow its default player to accept \*.mkv files.
- If you're using VLC, make sure you follow the guide below about setting it up with PlexDL.

### Supported Media
PlexDL currently supports the following media types
- Music - Artists, Albums and Tracks
- TV - Shows, Seasons and Episodes
- Movies

More will be on the way, but please note that photos are current very difficult to implement. As such, we can't support this feature until sufficient resources can be devoted to it.

### Performance?
PlexDL will work for almost any PMS out there (provided you have an account key/valid Plex.tv account). However, there may be instances where the software is underperforming due to a variety of reasons. One such reason, is that the custom interfaces built to interpret the data from the PMS aren't perfect, and may stutter from time to time. PlexDL is also heavily reliant on internet speeds and server reliability, so that is also a factor.

It should be noted, however, that PlexDL does support various forms of caching. This will store downloaded information in the  `%APPDATA%\.plexdl\caching` folder. The structure of the caching folder is as follows:
```
\caching                              -- Root Folder
└───\%TOKEN_HASH%                     -- MD5 of account token
    ├──\%SERVER_HASH%                 -- MD5 of server IP
    │  ├───\thumb                     -- Cached images with *.thumb filename
    │  │   └───%IMAGE_URL_HASH%.thumb -- *.thumb is named as a hashed URL, to be retrieved when a matching request is ID'd.
    │  └───\xml                       -- Cached XML API data with *.xml filename
    │      └───%XML_URL_HASH%.xml     -- *.xml is named as a hashed URL, to be retrieved when a matching request is ID'd.
    └──%TOKEN_HASH%.slst              -- Cached server details list (cached IP, port, etc.)
       
```
Using the Settings dialog in `File->Settings`, you can enable/disable the three forms of PlexDL caching:
- Server List Caching
- XML API Caching
- Image Caching

By using caching, you can drastically increase the performance of the application, as PlexDL can skip downloading a new copy of the file each time. However, the obvious downside is remembering to regularly clear the cache, as cached data will quickly become outdated.

### How to get started
#### __1. Building from Source__
PlexDL targets the .NET Framework 4.7.2, and was initially built with Visual Studio 2019. The PlexDL source comes preloaded with all necessary icons and resources, AltoHttp, chsarp-plex-api, WinFormAnimation, CircularProgressBar and PVS.MediaPlayer.

PlexDL includes the appropriate NuGet references to libbrhscgui (prebuilt for you) and RestSharp, and upon cloning this repo, you'll just need to restore the packages. In addition, LogDel (a separate project linked with PlexDL in this repo), will need to have Newtonsoft.Json restored from NuGet in order to build. Note that LogDel is required for PlexDL operation, and cannot be excluded without modifying the source.

Steps for building
1. `git clone http://github.com/Brhsoftco/PlexDL.git`
2. Open `PlexDL.sln` in Visual Studio 2017+
3. Enable restoring NuGet packages via `Tools->Options->NuGet Package Manager->Package Restore->Allow NuGet to download missing packages`
4. Right click the `PlexDL` Solution in the Solution Explorer
5. Select `Restore NuGet Packages`
6. `Build->Build Solution`
7. Run resulting `PlexDL.exe` in the `~\bin` folder

#### __2. Downloading from Releases__
Alternatively, can access the latest official build [here](https://github.com/Brhsoftco/PlexDL/releases/latest). Just download `Release.zip` to get all needed dependencies and the pre-built executable.
### __Using PlexDL__
#### __Basic Usage__
1. To get started, first obtain your Plex account token. A guide for this may be found [here](https://support.plex.tv/articles/204059436-finding-an-authentication-token-x-plex-token/). Alternatively, you can just use your Plex.tv account (v1.4.1+) to automatically retrieve your account token.
2. Select `Servers->Server Manager` from the main panel. This will load the Server Manager.
3. Select `Authenticate` and choose your preferred method.
4. Upon authenticating successfully, you can then use `Load->Servers` to populate the grid with your registered servers and `Load->Relays` to populate the grid with your registered Plex.tv indirect relays (`*.*.plex.direct` remote-access hostnames).
5. Select the server/relay you would like to connect to, and then select `Connect` on the Server Manager's menu.
6. Once you are connected (and the library is filled), you may start to browse your library.
7. The "Library Content" area is your hub for titles. Here, you can select TV Shows and Movies from their respective tabs.
8. If you select a movie from the "Movies" panel, the options to stream or download the content will become immediately available.
9. If you select a TV Show from the "TV" panel, you may browse the TV seasons (Top-Right grid) and episodes (Bottom-Right grid) associated with that title.
10. Music titles follow the exact same principles as TV titles.
11. You may only stream or download a TV/Movie/Music title upon selecting an item from the appropriate grid.
12. You may browse metadata associated with your selected content by using `Right-click->Metadata` or `Content->Metadata`.
13. PlexDL allows profile loading and saving, which allows you to save your account token for later use, or change internal settings to your liking and then save those changes. To do this, first follow Steps 1-6, and then select `File->Save` (it's also possible to use `Ctrl+S`). You can then edit the *.prof* XML file in any ordinary text editor.
14. Likewise, to load the profile, simply select `File->Load` (you can also use `Ctrl+O`), then browse to your generated XML *.prof* file.

#### __Content Filtering__
* PlexDL natively filters potentially adult-orientated content.
* It is possible to disable this filter by exporting a profile, then changing "AdultContentProtection" from "true" to "false" inside the resulting _.prof_ file, then reloading that profile back into PlexDL. You can also use `File->Settings`.
* For users' convenience, PlexDL will filter content that matches a genre-based criteria by pixelating posters in the metadata section, and warning users before streaming the content. The plot summary is also replaced with `Plot summary censored`.
* PlexDL can also filter adult content based on a keyword list. E.g. a text file that contains terms related to adult content.
* PlexDL includes a blank file named `keywordBlacklist.txt` in the `PlexDL\Resources` section of the source code, however, you must populate this list yourself and build the source from there. The format of this file is simply one blacklisted term per line.
* PlexDL does not provide populated keyword lists by default; please do not ask for any.

#### __Shortcut Keys__
##### __Main App__
* `Control+O` - Load a Profile
* `Control+S` - Save a Profile
* `Control+C` - Launches the Server Manager
* `Control+D` - Disconnects and Clears All Main Grids
* `Control+M` - Allows Viewing Metadata
* `Control+F` - Launches the Search Dialog
* `Control+E` - Allows Exporting a PMXML file of the currently selected content
##### __Server Manager__
* `Control+C` - Launches Token Authentication Dialog
* `Control+L` - Launches Plex.tv Login Dialog
* `Control+E` - Allows Exporting PlexMovie XML Data (for the Currently Selected Title)
* `Control+S` - Populates Grid With Registered Servers
* `Control+R` - Populates Grid With Registered Plex.tv Relays
* `Control+D` - Launches Direct Connection Dialog
* `Control+K` - Launches Local Machine Connection Dialog
##### __Metadata__
* `Control+S` - Export a PMXML File of the Currently Loaded Content
* `Control+O` - Load a PMXML File into the Metadata Window
* `Control+V` - Start Streaming the Currently Loaded Content in VLC Media Player
* `Control+B` - Start Streaming the Currently Loaded Content in your Default Browser
##### __PVS.MediaPlayer__
* `Spacebar`    - Play/Pause Current Content
* `Up Arrow`    - Load Previous Content in Grid
* `Down Arrow`  - Load Next Content in Grid
* `Right Arrow` - Skip Forward
* `Left Arrow`  - Skip Backward
* `F` - Toggle Fullscreen Mode

#### Setting up VLC for use with PlexDL
Because VLC accepts a vast array of command-line arguments and values, PlexDL only needs to know where the location of `vlc.exe` is. This way, it may execute it on the fly. To point PlexDL to VLC, please follow the following procedure:
1. Load a server of your choice and populate the gridviews.
2. Export a profile by using `Ctrl+S` on the home screen
3. Open the resulting *.prof* file in Notepad or Notepad++ (preferred).
4. Change `VLCMediaPlayerPath` to the location of `vlc.exe` on your system.
5. Save and close your editor.
6. Load this newly edited profile back into PlexDL, and it will now execute VLC perfectly.
