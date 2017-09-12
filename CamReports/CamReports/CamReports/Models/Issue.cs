using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using CamReports.Services;
using PropertyChanged;
using SQLite.Net.Attributes;
using Xamarin.Forms;

namespace CamReports.Models
{
    //[ImplementPropertyChanged]
    public class Issue
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [AlsoNotifyFor("Image")]
        public string ImagePath { get; set; }

        [Ignore]
        public ImageSource Image
        {
            get
            {
                if (!string.IsNullOrEmpty(ImagePath))
                {
                    var fileService = DependencyService.Get<ISaveAndLoad>();
                    var data = fileService.LoadFile(ImagePath);
                    var stream = new MemoryStream(data.ToArray());
                    var image = ImageSource.FromStream(() => stream);
                    return image;
                }

                return new FileImageSource();
            }
        }

        public string Title { get; set; }

        public string Description { get; set; }

        [Indexed]
        public int ScheduleId { get; set; }
    }
}
