using System.IO;
using System.Net;
using CamReports.Controls;
using CamReports.iOS.Renderers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PdfViewer), typeof(PdfViewerRenderer))]
namespace CamReports.iOS.Renderers
{
    public class PdfViewerRenderer : ViewRenderer<PdfViewer, UIWebView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<PdfViewer> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetNativeControl(new UIWebView());
            }
            if (e.OldElement != null)
            {
                // Cleanup
            }
            if (e.NewElement != null)
            {
                var customWebView = Element;
                string fileName = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", WebUtility.UrlEncode(customWebView.Uri)));
                Control.LoadRequest(new NSUrlRequest(new NSUrl(fileName, false)));
                Control.ScalesPageToFit = true;
            }
        }
    }
}