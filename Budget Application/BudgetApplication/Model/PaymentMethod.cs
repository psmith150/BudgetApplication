using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Abstract class to represent a method of payment for transactions.
    /// </summary>
    [XmlInclude(typeof(CreditCard))]
    [XmlInclude(typeof(CheckingAccount))]
    [Serializable]
    public abstract class PaymentMethod : ObservableObject, IComparable
    {
        /// <summary>
        /// Null parameter constructor for creating new instances automatically.
        /// </summary>
        public PaymentMethod() : this("New Payment Method")
        {
        }

        /// <summary>
        /// Instantiates a new PaymentMethod object with the given name.
        /// </summary>
        /// <param name="name"></param>
        public PaymentMethod(String name)
        {
            _Name = name;
            _StartDate = new DateTime();
            _EndDate = DateTime.Today;
        }
        #region Public Properties
        /// <summary>
        /// Defines the enumerations for the type of payment.
        /// </summary>
        public enum Type
        {
            [Description("Credit Card")]
            CreditCard,
            [Description("Checking Account")]
            CheckingAccount
        };
        private String _Name;   //The name of the payment method
        /// <summary>
        /// The name of the payment method.
        /// </summary>
        [Browsable(false)]
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.Set(ref this._Name, value);
            }
        }
        private DateTime _StartDate;    //The start date of the date filter. Used to store data used on the Payments tab.
        /// <summary>
        /// The start date of the payment filter.
        /// </summary>
        [Browsable(false)]
        public DateTime StartDate
        {
            get
            {
                return this._StartDate;
            }
            set
            {
                this.Set(ref this._StartDate, value);
            }
        }
        private DateTime _EndDate;  //The end date of the date filter. Used to store data used on the Payments tab
        /// <summary>
        /// The end date of the payment filter.
        /// </summary>
        [Browsable(false)]
        public DateTime EndDate
        {
            get
            {
                return this._EndDate;
            }
            set
            {
                this.Set(ref this._EndDate, value);
            }
        }

        /// <summary>
        /// Gets the type of payment.
        /// </summary>
        [Browsable(false)]
        abstract public Type PaymentType
        {
            get;
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Overrides the ToString method
        /// </summary>
        /// <returns>The payment type concatenated with the name.</returns>
        public override String ToString()
        {
            return (this.PaymentType + ", " + this.Name);
        }
        #endregion
        #region Private Methods
        public int CompareTo(object obj)
        {
            return this.Name.CompareTo((obj as PaymentMethod).Name);
        }

        #endregion
    }


}
