using GalaSoft.MvvmLight;
using System.ComponentModel;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Class to store items to be checked
    /// </summary>
    /// <typeparam name="T">Can be of any type</typeparam>
    public class CheckedListItem<T> : ObservableObject
    {

        private bool _isChecked;    //If the item has been checked
        private bool _isHidden; //If the item is hidden from view
        private T _item;    //The item held by the object

        /// <summary>
        /// Null parameter constructor does nothing
        /// </summary>
        public CheckedListItem()
        { }

        /// <summary>
        /// Instantiates a new CheckedListItem with the specified item
        /// </summary>
        /// <param name="item">The item held by the object</param>
        /// <param name="isChecked">Optional. Whether or not the item is checked.</param>
        /// <param name="isHidden">Optional. Whether or not the item is hidden.</param>
        public CheckedListItem(T item, bool isChecked = false, bool isHidden = false)
        {
            _item = item;
            _isChecked = isChecked;
            _isHidden = isHidden;
        }

        /// <summary>
        /// The item held by the object
        /// </summary>
        public T Item
        {
            get { return _item; }
            set
            {
                this.Set(ref this._item, value);
            }
        }

        /// <summary>
        /// If the item is checked
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                this.Set(ref this._isChecked, value);
            }
        }

        /// <summary>
        /// If the item is hidden from the view
        /// </summary>
        public bool IsHidden
        {
            get
            {
                return _isHidden;
            }
            set
            {
                this.Set(ref this._isHidden, value);
            }
        }
    }
}
