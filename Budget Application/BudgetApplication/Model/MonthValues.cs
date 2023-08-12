using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Represents a value for each month. Used in MoneyGridRow.
    /// </summary>
    public class MonthValues :  ObservableObject
    {

        /// <summary>
        /// Null parameter constructor for creating new instances automatically.
        /// </summary>
        public MonthValues() : this(new decimal[12])
        {
        }

        /// <summary>
        /// Instantiates a new MonthValues object with the given values. Useful for quickly reassigning, such as on loading data
        /// </summary>
        /// <param name="vals"></param>
        public MonthValues(decimal[] vals)
        {
            this.Values = vals;
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
                return this.Values[index];
            }
            set
            {
                this.SetProperty(ref this._Values[index], value);
            }
        }
        private decimal[] _Values;  //The array of values
        /// <summary>
        /// The array of values
        /// </summary>
        public decimal[] Values
        {
            get
            {
                return _Values;
            }
            set
            {
                this.SetProperty(ref this._Values, value);
            }
        }

        /// <summary>
        /// The length of the array of values
        /// </summary>
        public int Count
        {
            get
            {
                return this.Values.Length;
            }
        }
        #region Public Methods
        public MonthValues Copy()
        {
            MonthValues copy = new MonthValues();
            this.Values.CopyTo(copy.Values, 0);
            return copy;
        }
        #endregion
    }
}
