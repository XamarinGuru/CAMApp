using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using CamReports.Models;
using CamReports.Services;
using CamReports.Services.Database;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Newtonsoft.Json;
using Plugin.ImageResizer;
using PropertyChanged;
using Xamarin.Forms;
using INavigationService = CamReports.Services.INavigationService;

namespace CamReports.ViewModel.SendReport
{
    [ImplementPropertyChanged]
    public class SendReportViewModel : BaseViewModel
    {
        private List<Issue> _Issues;
        private readonly IContactPicker _ContactPicker;
        public LocalDatabaseService _Database;
        public SendReportViewModel(INavigationService navigationService, IContactPicker contactPicker, LocalDatabaseService database)
            : base(navigationService)
        {
            _ContactPicker = contactPicker;
            _ContactPicker.ContactsReceived += ContactPicker_ContactsReceived;
            _Database = database;
        }

        public void Initialize(CAMSvcSchedExt report, IEnumerable<Issue> issues)
        {
            ReportInfo = report;
            _Issues = issues.ToList();
        }

        #region Emails

        private void ContactPicker_ContactsReceived(object sender, Services.Contacts.ContactsReceivedEventArgs args)
        {
            foreach (var contact in args.Contacts)
            {
                foreach (var contactEmail in contact.Emails)
                {
                    if (_IsToPickerOpened)
                        ToField = ConcatEmails(ToField, contactEmail);
                    else 
                        CcField = ConcatEmails(CcField, contactEmail);
                }
            }
        }

        private string ConcatEmails(string source1, string source2)
        {
            if (!string.IsNullOrEmpty(source1))
            {
                var lastChar = source1.Substring(source1.Length - 1, 1);
                if (lastChar == "," || lastChar == ";")
                    return source1 + source2;

                return source1 + ";" + source2;
            }

            return source2;
        }

        private CAMSvcSchedExt _ReportInfo;

        public CAMSvcSchedExt ReportInfo
        {
            get { return _ReportInfo; }
            set
            {
                _ReportInfo = value;
                IsAvailableToSend = false;
            }
        }

        private bool _IsToPickerOpened;
        private bool _IsCcPickerOpened;
        private ContactsSearchViewModel _ContactsSearchViewModel;

        public RelayCommand PickToContactsCommand => new RelayCommand(() =>
        {
            _IsToPickerOpened = true;
            _IsCcPickerOpened = false;
            //_ContactPicker.PickContacts();
            _ContactsSearchViewModel = SimpleIoc.Default.GetInstance<ContactsSearchViewModel>();
            NavigationService.NavigateTo(ViewModelLocator.ContactsSearchPageKey, _ContactsSearchViewModel);
        });

        public RelayCommand PickCcContactsCommand => new RelayCommand(() =>
        {
            _IsToPickerOpened = false;
            _IsCcPickerOpened = true;
            //_ContactPicker.PickContacts();
            _ContactsSearchViewModel = SimpleIoc.Default.GetInstance<ContactsSearchViewModel>();
            NavigationService.NavigateTo(ViewModelLocator.ContactsSearchPageKey, _ContactsSearchViewModel);
        });

        public string ToField { get; set; }

        public string CcField { get; set; }

        public string SubjectField { get; set; }

        public string BodyField { get; set; }

        #endregion

        #region Preview report

        public RelayCommand PreviewReportCommand => new RelayCommand(async () =>
        {
            await DownloadReport();
        });

        private CAMSvcSchedEmp _CamViewModel;
        
        private async Task DownloadReport()
        {
            IsInProgress = true;
            var camdatasource = CAMDataSource.GetCurrData();

            CAMInProgressReport updaterep = new CAMInProgressReport(ReportInfo); //camdatasource.CAMInProgressReports.Where(d => d.ScheduleID == svc.ScheduleID).SingleOrDefault();

            //update stuff

            updaterep.SubjectLine = SubjectField;
            updaterep.Body = BodyField == _DefaultBody ? "" : BodyField;
            updaterep.CC = CcField;
            updaterep.CustomerEmail = ToField;
            updaterep.Images = _Issues.Select(issue => 
                new CAMInProgressReport.Image{ImagePath = issue.ImagePath
                , ImageDescription = issue.Description, ImageName = issue.Title}).ToList();

            CAMSvcSchedEmp svc = _CamViewModel;

            CAMSvcInspectReportRequest CRR = new CAMSvcInspectReportRequest();

            CRR.ScheduleID = updaterep.ScheduleID;
            CRR.UserCreated = camdatasource.CAMUser.Username;
            CRR.UserCreated = "";
            //CRR.Notes = updaterep.notes.Replace("\n", "<br>");

            foreach (var issue in _Issues)
            {
                CAMSvcInspectReportImage CI = new CAMSvcInspectReportImage();
                CI.ImageName = issue.Title;
                CI.ImageDescription = issue.Description;
                CI.ImageDateTime = DateTime.Now;
                
                CI.ImageData = await GetPhotoJsonAsync(issue.ImagePath);
                
                CRR.CAMSvcInspectReportImages.Add(CI);
            }

            //foreach (string checkcode in updaterep.Checkspeclst)
            //{
            //    CAMSvcInspectReportCheckList CL = new CAMSvcInspectReportCheckList();
            //    CL.SubItemDetailID = Convert.ToInt32(checkcode);
            //    //CL.SubItemID = 0;
            //    //CL.FieldCaption = "Test";
            //    //CL.FieldValue = "Test";

            //    CRR.CAMSvcInspectReportCheckLists.Add(CL);
            //}

            string getUri = "https://apps.camservices.com/CAMGateway/CAMServices.svc/CAMSvcInspectReportGet";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(getUri);
            request.Method = "POST";
            request.ContentType = "application/json";

            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(CAMSvcInspectReportRequest));

