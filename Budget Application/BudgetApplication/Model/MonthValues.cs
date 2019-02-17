using System;
using System.ComponentModel;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Represents a value for each month. Used in MoneyGridRow.
    /// </summary>
    public class MonthValues :  INotifyPropertyChanged
    {
        private decimal[] _values;  //The array of values

        /// <summary>
        /// Null parameter constructor for creating new instances automatically.
        /// </summary>
        public MonthValues()
        {
            _values = new decimal[12];
        }

        /// <summary>
        /// Instantiates a new MonthValues object with the given values. Useful for quickly reassigning, such as on loading data
        /// </summary>
        /// <param name="vals"></param>
        public MonthValues(decimal[] vals)
        {
            _values = vals;
        }

        /// <summary>
        /// Indexer to allow accessing the values as if the object is an array
        /// </summary>
        /// <param name="index">The index to access</param>
        /// <returns>The value at the specified index</returns>
        public decimal this[int index]
        {
            get
            {
                return _values[index];
            }
            set
            {
                _values[index] = value;
                NotifyPropertyChanged("Value");
            }
        }

        /// <summary>
        /// The array of values
        /// </summary>
        public decimal[] Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
                NotifyPropertyChanged("Values");
            }
        }

        /// <summary>
        /// The length of the array of values
        /// </summary>
        public int Count
        {
            get
            {
                return _values.Length;
            }
        }

        public MonthValues Copy()
        {
            MonthValues copy = new MonthValues();
            this.Values.CopyTo(copy.Values, 0);
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
