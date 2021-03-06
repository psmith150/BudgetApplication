﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace BudgetApplication.ValueConverters
{
    /// <summary>
    /// Converter used to color code cells depending on value. Only converts one way
    /// </summary>
    public class MoneyValueToBrushConverter : IValueConverter
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
                return Application.Current.Resources["NeutralMoneyBackgroundColor"];
            decimal num = 0;
            bool succeeded = true;
            succeeded = Decimal.TryParse(value as String, NumberStyles.Currency, CultureInfo.CurrentCulture, out num);
            if(!succeeded)
            {
                return Application.Current.Resources["NeutralMoneyBackgroundColor"];
            }
            if (num >= 0)
            {
                return Application.Current.Resources["PositiveMoneyBackgroundColor"];
            }
            else
            {
                return Application.Current.Resources["NegativeMoneyBackgroundColor"];
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
