using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CamReports.Services;
using CamReports.Services.Contacts;
using CamReports.ViewModel.SendReport.Contacts;
using Plugin.Contacts;
using PropertyChanged;

namespace CamReports.ViewModel.SendReport
{
    [ImplementPropertyChanged]
    public class ContactsSearchViewModel : BaseViewModel
    {
        public ObservableCollection<Grouping<string, Contact>> Contacts;
        public ObservableCollection<Grouping<string, Contact>> FilteredContacts;
        private IContactManager _ContactManager;

        public ContactsSearchViewModel(INavigationService navigationService, IContactManager contactManager) : base(navigationService)
        {
            _ContactManager = contactManager;
        }

        protected  override async void OnLoad()
        {
            base.OnLoad();
            LoadContacts();
        }

        private void LoadContacts()
        {
            Contacts = new ObservableCollection<Grouping<string, Contact>>();
            FilteredContacts = new ObservableCollection<Grouping<string, Contact>>();

            CrossContacts.Current.PreferContactAggregation = false;
            var hasPermission = CrossContacts.Current.RequestPermission().Result;

            if (hasPermission)
            {
                // First off convert all contacts into ContactViewModels...
                //var vms = CrossContacts.Current.Contacts.Where(c => Matches(c))
                //    .Select(c => new ContactViewModel(c)).ToList();
                var vms = _ContactManager.GetContacts();
                // And then setup the contact list
                var grouped = from contact in vms
                    orderby contact.LastName
                    group contact by (contact.FirstName ?? contact.LastName)[0].ToString().ToUpper() into contactGroup
                    select new Grouping<string, Contact>(contactGroup.Key, contactGroup);

                foreach (var g in grouped)
                {
                    Contacts.Add(g);
                    FilteredContacts.Add(g);
                }
            }
        }

        private string _SearchQuery;
        public string SearchQuery
        {
            get { return _SearchQuery; }
            set
            {
                _SearchQuery = value;
                FilterContacts(_SearchQuery);
            }
        }

        private void FilterContacts(string filter)
        {
            FilteredContacts.Clear();

            if (string.IsNullOrEmpty(filter))
            {
                foreach (var g in this.Contacts)
                    FilteredContacts.Add(g);
            }
            else
            {
                // Need to do some filtering
                foreach (var g in this.Contacts)
                {
                    var matches = g.Where(vm => vm.DisplayName.Contains(filter));

                    if (matches.Any())
                    {
                        FilteredContacts.Add(new Grouping<string, Contact>(g.Key, matches));
                    }
                }
            }
        }

        private Contact _SelectedContact;
        public Contact SelectedContact
        {
            get { return _SelectedContact; }
            set
            {
                _SelectedContact = value;
                BackCommand.Execute(null);
            }
        }
    }
}