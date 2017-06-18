﻿#pragma checksum "..\..\..\View\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B9D17CC8B34E310A0EAB92EB6E8D18B4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using BudgetApplication.Model;
using BudgetApplication.View;
using Microsoft.Windows.Themes;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Xceed.Wpf.DataGrid;
using Xceed.Wpf.DataGrid.Automation;
using Xceed.Wpf.DataGrid.Converters;
using Xceed.Wpf.DataGrid.FilterCriteria;
using Xceed.Wpf.DataGrid.Markup;
using Xceed.Wpf.DataGrid.ValidationRules;
using Xceed.Wpf.DataGrid.Views;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Chromes;
using Xceed.Wpf.Toolkit.Core.Converters;
using Xceed.Wpf.Toolkit.Core.Input;
using Xceed.Wpf.Toolkit.Core.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.Panels;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Xceed.Wpf.Toolkit.Zoombox;


namespace BudgetApplication.View {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 100 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl tabControl;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem BudgetTab;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem SpendingTab;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem ComparisonTab;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem ProgressTab;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem TransactionsTab;
        
        #line default
        #line hidden
        
        
        #line 129 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.Popup filterPopup;
        
        #line default
        #line hidden
        
        
        #line 132 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.WatermarkTextBox SearchBox;
        
        #line default
        #line hidden
        
        
        #line 134 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSelectAll;
        
        #line default
        #line hidden
        
        
        #line 142 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnUnselectAll;
        
        #line default
        #line hidden
        
        
        #line 151 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox FilterBox;
        
        #line default
        #line hidden
        
        
        #line 164 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid Transactions;
        
        #line default
        #line hidden
        
        
        #line 212 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem PaymentsTab;
        
        #line default
        #line hidden
        
        
        #line 216 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition CreditDetailRow;
        
        #line default
        #line hidden
        
        
        #line 220 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox PaymentSelector;
        
        #line default
        #line hidden
        
        
        #line 234 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker PaymentStartDate;
        
        #line default
        #line hidden
        
        
        #line 236 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker PaymentEndDate;
        
        #line default
        #line hidden
        
        
        #line 240 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label CreditLimitLabel;
        
        #line default
        #line hidden
        
        
        #line 242 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label TotalBillLabel;
        
        #line default
        #line hidden
        
        
        #line 246 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PaymentAmountBox;
        
        #line default
        #line hidden
        
        
        #line 265 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label NetBillLabel;
        
        #line default
        #line hidden
        
        
        #line 267 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label RemainingCreditLabel;
        
        #line default
        #line hidden
        
        
        #line 269 "..\..\..\View\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid PaymentTransactions;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/BudgetApplication;component/view/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 19 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Data.CollectionViewSource)(target)).Filter += new System.Windows.Data.FilterEventHandler(this.TransactionsView_Filter);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 24 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Data.CollectionViewSource)(target)).Filter += new System.Windows.Data.FilterEventHandler(this.PaymentTransactionsView_Filter);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 92 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 93 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.NewYear_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 96 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.GroupsAndCategories_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 97 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.PaymentMethods_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.tabControl = ((System.Windows.Controls.TabControl)(target));
            return;
            case 9:
            this.BudgetTab = ((System.Windows.Controls.TabItem)(target));
            return;
            case 10:
            this.SpendingTab = ((System.Windows.Controls.TabItem)(target));
            return;
            case 11:
            this.ComparisonTab = ((System.Windows.Controls.TabItem)(target));
            return;
            case 12:
            this.ProgressTab = ((System.Windows.Controls.TabItem)(target));
            return;
            case 13:
            this.TransactionsTab = ((System.Windows.Controls.TabItem)(target));
            return;
            case 14:
            this.filterPopup = ((System.Windows.Controls.Primitives.Popup)(target));
            
            #line 129 "..\..\..\View\MainWindow.xaml"
            this.filterPopup.Opened += new System.EventHandler(this.filterPopup_Opened);
            
            #line default
            #line hidden
            return;
            case 15:
            this.SearchBox = ((Xceed.Wpf.Toolkit.WatermarkTextBox)(target));
            
            #line 132 "..\..\..\View\MainWindow.xaml"
            this.SearchBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SearchBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 16:
            this.btnSelectAll = ((System.Windows.Controls.Button)(target));
            
            #line 134 "..\..\..\View\MainWindow.xaml"
            this.btnSelectAll.Click += new System.Windows.RoutedEventHandler(this.btnSelectAll_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            this.btnUnselectAll = ((System.Windows.Controls.Button)(target));
            
            #line 142 "..\..\..\View\MainWindow.xaml"
            this.btnUnselectAll.Click += new System.Windows.RoutedEventHandler(this.btnUnselectAll_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            this.FilterBox = ((System.Windows.Controls.ListBox)(target));
            return;
            case 20:
            this.Transactions = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 21:
            this.PaymentsTab = ((System.Windows.Controls.TabItem)(target));
            return;
            case 22:
            this.CreditDetailRow = ((System.Windows.Controls.RowDefinition)(target));
            return;
            case 23:
            this.PaymentSelector = ((System.Windows.Controls.ComboBox)(target));
            
            #line 220 "..\..\..\View\MainWindow.xaml"
            this.PaymentSelector.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.PaymentSelector_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 24:
            this.PaymentStartDate = ((System.Windows.Controls.DatePicker)(target));
            
            #line 234 "..\..\..\View\MainWindow.xaml"
            this.PaymentStartDate.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.PaymentStartDate_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 25:
            this.PaymentEndDate = ((System.Windows.Controls.DatePicker)(target));
            
            #line 236 "..\..\..\View\MainWindow.xaml"
            this.PaymentEndDate.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.PaymentEndDate_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 26:
            this.CreditLimitLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 27:
            this.TotalBillLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 28:
            this.PaymentAmountBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 246 "..\..\..\View\MainWindow.xaml"
            this.PaymentAmountBox.LostFocus += new System.Windows.RoutedEventHandler(this.PaymentExpression_Updated);
            
            #line default
            #line hidden
            
            #line 246 "..\..\..\View\MainWindow.xaml"
            this.PaymentAmountBox.GotFocus += new System.Windows.RoutedEventHandler(this.PaymentExpression_Editing);
            
            #line default
            #line hidden
            return;
            case 29:
            this.NetBillLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 30:
            this.RemainingCreditLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 31:
            this.PaymentTransactions = ((System.Windows.Controls.DataGrid)(target));
            
            #line 269 "..\..\..\View\MainWindow.xaml"
            this.PaymentTransactions.RowEditEnding += new System.EventHandler<System.Windows.Controls.DataGridRowEditEndingEventArgs>(this.PaymentTransactions_RowEditEnding);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 3:
            
            #line 36 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.FilterButton_Click);
            
            #line default
            #line hidden
            break;
            case 19:
            
            #line 154 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.RefreshTransactionFilters);
            
            #line default
            #line hidden
            
            #line 154 "..\..\..\View\MainWindow.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.RefreshTransactionFilters);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

