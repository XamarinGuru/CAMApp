using System;
using System.Collections.Generic;
using System.Linq;
using CamReports.Services.Contacts;
using Contacts;
using ContactsUI;
using Foundation;

namespace CamReports.iOS.Contacts
{
    public class ContactPickerDelegate : CNContactPickerDelegate
    {
        public override void ContactPickerDidCancel(CNContactPickerViewController picker)
        {
            Console.WriteLine("User canceled picker");
        }

        public override void DidChange(NSKeyValueChange changeKind, NSIndexSet indexes, NSString forKey)
        {
            base.DidChange(changeKind, indexes, forKey);
        }

        public override void DidSelectContactProperty(CNContactPickerViewController picker, CNContactProperty contactProperty)
        {
            // Raise the contact property selected event
            //RaiseContactPropertySelected(contactProperty);
        }

        public override void DidSelectContacts(CNContactPickerViewController picker, CNContact[] contacts)
        {
            var emails = new List<string>();
            var phones = new List<string>();
            var contactsMap = new List<Contact>();

            foreach (var contact in contacts)
            {
                emails.AddRange(contact.EmailAddresses.Select(item => item.Value.ToString()));
                phones.AddRange(contact.PhoneNumbers.Select(item => item.Value.ToString()));
                contactsMap.Add(new Contact{Emails = emails, Phones = phones});
            }    
            var eventArgs = new ContactsReceivedEventArgs(contactsMap);
            var contactsReceived = ContactsReceived;
            contactsReceived?.Invoke(this, eventArgs);
        }

        public event EventHandler<ContactsReceivedEventArgs> ContactsReceived;
    }
}