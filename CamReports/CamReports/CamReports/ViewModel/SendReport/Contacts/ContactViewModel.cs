using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CamReports.Services.Contacts;

namespace CamReports.ViewModel.SendReport.Contacts
{
    public class ContactViewModel
    {
        public ContactViewModel(Contact contact)
        {
            _contact = contact;
        }

        public string Forename { get { return _contact.FirstName; } }

        public string Surname
        {
            get { return _contact.LastName ?? _contact.FirstName; }
        }

        public string Name { get { return _contact.DisplayName; } }

        public string PhoneNumber
        {
            get { return _contact.Phones.FirstOrDefault(); }
        }

        public string SortByCharacter
        {
            get
            {
                var str = _contact.LastName ?? _contact.FirstName;

                if (String.IsNullOrEmpty(str))
                    throw new Exception("The contact has no first or last name");

                return str[0].ToString().ToUpper();
            }
        }

        public object Contact { get { return _contact; } }

        private readonly Contact _contact;
    }
}
