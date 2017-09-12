using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using PropertyChanged;
using Xamarin.Forms;

namespace CamReports.ViewModel.Issues
{
    //[ImplementPropertyChanged]
    public class TabViewModel
    {
        private static int _Id;
        public int Id { get; } = _Id++;
        
        public ImageSource TabImage { get; set; }

        public static readonly Color SelectedColor = Color.FromHex("#FF9500");
        public static readonly Color UnselectedColor = Color.FromHex("#95989A");

        public Color Color { get; set; }

        public static TabViewModel GetInstance(string tabImagePath, bool isSelected)
        {
            var tab = new TabViewModel
            {
                TabImage = ImageSource.FromFile(tabImagePath),
                Color = isSelected ? SelectedColor : UnselectedColor
            };
            return tab;
        }
    }
}
