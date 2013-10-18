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
using System.Security.Principal;
using System.Linq;
using Windows.Management.Deployment;

namespace MediaBrowser.Plugins.Channel5
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

        private bool checkChannel5Installed()
        {
            PackageManager packageManager = new PackageManager();
            IEnumerable<Windows.ApplicationModel.Package> packages = (IEnumerable<Windows.ApplicationModel.Package>)packageManager.FindPackagesForUser(WindowsIdentity.GetCurrent().User.ToString(), "Channel5.Demand5", "CN=D6547641-0EAB-4A6C-9559-B516C20C4050");

            if (packages.Count() > 0)
            {
                return true;
            }
            return false;
        }

        private void LaunchProcess()
        {
            if (checkChannel5Installed())
            {
                SendKeys.SendWait("^{ESC}");
                SendKeys.SendWait("demand 5");
                SendKeys.SendWait("{ENTER}");
            }
            else
            {
                throw new FileNotFoundException(@"Demand 5 Metro App is not installed on your system.");
            }
        }

        public string Name
        {
            get { return "Demand 5"; }
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
