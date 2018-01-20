using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using BudgetApplication.IoC;

namespace BudgetApplication.ValueConverters
{
    [ValueConversion(typeof(BaseViewModel), typeof(UserControl))]
    public class ViewModelToViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var viewModel = value as BaseViewModel;
            return viewModel != null ? IoCContainer.ResolveScreen(viewModel.GetType()) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}