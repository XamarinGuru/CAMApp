using System.Collections.Generic;
using System.Linq;
using CamReports.Services;
using CamReports.ViewModel.SendReport;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using PropertyChanged;
using INavigationService = CamReports.Services.INavigationService;

namespace CamReports.ViewModel.Home
{
    //[ImplementPropertyChanged]
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel(INavigationService navigationService) : base(navigationService)
        {
            Codes = CAMDataSource._CAMDataSource.CAMCodes.Select(item => item.RepairCode).ToList();
        }

        public List<string> Codes { get; }

        public CAMDataSource Data { get; }= CAMDataSource._CAMDataSource;

        public HomeAllViewModel HomeAllViewModel { get; set; } = SimpleIoc.Default.GetInstance<HomeAllViewModel>();
        
        public HomeCodesViewModel HomeCodesViewModel { get; set; } = SimpleIoc.Default.GetInstance<HomeCodesViewModel>();

        public HomeNearbyViewModel HomeNearbyViewModel { get; set; } = SimpleIoc.Default.GetInstance<HomeNearbyViewModel>();

        public HomeInProgressViewModel HomeInProgressViewModel { get; set; } = SimpleIoc.Default.GetInstance<HomeInProgressViewModel>();

        protected override void OnLoad()
        {
            base.OnLoad();
            //var _ContactsSearchViewModel = SimpleIoc.Default.GetInstance<ContactsSearchViewModel>();
            //NavigationService.NavigateTo(ViewModelLocator.ContactsSearchPageKey, _ContactsSearchViewModel);
            HomeInProgressViewModel.Update();
            HomeInProgressViewModel.SelectedReport = null;
            HomeNearbyViewModel.SelectedReport = null;
            HomeAllViewModel.SelectedReport = null;
        }
    }
}