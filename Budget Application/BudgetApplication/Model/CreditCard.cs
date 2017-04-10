using System;
using System.ComponentModel;
using System.Diagnostics;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Represents a credit card use for transactions.
    /// </summary>
    [Serializable]
    public class CreditCard : PaymentMethod
    {
        private decimal _creditLimit;   //The line of credit associated with the card.
        private decimal _paymentAmount;
        private String _paymentExpression;  //The expression used to evaluate the payment amount

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

        [Browsable(false)]
        public decimal PaymentAmount
        {
            get
            {
                return _paymentAmount;
            }
            set
            {
                _paymentAmount = value;
                NotifyPropertyChanged("PaymentAmount");
            }
        }

        [Browsable(false)]
        public String PaymentExpression
        {
            get
            {
                return _paymentExpression;
            }
            set
            {
                _paymentExpression = value;
                //Debug.WriteLine("payment expression set");
                if (EvaluateExpression(_paymentExpression, out _paymentAmount))
                {
                    NotifyPropertyChanged("PaymentAmount");
                }
                NotifyPropertyChanged("PaymentExpression");
            }
        }

        private bool EvaluateExpression(String expression, out decimal result)
        {
            //Debug.WriteLine("Evaluating expression");
            if (expression.Length <= 0)
            {
                result = 0;
                return false;
            }
            bool success = true;
            NCalc.Expression.CacheEnabled = false;
            NCalc.Expression ex = new NCalc.Expression(expression);
            if (ex.HasErrors())
            {
                success = false;
                result = 0;
                return success;
            }
            try
            {
                result = Decimal.Parse(ex.Evaluate().ToString());
                //result = Decimal.Parse(ex.Evaluate());
            }
            catch (Exception e)
            {
                result = 0;
                success = false;
            }
            return success;
        }
    }
}
