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
    /// User control used to display a year's worth of budgeting or spending data.
    /// </summary>
    public partial class MoneyGrid : UserControl
    {
        /// <summary>
        /// Initializes a new MoneyGrid control.
        /// </summary>
        public MoneyGrid()
        {
            InitializeComponent();
            this.Loaded += SetColumnMinWidth;
        }

        /// <summary>
        /// Runs on load of control. Used to make sure column widths never get too small.
        /// Also sets color coding as needed.
        /// </summary>
        /// <param name="sender">The loaded object (this)</param>
        /// <param name="e">The arguments</param>
        private void SetColumnMinWidth(object sender, RoutedEventArgs e)
        {
            DataGridColumn column = ValuesGrid.Columns[0];
            column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);  //Category width is always auto
            //Loops through other columns. Skips any columns that aren't text columns. Starts at 1 to avoid Category column
            for (int i = 1; i < ValuesGrid.Columns.Count; i++)
            {
                column = ValuesGrid.Columns[i] as DataGridTextColumn;
                if (column == null) //Non text columns are separators.
                    continue;
                column.MinWidth = 50;   //Sets the min width
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);  //Allows columns to expand
                //Entire DataGrid is color coded if it's a comparison view
                if (IsComparison)
                {
                    (column as DataGridTextColumn).ElementStyle = this.FindResource("ColorCodeStyle") as Style;
                    (TotalsGrid.Columns[i] as DataGridTextColumn).ElementStyle = this.FindResource("ColorCodeStyle") as Style;
                }
                //Budget and sum rows are always color coded.
                //Debug.WriteLine("Column number: " + i);
                (BudgetAndSumGrid.Columns[i] as DataGridTextColumn).ElementStyle = this.FindResource("ColorCodeStyle") as Style;
            }
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
        /// Field for the data source for the values grid
        /// </summary>
        public ListCollectionView ValuesDataSource
        {
            get { return (ListCollectionView)GetValue(ValuesDataSourceProperty); }
            set { SetValue(ValuesDataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValuesDataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValuesDataSourceProperty =
            DependencyProperty.Register("ValuesDataSource", typeof(ListCollectionView), typeof(MoneyGrid));

        /// <summary>
        /// Field for the data source for the totals grid
        /// </summary>
        public IEnumerable<MoneyGridRow> TotalsDataSource
        {
            get { return (IEnumerable<MoneyGridRow>)GetValue(TotalsDataSourceProperty); }
            set { SetValue(TotalsDataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalsDataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalsDataSourceProperty =
            DependencyProperty.Register("TotalsDataSource", typeof(IEnumerable<MoneyGridRow>), typeof(MoneyGrid));

        /// <summary>
        /// Field for the data source of the budget and sum rows.
        /// </summary>
        public IEnumerable<MoneyGridRow> BudgetAndSumDataSource
        {
            get { return (IEnumerable<MoneyGridRow>)GetValue(BudgetAndSumDataSourceProperty); }
            set { SetValue(BudgetAndSumDataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BudgetAndSumDataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BudgetAndSumDataSourceProperty =
            DependencyProperty.Register("BudgetAndSumDataSource", typeof(IEnumerable<MoneyGridRow>), typeof(MoneyGrid));

        /// <summary>
        /// Field to determine if values grid should be read only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(MoneyGrid), new PropertyMetadata(false));

        /// <summary>
        /// Field for if the control is part of the comparison view.
        /// </summary>
        public bool IsComparison
        {
            get { return (bool)GetValue(IsComparisonProperty); }
            set { SetValue(IsComparisonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsComparison.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsComparisonProperty =
            DependencyProperty.Register("IsComparison", typeof(bool), typeof(MoneyGrid), new PropertyMetadata(false));
    }
}
