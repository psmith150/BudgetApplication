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
                _category.PropertyChanged += CategoryModified;
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

        private void CategoryModified(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Category");
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    public abstract class PaymentMethod : INotifyPropertyChanged
    {
        private String _name;
        public enum Type { CreditCard, CheckingAccount };

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
                    NotifyPropertyChanged("Name");
                }
            }
        }

        abstract public Type PaymentType();

        public override String ToString()
        {
            return (this.PaymentType() + "," + this.Name);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    public class CreditCard : PaymentMethod
    {
        private decimal _creditLimit;
        private decimal _remainingCredit;
        
        public CreditCard(String name, decimal creditLimit = 300)
        {
            this.Name = name;
            if (creditLimit <= 0)
            {
                creditLimit = 0;
            }
            _creditLimit = creditLimit;
        }

        public override Type PaymentType()
        {
            return Type.CreditCard;
        }

        public decimal CreditLimit
        {
            get
            {
                return _creditLimit;
            }
            set
            {
                _creditLimit = value;
                NotifyPropertyChanged("CreditLimit");
            }
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
                    NotifyPropertyChanged("RemainingCredit");
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

        public override Type PaymentType()
        {
            return Type.CheckingAccount;
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
                    NotifyPropertyChanged("CheckNumber");
                }
            }
        }
    }

}