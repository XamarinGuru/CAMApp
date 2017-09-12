using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CamReports.Services.Contacts;

namespace CamReports.Services
{
    public interface IContactPicker
    {
        void PickContacts();

        event EventHandler<ContactsReceivedEventArgs> ContactsReceived;
    }
}
