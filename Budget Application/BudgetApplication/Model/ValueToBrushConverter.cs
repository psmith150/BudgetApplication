using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BudgetApplication.Model
{
    public class ValueToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || String.IsNullOrEmpty(value as String))
                return Brushes.White;
            //Debug.WriteLine(value as string);
            decimal num = decimal.Parse(value as String, NumberStyles.Currency);
            if (num >= 0)
            {
                return Brushes.Green;
            }
            else
            {
                return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
