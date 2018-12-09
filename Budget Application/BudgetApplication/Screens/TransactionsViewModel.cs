using System.Windows.Input;
using BudgetApplication.Popups;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Data;
using BudgetApplication.Model;
using System.ComponentModel;
using System.Diagnostics;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;

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
            this.TransactionsView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
            this.TransactionsView.Filter = ((transaction) => TransactionsView_Filter(transaction as Transaction));

            this.Transactions.CollectionChanged += ((s, a) => this.TransactionsView.Refresh());
            this.Transactions.CollectionChanged += ((s, a) => this.UpdateItemsList());

            //Set Commands
            this.AddTransactionCommand = new RelayCommand(() => this.AddTransaction());
            this.DeleteTransactionCommand = new RelayCommand(() => this.DeleteTransaction());
            this.DuplicateTransactionCommand = new RelayCommand(() => this.DuplicateTransaction());
        }

        public override void Initialize()
        {
        }

        public override void Deinitialize()
        {
        }

        #region Public Properties
        #region Commands
        public ICommand AddTransactionCommand { get; private set; }
        public ICommand DeleteTransactionCommand { get; private set; }
        public ICommand DuplicateTransactionCommand { get; private set; }
        #endregion
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
        private List<string> _Items;
        public List<string> Items
        {
            get
            {
                return this._Items;
            }
            private set
            {
                this.Set(ref this._Items, value);
            }
        }
        //public List<CheckedListItem<T>> FilterItems;
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
        private Transaction _SelectedTransaction;
        public Transaction SelectedTransaction
        {
            get
            {
                return this._SelectedTransaction;
            }
            set
            {
                this.Set(ref this._SelectedTransaction, value);
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
        #region Private Methods
        private void AddTransaction()
        {
            Transaction newTransaction = new Transaction();
            this.Transactions.Add(newTransaction);
            Messenger.Default.Send(new TransactionMessage(newTransaction));
        }
        private void DeleteTransaction()
        {
            if (this.SelectedTransaction != null)
                this.Transactions.Remove(SelectedTransaction);
        }
        private void DuplicateTransaction()
        {
            if (this.SelectedTransaction != null)
            {
                Transaction newTransaction = this.SelectedTransaction.Copy();
                newTransaction.Item += " - Copy";
                this.Transactions.Add(newTransaction);
                Messenger.Default.Send(new TransactionMessage(newTransaction));
            }
        }
        private void UpdateItemsList()
        {
            this.Items = new List<string>(this.Transactions.Select(x => x.Item).Distinct());
        }
        #endregion
    }
}