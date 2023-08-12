using BudgetApplication.Base.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace BudgetApplication.Model
{
    public class TransactionGraphConfiguration : ObservableObject
    {
        #region Constructor
        public TransactionGraphConfiguration()
        {
            this.Grouping = TransactionGraphGrouping.Category;
            this.DateFilterActive = false;
            this.StartDate = new DateTime(DateTime.Today.Year, 1, 1);
            this.EndDate = new DateTime(DateTime.Today.Year, 12, 31);
            this.CategoryFilterActive = false;
            this.CategoryFilter = new MyObservableCollection<CheckedListItem<Category>>();
            this.GroupFilterActive = false;
            this.GroupFilter = new MyObservableCollection<CheckedListItem<Group>>();
            this.IncomeFilterActive = false;
            this.IncomeFilter = IncomeFilterOption.Expenditures;
        }
        #endregion
        #region Public Properties
        private TransactionGraphGrouping _Grouping;
        public TransactionGraphGrouping Grouping
        {
            get
            {
                return this._Grouping;
            }
            set
            {
                this.SetProperty(ref this._Grouping, value);
                this.RaiseGroupingChanged();
            }
        }
        #region Filters
        private bool _DateFilterActive;
        public bool DateFilterActive
        {
            get
            {
                return this._DateFilterActive;
            }
            set
            {
                this.SetProperty(ref this._DateFilterActive, value);
                this.RaiseFilterChanged();

            }
        }
        private DateTime _StartDate;
        public DateTime StartDate
        {
            get
            {
                return this._StartDate;
            }
            set
            {
                this.SetProperty(ref this._StartDate, value);
                if (this.DateFilterActive)
                    this.RaiseFilterChanged();
            }
        }
        private DateTime _EndDate;
        public DateTime EndDate
        {
            get
            {
                return this._EndDate;
            }
            set
            {
                this.SetProperty(ref this._EndDate, value);
                if (this.DateFilterActive)
                    this.RaiseFilterChanged();
            }
        }
        private bool _CategoryFilterActive;
        public bool CategoryFilterActive
        {
            get
            {
                return this._CategoryFilterActive;
            }
            set
            {
                this.SetProperty(ref this._CategoryFilterActive, value);
                this.RaiseFilterChanged();
            }
        }
        private MyObservableCollection<CheckedListItem<Category>> _CategoryFilter;
        public MyObservableCollection<CheckedListItem<Category>> CategoryFilter
        {
            get
            {
                return this._CategoryFilter;
            }
            set
            {
                this.SetProperty(ref this._CategoryFilter, value);
                this.CategoryFilter.CollectionChanged += this.CategoryFilterCollectionChanged;
            }
        }
        private bool _GroupFilterActive;
        public bool GroupFilterActive
        {
            get
            {
                return this._GroupFilterActive;
            }
            set
            {
                this.SetProperty(ref this._GroupFilterActive, value);
                this.RaiseFilterChanged();
            }
        }
        private MyObservableCollection<CheckedListItem<Group>> _GroupFilter;
        public MyObservableCollection<CheckedListItem<Group>> GroupFilter
        {
            get
            {
                return this._GroupFilter;
            }
            set
            {
                this.SetProperty(ref this._GroupFilter, value);
                this.GroupFilter.CollectionChanged += this.GroupFilterCollectionChanged;
            }
        }
        private bool _IncomeFilterActive;
        public bool IncomeFilterActive
        {
            get
            {
                return this._IncomeFilterActive;
            }
            set
            {
                this.SetProperty(ref this._IncomeFilterActive, value);
                this.RaiseFilterChanged();
            }
        }
        private IncomeFilterOption _IncomeFilter;
        public IncomeFilterOption IncomeFilter
        {
            get
            {
                return this._IncomeFilter;
            }
            set
            {
                this.SetProperty(ref this._IncomeFilter, value);
                if (this.IncomeFilterActive)
                    this.RaiseFilterChanged();
            }
        }
        #endregion
        public event EventHandler GroupingChanged;
        public event EventHandler FilterChanged;
        public int SelectedCategories
        {
            get
            {
                return this.CategoryFilter.Where(x => x.IsChecked).Count();
            }
        }
        public int SelectedGroups
        {
            get
            {
                return this.GroupFilter.Where(x => x.IsChecked).Count();
            }
        }
        #endregion
        #region Public Methods
        public bool Filter(Transaction transaction, Group transactionGroup)
        {
            bool result = true;

            if (this.DateFilterActive && (transaction.Date < this.StartDate || transaction.Date > this.EndDate))
            {
                result = false;
            }
            if (this.CategoryFilterActive)
            {
                CheckedListItem<Category> category = this.CategoryFilter.FirstOrDefault(x => (x.Item as Category).Equals(transaction.Category));
                if (category == null || !category.IsChecked)
                    result = false;
            }
            if (this.GroupFilterActive)
            {
                CheckedListItem<Group> group = this.GroupFilter.FirstOrDefault(x => (x.Item as Group).Equals(transactionGroup));
                if (group == null || !group.IsChecked)
                    result = false;
            }
            if (this.IncomeFilterActive)
            {
                switch (this.IncomeFilter)
                {
                    case IncomeFilterOption.Both:
                        break;
                    case IncomeFilterOption.Income:
                        result = transactionGroup.IsIncome ? result : false;
                        break;
                    case IncomeFilterOption.Expenditures:
                        result = transactionGroup.IsIncome ? false : result;
                        break;
                    default:
                        result = false;
                        break;
                }
            }
            return result;
        }
        #endregion
        #region Private Methods
        private void RaiseGroupingChanged()
        {
            if (this.GroupingChanged != null)
            {
                this.GroupingChanged(this, new EventArgs());
            }
        }
        private void RaiseFilterChanged()
        {
            if (this.FilterChanged != null)
            {
                this.FilterChanged(this, new EventArgs());
            }
        }
        private void CategoryFilterCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (CheckedListItem<Category> item in e.NewItems)
                {
                    item.PropertyChanged += ((o, a) => this.UpdateCategoryListFilter());
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (CheckedListItem<Category> item in e.OldItems)
                {
                    item.PropertyChanged -= ((o, a) => this.UpdateCategoryListFilter());
                }
            }
            this.OnPropertyChanged(nameof(SelectedCategories));
        }
        private void GroupFilterCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (CheckedListItem<Group> item in e.NewItems)
                {
                    item.PropertyChanged += ((o, a) => this.UpdateGroupListFilter());
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (CheckedListItem<Group> item in e.OldItems)
                {
                    item.PropertyChanged -= ((o, a) => this.UpdateGroupListFilter());
                }
            }
            this.OnPropertyChanged(nameof(SelectedGroups));
        }
        private void UpdateCategoryListFilter()
        {
            if (this.CategoryFilterActive)
                this.RaiseFilterChanged();
            this.OnPropertyChanged(nameof(SelectedCategories));
        }
        private void UpdateGroupListFilter()
        {
            if (this.GroupFilterActive)
                this.RaiseFilterChanged();
            this.OnPropertyChanged(nameof(SelectedGroups));
        }
        #endregion
    }
}