            MemoryStream ms = new MemoryStream();
            jsonSerializer.WriteObject(ms, CRR);

            String json = Encoding.UTF8.GetString(ms.ToArray(), 0, ms.ToArray().Length);
            StreamWriter sw = new StreamWriter(await request.GetRequestStreamAsync());
            sw.Write(json);
            
            sw.Dispose();

            request.BeginGetResponse(PDFCallback, request);
        }

        private async Task<byte[]> GetPhotoJsonAsync(string photoFile)
        {
            try
            {
                var fileService = DependencyService.Get<ISaveAndLoad>();
                var contentBytes = fileService.LoadFile(photoFile);
                var resizedImageBytes = await CrossImageResizer.Current.ResizeImageWithAspectRatioAsync(contentBytes, 700, 700);
                //var newFilePath = rootFolder.Path + "/" + Guid.NewGuid() + "." + Path.GetExtension(pickedPhoto.Path);
                //var newFilePath = rootFolder.Path + "/" + pickedPhoto.Name;
                //var newFile = await fileSystem.GetFileFromPathAsync(newFilePath);
                //if (newFile != null)
                //    await newFile.DeleteAsync();

                //newFile = await rootFolder.CreateFileAsync(pickedPhoto.Name, CreationCollisionOption.ReplaceExisting);

                //using (var newImageFileStream = await newFile.OpenAsync(FileAccess.ReadAndWrite))
                //{
                //    await newImageFileStream.WriteAsync(resizedImageBytes, 0, resizedImageBytes.Length);
                //}
                //await pickedPhoto.MoveAsync(newFilePath, NameCollisionOption.ReplaceExisting);
                //return newFilePath;
                return resizedImageBytes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string _PdfPath;

        public async void PDFCallback(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;

            if (request != null)
            {
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                    Stream streamResponse = response.GetResponseStream();
                    int readed = 1;
                    byte[] responseBytes = new byte[response.ContentLength];
                    var readedSum = 0;
                    while (readed > 0)
                    {
                        readed = streamResponse.Read(responseBytes, readedSum, (int)response.ContentLength - readedSum);
                        readedSum += readed;
                    }
                    var responseString = Encoding.UTF8.GetString(responseBytes, 0, responseBytes.Length);
                    var bytes = JsonConvert.DeserializeObject<byte[]>(responseString);

                    var filename = Path.GetRandomFileName() + ".jpg";
                    var fileService = DependencyService.Get<ISaveAndLoad>();
                    await fileService.SaveFileAsync(filename, bytes);

                    //var jsonSerializer = new DataContractJsonSerializer(typeof(byte[]));
                    //byte[] bRepose = (byte[])jsonSerializer.ReadObject(streamResponse);
                    //File.WriteAllBytes(Server.MapPath("ScheduleID.pdf"), bRepose);
                    streamResponse.Dispose();
                    response.Dispose();
                    
                    IsInProgress = false;
                    IsAvailableToSend = true;
                    _PdfPath = filename;
                    OpenPdf(filename);
                }
                catch
                {
                    IsInProgress = false;
                }
            }
            else
            {
                IsInProgress = false;
                //OpenPdf();
            }
        }

        private void OpenPdf(string uri)
        {
            var previewViewModel = SimpleIoc.Default.GetInstance<PreviewReportViewModel>();
            previewViewModel.Uri = uri;
            Device.BeginInvokeOnMainThread(() =>
            {
                NavigationService.NavigateTo(ViewModelLocator.PreviewReportPageKey, previewViewModel);
            });
        }

        #endregion

        #region Send to email

        public bool IsAvailableToSend { get; set; }

        public RelayCommand SendReportCommand => new RelayCommand(async () =>
        {
            IsInProgress = true;
            try
            {
                var camdatasource = CAMDataSource.GetCurrData();
                var svc = ReportInfo;

                //CAMInProgressReport updaterep = camdatasource.CAMInProgressReports.SingleOrDefault(d => d.ScheduleID == svc.ScheduleID);


                CAMSvcInspectReportEmail oRepEmail = new CAMSvcInspectReportEmail();
                // CRR.ScheduleID = 548227;
                oRepEmail.ScheduleID = ReportInfo.ScheduleID;

                oRepEmail.SendFrom = camdatasource.CAMUser.Username + "@camservices.com";

                //oRepEmail.SendFrom = "service@camservices.com";

                //oRepEmail.SendFrom = from.Text;

                //oRepEmail.SendTo = "eugene@kspsystems.com";

                oRepEmail.SendTo = ToField;

                if (!string.IsNullOrEmpty(CcField))
                {
                    oRepEmail.SendTo += ",";
                    oRepEmail.SendTo += CcField.Replace(';', ',');
                }

                //oRepEmail.Subject = "testsubject";
                oRepEmail.Subject = SubjectField;

                oRepEmail.Body = BodyField.Replace("\n", "<br>");
                // oRepEmail.Body = "testbody";

                string getUri = "https://apps.camservices.com/CAMGateway/CAMServices.svc/CAMSvcInspectReportSend";

                HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(getUri);
                request.Method = "POST";
                request.ContentType = "application/json";
                DataContractJsonSerializer jsonSerializer =
                    new DataContractJsonSerializer(typeof(CAMSvcInspectReportEmail));

                MemoryStream ms = new MemoryStream();
                jsonSerializer.WriteObject(ms, oRepEmail);

                String json = Encoding.UTF8.GetString(ms.ToArray(), 0, ms.ToArray().Length);
                StreamWriter sw = new StreamWriter(await request.GetRequestStreamAsync());
                sw.Write(json);

                sw.Dispose();

                request.BeginGetResponse(EmailCallback, request);
            }
            catch (Exception e)
            {
                BodyField = e.Message;
            }
        });

        public async void EmailCallback(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                try
                {
                    HttpWebResponse response = (HttpWebResponse) request.EndGetResponse(result);
                    Stream streamResponse = response.GetResponseStream();
                    string content = String.Empty;
                    var jsonSerializer = new DataContractJsonSerializer(typeof(bool));
                    bool bRepose = (bool) jsonSerializer.ReadObject(streamResponse);

                    streamResponse.Dispose();
                    response.Dispose();


                    if (bRepose)
                    {
                        var fileService = DependencyService.Get<ISaveAndLoad>();
                        var camdatasource = CAMDataSource.GetCurrData();

                        CAMInProgressReport updaterep =
                            camdatasource.CAMInProgressReports.SingleOrDefault(
                                d => d.ScheduleID == ReportInfo.ScheduleID);

                        foreach (var image in updaterep.Images)
                        {
                            fileService.Delete(image.ImagePath);
                        }

                        fileService.Delete(_PdfPath);

                        camdatasource.CAMInProgressReports.Remove(updaterep);

                        await camdatasource.WriteJsonAsync();

                        foreach (var issue in _Issues)
                        {
                            _Database.Issues.DeleteIssue(issue);
                        }

                        //NavigationService.NavigateTo(ViewModelLocator.HomePageKey,
                        //    SimpleIoc.Default.GetInstance<HomeViewModel>());
                        IsAvailableToSend = false;
                        NavigationService.BackToMain();
                    }
                }
                catch (WebException e)
                {
                    return;
                }
                finally
                {
                    IsInProgress = false;
                }
            }

            IsInProgress = false;
        }

