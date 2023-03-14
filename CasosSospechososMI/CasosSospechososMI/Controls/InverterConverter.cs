﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CasosSospechososMI.Controls
{
    public class InverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            !(bool)value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            !(bool)value;
    }
}
