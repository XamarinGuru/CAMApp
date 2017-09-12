using System;
using System.Collections.Generic;
using System.Text;
using CamReports.Controls;
using CamReports.iOS.Contacts;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;

namespace CamReports.iOS.Renderers
{
    public class ItemSearchViewRenderer : BaseForceTouchViewRenderer<ItemSearchView, ItemSearchViewModel>
    {
        ForceTouchListView SearchListView;

        public ItemSearchViewRenderer()
        {
            SearchListView = Page.FindByName<ForceTouchListView>("SearchListView");
        }

        public override UIViewController GetPreviewControllerForLocation(CGPoint location)
        {
            //returns the native uitableview for the listview
            var nativeTableView = ForceTouchListViewRenderer.NativeTableViewForControl(SearchListView);

            //get index for list item
            var relativePoint = View.ConvertPointToView(location, nativeTableView);
            var index = nativeTableView.IndexPathForRowAtPoint(relativePoint);

            //returns the table item the user is interacting with
            var item = ViewModel.ListItems[(int)index.Item];

            //creates appropriate page
            PageToCommit = App.GetPage<ItemViewModel, ItemView>((vm, v) => vm.Initialize(item));

            return PageToCommit.CreateViewController();
        }

        public override bool ShouldHandleForceTouchForLocation(CGPoint location)
        {
            //do not react to 3D Touch if it’s not on the listview
            if (!SearchListView.Bounds.Contains(location.X, location.Y))
            {
                return false;
            }

            return true;
        }
    }
}
