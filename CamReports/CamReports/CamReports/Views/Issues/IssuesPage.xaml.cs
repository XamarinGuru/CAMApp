using System;
using CamReports.ViewModel.Issues;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CamReports.Views.Issues
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IssuesPage : ContentPage
    {
        public IssuesPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }

        private void IssuesPage_OnAppearing(object sender, EventArgs e)
        {
            var viewModel = BindingContext as IssuesViewModel;
            if (viewModel != null)
            {
                viewModel.SelectedIssue = null;
                viewModel.LoadedCommand.Execute(null);
            }
        }
    }
}
