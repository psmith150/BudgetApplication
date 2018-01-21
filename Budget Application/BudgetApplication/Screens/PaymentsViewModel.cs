﻿using System.Windows.Input;
using BudgetApplication.Popups;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Data;
using System.Collections.ObjectModel;
using BudgetApplication.Model;
using System;
using System.Windows;
using System.Diagnostics;

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

            this.PaymentTransactionsView = new ListCollectionView(session.Transactions);
            this.PaymentTransactionsView.SortDescriptions.Add(new System.ComponentModel.SortDescription("Date", System.ComponentModel.ListSortDirection.Ascending));
            this.PaymentTransactionsView.Filter = ((transaction) => PaymentTransactions_Filter(transaction as Transaction));

            //Default to showing all transactions in the last month
            CheckingAccount _allPayments = new CheckingAccount("All");
            _allPayments.StartDate = DateTime.Now.AddMonths(-1);
            _allPayments.EndDate = DateTime.Now;
            _allPaymentsCollection = new ObservableCollection<PaymentMethod>();
            _allPaymentsCollection.Add(_allPayments);
        }

        public override void Initialize()
        {
        }

        public override void Deinitialize()
        {
        }

        #region Public Properties

        private PaymentMethod _selectedPaymentMethod;
        public PaymentMethod SelectedPaymentMethod
        {
            get
            {
                return _selectedPaymentMethod;
            }
            set
            {
                this.Set(ref this._selectedPaymentMethod, value);
                //Debug.Write(PaymentStartDate.ToString());
                this.SelectedStartDate = this.SelectedPaymentMethod.StartDate;
                this.SelectedEndDate = this.SelectedPaymentMethod.EndDate;
                //Debug.WriteLine($"Payment method has start date {this.SelectedStartDate} and end date {this.SelectedEndDate}");
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
                this.Set(ref this._selectedStartDate, value);
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
                this.Set(ref this._selectedEndDate, value);
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
                this.Set(ref this._creditLimit, value);
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
                this.Set(ref this._totalBill, value);
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
                this.Set(ref this._netBill, value);
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
                this.Set(ref this._remainingCredit, value);
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
                this.Set(ref this._creditRowHeight, value);
            }
        }
        #endregion

        #region Private Methods

        private bool PaymentTransactions_Filter(Transaction transaction)
        {
            if (transaction != null && transaction.PaymentMethod != null && this.SelectedPaymentMethod != null)
            {
                //Debug.WriteLine(this.PaymentSelector.SelectedIndex);
                //Debug.WriteLine((PaymentSelector.SelectedItem).ToString());
                if ((this.SelectedPaymentMethod.Name.Equals(transaction.PaymentMethod.Name) || this.SelectedPaymentMethod == this.AllPaymentsCollection[0])
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
                //Debug.WriteLine(view.Count);
                //Debug.WriteLine((view.GetItemAt(0) as Transaction).Amount);
                for (int i = 0; i < this.PaymentTransactionsView.Count - 1; i++)
                {
                    Transaction transaction = this.PaymentTransactionsView.GetItemAt(i) as Transaction;
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
        #endregion
    }
}