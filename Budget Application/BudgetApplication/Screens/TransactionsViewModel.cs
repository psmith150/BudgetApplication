using System.Windows.Input;
using BudgetApplication.Popups;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Data;
using BudgetApplication.Model;
using System.ComponentModel;
using System.Diagnostics;

namespace BudgetApplication.Screens
{
    public class TransactionsViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;

        public TransactionsViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;
            this.Categories = session.Categories;
            this.PaymentMethods = session.PaymentMethods;
            this.Transactions = session.Transactions;
            this.TransactionsView = new ListCollectionView(this.Transactions);
            this.TransactionsView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Ascending));
            this.TransactionsView.Filter = ((transaction) => TransactionsView_Filter(transaction as Transaction));
        }

        public override void Initialize()
        {
        }

        public override void Deinitialize()
        {
        }

        #region Public Properties
        private ListCollectionView _transactionsView;
        public ListCollectionView TransactionsView
        {
            get
            {
                return _transactionsView;
            }
            private set
            {
                _transactionsView = value;
            }
        }

        private MyObservableCollection<Category> _categories;
        public MyObservableCollection<Category> Categories
        {
            get
            {
                return _categories;
            }
            private set
            {
                _categories = value;
                Debug.WriteLine($"Number of categories: {_categories.Count}");
            }
        }
        private MyObservableCollection<PaymentMethod> _paymentMethods;
        public MyObservableCollection<PaymentMethod> PaymentMethods
        {
            get
            {
                return _paymentMethods;
            }
            private set
            {
                _paymentMethods = value;
            }
        }
        #endregion
        #region Private Properties
        private MyObservableCollection<Transaction> _transactions;
        private MyObservableCollection<Transaction> Transactions
        {
            get
            {
                return _transactions;
            }
            set
            {
                _transactions = value;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Filter for the transactions tab. Filtering is based on which items are checked
        /// </summary>
        /// <param name="sender">The object requesting filtering</param>
        /// <param name="e">The arguments</param>
        private bool TransactionsView_Filter(Transaction transaction)
        { 
            return true;
            //Transaction transaction = e.Item as Transaction;
            //if (transaction != null && checkedItems != null)
            //{
            //    for (int i = 0; i < checkedItems.Length; i++)
            //    {
            //        String value = GetTransactionPropertyValueFromColumnIndex(i, transaction);
            //        int count = checkedItems[i].Where(x => x.IsChecked).Count(x => (x.Item as String).Equals(value));
            //        if (count == 0)
            //        {
            //            e.Accepted = false;
            //            return;
            //        }
            //    }
            //    e.Accepted = true;
            //}
        }
        #endregion
    }
}