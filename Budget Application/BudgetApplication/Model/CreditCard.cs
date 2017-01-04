using System;
using System.ComponentModel;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Represents a credit card use for transactions.
    /// </summary>
    [Serializable]
    public class CreditCard : PaymentMethod
    {
        private decimal _creditLimit;   //The line of credit associated with the card.

        /// <summary>
        /// Null parameter constructor for creating new instances automatically.
        /// </summary>
        public CreditCard() : base()
        {
            _creditLimit = 300;
        }

        /// <summary>
        /// Instantiates a new CreditCard object with the given name and credit limit
        /// </summary>
        /// <param name="name"></param>
        /// <param name="creditLimit"></param>
        public CreditCard(String name, decimal creditLimit = 300) : base(name)
        {
            if (creditLimit <= 0)
            {
                creditLimit = 0;
            }
            _creditLimit = creditLimit;
        }

        /// <summary>
        /// Payment type
        /// </summary>
        [Browsable(false)]
        public override Type PaymentType
        {
            get
            {
                return Type.CreditCard;
            }
        }

        /// <summary>
        /// Credit limit
        /// </summary>
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
}
