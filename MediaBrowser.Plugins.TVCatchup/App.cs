using MediaBrowser.Theater.Interfaces.Presentation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace MediaBrowser.Plugins.TVCatchup
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

        private bool checkTVCatchupInstalled()
        {
            string[] dirs = Directory.GetDirectories(@"C:\\Program Files\\WindowsApps\\", "*TVCatchup*");
            if (dirs.Length > 0)
            {
                return true;
            }
            return false;
        }

        private void LaunchProcess()
        {
            if (checkTVCatchupInstalled())
            {
                SendKeys.SendWait("^{ESC}");
                SendKeys.SendWait("TVCatchup");
                SendKeys.SendWait("{ENTER}");
            }
        }

        public string Name
        {
            get { return "TVCatchup"; }
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
