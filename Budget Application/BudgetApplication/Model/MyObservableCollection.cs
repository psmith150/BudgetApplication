using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace BudgetApplication
{
    public class MyObservableCollection<T> : ObservableCollection<T>
        where T : INotifyPropertyChanged
    {
        public MyObservableCollection() : base()
        {
            CollectionChanged += new NotifyCollectionChangedEventHandler(NewCollectionChanged);
        }

        public MyObservableCollection(IEnumerable<T> enumerable) : base(enumerable)
        {

        }

        void NewCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= MemberPropertyChanged;
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += MemberPropertyChanged;
                }
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
        }

        //protected override void MoveItem(int oldIndex, int newIndex)
        //{
        //    _suppressNotification = true;
        //    base.MoveItem(oldIndex, newIndex);
        //    _suppressNotification = false;
        //    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move));
        //}

        public void MemberPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            //Debug.WriteLine("A collection member was changed");
        }
    }
}
