﻿using System;
using System.Windows.Forms;
using PlexDL.UI;

namespace PlexDL.Common
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler.CriticalExceptionHandler;
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Home());
        }
    }
}