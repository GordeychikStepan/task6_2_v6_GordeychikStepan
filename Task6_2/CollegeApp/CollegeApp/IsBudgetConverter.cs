using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CollegeApp
{
    public class IsBudgetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isBudget = (bool)value;
            return isBudget ? "На бюджете" : "Не на бюджете";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
