using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Diagnostics;
using BudgetApplication.Model;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using BudgetApplication.ViewModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BudgetApplication.View
{
    /// <summary>
    /// This is the main window of the application. It shows all of the main data in a tabbed format.
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<CheckedListItem<string>>[] checkedItems;   //Used to keep track of what objects are checked

        /// <summary>
        /// Initializes a new MainWindow object.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.TransactionModifiedEvent += Transactions_Modified;
            vm.TransactionsChangedEvent += Transactions_Changed;

            //Initialize data on Payments tab
            DateTime startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            PaymentStartDate.SelectedDate = startDate;
            PaymentEndDate.SelectedDate = startDate.AddMonths(1).AddDays(-1);
            PaymentAmountBox.Text = 0.ToString("C");

            //Initialize data on Transactions tab
            checkedItems = new ObservableCollection<CheckedListItem<string>>[7];
            for (int i = 0; i < checkedItems.Length; i++)
            {
                checkedItems[i] = new ObservableCollection<CheckedListItem<string>>();
            }

            foreach (Object obj in Transactions.ItemsSource)    //Initializes transactions. Necessary because MainViewModel loads before event handler is added.
            {
                //Debug.WriteLine("Object of type " + obj.GetType().ToString());
                Transaction transaction = obj as Transaction;
                if (transaction == null)
                    continue;
                AddTransaction(transaction);
            }
        }

        /// <summary>
        /// Opens a GroupsAndCategoriesWindow modal object.
        /// </summary>
        /// <param name="sender">The sending button</param>
        /// <param name="e">The arguments</param>
        private void GroupsAndCategories_Click(object sender, RoutedEventArgs e)
        {
            //Open popup
            GroupsAndCategoriesWindow popup = new GroupsAndCategoriesWindow(this);
            popup.ShowDialog();
        }

        /// <summary>
        /// Opens a PaymentMethodsWindow modal object.
        /// </summary>
        /// <param name="sender">The sending button</param>
        /// <param name="e">The arguments</param>
        private void PaymentMethods_Click(object sender, RoutedEventArgs e)
        {
            PaymentMethodsWindow popup = new PaymentMethodsWindow(this);
            popup.ShowDialog();
        }

        #region Payment Transactions Tab

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

        #endregion

        private void Transactions_Modified(object sender, PropertyChangedEventArgs e)
        {
            int index = -1;
            if (e.PropertyName.Equals("Date"))
            {
                RefreshFilter();
                RecalculateCreditValues();
                index = 0;
            }
            else if (e.PropertyName.Equals("Item"))
            {
                index = 1;
            }
            else if (e.PropertyName.Equals("Payee"))
            {
                index = 2;
            }
            else if (e.PropertyName.Equals("Amount"))
            {
                //MessageBox.Show("Updating amount");
                RecalculateCreditValues();
                index = 3;
            }
            else if (e.PropertyName.Equals("Category"))
            {
                index = 4;
            }
            else if (e.PropertyName.Equals("Payment Method"))
            {
                //RefreshFilter();
                //MessageBox.Show("Updating method");
                index = 5;
            }
            else if (e.PropertyName.Equals("Comment"))
            {
                index = 6;
            }

            Transaction transaction = sender as Transaction;
            String value = GetTransactionPropertyValueFromColumnIndex(index, transaction);
            //Debug.WriteLine("Number of items: " + checkedItems[index].Count);
            int count = checkedItems[index].Count(x => (x.Item as String).Equals(value));
            if (count == 0)
            {
                checkedItems[index].Add(new CheckedListItem<string> { IsChecked = true, Item = value });
            }
        }

        #region Transactions tab

        private void Transactions_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (Object obj in e.NewItems)
                {
                    Debug.WriteLine("Adding transaction");
                    Transaction transaction = obj as Transaction;
                    AddTransaction(transaction);
                }
            }
        }

        private void AddTransaction(Transaction transaction)
        {
            Debug.WriteLine("Transaction added " + transaction.ToString());
            for (int i = 0; i < checkedItems.Length; i++)
            {
                String value;
                switch (i)
                {
                    case 0:
                        value = transaction.Date.ToString("MM/dd/yyyy");
                        break;
                    case 1:
                        value = transaction.Item;
                        break;
                    case 2:
                        value = transaction.Payee;
                        break;
                    case 3:
                        value = transaction.Amount.ToString("C");
                        break;
                    case 4:
                        value = transaction.Category.Name;
                        break;
                    case 5:
                        value = transaction.PaymentMethod.Name;
                        break;
                    case 6:
                        value = transaction.Comment;
                        break;
                    default:
                        value = "";
                        break;
                }
                int count = checkedItems[i].Count(x => (x.Item as String).Equals(value));
                if (count == 0)
                {
                    checkedItems[i].Add(new CheckedListItem<string> { IsChecked = true, Item = value });
                }
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridColumnHeader parentHeader = ((Button)sender).TemplatedParent as DataGridColumnHeader;
            DataGridColumn parentColumn = parentHeader.Column;
            int index = parentColumn.DisplayIndex;
            //MessageBox.Show(index.ToString());

            filterPopup.PlacementTarget = sender as Button;

            FilterBox.ItemsSource = checkedItems[index];
            filterPopup.IsOpen = true;
        }

        private void TransactionsView_Filter(object sender, FilterEventArgs e)
        {
            Transaction transaction = e.Item as Transaction;
            if (transaction != null && checkedItems != null)
            {
                for (int i=0; i<checkedItems.Length; i++)
                {
                    String value = GetTransactionPropertyValueFromColumnIndex(i, transaction);
                    int count = checkedItems[i].Where(x => x.IsChecked).Count(x => (x.Item as String).Equals(value));
                    if (count == 0)
                    {
                        e.Accepted = false;
                        return;
                    }
                }
                e.Accepted = true;
            }
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            Button filterBtn = filterPopup.PlacementTarget as Button;
            DataGridColumnHeader parentHeader = filterBtn.TemplatedParent as DataGridColumnHeader;
            DataGridColumn parentColumn = parentHeader.Column;
            int index = parentColumn.DisplayIndex;
            foreach (CheckedListItem<string> item in checkedItems[index])
            {
                item.IsChecked = true;
            }
        }

        private void btnUnselectAll_Click(object sender, RoutedEventArgs e)
        {
            Button filterBtn = filterPopup.PlacementTarget as Button;
            DataGridColumnHeader parentHeader = filterBtn.TemplatedParent as DataGridColumnHeader;
            DataGridColumn parentColumn = parentHeader.Column;
            int index = parentColumn.DisplayIndex;
            foreach (CheckedListItem<string> item in checkedItems[index])
            {
                item.IsChecked = false;
            }
        }

        private void ApplyFilters(object sender, RoutedEventArgs e)
        {
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(Transactions.ItemsSource);
            view.Refresh();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(FilterBox.ItemsSource);
            view.Refresh();
        }

        private void filterPopup_Opened(object sender, EventArgs e)
        {
            ICollectionView view = (ICollectionView)CollectionViewSource.GetDefaultView(FilterBox.ItemsSource);
            view.Filter += delegate (object obj)
            {
                if (String.IsNullOrEmpty(SearchBox.Text))
                {
                    return true;
                }
                else
                {
                    //Debug.WriteLine("Checking if " + (obj as CheckedListItem<string>).Item + " contains " + SearchBox.Text);
                    int index = (obj as CheckedListItem<string>).Item.IndexOf(SearchBox.Text, StringComparison.InvariantCultureIgnoreCase);
                    return index > -1;
                }
            };
        }

        private string GetTransactionPropertyValueFromColumnIndex(int index, Transaction transaction)
        {
            String value = "";
            switch (index)
            {
                case 0:
                    value = transaction.Date.ToString("MM/dd/yyyy");
                    break;
                case 1:
                    value = transaction.Item;
                    break;
                case 2:
                    value = transaction.Payee;
                    break;
                case 3:
                    value = transaction.Amount.ToString("C");
                    break;
                case 4:
                    value = transaction.Category.Name;
                    break;
                case 5:
                    value = transaction.PaymentMethod.Name;
                    break;
                case 6:
                    value = transaction.Comment;
                    break;
                default:
                    Debug.WriteLine("Unrecognized column: " + index);
                    value = "";
                    break;
            }
            return value;
        }

        #endregion
    }
}
