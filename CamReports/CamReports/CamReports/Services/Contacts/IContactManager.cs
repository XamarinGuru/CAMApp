using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamReports.Services.Contacts
{
    public interface IContactManager
    {
        IEnumerable<Contact> GetContacts();
    }
}
