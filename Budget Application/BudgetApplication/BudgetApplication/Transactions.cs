using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BudgetApplication.Model
{
    public class Transaction : INotifyPropertyChanged
    {
        private DateTime _date;
        private String _item;
        private String _payee;
        private decimal _amount;
        private Category _category;
        private String _comment;
        private PaymentMethod _paymentMethod;

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

        public Category Category
        {
            get
            {
                return _category;
            }
            set
            {
                //TODO: check if category is valid
                _category = value;
                NotifyPropertyChanged("Category");
            }
        }
        
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

        //TODO
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

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    public abstract class PaymentMethod
    {
        private String _name;

        public String Name
        {
            get
            {
                return String.Copy(_name);
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name = String.Copy(value);
                }
            }
        }

        public override String ToString()
        {
            return this.Name;
        }
    }

    public class CreditCard : PaymentMethod
    {
        private decimal _creditLimit;
        private decimal _remainingCredit;
        
        public CreditCard(String name)
        {
            this.Name = name;
            _creditLimit = 300;
        }

        public CreditCard(String name, decimal creditLimit)
        {
            if (creditLimit <= 0)
            {
                creditLimit = 0;
            }
            _creditLimit = creditLimit;
        }

        public decimal RemainingCredit
        {
            get
            {
                return _remainingCredit;
            }
            set
            {
                if (value >= 0)
                {
                    _remainingCredit = value;
                }
            }
        }
    }

    public class CheckingAccount : PaymentMethod
    {
        private int _checkNumber;
        public CheckingAccount(String name)
        {
            this.Name = name;
        }

        public int CheckNumber
        {
            get
            {
                return _checkNumber;
            }
            set
            {
                if (value >= 0)
                {
                    _checkNumber = value;
                }
            }
        }
    }

}