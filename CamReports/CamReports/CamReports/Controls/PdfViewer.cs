using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CamReports.Controls
{
    public class PdfViewer : WebView
    {
        public static readonly BindableProperty UriProperty = BindableProperty.Create(propertyName: "Uri",
            returnType: typeof(string),
            declaringType: typeof(PdfViewer),
            defaultValue: default(string));

        public string Uri
        {
            get => (string)GetValue(UriProperty);
            set => SetValue(UriProperty, value);
        }
    }
}
