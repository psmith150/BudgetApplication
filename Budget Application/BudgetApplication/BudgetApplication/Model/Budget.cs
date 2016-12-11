using System;
using System.ComponentModel;
using System.Diagnostics;

namespace BudgetApplication.Model
{
    public class Category : INotifyPropertyChanged
    {
        private String _name;

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
                    Debug.WriteLine("Category changed");
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

    public class Group : INotifyPropertyChanged
    {
        private String _name;
        private bool _isIncome;
        private MyObservableCollection<Category> _categories;

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
            _category.PropertyChanged += CategoryModified;
            _group.PropertyChanged += GroupModified;
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

        private void CategoryModified(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Category");
        }
        private void GroupModified(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Group");
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