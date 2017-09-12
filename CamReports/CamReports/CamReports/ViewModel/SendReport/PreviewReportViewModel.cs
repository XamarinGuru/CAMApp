using System.IO;
using System.Linq;
using CamReports.Services;
using GalaSoft.MvvmLight.Views;
using PropertyChanged;
using Xamarin.Forms;

namespace CamReports.ViewModel.SendReport
{
    [ImplementPropertyChanged]
    public class PreviewReportViewModel : BaseViewModel
    {
        public PreviewReportViewModel(Services.INavigationService navigationService) : base(navigationService)
        {
        }

        [AlsoNotifyFor("Image")]
        public string Uri { get; set; }

        public ImageSource Image
        {
            get
            {
                if (!string.IsNullOrEmpty(Uri))
                {
                    var fileService = DependencyService.Get<ISaveAndLoad>();
                    var data = fileService.LoadFile(Uri);
                    var stream = new MemoryStream(data);
                    var image = ImageSource.FromStream(() => stream);
                    return image;
                }

                return new FileImageSource();
            }
        }
    }
}
