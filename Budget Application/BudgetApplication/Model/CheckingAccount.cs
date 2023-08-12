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

        /// <summary>
        /// Null parameter constructor for creating new instances automatically.
        /// </summary>
        public CheckingAccount() : this("New Account")
        {
        }

        /// <summary>
        /// Instantiates a new CheckingAccount with the given name.
        /// </summary>
        /// <param name="name">The name of the account</param>
        public CheckingAccount(String name = "New Account") : base(name)
        {
            this.AccountNumber = 0;
            this.Bank = "";
        }
        #region Public Properties
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
        private int _AccountNumber; //Bank account number
        /// <summary>
        /// The account number of the account.
        /// </summary>
        [DisplayName("Account Number")]
        public int AccountNumber
        {
            get
            {
                return this._AccountNumber;
            }
            set
            {
                if (value >= 0)
                {
                    this.SetProperty(ref this._AccountNumber, value);
                }
            }
        }
        private string _Bank;   //Name of the bank
        /// <summary>
        /// The name of the bank the account is at.
        /// </summary>
        [DisplayName("Bank Name")]
        public String Bank
        {
            get
            {
                return this._Bank;
            }
            set
            {
                this.SetProperty(ref this._Bank, value);
            }
        }
        #endregion
        #region Public Methods
        public CheckingAccount Copy()
        {
            CheckingAccount copy = new CheckingAccount();
            copy.Name = this.Name;
            copy.AccountNumber = this.AccountNumber;
            copy.Bank = this.Bank;
            copy.StartDate = this.StartDate;
            copy.EndDate = this.EndDate;

            return copy;
        }
        #endregion
    }
}
