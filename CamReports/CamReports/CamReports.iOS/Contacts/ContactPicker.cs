using System;
using CamReports.Services;
using CamReports.Services.Contacts;
using ContactsUI;
using Foundation;
using UIKit;
using Xamarin.Forms.Xaml;

namespace CamReports.iOS.Contacts
{
    public class ContactPicker : IContactPicker
    {
        public void PickContacts()
        {
            // Create a new picker
            var picker = new ContactPickerViewController();
            
            picker.PredicateForEnablingContact = NSPredicate.FromValue(true); // make everything selectable

            // Respond to selection
            var contactPickerDelegate = new ContactPickerDelegate();
            picker.Delegate = contactPickerDelegate;
            contactPickerDelegate.ContactsReceived += (sender, args) =>
            {
                var contactsReceived = ContactsReceived;
                contactsReceived?.Invoke(this, args);
            };

            picker.Appearing += (sender, args) =>
            {
                //picker.NavigationController.HidesBarsWhenKeyboardAppears = false;
                //picker.NavigationItem.
            };
            
            picker.DismissViewController(false, () =>
            {
                
            });

            // Display picker
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(picker, true, null);
        }

        public event EventHandler<ContactsReceivedEventArgs> ContactsReceived;
    }
}
