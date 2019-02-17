using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace BudgetApplication.ValueConverters
{
    /// <summary>
    /// Class to convert a PaymentType enum to its string description. Only converts one way
    /// </summary>
    public class PaymentTypeToStringConverter : IValueConverter
    {
        /// <summary>
        /// PaymentType to string
        /// </summary>
        /// <param name="value">The enum value</param>
        /// <param name="targetType">The type to convert to</param>
        /// <param name="parameter">Optional parameter</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enum val = value as Enum;
            string description = GetEnumDescription(val);
            return description;
        }

        /// <summary>
        /// Returns an empty string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }

        /// <summary>
        /// Retrieves the description associated with an enum value.
        /// </summary>
        /// <param name="enumObj">The enum object</param>
        /// <returns>The description</returns>
        private string GetEnumDescription(Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            object[] attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)    //No description given
            {
                return enumObj.ToString();
            }
            else
            {
                DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
                return attrib.Description;
            }
        }
    }
}
