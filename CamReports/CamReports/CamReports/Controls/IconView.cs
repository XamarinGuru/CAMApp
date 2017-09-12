using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CamReports.Controls
{
    public class IconView : Image
    {
        public IconView()
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                var tapped = Tapped;
                tapped?.Invoke(this, e);
            };
            this.GestureRecognizers.Add(tapGestureRecognizer);
        }

        #region TappedEvent

        public event EventHandler Tapped;

        #endregion

        #region ForegroundProperty

        public static readonly BindableProperty ForegroundProperty = BindableProperty.Create(nameof(Foreground), typeof(Color), typeof(IconView), default(Color));

        public Color Foreground
        {
            get
            {
                return (Color)GetValue(ForegroundProperty);
            }
            set
            {
                SetValue(ForegroundProperty, value);
            }
        }

        #endregion

        #region SourceProperty

        public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(string), typeof(IconView), default(string));

        public string Source
        {
            get
            {
                return (string)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }

        #endregion
    }
}