        #endregion

        private string _DefaultBody = "";

        protected override void OnLoad()
        {
            base.OnLoad();

            if (IsAvailableToSend)
                return;

            var inProgressIds = _Database.Issues.GetInProgressReportScheduleIds();
            if (inProgressIds.Contains(ReportInfo.ScheduleID))
            {
                CAMInProgressReport updaterep = CAMDataSource.GetCurrData().CAMInProgressReports.SingleOrDefault(d => d.ScheduleID == ReportInfo.ScheduleID);

                if (updaterep?.SubjectLine != null)
                {
                    SubjectField = updaterep.SubjectLine;
                    BodyField = updaterep.Body;
                    CcField = updaterep.CC;
                }
                else
                {
                    _DefaultBody = "Dear " + updaterep.CustomerName +
                        ",\n\nPlease review the attached Property Inspection Report for your property located at " +
                        ReportInfo.ShortAddress + "" + updaterep.SiteCity + " [" + updaterep.SiteName +
                        "].\n\nShould you have any questions or concerns, please do not hesitate to email me or call me " +
                        CAMDataSource.GetCurrData().CAMUser.PhoneNo + ".\n\nSincerely,\n\n" +
                        CAMDataSource.GetCurrData().CAMUser.FullName + "\n";
                    updaterep = new CAMInProgressReport(ReportInfo);
                    SubjectField = "[" + updaterep.SiteName + "] CAM Services Property Inspection";
                    BodyField = _DefaultBody;
                }

                if (!_IsToPickerOpened && !_IsCcPickerOpened)
                    ToField = updaterep.CustomerEmail;
                else
                {
                    if (_IsToPickerOpened)
                    {
                        ToField = ConcatEmails(ToField,
                            _ContactsSearchViewModel.SelectedContact.Emails.FirstOrDefault() ?? "");
                    }
                    else
                    {
                        CcField = ConcatEmails(CcField,
                            _ContactsSearchViewModel.SelectedContact.Emails.FirstOrDefault() ?? "");
                    }
                    _IsToPickerOpened = false;
                    _IsCcPickerOpened = false;
                }
            }
        }
    }
}
