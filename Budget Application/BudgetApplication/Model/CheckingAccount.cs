using System;
using System.ComponentModel;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Class to represent a Checking Account used for transactions.
    /// </summary>
    [Serializable]
    public class CheckingAccount : PaymentMethod
    {
        private int _accountNumber; //Bank account number
        private string _bank;   //Name of the bank

        /// <summary>
        /// Null parameter constructor for creating new instances automatically.
        /// </summary>
        public CheckingAccount() : base()
        {
            _accountNumber = 0;
            _bank = "";
        }

        /// <summary>
        /// Instantiates a new CheckingAccount with the given name.
        /// </summary>
        /// <param name="name">The name of the account</param>
        public CheckingAccount(String name) : base(name)
        {
            _accountNumber = 0;
            _bank = "";
        }

        /// <summary>
        /// Payment type
        /// </summary>
        [Browsable(false)]
        public override Type PaymentType
        {
            get
            {
                return Type.CheckingAccount;
            }
        }

        /// <summary>
        /// The account number of the account.
        /// </summary>
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

        /// <summary>
        /// The name of the bank the account is at.
        /// </summary>
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
}
