using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Globalization;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using PCLStorage;
using Plugin.Geolocator;
using PropertyChanged;
using Xamarin.Forms;

namespace CamReports.Services
{
    [DataContract(Namespace = "")]
    public class CAMSvcSchedExt
    {
        [DataMember]
        public int RepairID { get; set; }
        [DataMember]
        public string RepairCode { get; set; }
        [DataMember]
        public int CustSiteID { get; set; }
        [DataMember]
        public string SiteName { get; set; }
        [DataMember]
        public int EmployeeID { get; set; }

        [DataMember]
        public string TechName { get; set; }

        [DataMember]
        public string ShortAddress { get; set; }

        [DataMember]
        public string FullAddress { get; set; }

        [DataMember]
        public double Lat { get; set; }

        [DataMember]
        public double Lng { get; set; }

        [DataMember]
        public int SunSched { get; set; }

        [DataMember]
        public int MonSched { get; set; }
        [DataMember]
        public int TueSched { get; set; }
        [DataMember]
        public int WedSched { get; set; }
        [DataMember]
        public int ThuSched { get; set; }
        [DataMember]
        public int FriSched { get; set; }
        [DataMember]
        public int SatSched { get; set; }
        [DataMember]
        public string ToSDate { get; set; }

        ////----from CAMSvcSchedRep----////

        [DataMember]
        public int ScheduleID { get; set; }
        [DataMember]
        public int SvcOrderID { get; set; }
        [DataMember]
        public string EndTime { get; set; }

        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string CustomerPhone { get; set; }
        [DataMember]
        public string CustomerEmail { get; set; }

        [DataMember]
        public int BranchID { get; set; }
        [DataMember]
        public string BranchName { get; set; }

        public double Distance { get; set; }

        [DataMember]
        public string AccountManager { get; set; }
        [DataMember]
        public string AreaManager { get; set; }
        [DataMember]
        public string BusDevManager { get; set; }

        public List<CodeToSchedule> CodeToSchedules { get; set; } = new List<CodeToSchedule>();

        public int GetCodeToSchedulesListHeight => CodeToSchedules?.Count * 28 ?? 0;

        public string GetMon
        {
            get
            {
                string toReturn = "";

                if (this.MonSched > 1)
                {
                    return "X";
                }

                return toReturn;

            }
        }

        public string GetTue
        {
            get
            {
                string toReturn = "";

                if (this.TueSched > 1)
                {
                    return "X";
                }

                return toReturn;

            }
        }

        public string GetWed
        {
            get
            {
                string toReturn = "";

                if (this.WedSched > 1)
                {
                    return "X";
                }

                return toReturn;

            }
        }

        public string GetThu
        {
            get
            {
                string toReturn = "";

                if (this.ThuSched > 1)
                {
                    return "X";
                }

                return toReturn;

            }
        }

        public string GetFri
        {
            get
            {
                string toReturn = "";

                if (this.FriSched > 1)
                {
                    return "X";
                }

                return toReturn;

            }
        }

        public string GetSat
        {
            get
            {
                string toReturn = "";

                if (this.SatSched > 1)
                {
                    return "X";
                }

                return toReturn;

            }
        }

        public string GetSun
        {
            get
            {
                string toReturn = "";

                if (this.SunSched > 1)
                {
                    return "X";
                }

                return toReturn;

            }
        }

        public string GetTOS
        {
            get
            {
                string toReturn = "";

                if (this.ToSDate.Length > 0)
                {
                    DateTime ts = Convert.ToDateTime(this.ToSDate);
                    toReturn = String.Format("{0:ddd} ", ts);

                    toReturn += String.Format("{0:dd}", ts);
                }

                return toReturn;

            }
        }

        public string GetDayString
        {
            get
            {
                string toReturn = "";

                if (this.MonSched > 1)
                    toReturn += "Mon ";

                if (this.TueSched > 1)
                    toReturn += "Tue ";

                if (this.WedSched > 1)
                    toReturn += "Wed ";

                if (this.ThuSched > 1)
                    toReturn += "Thu ";

                if (this.FriSched > 1)
                    toReturn += "Fri ";

                if (this.SatSched > 1)
                    toReturn += "Sat ";

                if (this.SunSched > 1)
                    toReturn += "Sun ";

                if (toReturn.Length > 0)
                    toReturn = toReturn.Substring(0, toReturn.Length - 1);

                return toReturn;
            }
        }

