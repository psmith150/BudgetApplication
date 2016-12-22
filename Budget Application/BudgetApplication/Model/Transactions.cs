using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Xml.Serialization;

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

    [XmlInclude(typeof(CreditCard))]
    [XmlInclude(typeof(CheckingAccount))]
    public abstract class PaymentMethod : INotifyPropertyChanged
    {
        private String _name;
        private DateTime _startDate;
        private DateTime _endDate;

        public PaymentMethod(String name)
        {
            _name = name;
            _startDate = new DateTime();
            _endDate = DateTime.Today;
        }

        public enum Type
        {
            [Description("Credit Card")]
            CreditCard,
            [Description("Checking Account")]
            CheckingAccount
        };
        protected string[] _typeNames = { "Credit Card", "Checking Account" };

        [Browsable(false)]
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

        [Browsable(false)]
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
            }
        }

        [Browsable(false)]
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
            }
        }

        [Browsable(false)]
        abstract public Type PaymentType
        {
            get;
        }


        public override String ToString()
        {
            return (this.PaymentType + ", " + this.Name);
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
        
        public CreditCard(String name, decimal creditLimit = 300) : base(name)
        {
            if (creditLimit <= 0)
            {
                creditLimit = 0;
            }
            _creditLimit = creditLimit;
        }

        [Browsable(false)]
        public override Type PaymentType
        {
            get
            {
                return Type.CreditCard;
            }
        }
        [DisplayName("Credit Limit")]
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
    }

    public class CheckingAccount : PaymentMethod
    {
        private int _accountNumber;
        private string _bank;
        public CheckingAccount(String name) : base(name)
        {
            _accountNumber = 0;
            _bank = "";
        }

        [Browsable(false)]
        public override Type PaymentType
        {
            get
            {
                return Type.CheckingAccount;
            }
        }

        [DisplayName("Account Number")]
        public int AccountNumber
        {
            get
            {
                return _accountNumber;
            }
            set
            {
                if (value >= 0)
                {
                    _accountNumber = value;
                    NotifyPropertyChanged("AccountNumber");
                }
            }
        }

        [DisplayName("Bank Name")]
        public String Bank
        {
            get
            {
                return _bank;
            }
            set
            {
                _bank = value;
            }
        }
    }

    public class PaymentTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enum val = value as Enum;
            string description = GetEnumDescription(val);
            return description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }

        private string GetEnumDescription(Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            object[] attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
                return attrib.Description;
            }
        }
    }
}