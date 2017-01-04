using System;
using System.ComponentModel;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Class to represent a spending category.
    /// </summary>
    [Serializable]
    public class Category : INotifyPropertyChanged
    {
        private String _name;   //Category name

        /// <summary>
        /// Null parameter constructor for creating new categories automatically
        /// </summary>
        public Category()
        {
            _name = "New Category";
        }

        /// <summary>
        /// Instantiates a category with a given name
        /// </summary>
        /// <param name="name">The desired category name.</param>
        public Category(String name = "New Category")
        {
            _name = String.Copy(name);
        }

        /// <summary>
        /// The name of the category
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
        /// Overrides the ToString method
        /// </summary>
        /// <returns>The name of the category</returns>
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
