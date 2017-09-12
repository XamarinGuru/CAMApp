using CamReports.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace CamReports.iOS.Contacts
{
    public class ForceTouchListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                ((ForceTouchListView)e.NewElement).NativeControl = Control;
            }
        }

        public static UITableView NativeTableViewForControl(ForceTouchListView forceTouchListView)
        {
            return forceTouchListView.NativeControl as UITableView;
        }
    }
}
