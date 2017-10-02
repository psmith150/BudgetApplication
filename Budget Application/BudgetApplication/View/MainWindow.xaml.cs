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
using NCalc;
using System.Windows.Input;

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
            //TestBox.Text = 0.ToString("C");

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
                //AddTransaction(transaction);
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

        #region Month Details tab
        #endregion

        #region Transactions tab

        /// <summary>
        /// Called when the transactions collection is modified.
        /// </summary>
        /// <param name="sender">The collection modified</param>
        /// <param name="e">The arguments</param>
        //TODO: handle removal of transactions
        private void Transactions_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (Object obj in e.NewItems)
                {
                    //Debug.WriteLine("Adding transaction");
                    Transaction transaction = obj as Transaction;
                    AddTransaction(transaction);
                }
            }
        }

        /// <summary>
        /// Adds the specified transaction to the transaction filters.
        /// </summary>
        /// <param name="transaction"></param>
        //TODO: disabled 3/28/2017
        private void AddTransaction(Transaction transaction)
        {
            //Debug.WriteLine("Transaction added " + transaction.ToString());
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
                        if (transaction.Category == null)
                            value = "";
                        else
                            value = transaction.Category.Name;
                        break;
                    case 5:
                        if (transaction.PaymentMethod == null)
                            value = "";
                        else
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

        /// <summary>
        /// Run when the filter button on the column headers is clicked. Opens the popup at that location and gives it the correct items source
        /// </summary>
        /// <param name="sender">The filter button</param>
        /// <param name="e">The arguments</param>
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridColumnHeader parentHeader = ((Button)sender).TemplatedParent as DataGridColumnHeader;
            DataGridColumn parentColumn = parentHeader.Column;
            int index = parentColumn.DisplayIndex;
            //MessageBox.Show(index.ToString());

            ObservableCollection<CheckedListItem<string>> columnValues = new ObservableCollection<CheckedListItem<string>>();
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(Transactions.ItemsSource);

            filterPopup.PlacementTarget = sender as Button;
            ICollectionView source = CollectionViewSource.GetDefaultView(checkedItems[index]);
            source.SortDescriptions.Add(new SortDescription("Item", ListSortDirection.Ascending));
            FilterBox.ItemsSource = source;
            filterPopup.IsOpen = true;
        }

        /// <summary>
        /// Filter for the transactions tab. Filtering is based on which items are checked
        /// </summary>
        /// <param name="sender">The object requesting filtering</param>
        /// <param name="e">The arguments</param>
        private void TransactionsView_Filter(object sender, FilterEventArgs e)
        {
            Transaction transaction = e.Item as Transaction;
            if (transaction != null && checkedItems != null)
            {
                for (int i = 0; i < checkedItems.Length; i++)
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

        /// <summary>
        /// Selects all the items in the filter popup
        /// </summary>
        /// <param name="sender">The pressed button</param>
        /// <param name="e">The arguments</param>
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

        /// <summary>
        /// De-selects all the items in the filter popup
        /// </summary>
        /// <param name="sender">The pressed button</param>
        /// <param name="e">The arguments</param>
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

        /// <summary>
        /// Refreshes the filters on the Transactions tab
        /// </summary>
        /// <param name="sender">The object requesting the refresh</param>
        /// <param name="e">The arguments</param>
        private void RefreshTransactionFilters(object sender, RoutedEventArgs e)
        {
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(Transactions.ItemsSource);
            view.Refresh();
        }

        /// <summary>
        /// Called when the text in the filter search box changes. Refreshes the filter box filter.
        /// </summary>
        /// <param name="sender">The TextBox that is being changed</param>
        /// <param name="e">The arguments</param>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(FilterBox.ItemsSource);
            view.Refresh();
        }

        /// <summary>
        /// Called when the filter popup is opened. Creates the filter used to search the filtering items.
        /// </summary>
        /// <param name="sender">The Popup bineg opened</param>
        /// <param name="e">The arguments</param>
        private void filterPopup_Opened(object sender, EventArgs e)
        {
            ICollectionView view = (ICollectionView)CollectionViewSource.GetDefaultView(FilterBox.ItemsSource);
            view.Filter += delegate (object obj)    //Filters based on the text in the SearchBox of the popup
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

        /// <summary>
        /// Converts a column index into the value of the corresponding Transaction property
        /// </summary>
        /// <param name="index">The column index</param>
        /// <param name="transaction">The transaction to get values from</param>
        /// <returns></returns>
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
                    if (transaction.Category != null)
                        value = transaction.Category.Name;
                    break;
                case 5:
                    if (transaction.PaymentMethod != null)
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

        #region Payment Transactions Tab

        /// <summary>
        /// Filters the transactions shown on the Payment tab based on the dates and selected payment method
        /// </summary>
        /// <param name="sender">The object requesting filtering</param>
        /// <param name="e">The filter arguments</param>
        private void PaymentTransactionsView_Filter(object sender, FilterEventArgs e)
        {
            Transaction transaction = e.Item as Transaction;
            if (transaction != null && transaction.PaymentMethod != null && this.PaymentSelector.SelectedIndex >= 0)
            {
                //Debug.WriteLine(this.PaymentSelector.SelectedIndex);
                //Debug.WriteLine((PaymentSelector.SelectedItem).ToString());
                if (((PaymentSelector.SelectedValue as PaymentMethod).Name.Equals(transaction.PaymentMethod.Name) || (PaymentSelector.SelectedValue as PaymentMethod).Name.Equals("All"))
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

        /// <summary>
        /// Temporary button for testing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateLayout();
        }

        /// <summary>
        /// Refreshes the filter on the Payments tab
        /// </summary>
        private void RefreshPaymentFilter()
        {
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(PaymentTransactions.ItemsSource);
            view.Refresh();
        }

        /// <summary>
        /// Called when the payment start data is changed. Stores the start date in the currently selected PaymentMethod object.
        /// </summary>
        /// <param name="sender">The DatePicker being changed</param>
        /// <param name="e">The arguments</param>
        private void PaymentStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaymentSelector.SelectedIndex >= 0)
                (PaymentSelector.SelectedItem as PaymentMethod).StartDate = PaymentStartDate.SelectedDate ?? DateTime.Now;
            RefreshPaymentFilter();
            RecalculateCreditValues();
        }

        /// <summary>
        /// Called when the payment end data is changed. Stores the end date in the currently selected PaymentMethod object.
        /// </summary>
        /// <param name="sender">The DatePicker being changed</param>
        /// <param name="e">The arguments</param>
        private void PaymentEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaymentSelector.SelectedIndex >= 0)
                (PaymentSelector.SelectedItem as PaymentMethod).EndDate = PaymentEndDate.SelectedDate ?? DateTime.Now;
            RefreshPaymentFilter();
            RecalculateCreditValues();
        }

        /// <summary>
        /// Called when the selected payment method is changed. Sets the start and end dates to its stored values.
        /// </summary>
        /// <param name="sender">The ComboBox being changed.</param>
        /// <param name="e">The arguments</param>
        private void PaymentSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaymentSelector.SelectedItem as PaymentMethod == null || PaymentStartDate == null || PaymentEndDate == null)
                return;
            //Debug.Write("SelectedItem " + (PaymentSelector.SelectedItem as PaymentMethod).StartDate + "\n");
            //Debug.Write(PaymentStartDate.ToString());
            PaymentStartDate.SelectedDate = (PaymentSelector.SelectedItem as PaymentMethod).StartDate;
            PaymentEndDate.SelectedDate = (PaymentSelector.SelectedItem as PaymentMethod).EndDate;
            if ((PaymentSelector.SelectedItem as CreditCard) != null)
            {
                PaymentAmountBox.Text = (PaymentSelector.SelectedItem as CreditCard).PaymentAmount.ToString("C");
                //TestBox.Text = (PaymentSelector.SelectedItem as CreditCard).PaymentAmount.ToString("C");
            }
            RefreshPaymentFilter();
            RecalculateCreditValues();
        }

        /// <summary>
        /// Calculates the values that show information about a CreditCard object
        /// </summary>
        private void RecalculateCreditValues()
        {
            CreditCard card = PaymentSelector.SelectedItem as CreditCard;
            if (card != null)
            {
                CreditDetailRow.Height = new GridLength(1, GridUnitType.Auto);  //Shows the detail row
                CreditLimitLabel.Content = card.CreditLimit;    //Shows the card's credit limit
                ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(PaymentTransactions.ItemsSource);
                decimal sum = 0;
                //Debug.WriteLine(view.Count);
                //Debug.WriteLine((view.GetItemAt(0) as Transaction).Amount);
                for (int i = 0; i < view.Count - 1; i++)
                {
                    Transaction transaction = view.GetItemAt(i) as Transaction;
                    sum += transaction.Amount;
                }
                TotalBillLabel.Content = sum;   //Shows the amount owed in the given data range.
                NetBillLabel.Content = sum - decimal.Parse(PaymentAmountBox.Text, System.Globalization.NumberStyles.Currency);  //Shows the total amount owed, given existing payments
                RemainingCreditLabel.Content = (decimal)CreditLimitLabel.Content - (decimal)NetBillLabel.Content;   //Shows the remaining credit
            }
            else
            {
                CreditDetailRow.Height = new GridLength(0); //HIdes the detail row
            }
        }

        /// <summary>
        /// Recalculates credit values when the amount entered in the payments box changes. 
        /// TODO: Parses the payment box text into a mathematical expression
        /// </summary>
        /// <param name="sender">The TextBox being changed</param>
        /// <param name="e">The arguments</param>
        private void PaymentAmountBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            CreditCard card = PaymentSelector.SelectedItem as CreditCard;
            if (card != null)
            {
                //card.PaymentAmount = decimal.Parse(PaymentAmountBox.Text, System.Globalization.NumberStyles.Currency);
            }
            RecalculateCreditValues();
        }

        private bool EvaluateExpression(String expression, out decimal result)
        {
            bool success = true;
            NCalc.Expression ex = new NCalc.Expression(expression);
            if (ex.HasErrors())
            {
                success = false;
                result = 0;
                return success;
            }
            try
            {
                result = Decimal.Parse(ex.Evaluate().ToString());
            }
            catch (Exception e)
            {
                result = 0;
                success = false;
            }
            return success;
        }

        /// <summary>
        /// Called when the Payments tab gets focus. Refreshes the filter and recalculates the values. Less intensive than triggering on transaction collection changes.
        /// 1/5/2017: Currently not in use due to same functionality in Transactions_Modified. May be re-used later if performance is bad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PaymentsTab_GotFocus(object sender, RoutedEventArgs e)
        {
            RefreshPaymentFilter();
            RecalculateCreditValues();
        }

        #endregion

        /// <summary>
        /// Called when a transaction object has been modified. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Transactions_Modified(object sender, PropertyChangedEventArgs e)
        {
            int index = -1; //Column index that was modified
            //Performs actions based on which property was modified
            bool refresh = false;
            bool recalculate = false;
            if (e.PropertyName.Equals("Date"))
            {
                refresh = true;
                recalculate = true;
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
                recalculate = true;
                index = 3;
            }
            else if (e.PropertyName.Equals("Category"))
            {
                index = 4;
            }
            else if (e.PropertyName.Equals("PaymentMethod"))
            {
                refresh = true;
                recalculate = true;
                //MessageBox.Show("Updating method");
                index = 5;
            }
            else if (e.PropertyName.Equals("Comment"))
            {
                index = 6;
            }
            else
            {
                Debug.WriteLine("Unrecognized property: " + e.PropertyName);
            }

            Transaction transaction = sender as Transaction;
            String value = GetTransactionPropertyValueFromColumnIndex(index, transaction); //Converts a column index into the property value of the transaction
            //Debug.WriteLine("Number of items: " + checkedItems[index].Count);

            //Check if modified property value exists in the checkedItems collection.
            int count = checkedItems[index].Count(x => (x.Item as String).Equals(value));
            if (count == 0)
            {
                checkedItems[index].Add(new CheckedListItem<string> { IsChecked = true, Item = value });
            }

            //Refresh and recalculate as needed
            if (refresh)
            {
                //RefreshPaymentFilter();
            }
            if (recalculate)
            {
                RecalculateCreditValues();
            }
        }

        /// <summary>
        /// Asks the user for a new year of data, and creates the file if the year is valid.
        /// </summary>
        /// <param name="sender">The button pressed</param>
        /// <param name="e">The arguments</param>
        private void NewYear_Click(object sender, RoutedEventArgs e)
        {
            AddYearPopup popup = new AddYearPopup(this);
            popup.ShowDialog();
        }

        /// <summary>
        /// This method is used to force the datagrid to commit an edit before the payment filter is refreshed
        /// </summary>
        /// <param name="sender">The PaymentTransactions datagrid</param>
        /// <param name="e">The parameters</param>
        private void PaymentTransactions_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (PaymentTransactions.SelectedItem != null)
            {
                (sender as DataGrid).RowEditEnding -= PaymentTransactions_RowEditEnding;
                (sender as DataGrid).CommitEdit();
                RefreshPaymentFilter();
                (sender as DataGrid).RowEditEnding += PaymentTransactions_RowEditEnding;
            }
            else return;
        }

        private void PaymentExpression_Updated(object sender, RoutedEventArgs e)
        {
            CreditCard card = PaymentSelector.SelectedItem as CreditCard;
            if (card != null)
            {
                //card.PaymentAmount = decimal.Parse(PaymentAmountBox.Text, System.Globalization.NumberStyles.Currency);
                card.PaymentExpression = (sender as TextBox).Text;
                (sender as TextBox).Text = card.PaymentAmount.ToString("C");
            }
            RecalculateCreditValues();
        }

        private void PaymentExpression_Editing(object sender, RoutedEventArgs e)
        {
            CreditCard card = PaymentSelector.SelectedItem as CreditCard;
            if (card != null)
            {
                //card.PaymentAmount = decimal.Parse(PaymentAmountBox.Text, System.Globalization.NumberStyles.Currency);
                (sender as TextBox).Text = card.PaymentExpression;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadYearPopup popup = new LoadYearPopup(this);
            popup.ShowDialog();
        }
    }
}
