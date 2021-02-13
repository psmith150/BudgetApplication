using GalaSoft.MvvmLight;
using System;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Class to represent a spending category.
    /// </summary>
    [Serializable]
    public class Category : ObservableObject, IComparable
    {

        /// <summary>
        /// Null parameter constructor for creating new categories automatically
        /// </summary>
        public Category() : this("New Category")
        {
        }

        /// <summary>
        /// Instantiates a category with a given name
        /// </summary>
        /// <param name="name">The desired category name.</param>
        public Category(String name = "New Category")
        {
            this.Name = String.Copy(name);
        }
        #region Public Properties
        private String _Name;   //Category name
        /// <summary>
        /// The name of the category
        /// </summary>
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.Set(ref this._Name, value);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Overrides the ToString method
        /// </summary>
        /// <returns>The name of the category</returns>
        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(object obj)
        {
            return this.Name.CompareTo((obj as Category).Name);
        }
        public Category Copy()
        {
            Category copy = new Category(string.Copy(this.Name));

            return copy;
        }
        #endregion
    }
}
