using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CamReports.Helpers;
using CamReports.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace CamReports.iOS
{

    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            var filePath = Path.Combine(libFolder, filename);

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return filePath;
        }
    }
}