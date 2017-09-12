
using System.IO;
using CamReports.iOS.Contacts;
using CamReports.Services;
using CamReports.Services.Contacts;
using Foundation;
using GalaSoft.MvvmLight.Ioc;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;
using UIKit;
using XLabs.Forms;
using XLabs.Forms.Services;
using XLabs.Platform.Device;
using XLabs.Platform.Mvvm;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Email;
using XLabs.Platform.Services.Media;

namespace CamReports.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : XFormsApplicationDelegate
    {
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
		    this.SetIoc();

            global::Xamarin.Forms.Forms.Init();

            SimpleIoc.Default.Register<ISQLitePlatform>(() => new SQLitePlatformIOS());

			LoadApplication(new App());
		    UIApplication.SharedApplication.SetStatusBarHidden(true, true);
            return base.FinishedLaunching(app, options);
		}

	    /// <summary>
	    /// Sets the IoC.
	    /// </summary>
	    private void SetIoc()
	    {
	        var resolverContainer = new global::XLabs.Ioc.SimpleContainer();

	        var app = new XFormsAppiOS();
	        app.Init(this);

	        var documents = app.AppDataDirectory;
	        var pathToDatabase = Path.Combine(documents, "xforms.db");

	        resolverContainer.Register(t => AppleDevice.CurrentDevice)
	            .Register<IDisplay>(t => t.Resolve<IDevice>().Display)
	            .Register<IFontManager>(t => new FontManager(t.Resolve<IDisplay>()))
	            //.Register<IJsonSerializer, Services.Serialization.SystemJsonSerializer>()
	            .Register<ITextToSpeechService, TextToSpeechService>()
	            .Register<IEmailService, EmailService>()
	            .Register<IMediaPicker, MediaPicker>()
	            .Register<IXFormsApp>(app)
	            .Register<ISecureStorage, SecureStorage>()
	            .Register<global::XLabs.Ioc.IDependencyContainer>(t => resolverContainer);

            XLabs.Ioc.Resolver.SetResolver(resolverContainer.GetResolver());

	        SimpleIoc.Default.Register<IContactPicker, ContactPicker>();
	        SimpleIoc.Default.Register<IContactManager, ContactsManager>();
	    }
    }
}
