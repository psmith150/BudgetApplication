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
            DateTime startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            PaymentStartDate.SelectedDate = startDate;
            PaymentEndDate.SelectedDate = startDate.AddMonths(1).AddDays(-1);
            PaymentAmountBox.Text = 0.ToString("C");
        }

        private void GroupsAndCategories_Click(object sender, RoutedEventArgs e)
        {
            //Open popup
            GroupsAndCategoriesWindow popup = new GroupsAndCategoriesWindow(this);
            popup.ShowDialog();
        }

        private void PaymentMethods_Click(object sender, RoutedEventArgs e)
        {
            PaymentMethodsWindow popup = new PaymentMethodsWindow(this);
            popup.ShowDialog();
        }

        private void PaymentTransactionsView_Filter(object sender, FilterEventArgs e)
        {
            Transaction transaction = e.Item as Transaction;
            if (transaction != null && transaction.PaymentMethod != null && this.PaymentSelector.SelectedIndex >= 0)
            {
                //Debug.WriteLine(this.PaymentSelector.SelectedIndex);
                //Debug.WriteLine((PaymentSelector.SelectedItem).ToString());
                if ((PaymentSelector.SelectedValue as PaymentMethod).Name.Equals(transaction.PaymentMethod.Name) 
                    && PaymentStartDate.SelectedDate <= transaction.Date && PaymentEndDate.SelectedDate > transaction.Date)
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

        private void RefreshFilter()
        {
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(PaymentTransactions.ItemsSource);
            view.Refresh();
        }

        private void PaymentStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaymentSelector.SelectedIndex >= 0)
                (PaymentSelector.SelectedItem as PaymentMethod).StartDate = PaymentStartDate.SelectedDate ?? DateTime.Now;
            RefreshFilter();
        }

        private void PaymentEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaymentSelector.SelectedIndex >= 0)
                (PaymentSelector.SelectedItem as PaymentMethod).EndDate = PaymentEndDate.SelectedDate ?? DateTime.Now;
            RefreshFilter();
        }

        private void PaymentSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaymentSelector.SelectedItem == null)
                return;
            PaymentStartDate.SelectedDate = (PaymentSelector.SelectedItem as PaymentMethod).StartDate;
            PaymentEndDate.SelectedDate = (PaymentSelector.SelectedItem as PaymentMethod).EndDate;
            RefreshFilter();
            RecalculateCreditValues();
        }

        private void RecalculateCreditValues()
        {
            CreditCard card = PaymentSelector.SelectedItem as CreditCard;
            if (card != null)
            {
                CreditDetailRow.Height = new GridLength(1, GridUnitType.Auto);
                CreditLimitLabel.Content = card.CreditLimit;
                ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(PaymentTransactions.ItemsSource);
                decimal sum = 0;
                //Debug.WriteLine(view.Count);
                //Debug.WriteLine((view.GetItemAt(0) as Transaction).Amount);
                for (int i=0; i<view.Count-1; i++)
                {
                    Transaction transaction = view.GetItemAt(i) as Transaction;
                    sum += transaction.Amount;
                }
                TotalBillLabel.Content = sum;
                NetBillLabel.Content = sum - decimal.Parse(PaymentAmountBox.Text, System.Globalization.NumberStyles.Currency);
                RemainingCreditLabel.Content = (decimal)CreditLimitLabel.Content - (decimal)NetBillLabel.Content;
            }
            else
            {
                CreditDetailRow.Height = new GridLength(0);
            }
        }

        private void PaymentAmountBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            RecalculateCreditValues();
        }
    }
}
