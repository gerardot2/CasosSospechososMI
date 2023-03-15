using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CasosSospechososMI.Droid.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CasosSospechososMI.Controls;
using Android.Content.Res;
using Android.Graphics;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace CasosSospechososMI.Droid.Implementations
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null) return;

            Control.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Black);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Entry.TextProperty.PropertyName)
            {
                
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                {
                    Control.SetTextCursorDrawable(0); //This API Intrduced in android 10
                }
                else
                {
                    IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                    IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");
                    JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, 0);
                }
            }
            if (Control != null)
            {
                Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Rgb(102, 204, 0));
                EditText nativeEditText = (global::Android.Widget.EditText)Control;
                nativeEditText.SetTextCursorDrawable(Resource.Drawable.CursorLine);

            }
        }
    }
}

