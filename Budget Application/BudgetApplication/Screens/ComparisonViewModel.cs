using System.Windows.Input;
using BudgetApplication.Popups;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.Command;
using System;
using BudgetApplication.Model;
using System.Windows.Data;

namespace BudgetApplication.Screens
{
    public class ComparisonViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;

        #region Commands

        #endregion

        #region Constructor
        public ComparisonViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;

            this.ComparisonValues = session.ComparisonValues;
            this.ComparisonTotals = session.ComparisonTotals;
            this.ComparisonBudgetAndSum = new TotalObservableCollection(this.ComparisonTotals);
            this.ComparisonBudgetAndSum.IsComparison = true;
            this.ComparisonRows = new ListCollectionView(this.ComparisonValues);
            this.ComparisonRows.GroupDescriptions.Add(new PropertyGroupDescription("Group"));

            this.Session.RequestListViewUpdate += ((o, e) => this.ComparisonRows.Refresh());
        }
        #endregion

        public override void Initialize()
        {
        }

        public override void Deinitialize()
        {
        }

        #region Public Properties

        private ListCollectionView _comparisonValueView;
        /// <summary>
        /// The collection of values in the comparison tab's Values grid
        /// </summary>
        public ListCollectionView ComparisonRows
        {
            get
            {
                return _comparisonValueView;
            }
            private set
            {
                _comparisonValueView = value;
            }
        }

        private MyObservableCollection<MoneyGridRow> _comparisonTotals;
        /// <summary>
        /// The collection of values in the comparison tab's Totals grid
        /// </summary>
        public MyObservableCollection<MoneyGridRow> ComparisonTotals
        {
            get
            {
                return _comparisonTotals;
            }
            private set
            {
                _comparisonTotals = value;
            }
        }

        private TotalObservableCollection _comparisonBudgetAndSum;
        /// <summary>
        /// The collection of values in the comparison tab's comparison and Sum grid
        /// </summary>
        public TotalObservableCollection ComparisonBudgetAndSum
        {
            get
            {
                return _comparisonBudgetAndSum;
            }
            private set
            {
                _comparisonBudgetAndSum = value;
            }
        }

        #endregion

        #region Private Properties

        private MyObservableCollection<MoneyGridRow> _comparisonValues;
        /// <summary>
        /// The collection of values in the comparison tab's Totals grid
        /// </summary>
        private MyObservableCollection<MoneyGridRow> ComparisonValues
        {
            get
            {
                return _comparisonValues;
            }
            set
            {
                _comparisonValues = value;
            }
        }
        #endregion
    }
}