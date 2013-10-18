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
using Windows.Management.Deployment;
using System.Security.Principal;
using System.Linq;

namespace MediaBrowser.Plugins.Netflix
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

        private bool checkNetflixInstalled()
        {
            PackageManager packageManager = new PackageManager();

            IEnumerable<Windows.ApplicationModel.Package> packages = (IEnumerable<Windows.ApplicationModel.Package>)packageManager.FindPackagesForUser(WindowsIdentity.GetCurrent().User.ToString(), "4DF9E0F8.Netflix", "CN=52120C15-ACFA-47FC-A7E3-4974DBA79445");

            if (packages.Count() > 0)
            {
                return true;
            }
            return false;
        }

        private void LaunchProcess()
        {
            if (checkNetflixInstalled())
            {
                SendKeys.SendWait("^{ESC}");
                SendKeys.SendWait("netflix");
                SendKeys.SendWait("{ENTER}");
            }
            else
            {
                throw new FileNotFoundException(@"Netflix Metro App is not installed on your system.");
            }
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
