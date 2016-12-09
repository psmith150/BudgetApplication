﻿#pragma checksum "..\..\GroupsAndCategoriesWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D7F19D38266B5A0369EFC84A17EEBB8C"
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
        
        
        #line 19 "..\..\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddGroup;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveGroup;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddCategory;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveCategory;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView GroupList;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\GroupsAndCategoriesWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView CategoryList;
        
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
            System.Uri resourceLocater = new System.Uri("/BudgetApplication;component/groupsandcategorieswindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\GroupsAndCategoriesWindow.xaml"
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
            
            #line 8 "..\..\GroupsAndCategoriesWindow.xaml"
            ((BudgetApplication.GroupsAndCategoriesWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.AddGroup = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\GroupsAndCategoriesWindow.xaml"
            this.AddGroup.Click += new System.Windows.RoutedEventHandler(this.AddGroup_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.RemoveGroup = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\GroupsAndCategoriesWindow.xaml"
            this.RemoveGroup.Click += new System.Windows.RoutedEventHandler(this.RemoveGroup_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.AddCategory = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\GroupsAndCategoriesWindow.xaml"
            this.AddCategory.Click += new System.Windows.RoutedEventHandler(this.AddCategory_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.RemoveCategory = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\GroupsAndCategoriesWindow.xaml"
            this.RemoveCategory.Click += new System.Windows.RoutedEventHandler(this.RemoveCategory_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.GroupList = ((System.Windows.Controls.ListView)(target));
            
            #line 26 "..\..\GroupsAndCategoriesWindow.xaml"
            this.GroupList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.GroupList_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.CategoryList = ((System.Windows.Controls.ListView)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

