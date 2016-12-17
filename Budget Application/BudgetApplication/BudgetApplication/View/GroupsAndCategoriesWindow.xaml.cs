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
        public GroupsAndCategoriesWindow()
        {
            InitializeComponent();
        }

        public event Action<Group> AddGroup;
        public event Action<Category> AddCategory;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GroupList.SelectedIndex = 0;
            (GroupList.ItemsSource as MyObservableCollection<Group>).CollectionChanged += GroupRemoved;
            (CategoryList.ItemsSource as MyObservableCollection<Category>).CollectionChanged += CategoryRemoved;

        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            GroupList.SelectedIndex = GroupList.Items.Count;
        }

        private void GroupRemoved(object sender, NotifyCollectionChangedEventArgs e)
        {
            //GroupList.SelectedIndex = 0;
            if (e.OldItems != null)
            {
                if (lastGroupIndex > 0)
                {
                    GroupList.SelectedIndex = lastGroupIndex - 1;
                }
                else
                {
                    GroupList.SelectedIndex = 0;
                }
            }
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            CategoryList.SelectedIndex = CategoryList.Items.Count;
        }

        private void CategoryRemoved(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
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
        }

        private void GroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((GroupList.SelectedItem as Group) == null)
                return;

            Group selectedGroup = (GroupList.SelectedItem as Group);
            CategoryList.ItemsSource = selectedGroup.Categories;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void RemoveGroupButton_Click(object sender, RoutedEventArgs e)
        {
            lastGroupIndex = GroupList.SelectedIndex;
        }

        private void RemoveCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            lastCategoryIndex = CategoryList.SelectedIndex;
        }
    }
}
