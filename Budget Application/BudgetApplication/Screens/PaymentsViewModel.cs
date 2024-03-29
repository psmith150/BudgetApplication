﻿using System.Windows.Input;
using BudgetApplication.Popups;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.Command;
using System.Windows.Data;
using System.Collections.ObjectModel;
using BudgetApplication.Model;
using System;
using System.Windows;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Linq;
using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;

namespace BudgetApplication.Screens
{
    public class PaymentsViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;

        public PaymentsViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;
            this.PaymentMethods = session.PaymentMethods;
            this.Categories = session.Categories;
            this.Transactions = session.Transactions;
            this.Transactions.CollectionChanged += TransactionCollectionChanged;

            this.PaymentTransactionsView = new ListCollectionView(session.Transactions);
            this.PaymentTransactionsView.SortDescriptions.Add(new System.ComponentModel.SortDescription("Date", System.ComponentModel.ListSortDirection.Descending));
            this.PaymentTransactionsView.Filter = ((transaction) => PaymentTransactions_Filter(transaction as Transaction));


            this.Session.Transactions.MemberChanged += TransactionPropertyChanged;

            //Default to showing all transactions in the last month
            CheckingAccount _allPayments = new CheckingAccount("All");
            _allPayments.StartDate = DateTime.Now.AddMonths(-1);
            _allPayments.EndDate = DateTime.Now;
            _allPaymentsCollection = new ObservableCollection<PaymentMethod>();
            _allPaymentsCollection.Add(_allPayments);

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

        private PaymentMethod _selectedPaymentMethod;
        public PaymentMethod SelectedPaymentMethod
        {
            get
            {
                return _selectedPaymentMethod;
            }
            set
            {
                this.SetProperty(ref this._selectedPaymentMethod, value);
                this._selectedPaymentMethod.PropertyChanged += ((o,a) => RecalculateCreditValues());
                this.SelectedStartDate = this.SelectedPaymentMethod.StartDate;
                this.SelectedEndDate = this.SelectedPaymentMethod.EndDate;
                this.PaymentTransactionsView.Refresh();
                RecalculateCreditValues();
            }
        }

