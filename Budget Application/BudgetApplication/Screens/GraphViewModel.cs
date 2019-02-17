﻿using System.Windows.Input;
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
using System.Drawing;
using System.Collections.Specialized;

namespace BudgetApplication.Screens
{
    public class GraphViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;

        #region Commands
        public ICommand RefreshChartCommand { get; private set; }
        public ICommand ShowCategoryFilterPopupCommand { get; private set; }
        public ICommand HideCategoryFilterPopupCommand { get; private set; }
        public ICommand ShowGroupFilterPopupCommand { get; private set; }
        public ICommand HideGroupFilterPopupCommand { get; private set; }
        #endregion

        #region Constructor
        public GraphViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;
            this.Series = new SeriesCollection();
            this.Session.Categories.CollectionChanged += this.Categories_CollectionChanged;
            this.Session.Groups.CollectionChanged += this.Groups_CollectionChanged;
            this.Session.Transactions.CollectionChanged += ((o, e) => this.RefreshChart());
            this.Session.Transactions.MemberChanged += ((o, e) => this.RefreshChart());

            this.RefreshChartCommand = new RelayCommand(() => this.RefreshChart());
            this.ShowCategoryFilterPopupCommand = new RelayCommand(() => this.ToggleCategoryFilterPopup(true));
            this.HideCategoryFilterPopupCommand = new RelayCommand(() => this.ToggleCategoryFilterPopup(false));
            this.ShowGroupFilterPopupCommand = new RelayCommand(() => this.ToggleGroupFilterPopup(true));
            this.HideGroupFilterPopupCommand = new RelayCommand(() => this.ToggleGroupFilterPopup(false));


            this.TransactionGraphConfiguration = new TransactionGraphConfiguration();
            this.TransactionGraphConfiguration.GroupingChanged += ((o, e) => this.RefreshChart());
            this.TransactionGraphConfiguration.FilterChanged += ((o, e) => this.RefreshChart());

