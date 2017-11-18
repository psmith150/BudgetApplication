using System.Windows;
using BudgetApplication.Model;
using System.Windows.Controls;
using System;
using System.Diagnostics;

namespace BudgetApplication.View
{
    /// <summary>
    /// Window used to manipulate the payment methods.
    /// </summary>
    public partial class PaymentMethodsWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the PaymentMethodsWindow class.
        /// </summary>
        public PaymentMethodsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the PaymentMethodsWindow class with the specified owner.
        /// </summary>
        public PaymentMethodsWindow(Window owner) : this()
        {
            this.Owner = owner;
        }

        /// <summary>
        /// Run when the window loads. Sets the index of the payment list to 0.
        /// </summary>
        /// <param name="sender">The window object (this)</param>
        /// <param name="e">The arguments</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PaymentList.SelectedIndex = 0;
        }

        /// <summary>
        /// Run when the user selects a different payment method. Changes the displayed properties.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PaymentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((PaymentList.SelectedItem as PaymentMethod) == null)
                return;

            PaymentMethod selectedPayment= (PaymentList.SelectedItem as PaymentMethod);
            PaymentPropertyList.SelectedObject = selectedPayment;
        }

        /// <summary>
        /// Run when a payment is removed. Sets the payment method to be removed and keeps the index relevant.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Run when a payment is added. Opens the AddPaymentPopup window and retrieves it's data if it is closed with "Ok".
        /// Creates a new PaymentMethod and passes it as a command parameter to the ViewModel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            AddPaymentPopup popup = new AddPaymentPopup(this);
            if(popup.ShowDialog() == true)
            {
                String name = popup.PaymentName;
                PaymentMethod payment;
                //Debug.WriteLine("Added payment " + name + " of type " + popup.PaymentType.ToString());
                switch(popup.PaymentType)   //Determines which type of payment to produce.
                {
                    case PaymentMethod.Type.CreditCard:
                        payment = new CreditCard(name);
                        break;
                    case PaymentMethod.Type.CheckingAccount:
                        payment = new CheckingAccount(name);
                        Debug.WriteLine((payment as CheckingAccount).AccountNumber);
                        break;
                    default:
                        Debug.WriteLine("Invalid payment method: " + popup.PaymentType);
                        payment = null;
                        break;
                }
                //Debug.WriteLine("Name is " + popup.PaymentName + "; type is " + popup.PaymentType);
                AddPaymentButton.CommandParameter = payment;
                //PaymentList.SelectedIndex = PaymentList.Items.Count;
            }
        }
    }

}