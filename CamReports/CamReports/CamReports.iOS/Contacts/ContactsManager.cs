using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CamReports.Services.Contacts;
using Contacts;
using Foundation;
using UIKit;

namespace CamReports.iOS.Contacts
{
    public class ContactsManager : IContactManager
    {
        private readonly Plugin.Contacts.AddressBook _book;

        private static IEnumerable<Contact> _contacts;

        public IEnumerable<Contact> GetContacts()
        {
            if (_contacts != null) return _contacts;

            var contacts = new List<Contact>();
            var fetchKeys = new[] { CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.Birthday };
            NSError error;
            using (var predicate = CNContact.GetPredicateForContacts(""))
            {
                using (var store = new CNContactStore())
                {
                    var cnContacts = store.GetUnifiedContacts(predicate, fetchKeys, out error);
                    if (error == null)
                    {
                        contacts = cnContacts.Select(contact => new Contact()
                            {
                                DisplayName = contact.GivenName + contact.FamilyName,
                                FirstName = contact.GivenName,
                                LastName = contact.FamilyName,
                                Phones = contact.PhoneNumbers.Select(phone => phone.Value.StringValue).ToList(),
                                Emails = contact.EmailAddresses.Select(email => email.Value.ToString()).ToList()
                            })
                            .ToList();
                        _contacts = (from c in contacts orderby c.FirstName select c).ToList();

                        return _contacts;
                    }
                    else
                    {
                        throw new Exception(error.ToString());
                    }
                }
            }

            
            return _contacts;
        }
    }
}