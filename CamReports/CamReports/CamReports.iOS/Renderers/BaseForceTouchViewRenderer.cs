//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using CamReports.ViewModel;
//using CoreGraphics;
//using Foundation;
//using UIKit;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.iOS;
//using XLabs.Forms.Mvvm;

//namespace CamReports.iOS.Renderers
//{
//    public abstract class BaseForceTouchViewRenderer<TView, TViewModel> : PageRenderer, IUIViewControllerPreviewingDelegate
//        where TView : BaseView
//        where TViewModel : BaseViewModel
//    {
//        public readonly NavigationPage NavPage;
//        public readonly TView Page;
//        public readonly TViewModel ViewModel;
//        public Page PageToCommit;

//        protected BaseForceTouchViewRenderer()
//        {
//            NavPage = App.GetNavPage();
//            Page = NavPage.CurrentPage as TView;
//            ViewModel = Page.BindingContext as TViewModel;
//        }

//        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
//        {
//            base.TraitCollectionDidChange(previousTraitCollection);

//            if (TraitCollection.ForceTouchCapability == UIForceTouchCapability.Available)
//            {
//                RegisterForPreviewingWithDelegate(this, View);
//            }
//        }

//        public void CommitViewController(IUIViewControllerPreviewing previewingContext, UIViewController viewControllerToCommit)
//        {
//            App.PushPageFrom3DTouch(PageToCommit);
//        }

//        public UIViewController GetViewControllerForPreview(IUIViewControllerPreviewing previewingContext, CGPoint location)
//        {
//            if (!ShouldHandleForceTouchForLocation(location))
//            {
//                return null;
//            }

//            return GetPreviewControllerForLocation(location);
//        }

//        public abstract bool ShouldHandleForceTouchForLocation(CGPoint location);

//        public abstract UIViewController GetPreviewControllerForLocation(CGPoint location);
//    }
//}