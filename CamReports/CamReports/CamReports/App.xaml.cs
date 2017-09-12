using System;
using CamReports.Services;
using CamReports.Services.Database;
using CamReports.ViewModel;
using CamReports.ViewModel.Home;
using CamReports.Views;
using CamReports.Views.Home;
using CamReports.Views.Issues;
using CamReports.Views.Report;
using CamReports.Views.SendReport;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Mvvm;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CamReports
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var navigationService = new NavigationService();
            navigationService.Configure(ViewModelLocator.LoginPageKey, typeof(LoginPage));
            navigationService.Configure(ViewModelLocator.HomePageKey, typeof(HomePage));
            navigationService.Configure(ViewModelLocator.ReportPageKey, typeof(ReportPage));
            navigationService.Configure(ViewModelLocator.IssuesPageKey, typeof(IssuesPage));
            navigationService.Configure(ViewModelLocator.EditIssuePageKey, typeof(EditIssuePage));
            navigationService.Configure(ViewModelLocator.EditPhotoPageKey, typeof(EditPhotoPage));
            navigationService.Configure(ViewModelLocator.SendReportPageKey, typeof(SendReportPage));
            navigationService.Configure(ViewModelLocator.PreviewReportPageKey, typeof(ReportPreviewPage));
            navigationService.Configure(ViewModelLocator.ContactsSearchPageKey, typeof(ContactsSearchPage));

            SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            SimpleIoc.Default.Register<Application>(() => this);

            var loginPage = new NavigationPage(new LoginPage { BindingContext = SimpleIoc.Default.GetInstance<LoginViewModel>() });
            NavigationPage.SetHasNavigationBar(loginPage, false);
            NavigationPage.SetHasBackButton(loginPage, false);
            
            var localDatabaseService = new LocalDatabaseService();
            localDatabaseService.Initialize();
            SimpleIoc.Default.Register(() => localDatabaseService);
            
            Current.MainPage = loginPage;
        }

        private static ViewModelLocator _locator;
        public static ViewModelLocator Locator => _locator ?? (_locator = new ViewModelLocator());
        private static MasterDetailPage _MainMasterDetailPage;

        public void SetMainPage()
        {
            var mainPage = new NavigationPage(new HomePage() { BindingContext = SimpleIoc.Default.GetInstance<HomeViewModel>() });
            Current.MainPage = mainPage;
            var navigationService = SimpleIoc.Default.GetInstance<INavigationService>() as NavigationService;
            navigationService.Initialize(mainPage);
        }

        public static void PushPageFrom3DTouch(Page page)
        {
            Application.Current.MainPage = _MainMasterDetailPage;

            //var styleBuilder = new StyleBuilder();
            //styleBuilder.InitGlobalStyles();

            var nav = _MainMasterDetailPage.Detail as NavigationPage;

            nav.PushAsync(page);
        }

        public static Page GetPage(Action<IViewModel, BaseView> initializer = null)
        {
            return ViewFactory.CreatePage(initializer) as Page;
        }
    }
}
