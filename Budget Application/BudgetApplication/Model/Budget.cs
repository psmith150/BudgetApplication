﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace BudgetApplication.Model
{
    [Serializable]
    public class Category : INotifyPropertyChanged
    {
        private String _name;

        public Category()
        {
            _name = "New Category";
        }

        public Category( String name = "New Category")
        {
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
                    //Debug.WriteLine("Category changed");
                }
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

    [Serializable]
    public class Group : INotifyPropertyChanged
    {
        private String _name;
        private bool _isIncome;
        private MyObservableCollection<Category> _categories;

        public Group()
        {
            _isIncome = false;
            _name = "New Group";
            _categories = new MyObservableCollection<Category>();
        }

        public Group(bool isIncome = false, String name = "New Group")
        {
            _isIncome = isIncome;
            _name = String.Copy(name);
            _categories = new MyObservableCollection<Category>();
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

        public MyObservableCollection<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
                NotifyPropertyChanged("Categories");
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
        private MonthValues _values;
        private bool _isSum;

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
            _category.PropertyChanged += CategoryModified;
            _group.PropertyChanged += GroupModified;
            _isSum = false;
        }

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

        public MonthValues Values
        {
            get
            {
                return _values;
            }
        }

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

        public Decimal Sum
        {
            get
            {
                if (_isSum)
                    return Values[Values.Count - 1];
                decimal sum = 0;
                for(int i=0; i<_values.Count; i++)
                {
                    sum += _values[i];
                }
                return sum;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void CategoryModified(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Category");
        }
        private void GroupModified(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Group");
        }
        private void ValuesModified(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Values");
            NotifyPropertyChanged("Sum");
            //Debug.WriteLine("A value changed for row " + Category.Name);
        }

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