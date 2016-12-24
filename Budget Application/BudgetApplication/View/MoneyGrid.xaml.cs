﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using BudgetApplication.Model;
using System.Collections;
using System.Globalization;

namespace BudgetApplication.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MoneyGrid : UserControl
    {
        public MoneyGrid()
        {
            InitializeComponent();
            this.Loaded += SetColumnMinWidth;
        }

        private void SetColumnMinWidth(object sender, RoutedEventArgs e)
        {
            DataGridColumn column = ValuesGrid.Columns[0];
            column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            for (int i = 2; i < ValuesGrid.Columns.Count; i++)
            {
                column = ValuesGrid.Columns[i];
                if (i == ValuesGrid.Columns.Count-2)
                    continue;
                column.MinWidth = 50;
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                if (IsComparison)
                {
                    (column as DataGridTextColumn).ElementStyle = this.FindResource("ColorCodeStyle") as Style;
                    (TotalsGrid.Columns[i] as DataGridTextColumn).ElementStyle = this.FindResource("ColorCodeStyle") as Style;
                }
            }
        }

        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            Debug.WriteLine("Sorting");
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        public ListCollectionView ValuesDataSource
        {
            get { return (ListCollectionView)GetValue(ValuesDataSourceProperty); }
            set { SetValue(ValuesDataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValuesDataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValuesDataSourceProperty =
            DependencyProperty.Register("ValuesDataSource", typeof(ListCollectionView), typeof(MoneyGrid));


        public IEnumerable<MoneyGridRow> TotalsDataSource
        {
            get { return (IEnumerable<MoneyGridRow>)GetValue(TotalsDataSourceProperty); }
            set { SetValue(TotalsDataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalsDataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalsDataSourceProperty =
            DependencyProperty.Register("TotalsDataSource", typeof(IEnumerable<MoneyGridRow>), typeof(MoneyGrid));



        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(MoneyGrid), new PropertyMetadata(false));



        public bool IsComparison
        {
            get { return (bool)GetValue(IsComparisonProperty); }
            set { SetValue(IsComparisonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsComparison.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsComparisonProperty =
            DependencyProperty.Register("IsComparison", typeof(bool), typeof(MoneyGrid), new PropertyMetadata(false));




        public ICommand OnEdit
        {
            get { return (ICommand)GetValue(OnEditProperty); }
            set { SetValue(OnEditProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnEdit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnEditProperty =
            DependencyProperty.Register("OnEdit", typeof(ICommand), typeof(MoneyGrid), new PropertyMetadata(null));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dataView = (ListCollectionView)CollectionViewSource.GetDefaultView(ValuesGrid.ItemsSource);
            dataView.Refresh();
            //dataView.CustomSort = Comparer;
            //dataView.Refresh();
            //cvs.GroupDescriptions.RemoveAt(0);
            //PropertyGroupDescription prop = new PropertyGroupDescription("Group");
            //cvs.GroupDescriptions.Add(prop);
        }
    }

    public class ValueToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal num = (decimal)value;
            if (num >= 0)
            {
                return Brushes.Green;
            }
            else
            {
                return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
