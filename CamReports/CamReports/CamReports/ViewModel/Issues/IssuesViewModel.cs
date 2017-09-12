using System.Collections.ObjectModel;
using System.Linq;
using CamReports.Models;
using CamReports.Services;
using CamReports.Services.Database;
using CamReports.ViewModel.SendReport;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using PropertyChanged;
using Xamarin.Forms;
using INavigationService = CamReports.Services.INavigationService;

namespace CamReports.ViewModel.Issues
{
    //[ImplementPropertyChanged]
    public class IssuesViewModel : BaseViewModel
    {
        private LocalDatabaseService _Database;
        public IssuesViewModel(INavigationService navigationService, LocalDatabaseService database) : base(navigationService)
        {
            _Database = database;
        }

        public void Initialize(CAMSvcSchedExt report)
        {
            Issues?.Clear();
            ReportInfo = report;
        }

        private CAMSvcSchedExt _ReportInfo;
        public CAMSvcSchedExt ReportInfo
        {
            get => _ReportInfo;
            set
            {
                _ReportInfo = value;
                if (_ReportInfo == null)
                    return;

                UpdateIssues();
            }
        }

        public bool EmptyIssues { get; set; }

        public ObservableCollection<Issue> Issues { get; private set; }
        public RelayCommand CreateIssueCommand => new RelayCommand(() =>
        {
            var editIssueViewModel = SimpleIoc.Default.GetInstance<EditIssueViewModel>();
            editIssueViewModel.Initialize(ReportInfo, new Issue(){ScheduleId = ReportInfo.ScheduleID});
            NavigationService.NavigateTo(ViewModelLocator.EditIssuePageKey, editIssueViewModel);
        });

        public RelayCommand<Issue> DeleteCommand => new RelayCommand<Issue>(issue =>
        {
            var fileService = DependencyService.Get<ISaveAndLoad>();

            if (fileService.FileExists(issue.ImagePath))
            {
                fileService.Delete(issue.ImagePath);
            }

            _Database.Issues.DeleteIssue(issue);
            UpdateIssues();
        });

        private async void UpdateIssues()
        {
            Issues = new ObservableCollection<Issue>(_Database.Issues.GetIssues(_ReportInfo.ScheduleID));

            EmptyIssues = Issues.Count == 0;
            var fileService = DependencyService.Get<ISaveAndLoad>();

            foreach (var issue in Issues)
            {
                //var file = await FileSystem.Current.LocalStorage.GetFileAsync(issue.ImagePath);
                if (!string.IsNullOrEmpty(issue.ImagePath))
                {
                    if (!fileService.FileExists(issue.ImagePath))
                        return;
                }
            }

            var camdatasource = CAMDataSource.GetCurrData();
            var svc = ReportInfo;//camdatasource.CAMSvcSchedExt.SingleOrDefault(d => d.ScheduleID == ReportInfo.ScheduleID);

            CAMInProgressReport report = camdatasource.CAMInProgressReports.SingleOrDefault(d => d.ScheduleID == svc.ScheduleID);
            if (report == null)
            {
                report = new CAMInProgressReport();

                report.CompanyName = svc.CompanyName;
                report.CustomerEmail = svc.CustomerEmail;
                report.CustomerName = svc.CustomerName;
                report.CustomerPhone = svc.CustomerPhone;
                report.CustSiteID = svc.CustSiteID;
                report.EndTime = svc.EndTime;
                report.FullAddress = svc.FullAddress;
                report.RepairCode = svc.RepairCode;
                report.RepairID = svc.RepairID;
                report.ScheduleID = svc.ScheduleID;
                report.ShortAddress = svc.ShortAddress;
                //report.SiteCity = svc.;
                report.SiteName = svc.SiteName;
                //report.SiteZip = svc.SiteZip;
                report.SvcOrderID = svc.SvcOrderID;

                camdatasource.CAMInProgressReports.Add(report);
            }

            report.Images.Clear();
            foreach (var issue in Issues)
            {
                //var file = await FileSystem.Current.LocalStorage.GetFileAsync(issue.ImagePath);
                if (!string.IsNullOrEmpty(issue.ImagePath))
                {
                    if (!fileService.FileExists(issue.ImagePath))
                        return;

                    report.Images.Add(new CAMInProgressReport.Image() { ImagePath = issue.ImagePath, ImageDescription = issue.Description, ImageName = issue.Title });
                }
            }
            
            await camdatasource.WriteJsonAsync();
            //var isFileExists = await FileSystem.Current.LocalStorage.CheckExistsAsync(Issues.First().ImagePath);
            //isFileExists = await FileSystem.Current.RoamingStorage.CheckExistsAsync(Issues.First().ImagePath);
        }

        private Issue _SelectedIssue;
        public Issue SelectedIssue
        {
            get => _SelectedIssue;
            set
            {
                _SelectedIssue = value;

                if (_SelectedIssue == null)
                    return;

                var viewModel = SimpleIoc.Default.GetInstance<EditIssueViewModel>();
                
                viewModel.Initialize(ReportInfo, _SelectedIssue);
                NavigationService.NavigateTo(ViewModelLocator.EditIssuePageKey, viewModel);
            }
        }

        public RelayCommand ReviewCommand => new RelayCommand(() =>
        {
            var viewModel = SimpleIoc.Default.GetInstance<SendReportViewModel>();
            viewModel.ReportInfo = null;
            viewModel.ReportInfo = ReportInfo;
            viewModel.Initialize(ReportInfo, Issues);
            NavigationService.NavigateTo(ViewModelLocator.SendReportPageKey, viewModel);
        });

        protected override void OnLoad()
        {
            base.OnLoad();
            UpdateIssues();
        }
    }
}
