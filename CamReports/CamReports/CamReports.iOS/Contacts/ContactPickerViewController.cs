using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContactsUI;
using Foundation;
using UIKit;

namespace CamReports.iOS.Contacts
{
    public class ContactPickerViewController : CNContactPickerViewController
    {
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            var appearing = Appearing;
            appearing?.Invoke(this, new EventArgs());
        }



        public event EventHandler Appearing;
    }
}