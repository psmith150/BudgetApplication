using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Class to represent a row of data in the Month Details tab
    /// </summary>
    public class MonthDetailRow : ObservableObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group">The group associated with the values</param>
        /// <param name="category">The category associated with the values</param>
        /// <param name="currentMonth">The current month (0-12)</param>
        /// <param name="currentYear">The current year</param>
        public MonthDetailRow(Group group, Category category, int currentMonth, int currentYear, decimal budgetedAmount = 0, decimal spentAmount = 0)
        {
            this.Group = group;
            this.Category = category;
            this.BudgetedAmount = budgetedAmount;
            this.SpentAmount = spentAmount;
            this.PercentSpent = 0.0;
            this.PercentMonth = (double)DateTime.Today.Day / DateTime.DaysInMonth(currentYear, currentMonth+1);
        }
        #region Public Properties
        private Group _Group;
        public Group Group
        {
            get
            {
                return this._Group;
            }
            private set
            {
                this.Set(ref this._Group, value);
            }
        }
        private Category _Category;
        public Category Category
        {
            get
            {
                return this._Category;
            }
            private set
            {
                this.Set(ref this._Category, value);
            }
        }
        private decimal _BudgetedAmount;
        public decimal BudgetedAmount
        {
            get
            {
                return _BudgetedAmount;
            }
            set
            {
                this.Set(ref this._BudgetedAmount, value);
                UpdatePercentSpent();
            }
        }
        decimal _SpentAmount;
        public decimal SpentAmount
        {
            get
            {
                return _SpentAmount;
            }
            set
            {
                this.Set(ref this._SpentAmount, value);
                UpdatePercentSpent();
            }
        }
        private double _PercentSpent;
        public double PercentSpent
        {
            get
            {
                return _PercentSpent;
            }
            private set
            {
                this.Set(ref this._PercentSpent, value);
            }
        }
        private double _PercentMonth;
        public double PercentMonth
        {
            get
            {
                return _PercentMonth;
            }
            private set
            {
                this.Set(ref this._PercentMonth, value);
            }
        }
        private bool _OnTarget;
        public bool OnTarget
        {
            get
            {
                return _OnTarget;
            }
            private set
            {
                this.Set(ref this._OnTarget, value);
            }
        }
        #endregion


        #region Private Methods
        private void UpdatePercentSpent()
        {
            if (this.BudgetedAmount == 0)
            {
                if (this.SpentAmount > 0)
                {
                    this.OnTarget = false;
                }
                else
                {
                    this.OnTarget = true;
                }
                this.PercentSpent = 1.0;
            }
            else
            {
                this.PercentSpent = (double)this.SpentAmount / (double)this.BudgetedAmount;
                this.OnTarget = this.PercentSpent - this.PercentMonth <= 0.001;
            }
            if (this.Group.IsIncome)
                this.OnTarget = !this.OnTarget;
        }
        /// <summary>
        /// 1/4/2017: Believed to no longer be needed due to outside changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryModified(object sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged("Category");
        }
        #endregion
        #region Public Methods
        public MonthDetailRow Copy()
        {
            MonthDetailRow copy = new MonthDetailRow(this.Group, this.Category, DateTime.Today.Month, DateTime.Today.Year, this.BudgetedAmount, this.SpentAmount);

            return copy;
        }
        #endregion
    }
}
