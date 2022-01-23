using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Graphics;
using Android.Content;
using Nekono.App.Pos.Renderer;
using Nekono.App.Pos.Droid;
using Android.Views.InputMethods;
using Android.Runtime;
using Android.Views;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(BarcodeEntry), typeof(BarcodeEntryRenderer))]
namespace Nekono.App.Pos.Droid
{
    public class BarcodeEntryRenderer : EntryRenderer
    {
        public BarcodeEntryRenderer(Context context) : base(context)
        {

        }

        //protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        //{
        //    base.OnElementChanged(e);

        //    if (Control != null)
        //    {
        //        Control.InputType = 0;
        //        Control.ShowSoftInputOnFocus = false;
        //        InputMethodManager inputMethodManager = Control.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
        //        inputMethodManager?.HideSoftInputFromWindow(Control.WindowToken, HideSoftInputFlags.None);
        //    }
        //}

        //protected override void OnFocusChanged(bool gainFocus, [GeneratedEnum] FocusSearchDirection direction, Android.Graphics.Rect previouslyFocusedRect)
        //{
        //    base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);

        //    if (Control != null)
        //    {
        //        Control.InputType = 0;
        //        Control.ShowSoftInputOnFocus = false;
        //        InputMethodManager inputMethodManager = Control.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
        //        inputMethodManager?.HideSoftInputFromWindow(Control.WindowToken, HideSoftInputFlags.None);
        //    }
        //}

        //protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    base.OnElementPropertyChanged(sender, e);

        //    if (Control != null)
        //    {
        //        Control.ShowSoftInputOnFocus = false;
        //    }
        //}
    }
}