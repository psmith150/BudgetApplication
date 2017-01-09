using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Converter used to color code cells depending on value. Only converts one way
    /// </summary>
    public class ValueToBrushConverter : IValueConverter
    {
        /// <summary>
        /// String value to Brush. Positive values are green, negative are red
        /// </summary>
        /// <param name="value">The value of the cell</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || String.IsNullOrEmpty(value as String))
                return Brushes.White;
            //Debug.WriteLine(value as string);
            decimal num = 0;
            bool succeeded = true;
            succeeded = Decimal.TryParse(value as String, NumberStyles.Currency, CultureInfo.CurrentCulture, out num);
            if(!succeeded)
            {
                return Brushes.White;
            }
            if (num >= 0)
            {
                return Brushes.Green;
            }
            else
            {
                return Brushes.Red;
            }
        }

        /// <summary>
        /// Cannot convert Brush to string
        /// </summary>
        /// <param name="value">The Brush value</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
