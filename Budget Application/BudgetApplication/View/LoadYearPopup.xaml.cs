using System.Windows;
using BudgetApplication.ViewModel;
using System;
using System.Windows.Controls;

namespace BudgetApplication.View
{
    /// <summary>
    /// Popup used to create a new payment method.
    /// </summary>
    public partial class LoadYearPopup : Window
    {
        /// <summary>
        /// Initializes a new instance of the AddPaymentPopup class.
        /// </summary>
        public LoadYearPopup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the AddPaymentPopup class with the specified parent window.
        /// </summary>
        /// <param name="owner">The parent window</param>
        public LoadYearPopup(Window owner) : this()
        {
            this.Owner = owner;
        }

        /// <summary>
        /// Closes the window and saves the data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            YearSelector.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
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
    }
}