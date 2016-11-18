using System;
using System.Collections.Generic;

namespace BudgetApplication.HelperClasses
{
    public class Transaction
    {
        private DateTime _date;
        private String _item;
        private String _payee;
        private decimal _price;
        private String _category;
        private String _comment;
        private PaymentMethod _paymentMethod;

        public Transaction(String item, String payee, decimal price, String category, DateTime? date = null, String comment = "", PaymentMethod paymentMethod = new CheckingAccount("DefaultChecking"))
        {
            if (date == null)
            {
                date = DateTime.Today;
            }
            _date = (DateTime) date;
            _item = item;
            _payee = payee;
            _price = price;
            _category = category;
            _comment = comment;
            _paymentMethod = paymentMethod;
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
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
                }
            }
        }

        public decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
            }
        }

        public String Category
        {
            get
            {
                return String.Copy(_category);
            }
            set
            {
                //TODO: check if category is valid
                _category = String.Copy(value);
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
            }
        }

        public PaymentMethod PaymentMethod
        {

        }
    }

    abstract class PaymentMethod
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
        private List<Transaction> _transactions;
        
        public CreditCard(decimal creditLimit)
        {
            if (creditLimit <= 0)
            {
                creditLimit = 0;
            }
            _creditLimit = creditLimit;
        }
    }

    public class CheckingAccount : PaymentMethod
    {
        public CheckingAccount(String name)
        {
            
        }
    }

}