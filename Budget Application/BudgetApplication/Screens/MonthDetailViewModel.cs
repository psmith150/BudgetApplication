using System.Windows.Input;
using BudgetApplication.Popups;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using BudgetApplication.Model;
using System.Windows.Data;
using System.Collections.Generic;

namespace BudgetApplication.Screens
{
    public class MonthDetailViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;

        #region Commands

        #endregion

        #region Constructor
        public MonthDetailViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;

            this.MonthDetails = session.MonthDetails;
            this.MonthDetailsView = new ListCollectionView(this.MonthDetails);
            this.MonthDetailsView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
            this.Session.RequestMonthDetailsUpdate += ((o, a) => this.UpdateMonthDetails());
        }
        #endregion

        public override void Initialize()
        {
        }

        public override void Deinitialize()
        {
        }

        #region Public Properties

        private int _selectedMonth = 0;

        /// <summary>
        /// Sets and gets the SelectedMonth property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int SelectedMonth
        {
            get
            {
                return _selectedMonth;
            }
            set
            {
                Set(ref _selectedMonth, value);
                this.UpdateMonthDetails();
                this.RaisePropertyChanged("PercentMonth");
                //Debug.WriteLine("Updating month to " + _selectedMonth);
                //RaisePropertyChanged("MonthDetails");
                //RaisePropertyChanged("PercentMonth");
            }
        }
        private ListCollectionView _monthDetailsView;
        public ListCollectionView MonthDetailsView
        {
            get
            {
                return _monthDetailsView;
            }
            private set
            {
                _monthDetailsView = value;
            }
        }

        public double PercentMonth
        {
            get
            {
                if (_selectedMonth != DateTime.Now.Month - 1)
                    return 1.0;
                return (double)DateTime.Now.Day / DateTime.DaysInMonth(this.Session.CurrentYear, _selectedMonth + 1);
            }
        }

        #endregion

        #region Private Properties
        private MyObservableCollection<MonthDetailRow> _monthDetails;
        /// <summary>
        /// Sets and gets the MyProperty property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        private MyObservableCollection<MonthDetailRow> MonthDetails
        {
            get
            {
                return _monthDetails;
            }
            set
            {
                Set(ref _monthDetails, value);
            }
        }
        #endregion

        #region Public Methods
        public void UpdateMonthDetails()
        {
            _monthDetails.Clear();
            int numRows = this.Session.BudgetValues.Count;
            List<MonthDetailRow> monthDetails = new List<MonthDetailRow>();
            for (int i = 0; i < numRows; i++)
            {
                Group currentGroup = this.Session.BudgetValues[i].Group;
                Category currentCategory = this.Session.BudgetValues[i].Category;
                MonthDetailRow currentDetail = new MonthDetailRow(currentGroup, currentCategory, _selectedMonth, this.Session.CurrentYear);
                currentDetail.BudgetedAmount = this.Session.BudgetValues[i].Values[_selectedMonth];
                currentDetail.SpentAmount = this.Session.SpendingValues[i].Values[_selectedMonth];
                monthDetails.Add(currentDetail);
            }
            numRows = this.Session.BudgetTotals.Count;
            for (int i = 0; i < numRows; i++)
            {
                Group currentGroup = this.Session.BudgetTotals[i].Group;
                Category currentCategory = this.Session.BudgetTotals[i].Category;
                MonthDetailRow currentDetail = new MonthDetailRow(currentGroup, currentCategory, _selectedMonth, this.Session.CurrentYear);
                currentDetail.BudgetedAmount = this.Session.BudgetTotals[i].Values[_selectedMonth];
                currentDetail.SpentAmount = this.Session.SpendingTotals[i].Values[_selectedMonth];
                monthDetails.Add(currentDetail);
            }

            //numRows = _budgetBudgetAndSum.Count;
            //for (int i = 0; i < numRows; i++)
            //{
            //    Group currentGroup = this.BudgetTotals.ElementAt(i).Group;
            //    Category currentCategory = this.BudgetTotals.ElementAt(i).Category;
            //    MonthDetailRow currentDetail = new MonthDetailRow(currentGroup, currentCategory, _selectedMonth, Int32.Parse(_currentYear));
            //    currentDetail.BudgetedAmount = _budgetBudgetAndSum.ElementAt(i).Values[_selectedMonth];
            //    currentDetail.SpentAmount = _spendingBudgetAndSum.ElementAt(i).Values[_selectedMonth];
            //    monthDetails.Add(currentDetail);
            //}
            this.MonthDetails.InsertRange(monthDetails);
            //Debug.WriteLine("Days so far: " + DateTime.Now.Day + "; total in month: " + DateTime.DaysInMonth(Int32.Parse(_currentYear), _selectedMonth + 1));
        }
        #endregion
    }
}