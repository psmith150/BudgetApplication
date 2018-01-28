using System.Windows;
using BudgetApplication.ViewModel;
using System;

namespace BudgetApplication.View
{
    /// <summary>
    /// Popup used to create a new payment method.
    /// </summary>
    public partial class AddYearPopup : Window
    {
        /// <summary>
        /// Initializes a new instance of the AddPaymentPopup class.
        /// </summary>
        public AddYearPopup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the AddPaymentPopup class with the specified parent window.
        /// </summary>
        /// <param name="owner">The parent window</param>
        public AddYearPopup(Window owner) : this()
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
            String year = YearBox.Text;
            MainViewModel vm = this.Owner.DataContext as MainViewModel;
            if (vm.AddYearCommand.CanExecute(year))
            {
                vm.AddYearCommand.Execute(year);
                if (vm.ValidYear)
                {
                    this.DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Enter a valid year!");
                }
            }
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