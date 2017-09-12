using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamReports.Controls;
using CamReports.iOS.Renderers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NoAutocorrectEntry), typeof(NoAutocorrectEntryRenderer))]

namespace CamReports.iOS.Renderers
{
    public class NoAutocorrectEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var textField = (UITextField) Control;

            // No auto-correct
            textField.AutocorrectionType = UITextAutocorrectionType.No;
            textField.SpellCheckingType = UITextSpellCheckingType.No;
            textField.AutocapitalizationType = UITextAutocapitalizationType.Words;
        }
    }
}