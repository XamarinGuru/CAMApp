using System;
using CamReports.ViewModel;
using CamReports.ViewModel.Home;
using Xamarin.Forms;

namespace CamReports.Views.Home
{
    public partial class HomePage
    {
        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            CurrentPageChanged += HomePage_CurrentPageChanged;
        }

        private async void HomePage_CurrentPageChanged(object sender, EventArgs e)
        {
            var navigationPage = CurrentPage as NavigationPage;
            
            if (navigationPage != null)
            {
                var codePageViewModel = navigationPage.CurrentPage.BindingContext as HomeCodesViewModel;

                if (codePageViewModel == null)
                    return;

                CurrentPage = Children[0];
                var ViewModel = BindingContext as HomeViewModel;
                var result = await CurrentPage.DisplayActionSheet("", "Cancel", null, ViewModel.Codes.ToArray());
                var homeAllViewModel = ViewModel.HomeAllViewModel;
                codePageViewModel.UpdateCodeIcon(result);
                homeAllViewModel.FilterByCode(result);
            }
        }

        private void HomePage_OnAppearing(object sender, EventArgs e)
        {
            var ViewModel = BindingContext as BaseViewModel;
            ViewModel.LoadedCommand.Execute(null);
        }
    }
}