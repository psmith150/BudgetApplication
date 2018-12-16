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
    public abstract class PaymentMethod : INotifyPropertyChanged, IComparable
    {
        private String _name;   //The name of the payment method
        private DateTime _startDate;    //The start date of the date filter. Used to store data used on the Payments tab.
        private DateTime _endDate;  //The end date of the date filter. Used to store data used on the Payments tab

        /// <summary>
        /// Null parameter constructor for creating new instances automatically.
        /// </summary>
        public PaymentMethod()
        {
            _name = "New Payment Method";
            _startDate = new DateTime();
            _endDate = DateTime.Today;
        }

        /// <summary>
        /// Instantiates a new PaymentMethod object with the given name.
        /// </summary>
        /// <param name="name"></param>
        public PaymentMethod(String name)
        {
            _name = name;
            _startDate = new DateTime();
            _endDate = DateTime.Today;
        }

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
        
        /// <summary>
        /// The name of the payment method.
        /// </summary>
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

        /// <summary>
        /// The start date of the payment filter.
        /// </summary>
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

        /// <summary>
        /// The end date of the payment filter.
        /// </summary>
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

        /// <summary>
        /// Gets the type of payment.
        /// </summary>
        [Browsable(false)]
        abstract public Type PaymentType
        {
            get;
        }
        
        /// <summary>
        /// Overrides the ToString method
        /// </summary>
        /// <returns>The payment type concatenated with the name.</returns>
        public override String ToString()
        {
            return (this.PaymentType + ", " + this.Name);
        }

        /// <summary>
        /// Implementation of INotifyPropertyChanged
        /// </summary>
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers
        /// <summary>
        /// Helper function to simplify raising the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">The property that has been changed</param>
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int CompareTo(object obj)
        {
            return this.Name.CompareTo((obj as PaymentMethod).Name);
        }

        #endregion
    }


}