        private DateTime _selectedStartDate;
        public DateTime SelectedStartDate
        {
            get
            {
                return _selectedStartDate;
            }
            set
            {
                this.SetProperty(ref this._selectedStartDate, value);
                if (this.SelectedPaymentMethod != this.AllPaymentsCollection[0])
                    (this.SelectedPaymentMethod as PaymentMethod).StartDate = this.SelectedStartDate;
                this.PaymentTransactionsView.Refresh();
                RecalculateCreditValues();
            }
        }
        private DateTime _selectedEndDate;
        public DateTime SelectedEndDate
        {
            get
            {
                return _selectedEndDate;
            }
            set
            {
                this.SetProperty(ref this._selectedEndDate, value);
                if (this.SelectedPaymentMethod != this.AllPaymentsCollection[0])
                    (this.SelectedPaymentMethod as PaymentMethod).EndDate = this.SelectedEndDate;
                this.PaymentTransactionsView.Refresh();
                RecalculateCreditValues();
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

        private ListCollectionView _paymentTransactionsView;
        public ListCollectionView PaymentTransactionsView
        {
            get
            {
                return _paymentTransactionsView;
            }
            private set
            {
                _paymentTransactionsView = value;
            }
        }
        private MyObservableCollection<Transaction> _Transactions;
        public MyObservableCollection<Transaction> Transactions
        {
            get
            {
                return this._Transactions;
            }
            private set
            {
                this.SetProperty(ref this._Transactions, value);
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
                this.SetProperty(ref this._SelectedTransaction, value);
            }
        }

        //The collection that contains the All Payments payment method
        private ObservableCollection<PaymentMethod> _allPaymentsCollection;
        public ObservableCollection<PaymentMethod> AllPaymentsCollection
        {
            get
            {
                return _allPaymentsCollection;
            }
        }

        private string _creditLimit;
        public string CreditLimit
        {
            get
            {
                return _creditLimit;
            }
            private set
            {
                this.SetProperty(ref this._creditLimit, value);
            }
        }

        private string _totalBill;
        public string TotalBill
        {
            get
            {
                return _totalBill;
            }
            private set
            {
                this.SetProperty(ref this._totalBill, value);
            }
        }

        private string _netBill;
        public string NetBill
        {
            get
            {
                return _netBill;
            }
            private set
            {
                this.SetProperty(ref this._netBill, value);
            }
        }

        private string _remainingCredit;
        public string RemainingCredit
        {
            get
            {
                return _remainingCredit;
            }
            private set
            {
                this.SetProperty(ref this._remainingCredit, value);
            }
        }

        private GridLength _creditRowHeight;
        public GridLength CreditRowHeight
        {
            get
            {
                return _creditRowHeight;
            }
            private set
            {
                this.SetProperty(ref this._creditRowHeight, value);
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
                this.SetProperty(ref this._Items, value);
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
                this.SetProperty(ref this._Payees, value);
            }
        }
        #endregion

        #region Private Methods
        private void AddTransaction()
        {
            Transaction newTransaction = new Transaction();
            if (this.SelectedPaymentMethod != null && this.SelectedPaymentMethod != this.AllPaymentsCollection[0])
                newTransaction.PaymentMethod = this.SelectedPaymentMethod;
            this.Transactions.Add(newTransaction);
            //Messenger.Default.Send(new TransactionMessage(newTransaction));
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
                //Messenger.Default.Send(new TransactionMessage(newTransaction));
            }
        }
        private bool PaymentTransactions_Filter(Transaction transaction)
        {
            if (transaction != null && this.SelectedPaymentMethod != null)
            {
                if ((this.SelectedPaymentMethod == this.AllPaymentsCollection[0] || (transaction.PaymentMethod != null && this.SelectedPaymentMethod.Name.Equals(transaction.PaymentMethod.Name)))
                    && this.SelectedStartDate <= transaction.Date && this.SelectedEndDate > transaction.Date)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Calculates the values that show information about a CreditCard object
        /// </summary>
        private void RecalculateCreditValues()
        {
            CreditCard card = this.SelectedPaymentMethod as CreditCard;
            if (card != null)
            {
                this.CreditRowHeight = new GridLength(1, GridUnitType.Auto);  //Shows the detail row
                this.CreditLimit = card.CreditLimit.ToString("C");    //Shows the card's credit limit
                decimal sum = 0;
                for (int i = 0; i < this.PaymentTransactionsView.Count; i++)
                {
                    Transaction transaction = this.PaymentTransactionsView.GetItemAt(i) as Transaction;
                    Group transactionGroup = this.Session.Groups.FirstOrDefault(x => x.Categories.Contains(transaction.Category));
                    if (transactionGroup != null && transactionGroup.IsIncome)
                        sum -= transaction.Amount;
                    else
                        sum += transaction.Amount;
                }
                this.TotalBill = sum.ToString("C");   //Shows the amount owed in the given data range.
                this.NetBill = (sum - card.PaymentAmount).ToString("C");  //Shows the total amount owed, given existing payments
                this.RemainingCredit = (Decimal.Parse(this.CreditLimit, System.Globalization.NumberStyles.Currency) - Decimal.Parse(this.NetBill, System.Globalization.NumberStyles.Currency)).ToString("C");   //Shows the remaining credit
            }
            else
            {
                this.CreditRowHeight = new GridLength(0); //Hides the detail row
            }
        }

        private void TransactionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //this.PaymentTransactionsView.Refresh();
            this.RecalculateCreditValues();
            this.UpdateItemsList();
            this.UpdatePayeesList();
        }

        private void TransactionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.RecalculateCreditValues();
            Transaction transaction = sender as Transaction;
            if (e.PropertyName.Equals("Item"))
            {
                this.UpdateItemsList();
            }
            else if (e.PropertyName.Equals("Payee"))
            {
                this.UpdatePayeesList();
            }
            if (!this.PaymentTransactionsView.IsEditingItem)
            {
                this.PaymentTransactionsView.Refresh();
            }
        }

        private void UpdateItemsList()
        {
            this.Items = new List<string>(this.Transactions.Select(x => x.Item).Distinct());
        }
        private void UpdatePayeesList()
        {
            this.Payees = new List<string>(this.Transactions.Select(x => x.Payee).Distinct());
        }
        #endregion
    }
}