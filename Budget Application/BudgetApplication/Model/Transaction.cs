using System;
using System.ComponentModel;
using GalaSoft.MvvmLight;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Class to represent a payment transaction.
    /// </summary>
    public class Transaction : ObservableObject
    {
        /// <summary>
        /// Instantiates a new transaction object. Null parameter constructor allows use of insertion row in datagrid.
        /// Generates default values for all fields.
        /// </summary>
        public Transaction() : this(DateTime.Today)
        {
        }

        public Transaction(DateTime? date = null, string item = "New item", string payee = "New payee", decimal amount = 0.0M, Category category = null, string comment = "", PaymentMethod paymentMethod = null)
        {
            this.Date = date ?? DateTime.Today;
            this.Item = item;
            this.Payee = payee;
            this.Amount = amount;
            this.Category = category;
            this.Comment = comment;
            this.PaymentMethod = paymentMethod;
        }

        #region Public Properties
        private DateTime _Date; //Transaction date
        /// <summary>
        /// The purchase date.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return this._Date;
            }
            set
            {
                this.Set(ref this._Date, value);
            }
        }
        private String _Item;   //String used to identify purchased item
        /// <summary>
        /// The item purchased.
        /// </summary>
        public String Item
        {
            get
            {
                return this._Item;
            }
            set
            {
                this.Set(ref this._Item, value);
            }
        }
        private String _Payee;  //String used to identify where item was purchased
        /// <summary>
        /// The vendor the item was purchased from.
        /// </summary>
        public String Payee
        {
            get
            {
                return this._Payee;
            }
            set
            {
                this.Set(ref this._Payee, value);
            }
        }
        private decimal _Amount;    //US dollar amount of purchase
        /// <summary>
        /// The amount of the purchase.
        /// It shoud be noted that the default amount is positive. For example, if the transaction is part of an expenditure group, a positive value represents money spent.
        /// If it is part of an income group, a positive value is money gained.
        /// </summary>
        public decimal Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                this.Set(ref this._Amount, value);
            }
        }
        private Category _Category; //The category used to classify the payment
        /// <summary>
        /// The category the purchase falls under.
        /// </summary>
        public Category Category
        {
            get
            {
                return _Category;
            }
            set
            {
                this.Set(ref this._Category, value);
            }
        }
        private String _Comment;    //String used to store user comments
        /// <summary>
        /// User comments.
        /// </summary>
        public String Comment
        {
            get
            {
                return this._Comment;
            }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                this.Set(ref this._Comment, value);
            }
        }
        private PaymentMethod _PaymentMethod;   //The payment method used for the purchase
        /// <summary>
        /// The payment method used
        /// </summary>
        public PaymentMethod PaymentMethod
        {
            get
            {
                return this._PaymentMethod;
            }
            set
            {
                this.Set(ref this._PaymentMethod, value);
            }
        }
        #endregion
        #region Public Methods
        public Transaction Copy()
        {
            Transaction newTransaction = new Transaction()
            {
                Date = this.Date,
                Item = this.Item,
                Payee = this.Payee,
                Amount = this.Amount,
                Category = this.Category,
                Comment = this.Comment,
                PaymentMethod = this.PaymentMethod
            };
            return newTransaction;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 1/4/2017: Believed to no longer be needed due to outside changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryModified(object sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged("Category");
        }
        #endregion
    }
}
