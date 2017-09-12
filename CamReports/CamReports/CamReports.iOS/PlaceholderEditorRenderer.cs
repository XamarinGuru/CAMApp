using CamReports.Controls;
using CamReports.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PlaceholderEditor), typeof(PlaceholderEditorRenderer))]
namespace CamReports.iOS
{
    public class PlaceholderEditorRenderer : EditorRenderer
    {
        private string Placeholder { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            var element = this.Element as PlaceholderEditor;

            if (Control != null && element != null)
            {
                Placeholder = element.Placeholder;
                Control.TextColor = UIColor.LightGray;
                Control.Text = Placeholder;

                Control.ShouldBeginEditing += (UITextView textView) =>
                {
                    if (textView.Text == Placeholder)
                    {
                        textView.Text = "";
                        textView.TextColor = UIColor.Black; // Text Color
                    }

                    return true;
                };

                Control.ShouldEndEditing += (UITextView textView) =>
                {
                    if (textView.Text == "")
                    {
                        textView.Text = Placeholder;
                        textView.TextColor = UIColor.LightGray; // Placeholder Color
                    }

                    return true;
                };
            }
        }
    }
}