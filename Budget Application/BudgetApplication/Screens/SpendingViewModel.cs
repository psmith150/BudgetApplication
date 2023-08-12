using System.Windows.Input;
using BudgetApplication.Popups;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.Command;
using System;
using BudgetApplication.Model;
using System.Windows.Data;

namespace BudgetApplication.Screens
{
    public class SpendingViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;

        #region Commands

        #endregion

        #region Constructor
        public SpendingViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;

            this.SpendingValues = session.SpendingValues;
            this.SpendingTotals = session.SpendingTotals;
            this.SpendingBudgetAndSum = new TotalObservableCollection(this.SpendingTotals);
            this.SpendingRows = new ListCollectionView(this.SpendingValues);
            this.SpendingRows.GroupDescriptions.Add(new PropertyGroupDescription("Group"));

            this.Session.RequestListViewUpdate += ((o, e) => this.SpendingRows.Refresh());
        }
        #endregion

        public override void Initialize()
        {
        }

        public override void Deinitialize()
        {
        }

        #region Public Properties

        private ListCollectionView _spendingValueView;
        /// <summary>
        /// The collection of values in the budget tab's Values grid
        /// </summary>
        public ListCollectionView SpendingRows
        {
            get
            {
                return _spendingValueView;
            }
            private set
            {
                _spendingValueView = value;
            }
        }

        private MyObservableCollection<MoneyGridRow> _spendingTotals;
        /// <summary>
        /// The collection of values in the budget tab's Totals grid
        /// </summary>
        public MyObservableCollection<MoneyGridRow> SpendingTotals
        {
            get
            {
                return _spendingTotals;
            }
            private set
            {
                _spendingTotals = value;
            }
        }

        private TotalObservableCollection _spendingBudgetAndSum;
        /// <summary>
        /// The collection of values in the budget tab's Budget and Sum grid
        /// </summary>
        public TotalObservableCollection SpendingBudgetAndSum
        {
            get
            {
                return _spendingBudgetAndSum;
            }
            private set
            {
                _spendingBudgetAndSum = value;
            }
        }

        #endregion

        #region Private Properties

        private MyObservableCollection<MoneyGridRow> _spendingValues;
        /// <summary>
        /// The collection of values in the budget tab's Totals grid
        /// </summary>
        private MyObservableCollection<MoneyGridRow> SpendingValues
        {
            get
            {
                return _spendingValues;
            }
            set
            {
                _spendingValues = value;
            }
        }
        #endregion
    }
}