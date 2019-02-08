using BudgetApplication.Base.Enums;
using GalaSoft.MvvmLight;
using System;

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
            this.CategoryFilter = null;
            this.GroupFilterActive = false;
            this.GroupFilter = null;
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
                this.Set(ref this._Grouping, value);
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
                this.Set(ref this._DateFilterActive, value);
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
                this.Set(ref this._StartDate, value);
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
                this.Set(ref this._EndDate, value);
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
            }
        }
        private Category _CategoryFilter;
        public Category CategoryFilter
        {
            get
            {
                return this._CategoryFilter;
            }
            set
            {
                this.Set(ref this._CategoryFilter, value);
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
            }
        }
        private Group _GroupFilter;
        public Group GroupFilter
        {
            get
            {
                return this._GroupFilter;
            }
            set
            {
                this.Set(ref this._GroupFilter, value);
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
            }
        }
        #endregion
        #endregion
        #region Public Methods
        public bool Filter(Transaction transaction, Group transactionGroup)
        {
            bool result = true;

            if (this.DateFilterActive && (transaction.Date < this.StartDate || transaction.Date > this.EndDate))
            {
                result = false;
            }
            if (this.CategoryFilterActive && !transaction.Category.Equals(this.CategoryFilter))
            {
                result = false;
            }
            if (this.GroupFilterActive && !transactionGroup.Equals(this.GroupFilter))
            {
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
    }
}
