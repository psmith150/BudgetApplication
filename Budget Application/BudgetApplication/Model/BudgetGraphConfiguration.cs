using BudgetApplication.Base.Enums;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace BudgetApplication.Model
{
    public class BudgetGraphConfiguration : ObservableObject
    {
        #region Constructor
        public BudgetGraphConfiguration()
        {
            this.Grouping = BudgetGraphGrouping.Category;
            this.MonthFilterActive = false;
            this.StartMonth = 0;
            this.EndMonth = 11;
            this.CategoryFilterActive = false;
            this.CategoryFilter = new MyObservableCollection<CheckedListItem<Category>>();
            this.GroupFilterActive = false;
            this.GroupFilter = new MyObservableCollection<CheckedListItem<Group>>();
            this.IncomeFilterActive = false;
            this.IncomeFilter = IncomeFilterOption.Expenditures;
        }
        #endregion
        #region Public Properties
        private BudgetGraphGrouping _Grouping;
        public BudgetGraphGrouping Grouping
        {
            get
            {
                return this._Grouping;
            }
            set
            {
                this.Set(ref this._Grouping, value);
                this.RaiseGroupingChanged();
            }
        }
        #region Filters
        private bool _MonthFilterActive;
        public bool MonthFilterActive
        {
            get
            {
                return this._MonthFilterActive;
            }
            set
            {
                this.Set(ref this._MonthFilterActive, value);
                this.RaiseFilterChanged();

            }
        }
        private int _StartMonth;
        public int StartMonth
        {
            get
            {
                return this._StartMonth;
            }
            set
            {
                this.Set(ref this._StartMonth, value);
                if (this.MonthFilterActive)
                    this.RaiseFilterChanged();
            }
        }
        private int _EndMonth;
        public int EndMonth
        {
            get
            {
                return this._EndMonth;
            }
            set
            {
                this.Set(ref this._EndMonth, value);
                if (this.MonthFilterActive)
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
                this.Set(ref this._CategoryFilterActive, value);
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
                this.Set(ref this._CategoryFilter, value);
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
                this.Set(ref this._GroupFilterActive, value);
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
                this.Set(ref this._GroupFilter, value);
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
                this.Set(ref this._IncomeFilterActive, value);
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
                this.Set(ref this._IncomeFilter, value);
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
        public bool Filter(MoneyGridRow row)
        {
            bool result = true;

            if (this.CategoryFilterActive)
            {
                CheckedListItem<Category> category = this.CategoryFilter.FirstOrDefault(x => (x.Item as Category).Equals(row.Category));
                if (category == null || !category.IsChecked)
                    result = false;
            }
            if (this.MonthFilterActive)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (i < this.StartMonth || i > this.EndMonth)
                        row.Values[i] = 0.0M;
                }
            }
            if (this.GroupFilterActive)
            {
                CheckedListItem<Group> group = this.GroupFilter.FirstOrDefault(x => (x.Item as Group).Equals(row.Group));
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
                        result = row.Group.IsIncome ? result : false;
                        break;
                    case IncomeFilterOption.Expenditures:
                        result = row.Group.IsIncome ? false : result;
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
            this.RaisePropertyChanged("SelectedCategories");
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
            this.RaisePropertyChanged("SelectedGroups");
        }
        private void UpdateCategoryListFilter()
        {
            if (this.CategoryFilterActive)
                this.RaiseFilterChanged();
            this.RaisePropertyChanged("SelectedCategories");
        }
        private void UpdateGroupListFilter()
        {
            if (this.GroupFilterActive)
                this.RaiseFilterChanged();
            this.RaisePropertyChanged("SelectedGroups");
        }
        #endregion
    }
}
