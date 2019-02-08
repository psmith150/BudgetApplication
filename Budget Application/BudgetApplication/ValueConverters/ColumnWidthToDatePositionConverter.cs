using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace BudgetApplication.ValueConverters
{
    /// <summary>
    /// Converter used to color code cells depending on value. Only converts one way
    /// </summary>
    public class ColumnWidthToDatePositionConverter : IMultiValueConverter
    {
        /// <summary>
        /// Boolean value to Brush. True values are green, false are red
        /// </summary>
        /// <param name="values">The array of values (category width, progress width, month percentage)</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">The desired offset</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[1] == null)
                return 0;
            int categoryWidth = System.Convert.ToInt32(values[0]);
            int progressWidth = System.Convert.ToInt32(values[1]);
            double percentage = System.Convert.ToDouble(values[2]);
            int offset = System.Convert.ToInt32(parameter);
            return new Thickness(categoryWidth + offset + progressWidth * percentage, 0, 0, 0);
        }

        /// <summary>
        /// Cannot convert Brush to string
        /// </summary>
        /// <param name="value">The Brush value</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
