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

        private string GetSteamPathFromRegistry()
        {
            RegistryKey regKey = Registry.CurrentUser;
            return regKey.OpenSubKey(@"Software\Valve\Steam").GetValue("SteamExe").ToString();
        }

        public Task Launch()
        {
            return Task.Run(() => LaunchProcess());
        }


        private string GetProcessArguments()
        {
            string arguments = "";
            if (Process.GetProcessesByName("Steam").Length > 0)
            {
                arguments = "steam://open/bigpicture";
            }
            else
            {
                arguments = "-bigpicture";
            }
            return arguments;
        }

        private void LaunchProcess()
        {
            var process = new Process
            {
                EnableRaisingEvents = true,

                StartInfo = new ProcessStartInfo
                {
                    FileName = GetSteamPathFromRegistry(),
                    WindowStyle = ProcessWindowStyle.Hidden,
                    ErrorDialog = false,
                    UseShellExecute = false,
                    Arguments = GetProcessArguments()
                }
            };

            process.Exited += process_Exited;

            process.Start();
        }

        void process_Exited(object sender, EventArgs e)
        {
            var process = (Process)sender;

            process.Dispose();
        }

        public string Name
        {
            get { return "Steam"; }
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
