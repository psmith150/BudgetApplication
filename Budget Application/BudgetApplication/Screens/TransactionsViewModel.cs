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
using System;
using System.Collections.Specialized;

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

            this.FilterDates = new MyObservableCollection<CheckedListItem<DateTime>>();
            this.FilterDates.CollectionChanged += this.FilterList_CollectionChanged;
            this.FilterDates.MemberChanged += this.FilterList_PropertyChanged;
            this.FilterItems = new MyObservableCollection<CheckedListItem<string>>();
            this.FilterItems.CollectionChanged += this.FilterList_CollectionChanged;
            this.FilterItems.MemberChanged += this.FilterList_PropertyChanged;
            this.FilterPayees = new MyObservableCollection<CheckedListItem<string>>();
            this.FilterPayees.CollectionChanged += this.FilterList_CollectionChanged;
            this.FilterPayees.MemberChanged += this.FilterList_PropertyChanged;
            this.FilterAmounts = new MyObservableCollection<CheckedListItem<decimal>>();
            this.FilterAmounts.CollectionChanged += this.FilterList_CollectionChanged;
            this.FilterAmounts.MemberChanged += this.FilterList_PropertyChanged;
            this.FilterCategories = new MyObservableCollection<CheckedListItem<Category>>();
            this.FilterCategories.CollectionChanged += this.FilterList_CollectionChanged;
            this.FilterCategories.MemberChanged += this.FilterList_PropertyChanged;
            this.FilterPaymentMethods = new MyObservableCollection<CheckedListItem<PaymentMethod>>();
            this.FilterPaymentMethods.CollectionChanged += this.FilterList_CollectionChanged;
            this.FilterPaymentMethods.MemberChanged += this.FilterList_PropertyChanged;
            this.FilterComments = new MyObservableCollection<CheckedListItem<string>>();
            this.FilterComments.CollectionChanged += this.FilterList_CollectionChanged;
            this.FilterComments.MemberChanged += this.FilterList_PropertyChanged;

            this.FilterView = new ListCollectionView(this.FilterDates);
            this.activeColumnName = "Date";

            this.Transactions.CollectionChanged += this.Transactions_CollectionChanged;
            this.Transactions.MemberChanged += this.Transaction_PropertyChanged;

            //Set Commands
            this.AddTransactionCommand = new RelayCommand(() => this.AddTransaction());
            this.DeleteTransactionCommand = new RelayCommand(() => this.DeleteTransaction());
            this.DuplicateTransactionCommand = new RelayCommand(() => this.DuplicateTransaction());
            this.UpdateFilterCommand = new RelayCommand(() => { this.TransactionsView.Refresh(); });
            this.OpenFilterPopupCommand = new RelayCommand<string>((a) => this.OpenPopup(a));
            this.SelectAllFiltersCommand = new RelayCommand(() => this.SelectFilters());
            this.DeselectAllFiltersCommand = new RelayCommand(() => this.DeselectFilters());
        }

        public override void Initialize()
        {
            //this.Transactions.CollectionChanged += this.Transactions_CollectionChanged;
            //this.Transactions.MemberChanged += this.Transaction_PropertyChanged;
            this.TransactionsView.Refresh();
        }

        public override void Deinitialize()
        {
            //this.Transactions.CollectionChanged -= this.Transactions_CollectionChanged;
            //this.Transactions.MemberChanged -= this.Transaction_PropertyChanged;
        }

        #region Public Properties
        #region Commands
        public ICommand AddTransactionCommand { get; private set; }
        public ICommand DeleteTransactionCommand { get; private set; }
        public ICommand DuplicateTransactionCommand { get; private set; }
        public ICommand UpdateFilterCommand { get; private set; }
        public ICommand OpenFilterPopupCommand { get; private set; }
        public ICommand SelectAllFiltersCommand { get; private set; }
        public ICommand DeselectAllFiltersCommand { get; private set; }
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
        private List<string> _Payees;
        public List<string> Payees
        {
            get
            {
                return this._Payees;
            }
            private set
            {
                this.Set(ref this._Payees, value);
            }
        }
        private ListCollectionView _FilterView;
        public ListCollectionView FilterView
        {
            get
            {
                return this._FilterView;
            }
            private set
            {
                this.Set(ref this._FilterView, value);
                this.FilterView.SortDescriptions.Add(new SortDescription("Item", ListSortDirection.Ascending));
                Type sourceType = this.FilterView.SourceCollection.GetType().GetGenericArguments().Single().GetGenericArguments().Single();
                if (sourceType == typeof(string))
                    this.FilterView.Filter = ((item) => FilterView_Filter(item as CheckedListItem<string>));
                else if (sourceType == typeof(Category))
                    this.FilterView.Filter = ((item) => FilterView_Filter(item as CheckedListItem<Category>));
                else if (sourceType == typeof(PaymentMethod))
                    this.FilterView.Filter = ((item) => FilterView_Filter(item as CheckedListItem<PaymentMethod>));
                else if (sourceType == typeof(DateTime))
                    this.FilterView.Filter = ((item) => FilterView_Filter(item as CheckedListItem<DateTime>));
                else if (sourceType == typeof(decimal))
                    this.FilterView.Filter = ((item) => FilterView_Filter(item as CheckedListItem<decimal>));
                else
                    this.FilterView.Filter = null;

            }
        }
        private string _FilterSearchText;
        public string FilterSearchText
        {
            get
            {
                return this._FilterSearchText;
            }
            set
            {
                this.Set(ref this._FilterSearchText, value);
               this.FilterView.Refresh();
            }
        }
        private bool _FilterPopupOpen;
        public bool FilterPopupOpen
        {
            get
            {
                return this._FilterPopupOpen;
            }
            set
            {
                this.Set(ref this._FilterPopupOpen, value);
            }
        }
        private MyObservableCollection<CheckedListItem<DateTime>> _FilterDates;
        public MyObservableCollection<CheckedListItem<DateTime>> FilterDates
        {
            get
            {
                return this._FilterDates;
            }
            private set
            {
                this.Set(ref this._FilterDates, value);
            }
        }

        private MyObservableCollection<CheckedListItem<string>> _FilterItems;
        public MyObservableCollection<CheckedListItem<string>> FilterItems
        {
            get
            {
                return this._FilterItems;
            }
            private set
            {
                this.Set(ref this._FilterItems, value);
            }
        }

        private MyObservableCollection<CheckedListItem<string>> _FilterPayees;
        public MyObservableCollection<CheckedListItem<string>> FilterPayees
        {
            get
            {
                return this._FilterPayees;
            }
            private set
            {
                this.Set(ref this._FilterPayees, value);
            }
        }
        private MyObservableCollection<CheckedListItem<decimal>> _FilterAmount;
        public MyObservableCollection<CheckedListItem<decimal>> FilterAmounts
        {
            get
            {
                return this._FilterAmount;
            }
            private set
            {
                this.Set(ref this._FilterAmount, value);
            }
        }
        private MyObservableCollection<CheckedListItem<Category>> _FilterCategories;
        public MyObservableCollection<CheckedListItem<Category>> FilterCategories
        {
            get
            {
                return this._FilterCategories;
            }
            private set
            {
                this.Set(ref this._FilterCategories, value);
            }
        }
        private MyObservableCollection<CheckedListItem<PaymentMethod>> _FilterPaymentMethods;
        public MyObservableCollection<CheckedListItem<PaymentMethod>> FilterPaymentMethods
        {
            get
            {
                return this._FilterPaymentMethods;
            }
            private set
            {
                this.Set(ref this._FilterPaymentMethods, value);
            }
        }
        private MyObservableCollection<CheckedListItem<string>> _FilterComments;
        public MyObservableCollection<CheckedListItem<string>> FilterComments
        {
            get
            {
                return this._FilterComments;
            }
            private set
            {
                this.Set(ref this._FilterComments, value);
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
        private string activeColumnName;
        #endregion

        #region Public Methods

        #endregion
        #region Private Methods
        /// <summary>
        /// Filter for the transactions tab. Filtering is based on which items are checked
        /// </summary>
        /// <param name="sender">The object requesting filtering</param>
        /// <param name="e">The arguments</param>
        private bool TransactionsView_Filter(Transaction transaction)
        {
            if (this.FilterDates.Where(x => x.IsChecked).Count(x => x.Item.Equals(transaction.Date)) == 0)
                return false;
            if (this.FilterItems.Where(x => x.IsChecked).Count(x => x.Item.Equals(transaction.Item)) == 0)
                return false;
            if (this.FilterPayees.Where(x => x.IsChecked).Count(x => x.Item.Equals(transaction.Payee)) == 0)
                return false;
            if (this.FilterAmounts.Where(x => x.IsChecked).Count(x => x.Item.Equals(transaction.Amount)) == 0)
                return false;
            if (transaction.Category != null && this.FilterCategories.Where(x => x.IsChecked).Count(x => x.Item.Equals(transaction.Category)) == 0)
                return false;
            if (transaction.PaymentMethod != null && this.FilterPaymentMethods.Where(x => x.IsChecked).Count(x => x.Item.Equals(transaction.PaymentMethod)) == 0)
                return false;
            if (this.FilterComments.Where(x => x.IsChecked).Count(x => x.Item.Equals(transaction.Comment)) == 0)
                return false;
            return true;
        }
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
        private void Transactions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Transaction transaction in e.NewItems)
                {
                    if (this.FilterDates.Where(x => x.Item.Equals(transaction.Date)).Count() == 0)
                        this.FilterDates.Add(new CheckedListItem<DateTime>(transaction.Date, true));
                    if (this.FilterItems.Where(x => x.Item.Equals(transaction.Item)).Count() == 0)
                        this.FilterItems.Add(new CheckedListItem<string>(transaction.Item, true));
                    if (this.FilterPayees.Where(x => x.Item.Equals(transaction.Payee)).Count() == 0)
                        this.FilterPayees.Add(new CheckedListItem<string>(transaction.Payee, true));
                    if (this.FilterAmounts.Where(x => x.Item.Equals(transaction.Amount)).Count() == 0)
                        this.FilterAmounts.Add(new CheckedListItem<decimal>(transaction.Amount, true));
                    if (this.FilterCategories.Where(x => x.Item.Equals(transaction.Category)).Count() == 0 && transaction.Category != null)
                        this.FilterCategories.Add(new CheckedListItem<Category>(transaction.Category, true));
                    if (this.FilterPaymentMethods.Where(x => x.Item.Equals(transaction.PaymentMethod)).Count() == 0 && transaction.PaymentMethod != null)
                        this.FilterPaymentMethods.Add(new CheckedListItem<PaymentMethod>(transaction.PaymentMethod, true));
                    if (this.FilterComments.Where(x => x.Item.Equals(transaction.Comment)).Count() == 0)
                        this.FilterComments.Add(new CheckedListItem<string>(transaction.Comment, true));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Transaction transaction in e.OldItems)
                {
                    int count = 0;
                    count = this.Transactions.Count(x => x.Date.Equals(transaction.Date));
                    if (count < 1)
                    {
                        var itemsToRemove = this.FilterDates.Where(x => x.Item.Equals(transaction.Date)).ToList();
                        foreach (var itemToRemove in itemsToRemove)
                            this.FilterDates.Remove(itemToRemove);
                    }
                    count = this.Transactions.Count(x => x.Item.Equals(transaction.Item));
                    if (count < 1)
                    {
                        var itemsToRemove = this.FilterItems.Where(x => x.Item.Equals(transaction.Item)).ToList();
                        foreach (var itemToRemove in itemsToRemove)
                            this.FilterItems.Remove(itemToRemove);
                    }
                    count = this.Transactions.Count(x => x.Payee.Equals(transaction.Payee));
                    if (count < 1)
                    {
                        var itemsToRemove = this.FilterPayees.Where(x => x.Item.Equals(transaction.Payee)).ToList();
                        foreach (var itemToRemove in itemsToRemove)
                            this.FilterPayees.Remove(itemToRemove);
                    }
                    count = this.Transactions.Count(x => x.Amount.Equals(transaction.Amount));
                    if (count < 1)
                    {
                        var itemsToRemove = this.FilterAmounts.Where(x => x.Item.Equals(transaction.Amount)).ToList();
                        foreach (var itemToRemove in itemsToRemove)
                            this.FilterAmounts.Remove(itemToRemove);
                    }
                    count = this.Transactions.Count(x => x.Category.Equals(transaction.Category));
                    if (count < 1)
                    {
                        var itemsToRemove = this.FilterCategories.Where(x => x.Item.Equals(transaction.Category)).ToList();
                        foreach (var itemToRemove in itemsToRemove)
                            this.FilterCategories.Remove(itemToRemove);
                    }
                    count = this.Transactions.Count(x => x.PaymentMethod.Equals(transaction.PaymentMethod));
                    if (count < 1)
                    {
                        var itemsToRemove = this.FilterPaymentMethods.Where(x => x.Item.Equals(transaction.PaymentMethod)).ToList();
                        foreach (var itemToRemove in itemsToRemove)
                            this.FilterPaymentMethods.Remove(itemToRemove);
                    }
                    count = this.Transactions.Count(x => x.Comment.Equals(transaction.Comment));
                    if (count < 1)
                    {
                        var itemsToRemove = this.FilterComments.Where(x => x.Item.Equals(transaction.Comment)).ToList();
                        foreach (var itemToRemove in itemsToRemove)
                            this.FilterComments.Remove(itemToRemove);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                List<CheckedListItem<DateTime>> tempDates = new List<CheckedListItem<DateTime>>();
                List<CheckedListItem<string>> tempItems = new List<CheckedListItem<string>>();
                List<CheckedListItem<string>> tempPayees = new List<CheckedListItem<string>>();
                List<CheckedListItem<decimal>> tempAmounts = new List<CheckedListItem<decimal>>();
                List<CheckedListItem<Category>> tempCategories = new List<CheckedListItem<Category>>();
                List<CheckedListItem<PaymentMethod>> tempPaymentMethods = new List<CheckedListItem<PaymentMethod>>();
                List<CheckedListItem<string>> tempComments = new List<CheckedListItem<string>>();
                foreach (Transaction transaction in this.Transactions)
                {
                    if (tempDates.Where(x => x.Item.Equals(transaction.Date)).Count() == 0)
                        tempDates.Add(new CheckedListItem<DateTime>(transaction.Date, true));
                    if (tempItems.Where(x => x.Item.Equals(transaction.Item)).Count() == 0)
                        tempItems.Add(new CheckedListItem<string>(transaction.Item, true));
                    if (tempPayees.Where(x => x.Item.Equals(transaction.Payee)).Count() == 0)
                        tempPayees.Add(new CheckedListItem<string>(transaction.Payee, true));
                    if (tempAmounts.Where(x => x.Item.Equals(transaction.Amount)).Count() == 0)
                        tempAmounts.Add(new CheckedListItem<decimal>(transaction.Amount, true));
                    if (tempCategories.Where(x => x.Item.Equals(transaction.Category)).Count() == 0)
                        tempCategories.Add(new CheckedListItem<Category>(transaction.Category, true));
                    if (tempPaymentMethods.Where(x => x.Item.Equals(transaction.PaymentMethod)).Count() == 0)
                        tempPaymentMethods.Add(new CheckedListItem<PaymentMethod>(transaction.PaymentMethod, true));
                    if (tempComments.Where(x => x.Item.Equals(transaction.Comment)).Count() == 0)
                        tempComments.Add(new CheckedListItem<string>(transaction.Comment, true));
                }
                this.FilterDates.InsertRange(tempDates);
                this.FilterItems.InsertRange(tempItems);
                this.FilterPayees.InsertRange(tempPayees);
                this.FilterAmounts.InsertRange(tempAmounts);
                this.FilterCategories.InsertRange(tempCategories);
                this.FilterPaymentMethods.InsertRange(tempPaymentMethods);
                this.FilterComments.InsertRange(tempComments);
            }
            this.UpdateItemsList();
            this.UpdatePayeesList();
            this.TransactionsView.Refresh();
        }

        private void Transaction_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Transaction transaction = sender as Transaction;
            if (e.PropertyName.Equals("Date"))
            {
                if (this.FilterDates.Where(x => x.Item.Equals(transaction.Date)).Count() == 0)
                    this.FilterDates.Add(new CheckedListItem<DateTime>(transaction.Date, true));
                var itemsToRemove = this.FilterDates.Where(y => (this.FilterDates.Select(x => x.Item).Except(this.Transactions.Select(x => x.Date).Distinct())).Contains(y.Item)).ToList();
                foreach (var itemToRemove in itemsToRemove)
                    this.FilterDates.Remove(itemToRemove);
            }
            else if (e.PropertyName.Equals("Item"))
            {
                if (this.FilterItems.Where(x => x.Item.Equals(transaction.Item)).Count() == 0)
                    this.FilterItems.Add(new CheckedListItem<string>(transaction.Item, true));
                var itemsToRemove = this.FilterItems.Where(y => (this.FilterItems.Select(x => x.Item).Except(this.Transactions.Select(x => x.Item).Distinct())).Contains(y.Item)).ToList();
                foreach (var itemToRemove in itemsToRemove)
                    this.FilterItems.Remove(itemToRemove);
                this.UpdateItemsList();
            }
            else if (e.PropertyName.Equals("Payee"))
            {
                if (this.FilterPayees.Where(x => x.Item.Equals(transaction.Payee)).Count() == 0)
                    this.FilterPayees.Add(new CheckedListItem<string>(transaction.Payee, true));
                var itemsToRemove = this.FilterPayees.Where(y => (this.FilterPayees.Select(x => x.Item).Except(this.Transactions.Select(x => x.Payee).Distinct())).Contains(y.Item)).ToList();
                foreach (var itemToRemove in itemsToRemove)
                    this.FilterPayees.Remove(itemToRemove);
                this.UpdatePayeesList();
            }
            else if (e.PropertyName.Equals("Amount"))
            {
                if (this.FilterAmounts.Where(x => x.Item.Equals(transaction.Amount)).Count() == 0)
                    this.FilterAmounts.Add(new CheckedListItem<decimal>(transaction.Amount, true));
                var itemsToRemove = this.FilterAmounts.Where(y => (this.FilterAmounts.Select(x => x.Item).Except(this.Transactions.Select(x => x.Amount).Distinct())).Contains(y.Item)).ToList();
                foreach (var itemToRemove in itemsToRemove)
                    this.FilterAmounts.Remove(itemToRemove);
            }
            else if (e.PropertyName.Equals("Category"))
            {
                if (this.FilterCategories.Where(x => x.Item.Equals(transaction.Category)).Count() == 0)
                    this.FilterCategories.Add(new CheckedListItem<Category>(transaction.Category, true));
                var itemsToRemove = this.FilterCategories.Where(y => (this.FilterCategories.Select(x => x.Item).Except(this.Transactions.Select(x => x.Category).Distinct())).Contains(y.Item)).ToList();
                foreach (var itemToRemove in itemsToRemove)
                    this.FilterCategories.Remove(itemToRemove);
            }
            else if (e.PropertyName.Equals("PaymentMethod"))
            {
                if (this.FilterPaymentMethods.Where(x => x.Item.Equals(transaction.PaymentMethod)).Count() == 0)
                    this.FilterPaymentMethods.Add(new CheckedListItem<PaymentMethod>(transaction.PaymentMethod, true));
                var itemsToRemove = this.FilterPaymentMethods.Where(y => (this.FilterPaymentMethods.Select(x => x.Item).Except(this.Transactions.Select(x => x.PaymentMethod).Distinct())).Contains(y.Item)).ToList();
                foreach (var itemToRemove in itemsToRemove)
                    this.FilterPaymentMethods.Remove(itemToRemove);
            }
            else if (e.PropertyName.Equals("Comment"))
            {
                if (this.FilterComments.Where(x => x.Item.Equals(transaction.Comment)).Count() == 0)
                    this.FilterComments.Add(new CheckedListItem<string>(transaction.Comment, true));
                var itemsToRemove = this.FilterComments.Where(y => (this.FilterComments.Select(x => x.Item).Except(this.Transactions.Select(x => x.Comment).Distinct())).Contains(y.Item)).ToList();
                foreach (var itemToRemove in itemsToRemove)
                    this.FilterComments.Remove(itemToRemove);
            }
        }
        private void FilterList_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.TransactionsView.Refresh();
        }
        private void FilterList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!this.TransactionsView.IsEditingItem)
                this.TransactionsView.Refresh();
        }
        private void UpdateItemsList()
        {
            this.Items = new List<string>(this.Transactions.Select(x => x.Item).Distinct());
        }
        private void UpdatePayeesList()
        {
            this.Payees = new List<string>(this.Transactions.Select(x => x.Payee).Distinct());
        }
        private void OpenPopup(string columnName)
        {
            switch(columnName)
            {
                case "Date":
                    this.FilterView = new ListCollectionView(this.FilterDates);
                    break;
                case "Item":
                    this.FilterView = new ListCollectionView(this.FilterItems);
                    break;
                case "Payee":
                    this.FilterView = new ListCollectionView(this.FilterPayees);
                    break;
                case "Amount":
                    this.FilterView = new ListCollectionView(this.FilterAmounts);
                    break;
                case "Category":
                    this.FilterView = new ListCollectionView(this.FilterCategories);
                    break;
                case "Payment Method":
                    this.FilterView = new ListCollectionView(this.FilterPaymentMethods);
                    break;
                case "Comment":
                    this.FilterView = new ListCollectionView(this.FilterComments);
                    break;
                default:
                    break;
            }
            this.activeColumnName = columnName;
            this.FilterPopupOpen = true;
            this.FilterSearchText = "";
        }
        private void SelectFilters()
        {
            switch (activeColumnName)
            {
                case "Date":
                    this.FilterDates.DisableMemberChanged();
                    foreach (CheckedListItem<DateTime> item in this.FilterDates)
                        item.IsChecked = true;
                    this.FilterDates.EnableMemberChanged();
                    break;
                case "Item":
                    this.FilterItems.DisableMemberChanged();
                    foreach (CheckedListItem<string> item in this.FilterItems)
                        item.IsChecked = true;
                    this.FilterItems.EnableMemberChanged();
                    break;
                case "Payee":
                    this.FilterPayees.DisableMemberChanged();
                    foreach (CheckedListItem<string> item in this.FilterPayees)
                        item.IsChecked = true;
                    this.FilterPayees.EnableMemberChanged();
                    break;
                case "Amount":
                    this.FilterAmounts.DisableMemberChanged();
                    foreach (CheckedListItem<decimal> item in this.FilterAmounts)
                        item.IsChecked = true;
                    this.FilterAmounts.EnableMemberChanged();
                    break;
                case "Category":
                    this.FilterCategories.DisableMemberChanged();
                    foreach (CheckedListItem<Category> item in this.FilterCategories)
                        item.IsChecked = true;
                    this.FilterCategories.EnableMemberChanged();
                    break;
                case "Payment Method":
                    this.FilterPaymentMethods.DisableMemberChanged();
                    foreach (CheckedListItem<PaymentMethod> item in this.FilterPaymentMethods)
                        item.IsChecked = true;
                    this.FilterPaymentMethods.EnableMemberChanged();
                    break;
                case "Comment":
                    this.FilterComments.DisableMemberChanged();
                    foreach (CheckedListItem<string> item in this.FilterComments)
                        item.IsChecked = true;
                    this.FilterComments.EnableMemberChanged();
                    break;
                default:
                    break;
            }
            this.FilterView.Refresh();
            this.TransactionsView.Refresh();
        }
        private void DeselectFilters()
        {
            switch (activeColumnName)
            {
                case "Date":
                    this.FilterDates.DisableMemberChanged();
                    foreach (CheckedListItem<DateTime> item in this.FilterDates)
                        item.IsChecked = false;
                    this.FilterDates.EnableMemberChanged();
                    break;
                case "Item":
                    this.FilterItems.DisableMemberChanged();
                    foreach (CheckedListItem<string> item in this.FilterItems)
                        item.IsChecked = false;
                    this.FilterItems.EnableMemberChanged();
                    break;
                case "Payee":
                    this.FilterPayees.DisableMemberChanged();
                    foreach (CheckedListItem<string> item in this.FilterPayees)
                        item.IsChecked = false;
                    this.FilterPayees.EnableMemberChanged();
                    break;
                case "Amount":
                    this.FilterAmounts.DisableMemberChanged();
                    foreach (CheckedListItem<decimal> item in this.FilterAmounts)
                        item.IsChecked = false;
                    this.FilterAmounts.EnableMemberChanged();
                    break;
                case "Category":
                    this.FilterCategories.DisableMemberChanged();
                    foreach (CheckedListItem<Category> item in this.FilterCategories)
                        item.IsChecked = false;
                    this.FilterCategories.EnableMemberChanged();
                    break;
                case "Payment Method":
                    this.FilterPaymentMethods.DisableMemberChanged();
                    foreach (CheckedListItem<PaymentMethod> item in this.FilterPaymentMethods)
                        item.IsChecked = false;
                    this.FilterPaymentMethods.EnableMemberChanged();
                    break;
                case "Comment":
                    this.FilterComments.DisableMemberChanged();
                    foreach (CheckedListItem<string> item in this.FilterComments)
                        item.IsChecked = false;
                    this.FilterComments.EnableMemberChanged();
                    break;
                default:
                    break;
            }
            this.FilterView.Refresh();
            this.TransactionsView.Refresh();
        }
        private bool FilterView_Filter<T>(CheckedListItem<T> item)
        {
            if (this.FilterSearchText == null || this.FilterSearchText.Equals(""))
                return true;
            return item.Item.ToString().ToLower().Contains(this.FilterSearchText.ToLower());
        }
        #endregion
    }
}