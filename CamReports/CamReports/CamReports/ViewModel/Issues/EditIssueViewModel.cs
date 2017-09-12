using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CamReports.Models;
using CamReports.Services;
using CamReports.Services.Database;
using ExifLib;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using PCLStorage;
using Plugin.ImageResizer;
using PropertyChanged;
using SkiaSharp;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using INavigationService = CamReports.Services.INavigationService;
using MediaFile = XLabs.Platform.Services.Media.MediaFile;

namespace CamReports.ViewModel.Issues
{
    //[ImplementPropertyChanged]
    public class EditIssueViewModel : BaseViewModel
    {
        public Issue Issue { get; set; }
        public CAMSvcSchedExt ReportInfo { get; private set; }
        public string Description { get; set; }

        public readonly List<string> ChangePhotoCommandNameList = new List<string>(){"Take Photo", "Choose Photo From Library"};

        private LocalDatabaseService _Database;
        private IMediaPicker _MediaPicker;

        public EditIssueViewModel(INavigationService navigationService, LocalDatabaseService database) : base(navigationService)
        {
            _Database = database;
            _MediaPicker = DependencyService.Get<IMediaPicker>();
            Description = "";
        }

        public void Initialize(CAMSvcSchedExt reportInfo, Issue issue)
        {
            ReportInfo = reportInfo;
            Issue = issue;
            Description = issue.Description;
        }

        public RelayCommand SaveIssueCommand => new RelayCommand(() =>
        {
            Issue.Description = Description;
            if(Issue.Id == 0)
                _Database.Issues.InsertIssue(Issue);
            else
                _Database.Issues.UpdateIssue(Issue);

            NavigationService.GoBack();
        });

        public RelayCommand EditPhotoCommand => new RelayCommand(() =>
        {
            if (Issue.ImagePath == null || Issue.Image == null)
                return;

            var editPhotoViewModel = SimpleIoc.Default.GetInstance<EditPhotoViewModel>();
            editPhotoViewModel.Initialize(ReportInfo, Issue);
            NavigationService.NavigateTo(ViewModelLocator.EditPhotoPageKey, editPhotoViewModel);
        });

        public new RelayCommand BackCommand => new RelayCommand(() =>
        {
            Description = "";
            base.BackCommand.Execute(null);
        });

        protected override void OnLoad()
        {
            base.OnLoad();

            // Trick to update issue photo
            var issue = Issue; Issue = null; Issue = issue;
            var description = Issue.Description;
            Issue.Description = Issue.Description + " ";
            Issue.Description = description;
        }

        #region Photo choosing

        public event EventHandler<EventArgs> ChangePhoto;

        public RelayCommand ChangePhotoCommand => new RelayCommand(() =>
        {
            var changePhoto = ChangePhoto;
            changePhoto?.Invoke(this, null);
        });

        public RelayCommand<string> TakePhotoCommand => new RelayCommand<string>( async(command) =>
        {
            Setup();
            await Task.Delay(500);
            if (command == ChangePhotoCommandNameList[0])
            {
                await _MediaPicker.TakePhotoAsync(new CameraMediaStorageOptions() { SaveMediaOnCapture = true })
                    .ContinueWith(HandlePhoto);
                //var mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { });
                //if (mediaFile == null)
                //    return;
                //HandlePhoto(mediaFile);
            }
            else if (command == ChangePhotoCommandNameList[1])
            {
                //var mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { });
                //if (mediaFile == null)
                //    return;
                //HandlePhoto(mediaFile);
                await _MediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions())
                    .ContinueWith(HandlePhoto);
            }
        });

        private async void HandlePhoto(Task<MediaFile> task)
        {
            if (task.IsCanceled || task.IsFaulted)
                return;

            var mediaFile = task.Result;
            mediaFile.Exif.Orientation = ExifOrientation.Undefined;
            var newFilePath = await CopyPhotoAsync(mediaFile);
            Issue.ImagePath = newFilePath;
        }

        private void Setup()
        {
            if (_MediaPicker != null)
            {
                return;
            }

            var device = Resolver.Resolve<IDevice>();

            ////RM: hack for working on windows phone? 
            _MediaPicker = DependencyService.Get<IMediaPicker>() ?? device.MediaPicker;
        }

        private async Task<string> CopyPhotoAsync(MediaFile mediaFile)
        {
            IFileSystem fileSystem = FileSystem.Current;
            IFolder rootFolder = fileSystem.LocalStorage;
            try
            {
                byte[] contentBytes;
                if (mediaFile.Exif.Orientation == ExifOrientation.TopLeft &&
                    mediaFile.Exif.Width > mediaFile.Exif.Height)
                {
                    contentBytes = Rotate(mediaFile);
                }
                else
                {
                    contentBytes = new byte[mediaFile.Source.Length];
                    mediaFile.Source.Read(contentBytes, 0, (int)mediaFile.Source.Length);
                }

                var fileService = DependencyService.Get<ISaveAndLoad>();
                //IFile pickedPhoto = await fileSystem.GetFileFromPathAsync(mediaFile.Path);
                var filename = Path.GetFileName(mediaFile.Exif.FileName);
                await fileService.SaveFileAsync(filename, contentBytes);
                return filename;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public byte[] Rotate(MediaFile imageFile)
        {
            using (var bitmap = SKBitmap.Decode(imageFile.Source))
            {
                var rotated = new SKBitmap(bitmap.Height, bitmap.Width);

                using (var surface = new SKCanvas(rotated))
                {
                    surface.Translate(rotated.Width, 0);
                    surface.RotateDegrees(90);
                    surface.DrawBitmap(bitmap, 0, 0);
                }

                SKEncodedImageFormat imageFormat = SKEncodedImageFormat.Jpeg;
                if (imageFile.Path.ToLower().EndsWith("jpg") || imageFile.Path.ToLower().EndsWith("jpeg"))
                    imageFormat = SKEncodedImageFormat.Jpeg;
                if (imageFile.Path.EndsWith("png"))
                    imageFormat = SKEncodedImageFormat.Png;
                if (imageFile.Path.EndsWith("bmp"))
                    imageFormat = SKEncodedImageFormat.Bmp;
                if (imageFile.Path.EndsWith("gif"))
                    imageFormat = SKEncodedImageFormat.Gif;

                var image = SKImage.FromBitmap(rotated);

                using (var data = image.Encode(imageFormat, 80))
                {
                    return data.ToArray();
                }
            }
        }

        #endregion

    }
}