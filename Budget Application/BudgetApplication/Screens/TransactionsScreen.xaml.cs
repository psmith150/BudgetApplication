using BudgetApplication.Model;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BudgetApplication.Screens
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TransactionsScreen : UserControl
    {
        public TransactionsScreen(TransactionsViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
            Messenger.Default.Register<TransactionMessage>(this, HandleTransactionMessage);
        }
        /// <summary>
        /// Handles the RequestBringIntoView event of the category combobox. Prevents annoying autoscroll.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            //Allows the keyboard to bring the items into view as expected:
            if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.Up))
                return;

            e.Handled = true;
        }
        private void HandleTransactionMessage(TransactionMessage msg)
        {
            this.Transactions.SelectedItem = msg.Transaction;
            this.Transactions.ScrollIntoView(msg.Transaction);
        }
    }
}
