using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Class to store items to be checked
    /// </summary>
    /// <typeparam name="T">Can be of any type</typeparam>
    public class CheckedListItem<T> : ObservableObject
    {
        /// <summary>
        /// Null parameter constructor does nothing
        /// </summary>
        public CheckedListItem()
        {
        }

        /// <summary>
        /// Instantiates a new CheckedListItem with the specified item
        /// </summary>
        /// <param name="item">The item held by the object</param>
        /// <param name="isChecked">Optional. Whether or not the item is checked.</param>
        /// <param name="isHidden">Optional. Whether or not the item is hidden.</param>
        public CheckedListItem(T item, bool isChecked = false, bool isHidden = false)
        {
            this.Item = item;
            this.IsChecked = isChecked;
            this.IsHidden = isHidden;
        }

        #region Public Properties
        private T _Item;    //The item held by the object
        /// <summary>
        /// The item held by the object
        /// </summary>
        public T Item
        {
            get
            {
                return this._Item;
            }
            set
            {
                this.SetProperty(ref this._Item, value);
            }
        }
        private bool _IsChecked;    //If the item has been checked
        /// <summary>
        /// If the item is checked
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return this._IsChecked;
            }
            set
            {
                this.SetProperty(ref this._IsChecked, value);
            }
        }
        private bool _IsHidden; //If the item is hidden from view
        /// <summary>
        /// If the item is hidden from the view
        /// </summary>
        public bool IsHidden
        {
            get
            {
                return this._IsHidden;
            }
            set
            {
                this.SetProperty(ref this._IsHidden, value);
            }
        }
        #endregion

        #region Public Methods
        public CheckedListItem<T> Copy()
        {
            CheckedListItem<T> copy = new CheckedListItem<T>();
            copy.Item = this.Item;
            copy.IsChecked = this.IsChecked;
            copy.IsHidden = this.IsHidden;

            return copy;
        }
        #endregion
    }
}
