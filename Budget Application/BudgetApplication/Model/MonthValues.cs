using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BudgetApplication.Model
{
    public class MonthValues :  INotifyPropertyChanged
    {
        private decimal[] _values;

        public MonthValues()
        {
            _values = new decimal[12];
        }

        public MonthValues(decimal[] vals)
        {
            _values = vals;
        }

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

        public int Count
        {
            get
            {
                return _values.Length;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

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
