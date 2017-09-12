using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamReports.Services
{
    public interface ISaveAndLoad
    {
        Task SaveTextAsync(string filename, string text);
        Task<string> LoadTextAsync(string filename);

        Task SaveFileAsync(string filename, byte[] data);
        byte[] LoadFile(string filename);
        bool FileExists(string filename);
        void Delete(string filename);
    }
}
