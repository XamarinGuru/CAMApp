using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CamReports.Services;
using CamReports.Services.Database;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using PropertyChanged;
using INavigationService = CamReports.Services.INavigationService;

namespace CamReports.ViewModel.Home
{
    [ImplementPropertyChanged]
    public class HomeInProgressViewModel : BaseViewModel
    {
        private LocalDatabaseService _Database;

        public HomeInProgressViewModel(INavigationService navigationService, LocalDatabaseService database) : base(navigationService)
        {
            _Database = database;
            Update();
        }

        private List<CAMSvcSchedExt> _Reports;
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
                                             || item.FullAddress.ToLower().Contains(query)));
        }

        public void Update()
        {
            var scheduleIds = _Database.Issues.GetInProgressReportScheduleIds();
            _Reports = CAMDataSource._CAMDataSource.CAMSvcSchedExt.Where(item => scheduleIds.Contains(item.ScheduleID)).ToList();
            Reports = new ObservableCollection<CAMSvcSchedExt>(_Reports);
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
