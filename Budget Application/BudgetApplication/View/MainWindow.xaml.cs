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
using System.Diagnostics;
using BudgetApplication.Model;

namespace BudgetApplication.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GroupsAndCategories_Click(object sender, RoutedEventArgs e)
        {
            //Open popup
            GroupsAndCategoriesWindow popup = new GroupsAndCategoriesWindow();
            popup.ShowDialog();
        }

        private void PaymentMethods_Click(object sender, RoutedEventArgs e)
        {
            PaymentMethodsWindow popup = new PaymentMethodsWindow();
            popup.ShowDialog();
        }

        private void PaymentTransactionsView_Filter(object sender, FilterEventArgs e)
        {
            Transaction transaction = e.Item as Transaction;
            if (transaction != null && transaction.PaymentMethod != null && this.PaymentSelector.SelectedIndex >= 0)
            {
                Debug.WriteLine(this.PaymentSelector.SelectedIndex);
                if ((PaymentSelector.SelectedValue as String).Equals(transaction.PaymentMethod.Name))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateLayout();
        }
    }
}
