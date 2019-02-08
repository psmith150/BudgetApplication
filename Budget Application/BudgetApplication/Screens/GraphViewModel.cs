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
using BudgetApplication.Base.Enums;

namespace BudgetApplication.Screens
{
    public class GraphViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;

        #region Commands
        public ICommand RefreshChartCommand { get; private set; }
        #endregion

        #region Constructor
        public GraphViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;
            this.Series = new SeriesCollection();

            this.RefreshChartCommand = new RelayCommand(() => this.RefreshChart());

            this.TransactionGraphConfiguration = new TransactionGraphConfiguration();
        }
        #endregion

        public override void Initialize()
        {
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

        private TransactionGraphConfiguration _TransactionGraphConfiguration;
        public TransactionGraphConfiguration TransactionGraphConfiguration
        {
            get
            {
                return this._TransactionGraphConfiguration;
            }
            set
            {
                this.Set(ref this._TransactionGraphConfiguration, value);
            }
        }
        public Array TransactionGraphGroupings
        {
            get
            {
                return Enum.GetValues(typeof(TransactionGraphGrouping));
            }
        }
        public Array IncomeFilters
        {
            get
            {
                return Enum.GetValues(typeof(IncomeFilterOption));
            }
        }
        #endregion

        #region Private Properties
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        private void RefreshChart()
        {
            this.RefreshTransactionPieChart();
        }
        private void RefreshTransactionPieChart()
        {
            List<Transaction> filteredTransactions = new List<Transaction>();
            foreach (Transaction transaction in this.Session.Transactions)
            {
                Group group = this.Session.Groups.FirstOrDefault(x => x.Categories.Contains(transaction.Category));
                if (group != null)
                {
                    if (this.TransactionGraphConfiguration.Filter(transaction, group))
                    {
                        filteredTransactions.Add(transaction);
                    }
                }
            }
            this.Series.Clear();
            this.PointLabel = chartpoint => string.Format("{0}", chartpoint.SeriesView.Title);
            decimal sum = 0.0M;
            switch (this.TransactionGraphConfiguration.Grouping)
            {
                case TransactionGraphGrouping.Category:
                    foreach (Category category in this.Session.Categories)
                    {
                        sum = 0.0M;
                        sum = filteredTransactions.Where(x => x.Category.Equals(category)).Sum(x => x.Amount);
                        this.Series.Add(new PieSeries
                        {
                            Title = category.Name,
                            Values = new ChartValues<decimal> { sum },
                            DataLabels = true,
                            LabelPoint = this.PointLabel
                        });
                    }
                    break;
                case TransactionGraphGrouping.Month:
                    for (int i = 1; i < 12; i++)
                    {
                        sum = 0.0M;
                        sum = filteredTransactions.Where(x => (x.Date.Month == i)).Sum(x => x.Amount);
                        this.Series.Add(new PieSeries
                        {
                            Title = (new DateTime(1, i, 1)).ToString("MMMM"),
                            Values = new ChartValues<decimal> { sum },
                            DataLabels = true,
                            LabelPoint = this.PointLabel
                        });
                    }
                    break;
                case TransactionGraphGrouping.Group:
                    foreach (Group group in this.Session.Groups)
                    {
                        sum = 0.0M;
                        sum = filteredTransactions.Where(x => group.Categories.Contains(x.Category)).Sum(x => x.Amount);
                        this.Series.Add(new PieSeries
                        {
                            Title = group.Name,
                            Values = new ChartValues<decimal> { sum },
                            DataLabels = true,
                            LabelPoint = this.PointLabel
                        });
                    }
                    break;
                case TransactionGraphGrouping.PaymentMethod:
                    foreach (PaymentMethod paymentMethod in this.Session.PaymentMethods)
                    {
                        sum = 0.0M;
                        sum = filteredTransactions.Where(x => x.PaymentMethod.Equals(paymentMethod)).Sum(x => x.Amount);
                        this.Series.Add(new PieSeries
                        {
                            Title = paymentMethod.Name,
                            Values = new ChartValues<decimal> { sum },
                            DataLabels = true,
                            LabelPoint = this.PointLabel
                        });
                    }
                    break;
                case TransactionGraphGrouping.IsIncome:
                    sum = 0.0M;
                    foreach (Group group in this.Session.Groups.Where(x => x.IsIncome))
                    {
                        sum += filteredTransactions.Where(x => group.Categories.Contains(x.Category)).Sum(x => x.Amount);
                    }
                    this.Series.Add(new PieSeries
                    {
                        Title = "Income",
                        Values = new ChartValues<decimal> { sum },
                        DataLabels = true,
                        LabelPoint = this.PointLabel
                    });
                    sum = 0.0M;
                    foreach (Group group in this.Session.Groups.Where(x => !x.IsIncome))
                    {
                        sum += filteredTransactions.Where(x => group.Categories.Contains(x.Category)).Sum(x => x.Amount);
                    }
                    this.Series.Add(new PieSeries
                    {
                        Title = "Expenditures",
                        Values = new ChartValues<decimal> { sum },
                        DataLabels = true,
                        LabelPoint = this.PointLabel
                    });
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}