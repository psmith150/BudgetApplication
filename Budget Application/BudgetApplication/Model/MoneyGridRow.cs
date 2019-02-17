using System;
using System.ComponentModel;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Represents a row of money values used on the budget and spending displays.
    /// </summary>
    public class MoneyGridRow : INotifyPropertyChanged
    {
        private Group _group;   //The group of the row
        private Category _category; //The category of the row
        private MonthValues _values;    //The set of monthly values
        private bool _isSum;    //Whether or not the row represents a summation of other rows
        private double _percentage;

        /// <summary>
        /// Instantiates a new MoneyGridRow object with the specified group and category.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="category"></param>
        public MoneyGridRow(Group group, Category category)
        {
            _values = new MonthValues();
            _values.PropertyChanged += ValuesModified;
            if (group == null)
                throw new ArgumentException("Group cannot be null");
            if (category == null)
                throw new ArgumentException("Category cannot be null");
            _group = group;
            _category = category;
            //Set event handlers for category and group being modified.
            _category.PropertyChanged += CategoryModified;
            _group.PropertyChanged += GroupModified;
            _isSum = false;
            _percentage = 0.0;
        }

        /// <summary>
        /// The group that the row is associated with.
        /// </summary>
        public Group Group
        {
            get
            {
                return _group;
            }
            set
            {
                _group = value;
            }
        }

        /// <summary>
        /// The category that the row is associated with.
        /// </summary>
        public Category Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
            }
        }

        /// <summary>
        /// The monthly values of the row.
        /// </summary>
        public MonthValues Values
        {
            get
            {
                return _values;
            }
            private set
            {
                this._values = value;
                NotifyPropertyChanged("Values");
            }
        }

        /// <summary>
        /// Whether or not this row is a summation of other rows.
        /// </summary>
        public bool IsSum
        {
            get
            {
                return _isSum;
            }
            set
            {
                _isSum = value;
            }
        }

        /// <summary>
        /// Returns the sum total of the monthly values. If the row is a sum row and already has a sum computed, returns that instead.
        /// </summary>
        public Decimal Sum
        {
            get
            {
                if (_isSum)
                    return Values[Values.Count - 1];
                decimal sum = 0;
                for (int i = 0; i < _values.Count; i++)
                {
                    sum += _values[i];
                }
                return sum;
            }
        }

        /// <summary>
        /// Represents what percentage of the group's sum is this category's sum
        /// </summary>
        public double Percentage
        {
            get
            {
                return _percentage;
            }

            set
            {
                if (Double.IsNaN(value))
                {
                    _percentage = 0.0;
                }
                else
                {
                    _percentage = value;
                }
                NotifyPropertyChanged("Percentage");
            }
        }

        public MoneyGridRow Copy()
        {
            MoneyGridRow copy = new MoneyGridRow(this.Group, this.Category);
            copy.IsSum = this.IsSum;
            copy.Values = this.Values.Copy();
            copy.Percentage = this.Percentage;

            return copy;
        }

        /// <summary>
        /// Implementation of INotifyPropertyChanged
        /// </summary>
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers
        /// <summary>
        /// Helper function for a category being modified.
        /// </summary>
        /// <param name="sender">The modified category</param>
        /// <param name="e">The arguments</param>
        private void CategoryModified(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Category");
        }
        /// <summary>
        /// Helper function for a group being modified.
        /// </summary>
        /// <param name="sender">The modified group</param>
        /// <param name="e">The arguments</param>
        private void GroupModified(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Group");
        }
        /// <summary>
        /// Helper function for the values being modified.
        /// </summary>
        /// <param name="sender">The modified group</param>
        /// <param name="e">The arguments</param>
        private void ValuesModified(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Values");
            NotifyPropertyChanged("Sum");
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
