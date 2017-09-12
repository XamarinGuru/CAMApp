using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CamReports.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CamReports.Views.Report
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportPage
    {
        public ReportPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }
    }
}