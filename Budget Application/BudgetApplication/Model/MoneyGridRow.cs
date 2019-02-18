using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Represents a row of money values used on the budget and spending displays.
    /// </summary>
    public class MoneyGridRow : ObservableObject
    {
        /// <summary>
        /// Instantiates a new MoneyGridRow object with the specified group and category.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="category"></param>
        public MoneyGridRow(Group group, Category category)
        {
            this.Values = new MonthValues();
            if (group == null)
                throw new ArgumentException("Group cannot be null");
            if (category == null)
                throw new ArgumentException("Category cannot be null");
            this.Group = group;
            this.Category = category;
            this.IsSum = false;
            this.Percentage = 0.0;
        }

        #region Public Properties
        private Group _Group;   //The group of the row
        /// <summary>
        /// The group that the row is associated with.
        /// </summary>
        public Group Group
        {
            get
            {
                return this._Group;
            }
            set
            {
                this.Set(ref this._Group, value);
            }
        }
        private Category _Category; //The category of the row
        /// <summary>
        /// The category that the row is associated with.
        /// </summary>
        public Category Category
        {
            get
            {
                return this._Category;
            }
            set
            {
                this.Set(ref this._Category, value);
            }
        }
        private MonthValues _Values;    //The set of monthly values
        /// <summary>
        /// The monthly values of the row.
        /// </summary>
        public MonthValues Values
        {
            get
            {
                return this._Values;
            }
            private set
            {
                this.Set(ref this._Values, value);
                this.Values.PropertyChanged += this.ValuesModified;
            }
        }
        private bool _IsSum;    //Whether or not the row represents a summation of other rows
        /// <summary>
        /// Whether or not this row is a summation of other rows.
        /// </summary>
        public bool IsSum
        {
            get
            {
                return this._IsSum;
            }
            set
            {
                this.Set(ref this._IsSum, value);
            }
        }

        /// <summary>
        /// Returns the sum total of the monthly values. If the row is a sum row and already has a sum computed, returns that instead.
        /// </summary>
        public Decimal Sum
        {
            get
            {
                if (_IsSum)
                    return Values[Values.Count - 1];
                decimal sum = 0;
                for (int i = 0; i < _Values.Count; i++)
                {
                    sum += _Values[i];
                }
                return sum;
            }
        }
        private double _Percentage;
        /// <summary>
        /// Represents what percentage of the group's sum is this category's sum
        /// </summary>
        public double Percentage
        {
            get
            {
                return _Percentage;
            }

            set
            {
                if (Double.IsNaN(value))
                {
                    this.Set(ref this._Percentage, 0.0);
                }
                else
                {
                    this.Set(ref this._Percentage, value);
                }
            }
        }
        #endregion
        #region Private Methods
        #region Private Helpers
        /// <summary>
        /// Helper function for a category being modified.
        /// </summary>
        /// <param name="sender">The modified category</param>
        /// <param name="e">The arguments</param>
        private void CategoryModified(object sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged("Category");
        }
        /// <summary>
        /// Helper function for a group being modified.
        /// </summary>
        /// <param name="sender">The modified group</param>
        /// <param name="e">The arguments</param>
        private void GroupModified(object sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged("Group");
        }
        /// <summary>
        /// Helper function for the values being modified.
        /// </summary>
        /// <param name="sender">The modified group</param>
        /// <param name="e">The arguments</param>
        private void ValuesModified(object sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged("Values");
            this.RaisePropertyChanged("Sum");
        }
        #endregion
        #region Public Methods
        public MoneyGridRow Copy()
        {
            MoneyGridRow copy = new MoneyGridRow(this.Group, this.Category);
            copy.IsSum = this.IsSum;
            copy.Values = this.Values.Copy();
            copy.Percentage = this.Percentage;

            return copy;
        }
        #endregion
    }
}
