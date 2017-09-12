using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CamReports.iOS;
using CamReports.Services;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveAndLoad))]

namespace CamReports.iOS
{
    public class SaveAndLoad : ISaveAndLoad
    {
        public static string DocumentsPath
        {
            get
            {
                var documentsDirUrl = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User).Last();
                return documentsDirUrl.Path;
            }
        }

        #region ISaveAndLoad implementation

        public async Task SaveTextAsync(string filename, string text)
        {
            string path = CreatePathToFile(filename);
            using (StreamWriter sw = File.CreateText(path))
                await sw.WriteAsync(text);
        }

        public async Task<string> LoadTextAsync(string filename)
        {
            string path = CreatePathToFile(filename);
            using (StreamReader sr = File.OpenText(path))
                return await sr.ReadToEndAsync();
        }

        public async Task SaveFileAsync(string filename, byte[] data)
        {
            string path = CreatePathToFile(filename);
            using (FileStream sw = File.Create(path))
                await sw.WriteAsync(data, 0, data.Length);
        }

        public byte[] LoadFile(string filename)
        {
            string path = CreatePathToFile(filename);
            if (!FileExists(path))
                return null;

            using (FileStream sr = File.OpenRead(path))
            {
                var data = new byte[sr.Length];
                var readed = sr.Read(data, 0, (int)sr.Length);
                return data;
            }
        }

        public bool FileExists(string filename)
        {
            return File.Exists(CreatePathToFile(filename));
        }

        public void Delete(string filename)
        {
            File.Delete(CreatePathToFile(filename));
        }

        #endregion

        static string CreatePathToFile(string fileName)
        {
            return Path.Combine(DocumentsPath, fileName);
        }
    }
}