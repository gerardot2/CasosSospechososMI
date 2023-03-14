using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CasosSospechososMI.Behavior
{
    public class DniValidator : Behavior<Entry>
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
                bool isValid = args.NewTextValue.ToCharArray().All(x => char.IsDigit(x)); //Make sure all characters are numbers
                ((Entry)sender).Text = isValid ? args.NewTextValue : args.NewTextValue.Remove(args.NewTextValue.Length - 1);
                if (args.NewTextValue.Length == 1)
                {
                    var first = args.NewTextValue.ToCharArray()[0].ToString();
                    bool firstLetter = (first == "0");

                    ((Entry)sender).Text = firstLetter ? string.Empty : args.NewTextValue;
                    return;
                }
                if (args.NewTextValue.Length == 8)
                {
                    ((Entry)sender).TextColor = (Color)Application.Current.Resources["Primary"];
                    return;
                }
                else
                {
                    ((Entry)sender).TextColor = Color.Black;
                    return;
                }
                return;
            }
        }


    }
}
