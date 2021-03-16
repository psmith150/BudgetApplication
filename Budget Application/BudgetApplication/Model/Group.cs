using GalaSoft.MvvmLight;
using System;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Represents a group used for organizing and categorizing budgets.
    /// </summary>
    [Serializable]
    public class Group : ObservableObject
    {
        /// <summary>
        /// Null parameter constructor for creating instances automatically.
        /// </summary>
        public Group() : this(false)
        {
        }

        /// <summary>
        /// Instantiates a Group object with the given name and income status.
        /// </summary>
        /// <param name="isIncome">Whether or not the group represents an income.</param>
        /// <param name="name">The name of the group.</param>
        public Group(bool isIncome = false, String name = "New Group")
        {
            _IsIncome = isIncome;
            _Name = name;
            _Categories = new MyObservableCollection<Category>();
        }
        #region Public Properties
        private String _Name;   //Name of the group
        /// <summary>
        /// The name of the group.
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
        private bool _IsIncome; //Whether or not the group represents an income or expense
        /// <summary>
        /// Returns whether or not the group represents an income.
        /// Income groups treat money normally; expenditure groups flip the sign.
        /// </summary>
        public bool IsIncome
        {
            get
            {
                return this._IsIncome;
            }
            set
            {
                this.Set(ref this._IsIncome, value);
            }
        }
        private MyObservableCollection<Category> _Categories;   //Collection of the categories associated with the group
        /// <summary>
        /// The collection of categories associated with the group.
        /// MyObservableCollection used to capture changes to category name.
        /// </summary>
        public MyObservableCollection<Category> Categories
        {
            get
            {
                return this._Categories;
            }
            set
            {
                this.Set(ref this._Categories, value);
            }
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Overrides the ToString method
        /// </summary>
        /// <returns>The name of the group</returns>
        public override string ToString()
        {
            return Name;
        }

        public Group Copy()
        {
            Group copy = new Group();
            copy.Name = this.Name;
            copy.IsIncome = this.IsIncome;
            foreach (Category category in this.Categories)
            {
                copy.Categories.Add(category.Copy());
            }

            return copy;
        }
        #endregion
    }
}
