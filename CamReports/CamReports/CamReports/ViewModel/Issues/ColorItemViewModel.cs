using PropertyChanged;
using Xamarin.Forms;

namespace CamReports.ViewModel.Issues
{
    //[ImplementPropertyChanged]
    public class ColorItemViewModel
    {
        private static int _Id;
        public int Id { get; } = _Id++;
        public Color Color { get; set; }

        public ImageSource RadioButtonImage { get; set; }

        public static readonly ImageSource EmptyCircle = ImageSource.FromFile("radio_button_empty_circle.png");
        public static readonly ImageSource SelectedCircle = ImageSource.FromFile("radio_button_choosen_circle.png");

        public override string ToString()
        {
            return "";
        }
    }
}
