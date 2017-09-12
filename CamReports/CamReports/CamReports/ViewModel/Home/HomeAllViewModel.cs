using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CamReports.Services;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using PropertyChanged;
using INavigationService = CamReports.Services.INavigationService;

namespace CamReports.ViewModel.Home
{
    //[ImplementPropertyChanged]
    public class HomeAllViewModel : BaseViewModel
    {
        public HomeAllViewModel(INavigationService navigationService) : base(navigationService)
        {
            _Reports = CAMDataSource._CAMDataSource.CAMSvcSchedExt.ToList();
            Reports = new ObservableCollection<CAMSvcSchedExt>(_Reports);

            SearchCommand = new RelayCommand<string>((query) =>
            {
                
            });
        }

        private List<CAMSvcSchedExt> _Reports;
        public ObservableCollection<CAMSvcSchedExt> Reports { get; set; }

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

        public RelayCommand<string> SearchCommand { get; }

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
                                             || item.FullAddress.ToLower().Contains(query)
                                             || item.GetDayString.ToLower().Contains(query)));
        }

        public void FilterByCode(string code)
        {
            SelectedCode = CAMDataSource._CAMDataSource.CAMCodes.FirstOrDefault(item => item.RepairCode == code);
        }

        private CAMSvcRepair _SelectedCode;

        public CAMSvcRepair SelectedCode
        {
            get => _SelectedCode;
            set
            {
                _SelectedCode = value;
                FilterReportsByCode(value);
            }
        }

        public void FilterReportsByCode(CAMSvcRepair code)
        {
            if (code != null && code.RepairID != -1)
                Reports = new ObservableCollection<CAMSvcSchedExt>(_Reports.Where(item => item.RepairCode == code.RepairCode));
            else
                Reports = new ObservableCollection<CAMSvcSchedExt>(_Reports);
        }
    }
}
