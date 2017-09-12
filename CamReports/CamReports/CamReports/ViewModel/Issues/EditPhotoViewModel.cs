using System.Collections.ObjectModel;
using System.Linq;
using CamReports.Models;
using CamReports.Services;
using GalaSoft.MvvmLight.Command;
using PropertyChanged;
using SkiaSharp;
using Xamarin.Forms;
using INavigationService = CamReports.Services.INavigationService;

namespace CamReports.ViewModel.Issues
{
    [ImplementPropertyChanged]
    public class EditPhotoViewModel : BaseViewModel
    {
        public EditPhotoViewModel(INavigationService navigationService) : base(navigationService)
        {
            Colors = new ObservableCollection<ColorItemViewModel>
            {
                new ColorItemViewModel{Color = Color.FromHex("#00BAFF"), RadioButtonImage = ColorItemViewModel.EmptyCircle},
                new ColorItemViewModel{Color = Color.FromHex("#0CFF01"), RadioButtonImage = ColorItemViewModel.EmptyCircle},
                new ColorItemViewModel{Color = Color.FromHex("#FF01FC"), RadioButtonImage = ColorItemViewModel.EmptyCircle},
                new ColorItemViewModel{Color = Color.FromHex("#EE1C25"), RadioButtonImage = ColorItemViewModel.SelectedCircle},
                new ColorItemViewModel{Color = Color.FromHex("#FF7E00"), RadioButtonImage = ColorItemViewModel.EmptyCircle},
                new ColorItemViewModel{Color = Color.FromHex("#FFFC00"), RadioButtonImage = ColorItemViewModel.EmptyCircle},
                new ColorItemViewModel{Color = Color.FromHex("#FFFFFF"), RadioButtonImage = ColorItemViewModel.EmptyCircle}
            };

            Tabs = new ObservableCollection<TabViewModel>
            {
                TabViewModel.GetInstance("arrow_icon.png", true),
                TabViewModel.GetInstance("circle_icon.png", false),
                TabViewModel.GetInstance("freehand_icon.png", false),
                TabViewModel.GetInstance("eraser_icon.png", false)
            };
            
            SelectedColor = Colors[3];
            SelectedTab = Tabs.First();
        }

        public void Initialize(CAMSvcSchedExt report, Issue issue)
        {
            ReportInfo = report;
            Issue = issue;
        }

        public CAMSvcSchedExt ReportInfo { get; set; }
        public Issue Issue { get; set; }

        #region Colors

        public ObservableCollection<ColorItemViewModel> Colors { get; set; }

        public ColorItemViewModel SelectedColor { get; set; }
        
        public RelayCommand<ColorItemViewModel> SelectColorCommand => new RelayCommand<ColorItemViewModel>(color =>
        {
            SelectedColor = color;
            color.RadioButtonImage = ColorItemViewModel.SelectedCircle;
            foreach (var colorViewModel in Colors)
            {
                if(colorViewModel.Id != color.Id)
                    colorViewModel.RadioButtonImage = ColorItemViewModel.EmptyCircle;
            }
        });

        #endregion

        #region Tabs

        public ObservableCollection<TabViewModel> Tabs { get; set; }

        public TabViewModel SelectedTab { get; set; }

        public RelayCommand<TabViewModel> SelectTabCommand => new RelayCommand<TabViewModel>(tab =>
        {
            SelectedTab = tab;
            tab.Color = TabViewModel.SelectedColor;
            foreach (var tabViewModel in Tabs)
            {
                if (tabViewModel.Id != tab.Id)
                    tabViewModel.Color = TabViewModel.UnselectedColor;
            }
        });

        #endregion

        #region Save image

        public RelayCommand<SKImage> SaveCommand => new RelayCommand<SKImage>(async (image) =>
        {
            SKEncodedImageFormat imageFormat = SKEncodedImageFormat.Jpeg;
            if(Issue.ImagePath.ToLower().EndsWith("jpg") || Issue.ImagePath.ToLower().EndsWith("jpeg"))
                imageFormat = SKEncodedImageFormat.Jpeg;
            if (Issue.ImagePath.EndsWith("png"))
                imageFormat = SKEncodedImageFormat.Png;
            if (Issue.ImagePath.EndsWith("bmp"))
                imageFormat = SKEncodedImageFormat.Bmp;
            if (Issue.ImagePath.EndsWith("gif"))
                imageFormat = SKEncodedImageFormat.Gif;

            var fileService = DependencyService.Get<ISaveAndLoad>();
            using (var data = image.Encode(imageFormat, 80))
            {
                await fileService.SaveFileAsync(Issue.ImagePath, data.ToArray());
            }

            BackCommand.Execute(null);
        });

        #endregion
    }
}
