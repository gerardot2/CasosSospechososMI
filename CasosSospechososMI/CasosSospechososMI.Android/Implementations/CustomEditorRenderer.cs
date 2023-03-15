using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using CasosSospechososMI.Controls;
using CasosSospechososMI.Droid.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedEditor),
typeof(CustomEditorRenderer))]
namespace CasosSospechososMI.Droid.Implementations
{
    class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Background = Xamarin.Forms.Forms.Context.GetDrawable(Resource.Drawable.RoundedEditor);
                Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                EditText nativeEditText = (global::Android.Widget.EditText)Control;
                nativeEditText.SetTextCursorDrawable(Resource.Drawable.CursorLine);
                
            }
        }
    }
}