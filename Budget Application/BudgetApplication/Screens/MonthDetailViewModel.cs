using System.Windows.Input;
using BudgetApplication.Popups;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using BudgetApplication.Model;
using System.Windows.Data;

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
                //UpdateMonthDetails();
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
                return (double)DateTime.Now.Day / DateTime.DaysInMonth(DateTime.Now.Year, _selectedMonth + 1);
                //TODO: support current year
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
    }
}