﻿#pragma checksum "..\..\..\Model\GroupsAndCategoriesWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A8313EA50CDB62AC5301F1E4EEA3BB79"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using BudgetApplication;
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


namespace BudgetApplication {
    
    
    /// <summary>
    /// GroupsAndCategoriesWindow
    /// </summary>
    public partial class GroupsAndCategoriesWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\Model\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddGroupButton;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Model\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveGroupButton;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\Model\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddCategoryButton;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Model\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveCategoryButton;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Model\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid GroupList;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\Model\GroupsAndCategoriesWindow.xaml"
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
            System.Uri resourceLocater = new System.Uri("/BudgetApplication;component/model/groupsandcategorieswindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Model\GroupsAndCategoriesWindow.xaml"
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
            
            #line 8 "..\..\..\Model\GroupsAndCategoriesWindow.xaml"
            ((BudgetApplication.GroupsAndCategoriesWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 8 "..\..\..\Model\GroupsAndCategoriesWindow.xaml"
            ((BudgetApplication.GroupsAndCategoriesWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.AddGroupButton = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\Model\GroupsAndCategoriesWindow.xaml"
            this.AddGroupButton.Click += new System.Windows.RoutedEventHandler(this.AddGroup_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.RemoveGroupButton = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\Model\GroupsAndCategoriesWindow.xaml"
            this.RemoveGroupButton.Click += new System.Windows.RoutedEventHandler(this.RemoveGroup_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.AddCategoryButton = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\Model\GroupsAndCategoriesWindow.xaml"
            this.AddCategoryButton.Click += new System.Windows.RoutedEventHandler(this.AddCategory_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.RemoveCategoryButton = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\Model\GroupsAndCategoriesWindow.xaml"
            this.RemoveCategoryButton.Click += new System.Windows.RoutedEventHandler(this.RemoveCategory_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.GroupList = ((System.Windows.Controls.DataGrid)(target));
            
            #line 26 "..\..\..\Model\GroupsAndCategoriesWindow.xaml"
            this.GroupList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.GroupList_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.CategoryList = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

