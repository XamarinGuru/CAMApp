using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CamReports.Services;
using GalaSoft.MvvmLight.Views;
using PropertyChanged;
using Xamarin.Forms;
using INavigationService = CamReports.Services.INavigationService;

namespace CamReports.ViewModel.Home
{
    [ImplementPropertyChanged]
    public class HomeCodesViewModel : BaseViewModel
    {
        public HomeCodesViewModel(INavigationService navigationService) : base(navigationService)
        {
            Codes = new ObservableCollection<CAMSvcRepair>(CAMDataSource._CAMDataSource.CAMCodes);

            _Reports = CAMDataSource._CAMDataSource.CAMSvcSchedExt.ToList();
            Reports = new ObservableCollection<CAMSvcSchedExt>(_Reports);
            CodeIcon = ImageSource.FromFile("tab_code.png");
        }

        public ObservableCollection<CAMSvcRepair> Codes { get; }

        private readonly List<CAMSvcSchedExt> _Reports;
        public ObservableCollection<CAMSvcSchedExt> Reports { get; set; }

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

        private void FilterReportsByCode(CAMSvcRepair code)
        {
            if(code.RepairID != -1)
                Reports = new ObservableCollection<CAMSvcSchedExt>(_Reports.Where(item => item.RepairCode == code.RepairCode));
            else
                Reports = new ObservableCollection<CAMSvcSchedExt>(_Reports);
        }

        public ImageSource CodeIcon { get; set; }

        public void UpdateCodeIcon(string repairCode)
        {
            if (repairCode.ToLower().EndsWith("sc"))
                CodeIcon = ImageSource.FromFile("codes/sc_code.png");
            if (repairCode.ToLower().EndsWith("ls"))
                CodeIcon = ImageSource.FromFile("codes/ls_code.png");
            if (repairCode.ToLower().EndsWith("ls extra"))
                CodeIcon = ImageSource.FromFile("codes/lse_code.png");
            if (repairCode.ToLower().EndsWith("jan"))
                CodeIcon = ImageSource.FromFile("codes/jan_code.png");
            if (repairCode.ToLower().EndsWith("jan extra"))
                CodeIcon = ImageSource.FromFile("codes/jane_code.png");
            if (repairCode.ToLower().EndsWith("cnst"))
                CodeIcon = ImageSource.FromFile("codes/cnst_code.png");
            if (repairCode.ToLower().EndsWith("prt"))
                CodeIcon = ImageSource.FromFile("codes/prt_code.png");
            if (repairCode.ToLower().EndsWith("swp"))
                CodeIcon = ImageSource.FromFile("codes/swp_code.png");
            if (repairCode.ToLower().EndsWith("vendor"))
                CodeIcon = ImageSource.FromFile("codes/vendor_code.png");
            if (repairCode.ToLower().EndsWith("labor"))
                CodeIcon = ImageSource.FromFile("codes/labor_code.png");
            if (repairCode.ToLower().EndsWith("vendor bill"))
                CodeIcon = ImageSource.FromFile("codes/vb_code.png");
            if (repairCode.ToLower().EndsWith("any"))
                CodeIcon = ImageSource.FromFile("codes/all_code.png");
        }
        //public CAMDataSource Data { get; } = CAMDataSource._CAMDataSource;
    }
}