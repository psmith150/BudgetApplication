using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Diagnostics;
using BudgetApplication.Model;

namespace BudgetApplication.View
{
    /// <summary>
    /// Description for MonthDetailView.
    /// </summary>
    public partial class MonthDetailView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the MonthDetailView class.
        /// </summary>
        public MonthDetailView()
        {
            InitializeComponent();
            this.Loaded += SetColumnMinWidth;
            this.Loaded += PositionDateIndictor;
        }

        /// <summary>
        /// Runs on load of control. Used to make sure column widths never get too small.
        /// Also sets color coding as needed.
        /// </summary>
        /// <param name="sender">The loaded object (this)</param>
        /// <param name="e">The arguments</param>
        private void SetColumnMinWidth(object sender, RoutedEventArgs e)
        {
            DataGridColumn column = DetailGrid.Columns[0];
            column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);  //Category width is always auto
            //Loops through other columns. Skips any columns that aren't text columns. Starts at 1 to avoid Category column
            for (int i = 2; i < DetailGrid.Columns.Count; i++)
            {
                column = DetailGrid.Columns[i] as DataGridTemplateColumn;
                if (column == null) //Non text columns are separators.
                    continue;
                column.MinWidth = 100;   //Sets the min width
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);  //Allows columns to expand
                //column.Width = new DataGridLength(500);  //Allows columns to expand
            }
        }
        private void PositionDateIndictor(object sender, RoutedEventArgs e)
        {
            DataGridColumn column = DetailGrid.Columns[2];
            //DateIndicator.Width = (double)column.ActualWidth * PercentMonthDataSource;
            //Debug.WriteLine("Column width is " + column.ActualWidth);
        }

        /// <summary>
        /// Handles scrolling of the datagrid.
        /// </summary>
        /// <param name="sender">The scrollviewer object</param>
        /// <param name="e">The arguments</param>
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        /// <summary>
        /// Field for the data source for the details grid
        /// </summary>
        public ListCollectionView MonthDetailsDataSource
        {
            get { return (ListCollectionView)GetValue(MonthDetailsDataSourceProperty); }
            set { SetValue(MonthDetailsDataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValuesDataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MonthDetailsDataSourceProperty =
            DependencyProperty.Register("MonthDetailsDataSource", typeof(ListCollectionView), typeof(MonthDetailView));

        /// <summary>
        /// Field for the data source for the selected month
        /// </summary>
        public int SelectedMonthDataSource
        {
            get { return (int)GetValue(SelectedMonthDataSourceProperty); }
            set { SetValue(SelectedMonthDataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalsDataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedMonthDataSourceProperty =
            DependencyProperty.Register("SelectedMonthDataSource", typeof(int), typeof(MonthDetailView));

        /// <summary>
        /// Field for the data source for the percentage of the month complete
        /// </summary>
        public double PercentMonthDataSource
        {
            get { return (double)GetValue(PercentMonthDataSourceProperty); }
            set { SetValue(PercentMonthDataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalsDataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PercentMonthDataSourceProperty =
            DependencyProperty.Register("PercentMonthDataSource", typeof(double), typeof(MonthDetailView));
    }
}