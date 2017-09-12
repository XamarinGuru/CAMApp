using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamReports.Services.Contacts
{
    public class ContactsReceivedEventArgs : EventArgs
    {
        public ContactsReceivedEventArgs(IEnumerable<Contact> contacts)
        {
            Contacts = contacts.ToList();
        }

        public List<Contact> Contacts;
    }
}
