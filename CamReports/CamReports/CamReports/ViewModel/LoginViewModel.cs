using System.Threading.Tasks;
using CamReports.Services;
using CamReports.ViewModel.Home;
using CamReports.ViewModel.SendReport;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Plugin.Geolocator;
using PropertyChanged;
using Xamarin.Forms;
using INavigationService = CamReports.Services.INavigationService;

namespace CamReports.ViewModel
{
    [ImplementPropertyChanged]
    public class LoginViewModel : BaseViewModel
    {
        private Application _Application;
        private IContactPicker _ContactPicker;

        public LoginViewModel(INavigationService navigationService, Application
            application, IContactPicker contactPicker) : base(navigationService)
        {
            _ContactPicker = contactPicker;
            //_ContactPicker.ContactsReceived += ContactPicker_ContactsReceived;
            _Application = application;
        }

        public string Login { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError { get; set; }

        protected override async void OnLoad()
        {
            base.OnLoad();

            //_ContactPicker.PickContacts();
        }

        public RelayCommand LoginCommand => new RelayCommand(async () =>
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            {
                HasError = true;
                return;
            }

            IsInProgress = true;
            CAMUser data = null;

            TaskFactory taskFactory = new TaskFactory();
            await taskFactory.StartNew(async () =>
            {
                CAMDataSource ds = CAMDataSource.GetCurrData();

                data = await ds.AuthenticateUser(Login, Password);
                if (data.UniqueId == "-1" || data.UniqueId == "0")
                {
                    HasError = true;
                    IsInProgress = false;
                    return;
                }

                HasError = false;
                await ds.LoadData();


                Device.BeginInvokeOnMainThread(() =>
                {
                    IsInProgress = false;
                    if (data != null && data.UniqueId != "-1" && data.UniqueId != "0")
                    {
                        ErrorMessage = "";

                        (_Application as App).SetMainPage();
                        //NavigationService.NavigateTo(ViewModelLocator.HomePageKey,
                        //    SimpleIoc.Default.GetInstance<HomeViewModel>());
                    }
                });

            });
        });
    }
}