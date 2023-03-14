using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CasosSospechososMI.Converters
{
    class IsVisibleToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            try
            {
                GridUnitType t = GridUnitType.Star;
                if (parameter != null)
                {
                    Enum.TryParse<GridUnitType>((string)parameter, true, out t);
                }

                if (value != null)
                {
                    bool d = (bool)value;
                    return d == false ? new GridLength(0, GridUnitType.Absolute) : new GridLength(1, t);
                }
                return null;
            }
            catch (Exception exp)
            {
                return null;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            return null;
        }
    }
}
