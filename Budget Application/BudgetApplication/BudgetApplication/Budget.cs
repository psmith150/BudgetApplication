using System;
using System.ComponentModel;

namespace BudgetApplication.Model
{
    public class Category : INotifyPropertyChanged
    {
        private String _name;
        private Group _group;

        public Category(Group group, String name = "New Category")
        {
            _group = group;
            _name = String.Copy(name);
            if (group == null)
            {
                throw new ArgumentException("Null group");
            }
        }

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

        public Group Group
        {
            get
            {
                return _group;
            }
        }

        public bool IsIncome()
        {
            return _group.IsIncome;
        }

        public override string ToString()
        {
            return Name;
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

    public class Group : INotifyPropertyChanged
    {
        private String _name;
        private bool _isIncome;

        public Group(bool isIncome = false, String name = "New Group")
        {
            _isIncome = isIncome;
            _name = String.Copy(name);
        }

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

        public bool IsIncome
        {
            get
            {
                return _isIncome;
            }
            set
            {
                _isIncome = value;
                NotifyPropertyChanged("IsIncome");
            }
        }

        public override string ToString()
        {
            return Name;
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

    public class MoneyGridRow : INotifyPropertyChanged
    {
        private Group _group;
        private Category _category;
        private decimal[] _values;
        public MoneyGridRow(Group group, Category category)
        {
            _values = new decimal[12];
            if (group == null)
                throw new ArgumentException("Group cannot be null");
            if (category == null)
                throw new ArgumentException("Category cannot be null");
            _group = group;
            _category = category;
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