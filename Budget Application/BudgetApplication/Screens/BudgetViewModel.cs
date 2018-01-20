using System.Windows.Input;
using BudgetApplication.Popups;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using BudgetApplication.Model;
using System.Windows.Data;
using System.Diagnostics;

namespace BudgetApplication.Screens
{
    public class BudgetViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;

        #region Commands

        #endregion

        #region Constructor
        public BudgetViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;

            this.BudgetValues = session.BudgetValues;
            this.BudgetTotals = session.BudgetTotals;
            this.BudgetBudgetAndSum = new TotalObservableCollection(this.BudgetTotals);
            this.BudgetRows = new ListCollectionView(this.BudgetValues);
            this.BudgetRows.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        }
        #endregion

        public override void Initialize()
        {
        }

        public override void Deinitialize()
        {
        }

        #region Public Properties

        private ListCollectionView _budgetValueView;
        /// <summary>
        /// The collection of values in the budget tab's Values grid
        /// </summary>
        public ListCollectionView BudgetRows
        {
            get
            {
                return _budgetValueView;
            }
            private set
            {
                _budgetValueView = value;
            }
        }

        private MyObservableCollection<MoneyGridRow> _budgetTotals;
        /// <summary>
        /// The collection of values in the budget tab's Totals grid
        /// </summary>
        public MyObservableCollection<MoneyGridRow> BudgetTotals
        {
            get
            {
                return _budgetTotals;
            }
            private set
            {
                _budgetTotals = value;
            }
        }

        private TotalObservableCollection _budgetBudgetAndSum;
        /// <summary>
        /// The collection of values in the budget tab's Budget and Sum grid
        /// </summary>
        public TotalObservableCollection BudgetBudgetAndSum
        {
            get
            {
                return _budgetBudgetAndSum;
            }
            private set
            {
                _budgetBudgetAndSum = value;
            }
        }

        #endregion

        #region Private Properties

        private MyObservableCollection<MoneyGridRow> _budgetValues;
        /// <summary>
        /// The collection of values in the budget tab's Totals grid
        /// </summary>
        private MyObservableCollection<MoneyGridRow> BudgetValues
        {
            get
            {
                return _budgetValues;
            }
            set
            {
                _budgetValues = value;
            }
        }
        #endregion
    }
}