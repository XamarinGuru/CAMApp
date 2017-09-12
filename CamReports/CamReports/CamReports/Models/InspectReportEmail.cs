using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamReports.Models
{
    public class InspectReportEmail
    {
        [DataMember]
        public int ScheduleID { get; set; }

        [DataMember]
        public string SendTo { get; set; }

        [DataMember]
        public string SendFrom { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public string Body { get; set; }
    }
}
