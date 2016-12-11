using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BudgetApplication.Model
{
    class MoneyGrid : INotifyPropertyChanged
    {
        private int _year;
        private List<Group> _groups;
        private List<Category> _categories;
        private List<decimal>[] _values;
        private int _numGroups;
        private int _numCategories;

        public MoneyGrid(int year)
        {
            _year = year;
            _groups = new List<Group>();
            _categories = new List<Category>();
            _numGroups = _groups.Count;
            _numCategories = _categories.Count;

            _values = new List<decimal>[12];
        }

        #region Getters and Setters

        public int Year
        {
            get
            {
                return _year;
            }
            private set
            {
                _year = value;
                NotifyPropertyChanged("Year");
            }
        }

        public List<Group> Groups
        {
            get
            {
                return _groups;
            }
            private set
            {
                _groups = value;
                NotifyPropertyChanged("Groups");
            }
        }

        public List<Category> Categories
        {
            get
            {
                return _categories;
            }
            private set
            {
                _categories = value;
                NotifyPropertyChanged("Categories");
            }
        }

        public List<decimal>[] Values
        {
            get
            {
                return _values;
            }
            private set
            {
                _values = value;
                NotifyPropertyChanged("Values");
            }
        }

        #endregion

        #region Group Helper functions
        public void AddGroup(Group group)
        {
            foreach (Group currGroup in _groups)
            {
                if (currGroup.Name.Equals(group.Name))
                {
                    throw new ArgumentException("Group of same name already exists");
                }
            }
            _groups.Add(group);
            NotifyPropertyChanged("Groups");
        }

        public void RemoveGroup(Group group)
        {
            NotifyPropertyChanged("Groups");
            if (!_groups.Remove(group))
            {
                throw new ArgumentException("Cannot remove group");
            }
        }

        public void ModifyGroup(Group group, String name, bool isIncome)
        {
            int index;
            foreach (Group currGroup in _groups)
            {
                if (currGroup.Name.Equals(name))
                {
                    throw new ArgumentException("Group with that name already exists");
                }
            }
            index = _groups.IndexOf(group);
            _groups[index].Name = name;
            _groups[index].IsIncome = isIncome;
            NotifyPropertyChanged("Groups");
        }
        #endregion

        #region Category Helper functions
        public void AddCategory(Category category)
        {
            foreach (Category currCategory in _categories)
            {
                if (currCategory.Name.Equals(category.Name))
                {
                    throw new ArgumentException("Category with that name already exists");
                }
            }
            _categories.Add(category);
            NotifyPropertyChanged("Categories");
            foreach(List<decimal> month in _values)
            {
                month.Add(0);
            }
        }

        public void RemoveCategory(Category category)
        {
            NotifyPropertyChanged("Categories");
            int index = _categories.IndexOf(category);
            if (index == -1 || !_categories.Remove(category))
            {
                throw new ArgumentException("Category does not exist");
            }
            foreach (List<decimal> month in _values)
            {
                month.RemoveAt(index);
            }
        }

        public void ModifyCategory(Category category, String name, bool isIncome)
        {
            int index;
            foreach (Category currcategory in _categories)
            {
                if (currcategory.Name.Equals(name))
                {
                    throw new ArgumentException("Category name is already used");
                }
            }
            index = _categories.IndexOf(category);
            _categories[index].Name = name;
            NotifyPropertyChanged("Categories");
        }
        #endregion
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
