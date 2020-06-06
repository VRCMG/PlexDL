﻿using PlexDL.Common.Caching;
using PlexDL.Common.Caching.Handlers;
using PlexDL.Common.Globals;
using PlexDL.Common.Logging;
using PlexDL.Common.Structures.Plex;
using PlexDL.PlexAPI;
using PlexDL.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace PlexDL.Common
{
    public static class Methods
    {
        public static int StringToInt(string value, int defaultValue = 0)
        {
            var val = defaultValue;

            try
            {
                if (int.TryParse(value, out var r))
                    val = r;
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "StringToIntError");
            }

            return val;
        }

        public static bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "HEAD";

                //Getting the Web Response.
                var response = (HttpWebResponse)request.GetResponse();

                //Release the stream
                response.Close();

                //Returns TRUE if the Status code == 200
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                //Any exception will return false.
                return false;
            }
        }

        public static bool IsPrivateIp(string ipAddress)
        {
            var ipParts = ipAddress.Split(new[]
            {
                "."
            }, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse).ToArray();
            // in private ip range
            return ipParts[0] == 10 ||
                   ipParts[0] == 192 && ipParts[1] == 168 ||
                   ipParts[0] == 172 && ipParts[1] >= 16 && ipParts[1] <= 31;

            // IP Address is probably public.
            // This doesn't catch some VPN ranges like OpenVPN and Hamachi.
        }

        public static string MatchUriToToken(string uri, List<Server> plexServers)
        {
            foreach (var s in from s in plexServers let serverUri = "http://" + s.address + ":" + s.port + "/" where uri.Contains(serverUri) select s)
            {
                return s.accessToken;
            }

            return "";
        }

        public static void SetHeaderText(DataGridView dgv, DataTable table)
        {
            //Copy column captions into DataGridView
            foreach (DataGridViewColumn col in dgv.Columns)
                if (table.Columns[col.Name].Caption != null)
                    col.HeaderText = table.Columns[col.Name].Caption;
        }

        public static void SortingEnabled(DataGridView dgv, bool enabled)
        {
            if (enabled)
                foreach (DataGridViewColumn c in dgv.Columns)
                    c.SortMode = DataGridViewColumnSortMode.Automatic;
            else
                foreach (DataGridViewColumn c in dgv.Columns)
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        public static bool PlexXmlValid(XmlDocument doc)
        {
            var checkNodes = doc.GetElementsByTagName("MediaContainer");
            return checkNodes.Count > 0;
        }

        public static List<string> OrderMatch(List<string> ordered, List<string> unordered)
        {
            var newList = unordered.OrderBy(ordered.IndexOf).ToList();
            return newList;
        }

        public static string CalculateTime(double time, bool ms = true)
        {
            //convert back to seconds from miliseconds
            if (ms)
                time = Math.Round(time) / 1000;
            var T = Convert.ToInt32(time);

            var h = T / 3600;
            T %= 3600;
            var m = T / 60;
            var s = T % 60;

            var mm = m < 10 ? $"0{m}" : m.ToString();
            var ss = s < 10 ? $"0{s}" : s.ToString();

            var calculatedTime = $"{h}:{mm}:{ss}";

            return calculatedTime;
        }

        public static string GenerateRandomNumber(int length)
        {
            var random = new Random();
            var r = "";

            for (var i = 1; i < length + 1; i++)
            {
                r += random.Next(0, 9).ToString();
            }
            return r;
        }

        public static string FormatBytes(long bytes, bool includeSpace = false)
        {
            string[] suffix =
            {
                "B", "KB", "MB", "GB", "TB"
            };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < suffix.Length && bytes >= 1024; i++, bytes /= 1024)
                dblSByte = bytes / 1024.0;

            return includeSpace ? $"{dblSByte:0.##} {suffix[i]}" : $"{dblSByte:0.##}{suffix[i]}";
        }

        public static bool AdultKeywordCheck(PlexObject stream)
        {
            var keywords = Resources.keywordBlacklist.Split('\n');
            var search = stream.StreamInformation.ContentTitle.ToLower();
            var rgx = new Regex("[^a-zA-Z0-9_. ]+");
            //MessageBox.Show(keywords.Length.ToString());
            search = rgx.Replace(search, "");
            var match = false;
            if (string.Equals(stream.ContentGenre.ToLower(), "porn") || string.Equals(stream.ContentGenre.ToLower(), "adult"))
            {
                match = true;
            }
            else
            {
                if (keywords.Length > 0)
                    foreach (var k in keywords)
                        if (!string.IsNullOrEmpty(k) && !string.IsNullOrWhiteSpace(k))
                        {
                            var clean = rgx.Replace(k, "").ToLower();
                            if (search.Contains(clean))
                            {
                                match = true;
                                break;
                            }
                        }
            }

            return match;
        }

        public static bool StreamAdultContentCheck(PlexObject stream)
        {
            if (!GlobalStaticVars.Settings.Generic.AdultContentProtection) return true;
            if (!AdultKeywordCheck(stream)) return true;

            var result =
                MessageBox.Show(@"The content you're about to view may contain adult (18+) themes. Are you okay with viewing this content?", @"Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            return result != DialogResult.No;
        }

        public static Bitmap GetImageFromUrl(string url, bool forceNoCache = false)
        {
            try
            {
                Helpers.CacheStructureBuilder();
                if (string.IsNullOrEmpty(url))
                    return Resources.image_not_available_png_8;

                if (!forceNoCache)
                    if (ThumbCaching.ThumbInCache(url))
                        return ThumbCaching.ThumbFromCache(url);
            }
            catch (UnauthorizedAccessException ex)
            {
                LoggingHelpers.RecordException(ex.Message, "ThumbIOAccessError");
                return ForceImageFromUrl(url);
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "ImageFetchError");
                return Resources.image_not_available_png_8;
            }

            return ForceImageFromUrl(url);
        }

        private static Bitmap ForceImageFromUrl(string url)
        {
            try
            {
                var request = WebRequest.Create(url);

                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    var result = (Bitmap)Image.FromStream(stream);
                    ThumbCaching.ThumbToCache(result, url);
                    return result;
                }
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "ImageFetchError");
                return Resources.image_not_available_png_8;
            }
        }

        public static bool ValidateIPv4(string ipString)
        {
            if (string.IsNullOrWhiteSpace(ipString))
                return false;

            var splitValues = ipString.Split('.');
            return splitValues.Length == 4 && splitValues.All(r => byte.TryParse(r, out _));
        }

        public static string RemoveIllegalCharacters(string illegal)
        {
            var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex($"[{Regex.Escape(regexSearch)}]");
            return r.Replace(illegal, "");
        }
    }
}