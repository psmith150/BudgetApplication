using System.Windows;
using BudgetApplication.Model;
using System.Windows.Controls;
using System.Windows.Data;
using System;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using Xceed.Wpf.Toolkit.PropertyGrid;

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

        public PaymentMethodsWindow(Window owner) : this()
        {
            this.Owner = owner;
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
            PaymentPropertyList.SelectedObject = selectedPayment;
        }

        private void RemovePaymentButton_Click(object sender, RoutedEventArgs e)
        {
            int index = PaymentList.SelectedIndex;
            PaymentMethod selectedPayment = (PaymentList.SelectedItem as PaymentMethod);
            if (selectedPayment == null)
                return;

            RemovePaymentButton.CommandParameter = selectedPayment;

            if (index > 0)
                PaymentList.SelectedIndex = index - 1;
            else
                PaymentList.SelectedIndex = 0;
        }

        private void AddPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            AddPaymentPopup popup = new AddPaymentPopup(this);
            if(popup.ShowDialog() == true)
            {
                String name = popup.PaymentName;
                PaymentMethod payment;
                switch(popup.PaymentType)
                {
                    case PaymentMethod.Type.CreditCard:
                    {
                        payment = new CreditCard(name);
                        break;
                    }
                    case PaymentMethod.Type.CheckingAccount:
                    {
                        payment = new CheckingAccount(name);
                        Debug.WriteLine((payment as CheckingAccount).AccountNumber);
                        break;
                    }
                    default:
                    {
                        payment = null;
                        break;
                    } 
                }
                Debug.WriteLine("Name is " + popup.PaymentName + "; type is " + popup.PaymentType);
                AddPaymentButton.CommandParameter = payment;
                PaymentList.SelectedIndex = PaymentList.Items.Count;
            }
        }
    }

}