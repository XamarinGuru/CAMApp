using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace CamReports.Services.Contacts
{
    //[ImplementPropertyChanged]
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public List<string> Emails { get; set; }
        public List<string> Phones { get; set; }
    }
}