        public CAMSvcSchedExt()
        {
            RepairID = 0;
            RepairCode = "";
            CustSiteID = 0;
            SiteName = "";
            EmployeeID = 0;
            TechName = "";
            ShortAddress = "";
            FullAddress = "";
            Lat = 0;
            Lng = 0;
            SunSched = 0;
            MonSched = 0;
            TueSched = 0;
            WedSched = 0;
            ThuSched = 0;
            FriSched = 0;
            SatSched = 0;
            ToSDate = "";
            AccountManager = "";
            AreaManager = "";
            ////----from CAMSvcSchedRep----////
            ScheduleID = 0;
            SvcOrderID = 0;
            EndTime = "";
            CompanyName = "";
            CustomerName = "";
            CustomerPhone = "";
            CustomerEmail = "";
            BranchID = 0;
            BranchName = "";
            Distance = 99999999999999.00;
        }
    }

    [ImplementPropertyChanged]
    public class CodeToSchedule
    {
        public string Code { get; set; }
        public string DaysString { get; set; }
    }

    public class CAMSvcIssue
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string ImageName { get; set; }

        /// <summary>
        /// Base64 encoded image file
        /// </summary>
        [DataMember]
        public string Image { get; set; }


    }


    public class CAMSvcInspectReportEmail
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

        public CAMSvcInspectReportEmail()
        {
            ScheduleID = 0;
            SendFrom = "";
            SendTo = "";//semicolon delimited string of destination email addresses comes with status code 4.
            Subject = "";
            Body = "";
        }
    }


    public class CAMSvcInspectReportRequest
    {
        [DataMember]
        public int ScheduleID { get; set; }

        [DataMember]
        public string UserCreated { get; set; }
        
        [DataMember]
        public List<CAMSvcInspectReportCheckList> CAMSvcInspectReportCheckLists { get; set; }

        [DataMember]
        public List<CAMSvcInspectReportImage> CAMSvcInspectReportImages { get; set; }

        public CAMSvcInspectReportRequest()
        {
            ScheduleID = 0;
            CAMSvcInspectReportCheckLists = new List<CAMSvcInspectReportCheckList>();
            CAMSvcInspectReportImages = new List<CAMSvcInspectReportImage>();
        }
    }

    public class CAMSvcCheckList
    {
        [DataMember]
        public int SubItemDetailID { get; set; }

        [DataMember]
        public int RepairID { get; set; }

        [DataMember]
        public string FieldType { get; set; }
        [DataMember]
        public string FieldCaption { get; set; }
        [DataMember]
        public string FieldValueList { get; set; }

        public CAMSvcCheckList()
        {
            SubItemDetailID = 0;
            RepairID = 0;
            FieldType = "";
            FieldCaption = "";
            FieldValueList = "";

        }
    }


    public class CAMSvcInspectReportCheckList
    {
        [DataMember]
        public int SubItemDetailID { get; set; }

        [DataMember]
        public int RepairID { get; set; }



        public CAMSvcInspectReportCheckList()
        {
            SubItemDetailID = 0;

            RepairID = 0;

        }
    }


    public class CAMSvcInspectReportImage
    {
        public DateTime ImageDateTime { get; set; }

        public string ImageName { get; set; }

        public string ImageDescription { get; set; }
        //public string Title { get; set; }

        public byte[] ImageData { get; set; }
        public CAMSvcInspectReportImage()
        {
            ImageDateTime = DateTime.MinValue;
            ImageName = "";
            ImageDescription = "";
            ImageData = null;
        }
    }


    public class CAMInProgressReport
    {
        [DataMember]
        public int ScheduleID { get; set; }
        [DataMember]
        public int SvcOrderID { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public int RepairID { get; set; }
        [DataMember]
        public string RepairCode { get; set; }
        [DataMember]
        public int CustSiteID { get; set; }
        [DataMember]
        public string SiteName { get; set; }
        [DataMember]
        public string ShortAddress { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public string SiteCity { get; set; }
        [DataMember]
        public string SiteZip { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string CustomerPhone { get; set; }
        [DataMember]
        public string CustomerEmail { get; set; }

        [DataMember]
        public string SubjectLine { get; set; }
        [DataMember]
        public string Body { get; set; }
        [DataMember]
        public string CC { get; set; }
        [DataMember]
        public string pdf { get; set; }



        [DataMember]
        public string[] Checkspeclst { get; set; }

        [DataMember]
        public string[] Checksotherlst { get; set; }

        [DataMember]
        public List<Image> Images { get; set; } = new List<Image>();

        [DataMember]
        public string notes { get; set; }

        public string Status
        {
            get
            {
                return "Assigned TO DO";
            }
        }

        public string LongerDate
        {
            get
            {
                string pattern = CultureInfo.CurrentCulture.DateTimeFormat.MonthDayPattern;
                pattern = pattern.Replace("MMMM", "MMM");

                DateTime ts = Convert.ToDateTime(this.EndTime);
                string formatted = ts.ToString(pattern);

                string toReturn = String.Format("{0:ddd}", ts);

                toReturn = toReturn + ", " + formatted + ", " + ts.Year.ToString();

                return toReturn;
            }
        }

        public CAMInProgressReport() { }

        public CAMInProgressReport(CAMSvcSchedExt reportInfo)
        {
            ScheduleID = reportInfo.ScheduleID;
            SvcOrderID = reportInfo.SvcOrderID;
            EndTime = reportInfo.EndTime;
            RepairID = reportInfo.RepairID;
            RepairCode = reportInfo.RepairCode;
            CustSiteID = reportInfo.CustSiteID;
            SiteName = reportInfo.SiteName;
            ShortAddress = reportInfo.ShortAddress;
            FullAddress = reportInfo.FullAddress;

            CompanyName = reportInfo.CompanyName;
            CustomerName = reportInfo.CustomerName;
            CustomerPhone = reportInfo.CustomerPhone;
            CustomerEmail = reportInfo.CustomerEmail;
        }

        [ImplementPropertyChanged]
        public class Image
        {
            public string ImagePath { get; set; }
            public string ImageName { get; set; }
            public string ImageDescription { get; set; }
        }
    }

    public class CAMSvcSchedEmpGroup
    {
        public string SiteName { get; set; }
        public string FullAddress { get; set; }
        public List<CAMSvcSchedEmp> Children { get; set; }
    }

    [ImplementPropertyChanged]
    public class CAMSvcSchedEmp
    {
        public int ScheduleID { get; set; }
        public int SvcOrderID { get; set; }
        public string EndTime { get; set; }
        public int RepairID { get; set; }
        public string RepairCode { get; set; }
        public int CustSiteID { get; set; }
        public string SiteName { get; set; }
        public string ShortAddress { get; set; }
        public string FullAddress { get; set; }
        public string SiteCity { get; set; }
        public string SiteZip { get; set; }
        public string CompanyName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }

        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public string AccountManager { get; set; }
        public string AreaManager { get; set; }

        public double Distance
        {
            get
            {
                try
                {
                    var camdatasource = CAMDataSource.GetCurrData();

                    var found = camdatasource.CAMSvcSchedExt.Where(u => u.CustSiteID == this.CustSiteID).FirstOrDefault();

                    return found.Distance;
                }
                catch
                {
                    return 99999999999999.00;
                }
            }
        }

        public string Status
        {
            get
            {
                return "";
            }
        }

        public string TechName
        {
            get
            {
                return "";
            }
        }


        public string ShortDate
        {
            get
            {
                string pattern = CultureInfo.CurrentCulture.DateTimeFormat.MonthDayPattern;
                pattern = pattern.Replace("MMMM", "MMM");

                DateTime ts = Convert.ToDateTime(this.EndTime);
                string formatted = ts.ToString(pattern);

                string toReturn = String.Format("{0:ddd}", ts);

                toReturn = toReturn + ", " + formatted;

                return toReturn;
            }
        }

        public string LongerDate
        {
            get
            {
                string pattern = CultureInfo.CurrentCulture.DateTimeFormat.MonthDayPattern;
                pattern = pattern.Replace("MMMM", "MMM");

                DateTime ts = Convert.ToDateTime(this.EndTime);
                string formatted = ts.ToString(pattern);

                string toReturn = String.Format("{0:ddd}", ts);

                toReturn = toReturn + ", " + formatted + ", " + ts.Year.ToString();

                return toReturn;
            }
        }

        public string FormatSvcOrderID
        {
            get
            {
                return "Order# " + this.SvcOrderID.ToString();
            }
        }

        public CAMSvcSchedEmp()
        {
            ScheduleID = 0;
            SvcOrderID = 0;
            EndTime = "";
            RepairID = 0;
            CustSiteID = 0;
            SiteName = "";
            ShortAddress = "";
            FullAddress = "";
            CompanyName = "";
            CustomerName = "";
            CustomerPhone = "";
            CustomerEmail = "";
            EmployeeID = 0;
            EmployeeName = "";
            BranchID = 0;
            BranchName = "";
            AccountManager = "";
            AreaManager = "";
        }
    }


    public class CAMSvcSched
    {
        public int EmployeeID { get; set; }
        public string ShortAddress { get; set; }
        public string FullAddress { get; set; }

        public int RepairID { get; set; }
        public string RepairCode { get; set; }
        public int CustSiteID { get; set; }
        public string SiteName { get; set; }
        public string SiteAddress { get; set; }
        public int SunSched { get; set; }

        public int MonSched { get; set; }
        public int TueSched { get; set; }
        public int WedSched { get; set; }
        public int ThuSched { get; set; }
        public int FriSched { get; set; }
        public int SatSched { get; set; }
        public string ToSDate { get; set; }

        public string TechName { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public double Distance { get; set; }

        public string GetMon
        {
            get
            {
                string toReturn = "";

                if (this.MonSched > 1)
                {
                    return "X";
                }

                return toReturn;

            }
        }

        public string GetTue
        {
            get
            {
                string toReturn = "";

                if (this.TueSched > 1)
                {
                    return "X";
                }

                return toReturn;

            }
        }

        public string GetWed
        {
            get
            {
                string toReturn = "";

                if (this.WedSched > 1)
                {
                    return "X";
                }

                return toReturn;

            }
        }

        public string GetThu
        {
            get
            {
                string toReturn = "";

                if (this.ThuSched > 1)
                {
                    return "X";
                }

                return toReturn;

            }
        }

        public string GetFri
        {
            get
            {
                string toReturn = "";

                if (this.FriSched > 1)
                {
                    return "X";
                }

                return toReturn;

            }
        }

        public string GetSat
        {
            get
            {
                string toReturn = "";

                if (this.SatSched > 1)
                {
                    return "X";
                }

                return toReturn;

            }
        }

        public string GetSun
        {
            get
            {
                string toReturn = "";

                if (this.SunSched > 1)
                {
                    return "X";
                }

                return toReturn;

            }
        }

        public string GetTOS
        {
            get
            {
                string toReturn = "";

                if (this.ToSDate.Length > 0)
                {
                    DateTime ts = Convert.ToDateTime(this.ToSDate);
                    toReturn = String.Format("{0:ddd} ", ts);

                    toReturn += String.Format("{0:dd}", ts);
                }

                return toReturn;

            }
        }

        public string GetDayString
        {
            get
            {
                string toReturn = "";

                if (this.MonSched > 1)
                {
                    toReturn += "Mon ";
                }

                if (this.TueSched > 1)
                {
                    toReturn += "Tue ";
                }

                if (this.WedSched > 1)
                {
                    toReturn += "Wed ";
                }

                if (this.ThuSched > 1)
                {
                    toReturn += "Thu ";
                }

                if (this.FriSched > 1)
                {
                    toReturn += "Fri ";
                }

                if (this.SatSched > 1)
                {
                    toReturn += "Sat ";
                }

                if (this.SunSched > 1)
                {
                    toReturn += "Sun ";
                }

                return toReturn;

            }
        }

        public CAMSvcSched()
        {
            RepairID = 0;
            CustSiteID = 0;
            SiteName = "";
            SiteAddress = "";
            SunSched = 0;
            MonSched = 0;
            TueSched = 0;
            WedSched = 0;
            ThuSched = 0;
            FriSched = 0;
            SatSched = 0;
            ToSDate = "";
            TechName = "";
            Lat = 0;
            Lng = 0;
            EmployeeID = 0;
            ShortAddress = "";
            FullAddress = "";
            Distance = 99999999999999.00;

        }


    }


    public class CAMSvcRepair
    {
        public int RepairID { get; set; }
        public string RepairCode { get; set; }

        public CAMSvcRepair()
        {
            RepairID = 0;
            RepairCode = "";
        }

    }


    public class CAMUser
    {
        public CAMUser(String uniqueId, String fullName, String emailAddress, String username, String phoneno)
        {
            this.UniqueId = uniqueId;
            this.FullName = fullName;
            this.EmailAddress = emailAddress;
            this.Username = username;
            this.PhoneNo = phoneno;

        }

        public string UniqueId { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string PhoneNo { get; set; }



        public override string ToString()
        {
            return this.FullName;
        }
    }

    public sealed class CAMDataSource
    {
        public static CAMDataSource _CAMDataSource = new CAMDataSource();

        public IList<CAMSvcRepair> _CAMCodes = new List<CAMSvcRepair>();
        public CAMUser _CAMUser = new CAMUser("-1", "", "", "", "");
        public IList<CAMSvcSched> _CAMSchedules = new List<CAMSvcSched>();

        public IList<CAMInProgressReport> _CAMInProgressReports = new List<CAMInProgressReport>();
        public IList<CAMSvcCheckList> _CAMSvcCheckList = new List<CAMSvcCheckList>();
        public IList<CAMSvcSchedExt> _CAMSvcSchedExt = new List<CAMSvcSchedExt>();

        public CAMDataSource()
        {
            CAMSvcSchedEmp = new List<CAMSvcSchedEmp>();
        }

        public CAMUser CAMUser
        {
            get { return this._CAMUser; }
        }

        public IList<CAMSvcRepair> CAMCodes
        {
            get { return this._CAMCodes; }
        }

        public IList<CAMSvcSched> CAMSchedules
        {
            get { return this._CAMSchedules; }
        }

        public IList<CAMSvcSchedEmp> CAMSvcSchedEmp { get; set; }

        public IList<CAMInProgressReport> CAMInProgressReports
        {
            get { return this._CAMInProgressReports; }
        }

        public IList<CAMSvcCheckList> CAMSvcCheckList
        {
            get { return this._CAMSvcCheckList; }
        }

        public IList<CAMSvcSchedExt> CAMSvcSchedExt
        {
            get { return this._CAMSvcSchedExt; }
        }

        public async Task<CAMUser> AuthenticateUser(String username, String password)
        {
            this.CAMUser.UniqueId = "-1";

            string getUri = "https://apps.camservices.com/CAMGateway/CAMServices.svc/CAMUserAuthGetV11/" + Uri.EscapeDataString(username) + "/" + Uri.EscapeDataString(password);
            HttpWebRequest request =
                (HttpWebRequest)HttpWebRequest.Create(getUri);
            //request.ContinueTimeout = 5000;

            try
            {
                var response = await request.GetResponseAsync();
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                string responseString = streamRead.ReadToEnd();

                var jsonObject = JObject.Parse(responseString);
                
                if (Convert.ToBoolean(jsonObject["isAuth"].ToString()))
                {
                    this.CAMUser.UniqueId = jsonObject["EmployeeID"].ToString();
                    this.CAMUser.Username = jsonObject["UserName"].ToString();
                    this.CAMUser.FullName = jsonObject["FullName"].ToString();
                    this.CAMUser.PhoneNo = jsonObject["PhoneNo"].ToString();
                }
                else
                {
                    this.CAMUser.UniqueId = "0";
                    this.CAMUser.Username = "test";
                    this.CAMUser.FullName = "test";
                    this.CAMUser.PhoneNo = "test";
                }

                /*
                string content = String.Empty;

                List<CAMUserAuth> usrs;
                var jsonSerializer = new DataContractJsonSerializer(typeof(List<CAMUserAuth>));

                usrs = (List<CAMUserAuth>)jsonSerializer.ReadObject(streamResponse);

                foreach (var usr in usrs)
                {

                }
                */


                streamResponse.Dispose();
                streamRead.Dispose();

                response.Dispose();
            }
            catch (WebException e)
            {

            }

            return _CAMDataSource.CAMUser;
        }

        private Dictionary<string, TimeSpan> log = new Dictionary<string, TimeSpan>();
        private DateTime startDateTime;

        public bool IsDataLoading { get; set; }

        public async Task LoadData()
        {
            //
            startDateTime = DateTime.Now;
            //log.Add("Start login", TimeSpan.Zero);

            IsDataLoading = true;

            await ScheduleNewCallback();
            await GetReports();
            await RepairsGet();


            //load the data from device

#if WINDOWS_APP

            //await GetSchedules();

#endif

            IsDataLoading = false;
            await deserializeJsonAsync();
        }

        public async Task WriteJsonAsync()
        {
            var myReports = _CAMInProgressReports;

            var serializer = new DataContractJsonSerializer(typeof(List<CAMInProgressReport>));
            using(var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, myReports);
                stream.Seek(0, SeekOrigin.Begin);
                var streamReader = new StreamReader(stream);
                var content = streamReader.ReadToEnd();
                var saveAndLoad = DependencyService.Get<ISaveAndLoad>();
                await saveAndLoad.SaveTextAsync("data.json", content);
            }
        }

        private async Task deserializeJsonAsync()
        {
            try
            {
                List<CAMInProgressReport> myReports;
                var jsonSerializer = new DataContractJsonSerializer(typeof(List<CAMInProgressReport>));

                var content = await DependencyService.Get<ISaveAndLoad>().LoadTextAsync("data.json");
                var myStream = new MemoryStream(Encoding.UTF8.GetBytes(content));

                myReports = (List<CAMInProgressReport>)jsonSerializer.ReadObject(myStream);

                this._CAMInProgressReports = myReports;

                if (this._CAMInProgressReports == null)
                {
                    this._CAMInProgressReports = new List<CAMInProgressReport>();
                }

                myStream.Dispose();

            }
            catch
            { this._CAMInProgressReports = new List<CAMInProgressReport>(); }
        }

        public static CAMDataSource GetCurrData()
        {
            var data = _CAMDataSource;

            return data;
        }

        public async Task ScheduleNewCallback()
        {
            string getUri1 = "https://apps.camservices.com/CAMGateway/CAMServices.svc/CAMSvcSchedExtAllGetV11";
            HttpWebRequest request3i =
                (HttpWebRequest)HttpWebRequest.Create(getUri1);
            request3i.Proxy = null;

            /////////////////////////////////////////////////////////////
            var response = await request3i.GetResponseAsync();
            /////////////////////////////////////////////////////////////

            ////////// TEST /////////////
            //var response = "1";
            //var responseStream = new MemoryStream(Encoding.UTF8.GetBytes("[{\"BranchID\":1,\"BranchName\":\"Los Angeles\",\"CompanyName\":\"CVS Pharmacy, Inc.\",\"CustSiteID\":937,\"CustomerEmail\":\"rlbensinger@cvs.com\",\"CustomerName\":\"Robert Bensinger\",\"CustomerPhone\":\"(407) 4735195\",\"EmployeeID\":151,\"EmployeeName\":\"\",\"EndTime\":\"6\\/1\\/2017 12:00:00 AM\",\"FriSched\":0,\"FullAddress\":\"1001 Westwood Blvd., Westwood, CA 93063\",\"Lat\":34.062351,\"Lng\":-118.445714,\"MonSched\":0,\"RepairCode\":\"03 - JAN\",\"RepairID\":5,\"SatSched\":0,\"ScheduleID\":1104919,\"ShortAddress\":\"1001 Westwood Blvd., Westwood\",\"SiteName\":\"LACVS5828\",\"SunSched\":0,\"SvcOrderID\":134770,\"TechName\":\"0047 - Rocio Palacios\",\"ThuSched\":0,\"ToSDate\":\"Jun  1 2017 12:00AM\",\"TueSched\":0,\"WedSched\":0,\"AccountManager\":\"\",\"AreaManager\":\"JNAM01 rjimenez\"},{\"BranchID\":1,\"BranchName\":\"Los Angeles\",\"CompanyName\":\"Premier America FCU\",\"CustSiteID\":481,\"CustomerEmail\":\"mike.crosby@PremierAmerica.com\",\"CustomerName\":\"Mike Crosby\",\"CustomerPhone\":\"(818) 7724079\",\"EmployeeID\":337,\"EmployeeName\":\"\",\"EndTime\":\"6\\/1\\/2017 12:00:00 AM\",\"FriSched\":0,\"FullAddress\":\"27550 Newhall Ranch Rd. #203, Valencia, CA 91355\",\"Lat\":34.4363248,\"Lng\":-118.5633591,\"MonSched\":0,\"RepairCode\":\"03 - JAN\",\"RepairID\":5,\"SatSched\":0,\"ScheduleID\":1117211,\"ShortAddress\":\"27550 Newhall Ranch Rd. #203, Valencia\",\"SiteName\":\"LAPREMIER6\",\"SunSched\":0,\"SvcOrderID\":135675,\"TechName\":\"3018 - CVA Janitor Services\",\"ThuSched\":0,\"ToSDate\":\"Jun  1 2017 12:00AM\",\"TueSched\":0,\"WedSched\":0,\"AccountManager\":\"\",\"AreaManager\":\"JNAM02 mcobian\"}]"));
            //var testFile = await FileSystem.Current.GetFileFromPathAsync("test_data.json");
            //var responseStream = await testFile.OpenAsync(FileAccess.Read);
            //////////
            if (response != null)
            {
                try
                {
                    /////////////////////////////////////////////////////////////
                    var responseStream = response.GetResponseStream();
                    /////////////////////////////////////////////////////////////
                    var responseStreamReader = new StreamReader(responseStream);
                    responseStream = new MemoryStream(Encoding.UTF8.GetBytes(responseStreamReader.ReadToEnd()));
#if WINDOWS_APP
//var jsonObject = JArray.Parse(streamResponse.ReadToEnd());
		            var responseString = streamResponse.ReadToEnd();
		            var jsonObjects =
						Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Dictionary<string, object>>>(responseString);
		            //var jsonObjects = jsonObject.OfType<Dictionary<string, object>>();
#endif

#if WINDOWS_PHONE_APP
					var jsonObject = fastJSON.JSON.Parse(streamResponse.ReadToEnd()) as List<Object>;
					var jsonObjects = jsonObject.OfType<Dictionary<string, object>>();
#endif
                    var jsonSerializer = new DataContractJsonSerializer(typeof(List<CAMSvcSchedExt>));

                    var records = (List<CAMSvcSchedExt>) jsonSerializer.ReadObject(responseStream);
                    records = records.GroupBy(item => item.SiteName)
                        .Select(group =>
                        {
                            var groupedItem = group.FirstOrDefault();

                            if (groupedItem == null)
                                return null;

                            groupedItem.CodeToSchedules = new List<CodeToSchedule>();

                            foreach (var camSvcSchedExt in group)
                            {
                                groupedItem.CodeToSchedules.Add(new CodeToSchedule{Code = camSvcSchedExt.RepairCode, DaysString = camSvcSchedExt.GetDayString});
                            }

                            return groupedItem;
                        }).ToList();
                    //responseStream.Seek(0, SeekOrigin.Begin);
                    //var responseText = responseStreamReader.ReadToEnd();

                    //var jsonObject = fastJson.JSON.Parse(responseText) as List<Object>;
                    //var jsonObjects = jsonObject.OfType<Dictionary<string, object>>();

                    
                    GeoCoordinate pin1 = null;

                    try
                    {
                        var locator = CrossGeolocator.Current;
                        locator.DesiredAccuracy = 50;
                        var position = await locator.GetPositionAsync(30000);

                        if (position != null)
                            pin1 = new GeoCoordinate(position.Latitude, position.Longitude);
                    }
                    catch(Exception e) { }

                    double distance = 0;

                    foreach (var record in records)
                    {
                        if (pin1 != null)
                        {
                            GeoCoordinate pin2 = new GeoCoordinate(record.Lat, record.Lng);
                            distance = pin1.GetDistanceTo(pin2);
                        }
                        else
                            distance = 0;

                        record.Distance = distance;
                    }

                    this._CAMSvcSchedExt = records;
                    responseStreamReader.Dispose();
                    /////////////////////////////////////////////////////////////
                    //response.Dispose();
                    /////////////////////////////////////////////////////////////
                }
                catch (Exception e)
                {
                    
                }
                finally
                {
                }
            }

        }

        public bool IsReportsLoding { get; set; }

        public async Task GetReports()
        {
            string getReportsUri = "https://apps.camservices.com/CAMGateway/CAMServices.svc/CAMSvcSchedRepAllGet";
            HttpWebRequest request3 =
                (HttpWebRequest)HttpWebRequest.Create(getReportsUri);

            var response = await request3.GetResponseAsync();

            if (response != null)
            {
                try
                {
                    IsReportsLoding = true;

                    Stream streamResponse = response.GetResponseStream();

                    bool canRead = true;
                    byte[] responseBytes = new byte[20000000];
                    int position = 0;
                    while (canRead)
                    {
                        int length = streamResponse.Read(responseBytes, (int)position, 1000000);
                        position += length;
                        if (length == 0)
                            canRead = false;
                    }

                    //string content = Encoding.UTF8.GetString(responseBytes, 0, position);

                    List<CAMSvcSchedEmp> reports;
                    var jsonSerializer = new DataContractJsonSerializer(typeof(List<CAMSvcSchedEmp>));
                    var responseString = Encoding.UTF8.GetString(responseBytes, 0, responseBytes.Length);
                    reports = (List<CAMSvcSchedEmp>)jsonSerializer.ReadObject(new MemoryStream(responseBytes, 0, position));

                    this.CAMSvcSchedEmp = reports;

                    streamResponse.Dispose();

                    response.Dispose();

                }
                catch (WebException e)
                {
                    return;
                }
                catch (Exception e)
                {
                    return;
                }
                finally
                {
                    IsReportsLoding = false;
                }
            }

        }

        public async Task GetSchedules()
        {
            var getUri = "https://apps.camservices.com/CAMGateway/CAMServices.svc/CAMSvcSchedAllGet";
            HttpWebRequest request =
                (HttpWebRequest)HttpWebRequest.Create(getUri);

            var response = await request.GetResponseAsync();

            if (response != null)
            {
                try
                {
                    Stream streamResponse = response.GetResponseStream();

                    string content = String.Empty;

                    List<CAMSvcSched> schedules;
                    var jsonSerializer = new DataContractJsonSerializer(typeof(List<CAMSvcSched>));

                    schedules = (List<CAMSvcSched>)jsonSerializer.ReadObject(streamResponse);

                    this._CAMSchedules = schedules;

                    streamResponse.Dispose();

                    response.Dispose();

                    // allDone.Set();


                }
                catch (WebException e)
                {

                    return;
                }
            }

        }

        public async Task RepairsGet()
        {
            string getUri = "https://apps.camservices.com/CAMGateway/CAMServices.svc/CAMSvcRepairsGet";
            HttpWebRequest request =
                (HttpWebRequest)HttpWebRequest.Create(getUri);
            request.Proxy = null;
            /////////////////////////////////////////////////////////////
            var response = await request.GetResponseAsync();
            /////////////////////////////////////////////////////////////
            
            //var response = "1";
            //var streamResponse = new MemoryStream(Encoding.UTF8.GetBytes("[{\"RepairID\":1, \"RepairCode\":\"02 - SWP\"},{\"RepairID\":5, \"RepairCode\":\"03 - JAN\"}]"));

            if (response != null)
            {
                try
                {
                    /////////////////////////////////////////////////////////////
                    Stream streamResponse = response.GetResponseStream();
                    /////////////////////////////////////////////////////////////

                    string content = String.Empty;
                    //log.Add("Code deserialization start", DateTime.Now - startDateTime);
                    List<CAMSvcRepair> codes;
                    var jsonSerializer = new DataContractJsonSerializer(typeof(List<CAMSvcRepair>));

                    codes = (List<CAMSvcRepair>)jsonSerializer.ReadObject(streamResponse);
                    //log.Add("Code deserialization end", DateTime.Now - startDateTime);

                    this._CAMCodes = codes;

                    CAMSvcRepair anyoption = new CAMSvcRepair();
                    anyoption.RepairCode = "ANY";
                    anyoption.RepairID = -1;

                    this._CAMCodes.Insert(0, anyoption);

                    streamResponse.Dispose();

                    /////////////////////////////////////////////////////////////
                    //response.Dispose();
                    /////////////////////////////////////////////////////////////

                    //now load the checklist

                    foreach (CAMSvcRepair rpr in codes)
                    {
                        //string getCheckListUri = "http://apps.camservices.com/CAMGateway/CAMServices.svc/CAMSvcCheckListGet/" + rpr.RepairID;
                        //HttpWebRequest request3 =
                        //    (HttpWebRequest)HttpWebRequest.Create(getCheckListUri);

                        //ChecklistCallback(await request3.GetResponseAsync());
                    }


                }
                catch (WebException e)
                {
                    return;
                }
            }

        }

        public void ChecklistCallback(WebResponse response)
        {
            if (response != null)
            {
                try
                {
                    Stream streamResponse = response.GetResponseStream();

                    string content = String.Empty;

                    List<CAMSvcCheckList> list;
                    var jsonSerializer = new DataContractJsonSerializer(typeof(List<CAMSvcCheckList>));

                    list = (List<CAMSvcCheckList>)jsonSerializer.ReadObject(streamResponse);

                    foreach (CAMSvcCheckList item in list)
                    {
                        this.CAMSvcCheckList.Add(item);
                    }

                    streamResponse.Dispose();

                    response.Dispose();

                    // allDone.Set();


                }
                catch (WebException e)
                {
                    return;
                }
            }

        }
    }

}
