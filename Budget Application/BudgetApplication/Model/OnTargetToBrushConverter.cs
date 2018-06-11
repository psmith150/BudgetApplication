using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Converter used to color code cells depending on value. Only converts one way
    /// </summary>
    public class OnTargetToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Boolean value to Brush. True values are green, false are red
        /// </summary>
        /// <param name="value">The OnTarget value</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Brushes.White;
            bool result = System.Convert.ToBoolean(value);
            if (result)
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
