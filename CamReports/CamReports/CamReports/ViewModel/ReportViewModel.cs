using System.Linq;
using CamReports.Services;
using CamReports.ViewModel.Issues;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using INavigationService = CamReports.Services.INavigationService;

namespace CamReports.ViewModel
{
    public class ReportViewModel : BaseViewModel
    {
        public ReportViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public CAMSvcSchedExt ReportInfo { get; set; }
        public CAMSvcSchedEmp EmployeeInfo { get; set; }

        public void Initialize(CAMSvcSchedExt reportInfo)
        {
            ReportInfo = reportInfo;
            EmployeeInfo = CAMDataSource._CAMDataSource.CAMSvcSchedEmp.SingleOrDefault(d => d.ScheduleID == reportInfo.ScheduleID);
        }

        public RelayCommand CreateReportCommand => new RelayCommand(() =>
        {
            var issuesViewModel = SimpleIoc.Default.GetInstance<IssuesViewModel>();
            issuesViewModel.Initialize(ReportInfo);
            NavigationService.NavigateTo(ViewModelLocator.IssuesPageKey, issuesViewModel);
        });
    }
}