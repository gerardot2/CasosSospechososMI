using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CasosSospechososMI.Behavior
{
    public class TrapValidator : Behavior<Entry>
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
                if (args.OldTextValue != null && args.OldTextValue.Length > args.NewTextValue.Length && args.NewTextValue.Length == 1)
                {
                    ((Entry)sender).Text = string.Empty;
                    return;
                }
                ((Entry)sender).TextColor = (Color)Application.Current.Resources["Primary"];
                if (args.NewTextValue.Length == 1)
                {
                    var first = args.NewTextValue.ToCharArray()[0].ToString().ToLower();
                    bool firstLetter = (first == "a")
                        || first =="k";

                    ((Entry)sender).Text = firstLetter ? $"{first.ToUpper()}-" : string.Empty;
                    return;
                }

                if (args.NewTextValue.Length > 2 && args.NewTextValue.Length < 9)
                {

                    bool isValid = char.IsDigit(args.NewTextValue.ToCharArray()[args.NewTextValue.Length - 1]); //Make sure all characters are numbers

                    ((Entry)sender).Text = isValid ? args.NewTextValue : args.NewTextValue.Remove(args.NewTextValue.Length - 1);
                   
                    return;

                }
                
                return;
            }
        }


    }
}
