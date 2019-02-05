using System.Windows.Input;
using BudgetApplication.Popups;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using BudgetApplication.Model;
using System.Windows.Data;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Wpf;
using System.Linq;

namespace BudgetApplication.Screens
{
    public class GraphViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;

        #region Commands

        #endregion

        #region Constructor
        public GraphViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;
            this.Series = new SeriesCollection();
        }
        #endregion

        public override void Initialize()
        {
            this.Series.Clear();
            this.PointLabel = chartpoint => string.Format("{0}", chartpoint.SeriesView.Title);
            foreach (Category category in this.Session.Categories)
            {
                decimal sum = 0.0M;
                sum = this.Session.Transactions.Where(x => x.Category.Equals(category)).Sum(x => x.Amount);
                this.Series.Add(new PieSeries
                {
                    Title = category.Name,
                    Values = new ChartValues<decimal> { sum },
                    DataLabels = true,
                    LabelPoint = this.PointLabel
                });
            }
        }

        public override void Deinitialize()
        {
        }

        #region Public Properties
        private SeriesCollection _Series;
        public SeriesCollection Series
        {
            get
            {
                return this._Series;
            }
            private set
            {
                this.Set(ref this._Series, value);
            }
        }
        public Func<ChartPoint, string> PointLabel { get; set; }
        #endregion

        #region Private Properties
        #endregion

        #region Public Methods
        #endregion
    }
}