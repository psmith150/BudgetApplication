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

namespace BudgetApplication
{
    /// <summary>
    /// Interaction logic for GroupsAndCategoriesWindow.xaml
    /// </summary>
    public partial class GroupsAndCategoriesWindow : Window
    {
        public GroupsAndCategoriesWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(CategoryList.ItemsSource);
            GroupList.SelectedIndex = 0;
            view.Filter = GroupFilter;
        }

        private bool GroupFilter(object item)
        {
            String selectedGroup = (GroupList.SelectedItem as Group).Name;
            return ((item as Category).Group.Name.Equals(selectedGroup));
            //return true;
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveGroup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selectedGroup = (GroupList.SelectedItem as Group).Name;
            CollectionViewSource.GetDefaultView(CategoryList.ItemsSource).Refresh();
        }
    }
}
