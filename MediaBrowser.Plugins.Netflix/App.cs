using MediaBrowser.Theater.Interfaces.Presentation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MediaBrowser.Plugins.Steam
{
    public class App : IApp
    {
        private readonly IImageManager _imageManager;

        public App(IImageManager imageManager)
        {
            _imageManager = imageManager;
        }

        public FrameworkElement GetThumbImage()
        {
            var image = new Image
            {
                Source = _imageManager.GetBitmapImage(Plugin.GetThumbUri())
            };

            return image;
        }

        public Task Launch()
        {
            return Task.Run(() => LaunchProcess());
        }

        private void LaunchProcess()
        {
            Version win8version = new Version(6, 2, 9200, 0);

            if (Environment.OSVersion.Platform == PlatformID.Win32NT &&
                Environment.OSVersion.Version >= win8version)
            {
                // check if Netflix Metro app is installed to stop it trying to load the app and then windows asking for a program to run the protocol

                var process = new Process
                {
                    EnableRaisingEvents = true,

                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"netflix://",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        ErrorDialog = true,
                        UseShellExecute = true,
                    }
                };

                process.Exited += process_Exited;

                process.Start();
            }
            // Return error becuase its not windows 8 or 8.1
        }

        void process_Exited(object sender, EventArgs e)
        {
            var process = (Process)sender;

            process.Dispose();
        }

        public string Name
        {
            get { return "Netflix"; }
        }

        public void Dispose()
        {
        }
    }

    public class AppFactory : IAppFactory
    {
        private readonly IImageManager _imageManager;

        public AppFactory(IImageManager imageManager)
        {
            _imageManager = imageManager;
        }

        public IEnumerable<IApp> GetApps()
        {
            return new[] { new App(_imageManager) };
        }
    }
}
