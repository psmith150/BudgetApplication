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
        /// <summary>
        /// Null parameter constructor for creating new instances automatically.
        /// </summary>
        public CreditCard() : this("New Card")
        {
        }

        /// <summary>
        /// Instantiates a new CreditCard object with the given name and credit limit
        /// </summary>
        /// <param name="name"></param>
        /// <param name="creditLimit"></param>
        public CreditCard(String name = "New Card", decimal creditLimit = 300) : base(name)
        {
            if (creditLimit <= 0)
            {
                creditLimit = 0;
            }
            this.CreditLimit = creditLimit;
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
                return Type.CreditCard;
            }
        }
        private decimal _CreditLimit;   //The line of credit associated with the card.
        /// <summary>
        /// Credit limit
        /// </summary>
        [DisplayName("Credit Limit")]
        public decimal CreditLimit
        {
            get
            {
                return this._CreditLimit;
            }
            set
            {
                this.Set(ref this._CreditLimit, value);
            }
        }
        private decimal _PaymentAmount;
        [Browsable(false)]
        public decimal PaymentAmount
        {
            get
            {
                return this._PaymentAmount;
            }
            set
            {
                this.Set(ref this._PaymentAmount, value);
            }
        }
        private String _PaymentExpression;  //The expression used to evaluate the payment amount
        [Browsable(false)]
        public String PaymentExpression
        {
            get
            {
                return this._PaymentExpression;
            }
            set
            {
                this.Set(ref this._PaymentExpression, value);
                decimal amount = 0.0M;
                if (EvaluateExpression(this.PaymentExpression, out amount))
                {
                    this.PaymentAmount = amount;
                }
            }
        }
        #endregion

        #region Private Methods
        private bool EvaluateExpression(String expression, out decimal result)
        {
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
            catch (Exception)
            {
                result = 0;
                success = false;
            }
            return success;
        }
        #endregion
        #region Public Methods
        public CreditCard Copy()
        {
            CreditCard copy = new CreditCard();
            copy.Name = this.Name;
            copy.StartDate = this.StartDate;
            copy.EndDate = this.EndDate;
            copy.CreditLimit = this.CreditLimit;
            copy.PaymentExpression = this.PaymentExpression;
            return copy;
        }
        #endregion
    }
}
