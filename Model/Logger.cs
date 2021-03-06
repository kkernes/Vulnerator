﻿using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using log4net.Util;
using System;
using System.Diagnostics;

namespace Vulnerator.Model
{
    public class Logger
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static bool IsDebug = false;

        public void Setup()
        {
            SetIsDebugFlag();
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders();

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.AddConverter(new ConverterInfo
                {
                    Name = "fileNameNoPath",
                    Type = typeof(FileNameNoPathConverter)
                }
            );
            patternLayout.ConversionPattern = "%-30d %-10level %fileNameNoPath : %L %m%n";
            patternLayout.ActivateOptions();
            RollingFileAppender rollingFileAppender = new RollingFileAppender();
            rollingFileAppender.LockingModel = new FileAppender.MinimalLock();
            rollingFileAppender.AppendToFile = true;
            rollingFileAppender.File = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Vulnerator\V6Log.txt";
            rollingFileAppender.Layout = patternLayout;
            rollingFileAppender.MaxSizeRollBackups = 5;
            rollingFileAppender.RollingStyle = RollingFileAppender.RollingMode.Once;
            rollingFileAppender.StaticLogFileName = true;
            rollingFileAppender.ActivateOptions();
            hierarchy.Root.AddAppender(rollingFileAppender);
            log4net.Config.BasicConfigurator.Configure(rollingFileAppender);

            if (IsDebug)
            { hierarchy.Root.Level = Level.All; }
            else
            { hierarchy.Root.Level = Level.Info; }
        }

        [Conditional("DEBUG")]
        private void SetIsDebugFlag()
        { IsDebug = true; }
    }
}
