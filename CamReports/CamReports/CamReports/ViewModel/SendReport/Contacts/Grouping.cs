using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamReports.ViewModel.SendReport.Contacts
{
    public class Grouping<TKey, TValue> : ObservableCollection<TValue>
    {
        public TKey Key { get; private set; }

        public Grouping(TKey key, IEnumerable<TValue> items)
        {
            Key = key;

            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}
