using System.ComponentModel;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Class to store items to be checked
    /// </summary>
    /// <typeparam name="T">Can be of any type</typeparam>
    public class CheckedListItem<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isChecked;
        private bool _isHidden;
        private T _item;

        public CheckedListItem()
        { }

        public CheckedListItem(T item, bool isChecked = false, bool isHidden = false)
        {
            _item = item;
            _isChecked = isChecked;
            _isHidden = isHidden;
        }

        public T Item
        {
            get { return _item; }
            set
            {
                _item = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Item"));
            }
        }


        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }

        public bool IsHidden
        {
            get
            {
                return _isHidden;
            }
            set
            {
                _isHidden = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("IsHidden"));
            }
        }
    }
}
