using System.Windows;
using BudgetApplication.Model;
using System;

namespace BudgetApplication.View
{
    /// <summary>
    /// Popup used to create a new payment method.
    /// </summary>
    public partial class AddPaymentPopup : Window
    {
        /// <summary>
        /// Initializes a new instance of the AddPaymentPopup class.
        /// </summary>
        public AddPaymentPopup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the AddPaymentPopup class with the specified parent window.
        /// </summary>
        /// <param name="owner">The parent window</param>
        public AddPaymentPopup(Window owner) : this()
        {
            this.Owner = owner;
        }

        /// <summary>
        /// Run when the popup is loaded. Sets the source of the ComboBox used to select payment type.
        /// </summary>
        /// <param name="sender">The object loaded (this)</param>
        /// <param name="e">The arguments</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PaymentTypeSelector.ItemsSource = Enum.GetValues(typeof(PaymentMethod.Type));
        }

        /// <summary>
        /// Closes the window and saves the data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// Closes the window and discards the data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// The entered payment name.
        /// </summary>
        public String PaymentName
        {
            get
            {
                return PaymentNameBox.Text;
            }
        }

        /// <summary>
        /// The chosen payment type.
        /// </summary>
        public PaymentMethod.Type PaymentType
        {
            get
            {
                return (PaymentMethod.Type) PaymentTypeSelector.SelectedItem;
            }
        }
    }
}