﻿using PlexDL.Common.Globals;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PlexDL.Common.Logging
{
    public static class LoggingHelpers
    {
        private static int logIncrementer;

        public static void RecordGenericEntry(string logEntry)
        {
            try
            {
                logIncrementer++;
                string[] headers =
                {
                    "SessionID","EntryID", "DateTime", "Entry"
                };
                string[] logEntryToAdd =
                {
                    GlobalStaticVars.CurrentSessionId, logIncrementer.ToString(), DateTime.Now.ToString(), logEntry
                };
                if (GlobalStaticVars.Settings.Logging.EnableGenericLogDel)
                    LogDelWriter("PlexDL.logdel", headers, logEntryToAdd);
            }
            catch (Exception ex)
            {
                //for debugging only!
                //MessageBox.Show(ex);
                //ignore the error
            }
        }

        public static void RecordCacheEvent(string eventEntry, string reqUrl = "Unknown")
        {
            try
            {
                string[] headers =
                {
                    "SessionID", "RequestedURL", "DateTime", "Entry"
                };
                string[] logEntryToAdd =
                {
                    GlobalStaticVars.CurrentSessionId, reqUrl, DateTime.Now.ToString(), eventEntry
                };
                if (GlobalStaticVars.Settings.Logging.EnableGenericLogDel)
                    LogDelWriter("Caching.logdel", headers, logEntryToAdd);
            }
            catch (Exception ex)
            {
                //for debugging only!
                //MessageBox.Show(ex);
                //ignore the error
            }
        }

        public static void RecordException(string message, string type)
        {
            try
            {
                ////Options weren't too great performance-wise, so I ended up using a stack-walk.
                ////If there's minimal errors happening at once, this shouldn't be a problem, otherwise disable
                ////The in-app setting to prevent this method from firing.
                if (GlobalStaticVars.Settings.Logging.EnableExceptionLogDel)
                {
                    var stackTrace = new StackTrace();
                    var function = stackTrace.GetFrame(1).GetMethod().Name;
                    string[] headers =
                    {
                        "SessionID", "DateTime", "ExceptionMessage", "OccurredIn", "ExceptionType"
                    };
                    string[] LogEntry =
                    {
                        GlobalStaticVars.CurrentSessionId, DateTime.Now.ToString(), message, function, type
                    };
                    LogDelWriter("ExceptionLog.logdel", headers, LogEntry);
                }
            }
            catch
            {
                //ignore the error
            }
        }

        public static void RecordTransaction(string uri, string statusCode)
        {
            try
            {
                if (GlobalStaticVars.Settings.Logging.EnableXMLTransactionLogDel)
                {
                    string[] headers =
                    {
                        "SessionID","DateTime", "Uri", "StatusCode"
                    };
                    string[] LogEntry =
                    {
                        GlobalStaticVars.CurrentSessionId, DateTime.Now.ToString(), uri, statusCode
                    };
                    LogDelWriter("TransactionLog.logdel", headers, LogEntry);
                }
            }
            catch
            {
                //ignore the error
            }
        }

        private static string CleanseLogDel(string line)
        {
            var clean = line;
            char[] bannedChars =
            {
                '#', '!'
            };
            foreach (var c in line)
                if (bannedChars.Contains(c))
                {
                    var index = clean.IndexOf(c);
                    clean.Remove(index, 1);
                }

            return clean;
        }

        public static void LogDelWriter(string fileName, string[] headers, string[] logEntry)
        {
            try
            {
                if (!Directory.Exists("Logs"))
                    Directory.CreateDirectory("Logs");

                var logdelLine = "";
                var fqFile = @"Logs\" + fileName;

                foreach (var l in logEntry)
                    logdelLine += l + "!";

                logdelLine = CleanseLogDel(logdelLine.TrimEnd('!'));

                var headersString = "###";
                foreach (var h in headers)
                    headersString += h + "!";

                headersString = CleanseLogDel(headersString.TrimEnd('!'));

                if (File.Exists(fqFile))
                {
                    var existing = File.ReadAllText(fqFile);
                    if (string.IsNullOrEmpty(existing))
                    {
                        var contentToWrite = headersString + "\n" + logdelLine;
                        File.WriteAllText(fqFile, contentToWrite);
                    }
                    else
                    {
                        var contentToWrite = existing + "\n" + logdelLine;
                        File.WriteAllText(fqFile, contentToWrite);
                    }
                }
                else
                {
                    var contentToWrite = headersString + "\n" + logdelLine;
                    File.WriteAllText(fqFile, contentToWrite);
                }
            }
            catch (Exception ex)
            {
                //for debugging only!
                //MessageBox.Show(ex);
                //ignore the error
            }
        }
    }
}