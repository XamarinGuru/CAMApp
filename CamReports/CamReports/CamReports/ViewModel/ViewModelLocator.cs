using CamReports.ViewModel.Home;
using CamReports.ViewModel.Issues;
using CamReports.ViewModel.SendReport;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CamReports.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        public const string LoginPageKey = "LoginPage";
        public const string HomePageKey = "HomePage";
        public const string ReportPageKey = "ReportPage";
        public const string IssuesPageKey = "IssuesPage";
        public const string EditIssuePageKey = "EditIssuePage";
        public const string EditPhotoPageKey = "EditPhotoPage";
        public const string SendReportPageKey = "SendReportPage";
        public const string PreviewReportPageKey = "PreviewReportPage";
        public const string ContactsSearchPageKey = "ContactsSearchPage";

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<HomeAllViewModel>();
            SimpleIoc.Default.Register<HomeCodesViewModel>();
            SimpleIoc.Default.Register<HomeNearbyViewModel>();
            SimpleIoc.Default.Register<HomeInProgressViewModel>();
            SimpleIoc.Default.Register<ReportViewModel>();
            SimpleIoc.Default.Register<IssuesViewModel>();
            SimpleIoc.Default.Register<EditIssueViewModel>();
            SimpleIoc.Default.Register<EditPhotoViewModel>();
            SimpleIoc.Default.Register<SendReportViewModel>();
            SimpleIoc.Default.Register<PreviewReportViewModel>();
            SimpleIoc.Default.Register<ContactsSearchViewModel>();
        }

        public LoginViewModel LoginViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}