            this.BudgetGraphConfiguration = new BudgetGraphConfiguration();
            this.BudgetGraphConfiguration.GroupingChanged += ((o, e) => this.RefreshChart());
            this.BudgetGraphConfiguration.FilterChanged += ((o, e) => this.RefreshChart());
        }
        #endregion

        public override void Initialize()
        {
            this.RefreshChart();
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
        private BudgetGraphConfiguration _BudgetGraphConfiguration;
        public BudgetGraphConfiguration BudgetGraphConfiguration
        {
            get
            {
                return this._BudgetGraphConfiguration;
            }
            set
            {
                this.Set(ref this._BudgetGraphConfiguration, value);
            }
        }
        public Array TransactionGraphGroupings
        {
            get
            {
                return Enum.GetValues(typeof(TransactionGraphGrouping));
            }
        }
        public Array BudgetGraphGroupings
        {
            get
            {
                return Enum.GetValues(typeof(BudgetGraphGrouping));
            }
        }
        public Array IncomeFilters
        {
            get
            {
                return Enum.GetValues(typeof(IncomeFilterOption));
            }
        }
        private string[] _Months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public Array Months
        {
            get
            {
                return this._Months;
            }
        }
        private bool _CategoryFilterPopupVisible;
        public bool CategoryFilterPopupVisible
        {
            get
            {
                return this._CategoryFilterPopupVisible;
            }
            set
            {
                this.Set(ref this._CategoryFilterPopupVisible, value);
            }
        }
        private bool _GroupFilterPopupVisible;
        public bool GroupFilterPopupVisible
        {
            get
            {
                return this._GroupFilterPopupVisible;
            }
            set
            {
                this.Set(ref this._GroupFilterPopupVisible, value);
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
            //this.RefreshTransactionPieChart();
            this.RefreshBudgetPieChart();
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
            this.PointLabel = this.FormatPieChartLabel;
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
                            LabelPoint = this.PointLabel,
                            LabelPosition = PieLabelPosition.OutsideSlice,
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
                            LabelPoint = this.PointLabel,
                            LabelPosition = PieLabelPosition.OutsideSlice
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
                            LabelPoint = this.PointLabel,
                            LabelPosition = PieLabelPosition.OutsideSlice
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
                            LabelPoint = this.PointLabel,
                            LabelPosition = PieLabelPosition.OutsideSlice,
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
                        LabelPoint = this.PointLabel,
                        LabelPosition = PieLabelPosition.OutsideSlice
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
                        LabelPoint = this.PointLabel,
                        LabelPosition = PieLabelPosition.OutsideSlice
                    });
                    break;
                default:
                    break;
            }
        }
        private void RefreshBudgetPieChart()
        {
            List<MoneyGridRow> filteredRows = new List<MoneyGridRow>();
            foreach (MoneyGridRow row in this.Session.BudgetValues)
            {
                MoneyGridRow tempRow = row.Copy();
                if (this.BudgetGraphConfiguration.Filter(tempRow))
                {
                    filteredRows.Add(tempRow);
                }
            }
            this.Series.Clear();
            this.PointLabel = this.FormatPieChartLabel;
            decimal sum = 0.0M;
            switch (this.BudgetGraphConfiguration.Grouping)
            {
                case BudgetGraphGrouping.Category:
                    foreach (Category category in this.Session.Categories)
                    {
                        sum = 0.0M;
                        sum = filteredRows.Where(x => x.Category.Equals(category)).Sum(x => x.Sum);
                        this.Series.Add(new PieSeries
                        {
                            Title = category.Name,
                            Values = new ChartValues<decimal> { sum },
                            DataLabels = true,
                            LabelPoint = this.PointLabel,
                            LabelPosition = PieLabelPosition.OutsideSlice,
                        });
                    }
                    break;
                case BudgetGraphGrouping.Month:
                    for (int i = 0; i < 12; i++)
                    {
                        sum = 0.0M;
                        sum = filteredRows.Select(x => x.Values[i]).Sum();
                        this.Series.Add(new PieSeries
                        {
                            Title = (new DateTime(1, i+1, 1)).ToString("MMMM"),
                            Values = new ChartValues<decimal> { sum },
                            DataLabels = true,
                            LabelPoint = this.PointLabel,
                            LabelPosition = PieLabelPosition.OutsideSlice
                        });
                    }
                    break;
                case BudgetGraphGrouping.Group:
                    foreach (Group group in this.Session.Groups)
                    {
                        sum = 0.0M;
                        sum = filteredRows.Where(x => x.Group.Equals(group)).Sum(x => x.Sum);
                        this.Series.Add(new PieSeries
                        {
                            Title = group.Name,
                            Values = new ChartValues<decimal> { sum },
                            DataLabels = true,
                            LabelPoint = this.PointLabel,
                            LabelPosition = PieLabelPosition.OutsideSlice
                        });
                    }
                    break;
                case BudgetGraphGrouping.IsIncome:
                    sum = 0.0M;
                    sum += filteredRows.Where(x => x.Group.IsIncome).Sum(x => x.Sum);
                    this.Series.Add(new PieSeries
                    {
                        Title = "Income",
                        Values = new ChartValues<decimal> { sum },
                        DataLabels = true,
                        LabelPoint = this.PointLabel,
                        LabelPosition = PieLabelPosition.OutsideSlice
                    });
                    sum = 0.0M;
                    sum += filteredRows.Where(x => !x.Group.IsIncome).Sum(x => x.Sum);
                    this.Series.Add(new PieSeries
                    {
                        Title = "Expenditures",
                        Values = new ChartValues<decimal> { sum },
                        DataLabels = true,
                        LabelPoint = this.PointLabel,
                        LabelPosition = PieLabelPosition.OutsideSlice
                    });
                    break;
                default:
                    break;
            }
        }
        private string FormatPieChartLabel(ChartPoint chartpoint)
        {
            if (chartpoint.Y <= 0.0)
            {
                return "";
            }
            else
            {
                return string.Format("{0}", chartpoint.SeriesView.Title);
            }
        }
        private void ToggleCategoryFilterPopup(bool state)
        {
            this.CategoryFilterPopupVisible = state;
        }
        private void ToggleGroupFilterPopup(bool state)
        {
            this.GroupFilterPopupVisible = state;
        }
        private void Categories_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Category category in e.NewItems)
                {
                    this.TransactionGraphConfiguration.CategoryFilter.Add(new CheckedListItem<Category>(category, true));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Category category in e.OldItems)
                {
                    CheckedListItem<Category> item = this.TransactionGraphConfiguration.CategoryFilter.FirstOrDefault(x => x.Item.Equals(category));
                    if (item != null)
                        this.TransactionGraphConfiguration.CategoryFilter.Remove(item);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                List<CheckedListItem<Category>> tempCategories = new List<CheckedListItem<Category>>();
                foreach (Category category in this.Session.Categories)
                {
                    tempCategories.Add(new CheckedListItem<Category>(category, true));
                }
                this.TransactionGraphConfiguration.CategoryFilter.InsertRange(tempCategories);
            }
            this.RefreshChart();
        }
        private void Groups_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Group group in e.NewItems)
                {
                    this.TransactionGraphConfiguration.GroupFilter.Add(new CheckedListItem<Group>(group, true));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Group group in e.OldItems)
                {
                    CheckedListItem<Group> item = this.TransactionGraphConfiguration.GroupFilter.FirstOrDefault(x => x.Item.Equals(group));
                    if (item != null)
                        this.TransactionGraphConfiguration.GroupFilter.Remove(item);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                List<CheckedListItem<Group>> tempGroups = new List<CheckedListItem<Group>>();
                foreach (Group group in this.Session.Groups)
                {
                    tempGroups.Add(new CheckedListItem<Group>(group, true));
                }
                this.TransactionGraphConfiguration.GroupFilter.InsertRange(tempGroups);
            }
            this.RefreshChart();
        }
        #endregion
    }
}