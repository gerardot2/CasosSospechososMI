using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CasosSospechososMI.Behavior
{
    public class PassValidator : Behavior<Entry>
    {

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(args.NewTextValue))
            {
                
                if (args.NewTextValue.Length > 5 && args.NewTextValue.Length < 11)
                {
                    ((Entry)sender).TextColor = (Color)Application.Current.Resources["Primary"];
                    return;
                }
                else
                {
                    ((Entry)sender).TextColor = Color.Black;
                    return;
                }
            }
        }


    }
}
