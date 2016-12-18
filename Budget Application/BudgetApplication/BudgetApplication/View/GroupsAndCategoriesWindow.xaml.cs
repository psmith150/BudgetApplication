using System;
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
using System.Windows.Shapes;
using BudgetApplication.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections.Specialized;


namespace BudgetApplication.View
{
    /// <summary>
    /// Interaction logic for GroupsAndCategoriesWindow.xaml
    /// </summary>
    public partial class GroupsAndCategoriesWindow : Window
    {
        private int lastGroupIndex;
        private int lastCategoryIndex;
        private Group lastGroup;
        private Category lastCategory;
        public GroupsAndCategoriesWindow()
        {
            InitializeComponent();
        }

        public event Action<Group> AddGroup;
        public event Action<Category> AddCategory;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GroupList.SelectedIndex = 0;
            (GroupList.ItemsSource as MyObservableCollection<Group>).CollectionChanged += GroupModified;
            (CategoryList.ItemsSource as MyObservableCollection<Category>).CollectionChanged += CategoryModified;

        }

        private void ModifyGroup_Click(object sender, RoutedEventArgs e)
        {
            GroupList.SelectedIndex = GroupList.Items.Count;
        }

        private void GroupModified(object sender, NotifyCollectionChangedEventArgs e)
        {
            //GroupList.SelectedIndex = 0;
            if (e.OldItems != null)
            {
                if (lastGroupIndex > 0 && e.Action == NotifyCollectionChangedAction.Remove)
                {
                    GroupList.SelectedIndex = lastGroupIndex - 1;
                }
                else
                {
                    GroupList.SelectedIndex = 0;
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                GroupList.SelectedItem = lastGroup;
            }
        }

        private void ModifyCategory_Click(object sender, RoutedEventArgs e)
        {
            CategoryList.SelectedIndex = CategoryList.Items.Count;
        }

        private void CategoryModified(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null && e.Action == NotifyCollectionChangedAction.Remove)
            {
                if(lastCategoryIndex > 0)
                {
                    CategoryList.SelectedIndex = lastCategoryIndex - 1;
                }
                else
                {
                    CategoryList.SelectedIndex = 0;
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                CategoryList.SelectedItem = lastCategory;
            }
        }

        private void GroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((GroupList.SelectedItem as Group) == null)
                return;

            Group selectedGroup = (GroupList.SelectedItem as Group);
            CategoryList.ItemsSource = selectedGroup.Categories;
            CategoryList.SelectedIndex = 0;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void SaveGroupIndex_Click(object sender, RoutedEventArgs e)
        {
            lastGroupIndex = GroupList.SelectedIndex;
            lastGroup = GroupList.SelectedItem as Group;
        }

        private void SaveCategoryIndex_Click(object sender, RoutedEventArgs e)
        {
            lastCategoryIndex = CategoryList.SelectedIndex;
            lastCategory = CategoryList.SelectedItem as Category;
        }
    }
}
