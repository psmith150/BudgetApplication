using System;
using System.ComponentModel;
using System.Diagnostics;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Class to represent a payment transaction.
    /// </summary>
    public class Transaction : INotifyPropertyChanged
    {
        private DateTime _date; //Transaction date
        private String _item;   //String used to identify purchased item
        private String _payee;  //String used to identify where item was purchased
        private decimal _amount;    //US dollar amount of purchase
        private Category _category; //The category used to classify the payment
        private String _comment;    //String used to store user comments
        private PaymentMethod _paymentMethod;   //The payment method used for the purchase

        /// <summary>
        /// Instantiates a new transaction object. Null parameter constructor allows use of insertion row in datagrid.
        /// Generates default values for all fields.
        /// </summary>
        public Transaction()
        {
            _date = DateTime.Today;
            _item = "";
            _payee = "";
            _amount = 0;
            _category = null;
            _comment = "";
            _paymentMethod = null;
        }

        #region Getters and setters

        /// <summary>
        /// The purchase date.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                NotifyPropertyChanged("Date");
            }
        }

        /// <summary>
        /// The item purchased.
        /// </summary>
        public String Item
        {
            get
            {
                return String.Copy(_item);
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _item = String.Copy(value);
                    NotifyPropertyChanged("Item");
                }
            }
        }

        /// <summary>
        /// The vendor the item was purchased from.
        /// </summary>
        public String Payee
        {
            get
            {
                return String.Copy(_payee);
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _payee = String.Copy(value);
                    NotifyPropertyChanged("Payee");
                }
            }
        }

        /// <summary>
        /// The amount of the purchase.
        /// It shoud be noted that the default amount is positive. For example, if the transaction is part of an expenditure group, a positive value represents money spent.
        /// If it is part of an income group, a positive value is money gained.
        /// </summary>
        public decimal Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
                NotifyPropertyChanged("Amount");
            }
        }

        /// <summary>
        /// The category the purchase falls under.
        /// </summary>
        public Category Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
                //_category.PropertyChanged += CategoryModified;
                NotifyPropertyChanged("Category");
            }
        }

        /// <summary>
        /// User comments.
        /// </summary>
        public String Comment
        {
            get
            {
                return String.Copy(_comment);
            }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                _comment = value;
                NotifyPropertyChanged("Comment");
            }
        }

        /// <summary>
        /// The payment method used
        /// </summary>
        public PaymentMethod PaymentMethod
        {
            get
            {
                return _paymentMethod;
            }
            set
            {
                _paymentMethod = value;
                NotifyPropertyChanged("PaymentMethod");
            }
        }

        #endregion

        /// <summary>
        /// Implementation of INotifyPropertyChanged
        /// </summary>
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        /// <summary>
        /// 1/4/2017: Believed to no longer be needed due to outside changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryModified(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Category");
        }

        /// <summary>
        /// Helper function to simplify raising the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">The property that has been changed</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
