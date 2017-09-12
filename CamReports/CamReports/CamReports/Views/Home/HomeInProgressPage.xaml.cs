using System;
using CamReports.ViewModel;
using CamReports.ViewModel.Home;
using CamReports.ViewModel.Issues;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CamReports.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeInProgressPage : ContentPage
    {
        public HomeInProgressPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }

        private void HomeInProgressPage_OnAppearing(object sender, EventArgs e)
        {
            var viewModel = BindingContext as BaseViewModel;
            if (viewModel == null)
                return;

            viewModel.LoadedCommand.Execute(null);
        }
    }
}
