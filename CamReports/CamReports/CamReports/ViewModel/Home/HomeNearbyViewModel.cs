using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CamReports.Services;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using PropertyChanged;
using INavigationService = CamReports.Services.INavigationService;

namespace CamReports.ViewModel.Home
{
    //[ImplementPropertyChanged]
    public class HomeNearbyViewModel : BaseViewModel
    {
        public HomeNearbyViewModel(INavigationService navigationService) : base(navigationService)
        {
            _Reports = CAMDataSource._CAMDataSource.CAMSvcSchedExt.ToList();
            Reports = new ObservableCollection<CAMSvcSchedExt>(_Reports.OrderBy(item => item.Distance));
        }

        private readonly List<CAMSvcSchedExt> _Reports;
        public ObservableCollection<CAMSvcSchedExt> Reports { get; set; }

        private string _SearchQuery;

        public string SearchQuery
        {
            get => _SearchQuery;
            set
            {
                SearchSchedules(value);
                _SearchQuery = value;
            }
        }

        private void SearchSchedules(string query)
        {
            query = query.ToLower();
            Reports =
                new ObservableCollection<CAMSvcSchedExt>(
                    _Reports.Where(item => item.SiteName.ToLower().Contains(query)
                                           || item.RepairCode.ToLower().Contains(query)
                                           || item.FullAddress.ToLower().Contains(query)).OrderBy(item => item.Distance));
        }

        private CAMSvcSchedExt _SelectedReport;
        public CAMSvcSchedExt SelectedReport
        {
            get => _SelectedReport;
            set
            {
                _SelectedReport = value;
                if (value == null)
                    return;

                var reportViewModel = SimpleIoc.Default.GetInstance<ReportViewModel>();
                reportViewModel.Initialize(value);
                NavigationService.NavigateTo(ViewModelLocator.ReportPageKey, reportViewModel);
            }
        }
    }
}
