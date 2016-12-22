using System.Windows;
using BudgetApplication.Model;
using System;

namespace BudgetApplication.View
{
    /// <summary>
    /// Description for MvvmView1.
    /// </summary>
    public partial class AddPaymentPopup : Window
    {
        /// <summary>
        /// Initializes a new instance of the MvvmView1 class.
        /// </summary>

        public AddPaymentPopup()
        {
            InitializeComponent();
        }

        public AddPaymentPopup(Window owner) : this()
        {
            this.Owner = owner;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PaymentTypeSelector.ItemsSource = Enum.GetValues(typeof(PaymentMethod.Type));
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public String PaymentName
        {
            get
            {
                return PaymentNameBox.Text;
            }
        }

        public PaymentMethod.Type PaymentType
        {
            get
            {
                return (PaymentMethod.Type) PaymentTypeSelector.SelectedItem;
            }
        }
    }
}