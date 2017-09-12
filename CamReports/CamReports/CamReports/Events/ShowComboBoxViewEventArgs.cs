using System;
using System.Collections.Generic;

namespace CamReports.Events
{
    public class ShowComboBoxViewEventArgs : EventArgs
    {
        public ShowComboBoxViewEventArgs(IEnumerable<string> options, string title)
        {
            Options = new List<string>(options);
            Title = title;
        }

        public List<string> Options { get; set; }

        public string Title { get; set; }
    }
}
