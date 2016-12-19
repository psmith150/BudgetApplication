using System.Windows;
using BudgetApplication.Model;
using System.Windows.Controls;
using System.Windows.Data;
using System;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;

namespace BudgetApplication.View
{
    /// <summary>
    /// Description for MvvmView1.
    /// </summary>
    public partial class PaymentMethodsWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MvvmView1 class.
        /// </summary>
        public PaymentMethodsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PaymentList.SelectedIndex = 0;
        }
        private void PaymentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((PaymentList.SelectedItem as PaymentMethod) == null)
                return;

            PaymentMethod selectedPayment= (PaymentList.SelectedItem as PaymentMethod);

            if (selectedPayment.PaymentType == PaymentMethod.Type.CreditCard)
            {
                ObservableCollection<PropertyInfo> properties = new ObservableCollection<PropertyInfo>(selectedPayment.GetType().GetProperties());
                Debug.WriteLine(properties[0].GetValue(selectedPayment));
            }
        }
    }

}