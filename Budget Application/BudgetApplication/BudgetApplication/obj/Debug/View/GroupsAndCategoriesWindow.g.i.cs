﻿#pragma checksum "..\..\..\View\GroupsAndCategoriesWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "62325C62DEFE549F0D50DC82E493052D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using BudgetApplication.View;
using System;
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


namespace BudgetApplication.View {
    
    
    /// <summary>
    /// GroupsAndCategoriesWindow
    /// </summary>
    public partial class GroupsAndCategoriesWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddGroupButton;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveGroupButton;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button GroupUpButton;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button GroupDownButton;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddCategoryButton;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveCategoryButton;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CategoryUpButton;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CategoryDownButton;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid GroupList;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid CategoryList;
        
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
            System.Uri resourceLocater = new System.Uri("/BudgetApplication;component/view/groupsandcategorieswindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            
            #line 9 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
            ((BudgetApplication.View.GroupsAndCategoriesWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 9 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
            ((BudgetApplication.View.GroupsAndCategoriesWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.AddGroupButton = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
            this.AddGroupButton.Click += new System.Windows.RoutedEventHandler(this.ModifyGroup_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.RemoveGroupButton = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
            this.RemoveGroupButton.Click += new System.Windows.RoutedEventHandler(this.SaveGroupIndex_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.GroupUpButton = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
            this.GroupUpButton.Click += new System.Windows.RoutedEventHandler(this.SaveGroupIndex_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.GroupDownButton = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
            this.GroupDownButton.Click += new System.Windows.RoutedEventHandler(this.SaveGroupIndex_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.AddCategoryButton = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
            this.AddCategoryButton.Click += new System.Windows.RoutedEventHandler(this.ModifyCategory_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.RemoveCategoryButton = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
            this.RemoveCategoryButton.Click += new System.Windows.RoutedEventHandler(this.SaveCategoryIndex_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.CategoryUpButton = ((System.Windows.Controls.Button)(target));
            
            #line 54 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
            this.CategoryUpButton.Click += new System.Windows.RoutedEventHandler(this.SaveCategoryIndex_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.CategoryDownButton = ((System.Windows.Controls.Button)(target));
            
            #line 57 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
            this.CategoryDownButton.Click += new System.Windows.RoutedEventHandler(this.SaveCategoryIndex_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.GroupList = ((System.Windows.Controls.DataGrid)(target));
            
            #line 62 "..\..\..\View\GroupsAndCategoriesWindow.xaml"
            this.GroupList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.GroupList_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            this.CategoryList = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

