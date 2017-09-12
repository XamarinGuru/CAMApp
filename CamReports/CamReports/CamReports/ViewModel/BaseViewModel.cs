using System;
using System.ComponentModel;
using CamReports.Events;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PropertyChanged;

namespace CamReports.ViewModel
{
    //[ImplementPropertyChanged]
    public abstract class BaseViewModel : ViewModelBase
    {
        protected Services.INavigationService NavigationService;

        protected BaseViewModel(Services.INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public bool IsInProgress { get; set; }

        public RelayCommand BackCommand => new RelayCommand(() =>
        {
            NavigationService.GoBack();
        });

        public RelayCommand LoadedCommand => new RelayCommand(OnLoad);

        protected virtual void OnLoad()
        {
            
        }
    }
}
