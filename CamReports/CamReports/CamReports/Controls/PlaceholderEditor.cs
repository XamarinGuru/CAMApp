using System;
using Xamarin.Forms;

namespace CamReports.Controls
{
    public class PlaceholderEditor : Editor
    {
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create<PlaceholderEditor, string>(view => view.Placeholder, String.Empty);

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }
    }
}
