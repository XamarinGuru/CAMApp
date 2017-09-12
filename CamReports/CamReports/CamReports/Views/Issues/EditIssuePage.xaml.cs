using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CamReports.ViewModel.Issues;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CamReports.Views.Issues
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditIssuePage : ContentPage
    {
        public EditIssuePage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);

            Appearing += EditIssuePage_Appearing;
            Disappearing += EditIssuePage_Disappearing;
        }

        private void EditIssuePage_Disappearing(object sender, EventArgs e)
        {
            var viewModel = BindingContext as EditIssueViewModel;
            viewModel.ChangePhoto -= ChangePhoto;
        }

        private void EditIssuePage_Appearing(object sender, EventArgs e)
        {
            var viewModel = BindingContext as EditIssueViewModel;
            viewModel.ChangePhoto += ChangePhoto;
        }

        private async void ChangePhoto(object sender, EventArgs args)
        {
            var viewModel = BindingContext as EditIssueViewModel;
            var result = await DisplayActionSheet("", "Cancel", null, viewModel.ChangePhotoCommandNameList.ToArray());
            viewModel.TakePhotoCommand.Execute(result);
        }

        private void EditIssuePage_OnAppearing(object sender, EventArgs e)
        {
            var viewModel = BindingContext as EditIssueViewModel;
            viewModel?.LoadedCommand.Execute(null);
        }
    }
}
