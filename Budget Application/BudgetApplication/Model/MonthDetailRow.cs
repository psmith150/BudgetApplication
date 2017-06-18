using System;
using System.ComponentModel;
using System.Diagnostics;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Class to represent a row of data in the Month Details tab
    /// </summary>
    public class MonthDetailRow : INotifyPropertyChanged
    {
        Group _group;
        Category _category;
        decimal _budgetedAmount;
        decimal _spentAmount;
        double _percentSpent;
        double _percentMonth;
        bool _onTarget;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group">The group associated with the values</param>
        /// <param name="category">The category associated with the values</param>
        /// <param name="currentMonth">The current month (0-12)</param>
        /// <param name="currentYear">The current year</param>
        public MonthDetailRow(Group group, Category category, int currentMonth, int currentYear, int budgetedAmount = 0, int spentAmount = 0)
        {
            _group = group;
            _category = category;
            BudgetedAmount = budgetedAmount;
            SpentAmount = spentAmount;
            _percentSpent = 0.0;
            _percentMonth = (double)DateTime.Now.Day / DateTime.DaysInMonth(currentYear, currentMonth+1);
        }

        public Group Group
        {
            get
            {
                return _group;
            }
        }

        public Category Category
        {
            get
            {
                return _category;
            }
        }

        public decimal BudgetedAmount
        {
            get
            {
                return _budgetedAmount;
            }
            set
            {
                _budgetedAmount = value;
                UpdatePercentSpent();
                NotifyPropertyChanged("BudgetedAmount");
            }
        }

        public decimal SpentAmount
        {
            get
            {
                return _spentAmount;
            }
            set
            {
                _spentAmount = value;
                UpdatePercentSpent();
                NotifyPropertyChanged("SpentAmount");
            }
        }

        public double PercentSpent
        {
            get
            {
                return _percentSpent;
            }
        }

        public double PercentMonth
        {
            get
            {
                return _percentMonth;
            }
        }

        public bool OnTarget
        {
            get
            {
                return _onTarget;
            }
        }

        private void UpdatePercentSpent()
        {
            if (_budgetedAmount == 0)
            {
                if (_spentAmount > 0)
                {
                    _onTarget = false;
                }
                else
                {
                    _onTarget = true;
                }
                _percentSpent = 1.0;
            }
            else
            {
                _percentSpent = (double)_spentAmount / (double)_budgetedAmount;
                _onTarget = _percentSpent <= _percentMonth;
            }
            if (_group.IsIncome)
                _onTarget = !_onTarget;
            NotifyPropertyChanged("PercentSpent");
            NotifyPropertyChanged("OnTarget");
        }

        /// <summary>
        /// Implementation of INotifyPropertyChanged
        /// </summary>
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        /// <summary>
        /// 1/4/2017: Believed to no longer be needed due to outside changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryModified(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Category");
        }

        /// <summary>
        /// Helper function to simplify raising the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">The property that has been changed</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
