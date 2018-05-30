using System;
using System.ComponentModel;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Represents a group used for organizing and categorizing budgets.
    /// </summary>
    [Serializable]
    public class Group : INotifyPropertyChanged
    {
        private String _name;   //Name of the group
        private bool _isIncome; //Whether or not the group represents an income or expense
        private MyObservableCollection<Category> _categories;   //Collection of the categories associated with the group

        /// <summary>
        /// Null parameter constructor for creating instances automatically.
        /// </summary>
        public Group()
        {
            _isIncome = false;
            _name = "New Group";
            _categories = new MyObservableCollection<Category>();
        }

        /// <summary>
        /// Instantiates a Group object with the given name and income status.
        /// </summary>
        /// <param name="isIncome">Whether or not the group represents an income.</param>
        /// <param name="name">The name of the group.</param>
        public Group(bool isIncome = false, String name = "New Group")
        {
            _isIncome = isIncome;
            _name = String.Copy(name);
            _categories = new MyObservableCollection<Category>();
        }

        /// <summary>
        /// The name of the group.
        /// </summary>
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

        /// <summary>
        /// Returns whether or not the group represents an income.
        /// Income groups treat money normally; expenditure groups flip the sign.
        /// </summary>
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

        /// <summary>
        /// The collection of categories associated with the group.
        /// MyObservableCollection used to capture changes to category name.
        /// </summary>
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


        /// <summary>
        /// Overrides the ToString method
        /// </summary>
        /// <returns>The name of the group</returns>
        public override string ToString()
        {
            return Name;
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
