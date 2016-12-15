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


namespace BudgetApplication.View
{
    /// <summary>
    /// Interaction logic for GroupsAndCategoriesWindow.xaml
    /// </summary>
    public partial class GroupsAndCategoriesWindow : Window
    {
        private CollectionView view;
        public GroupsAndCategoriesWindow()
        {
            InitializeComponent();

        }

        public event Action<Group> AddGroup;
        public event Action<Category> AddCategory;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GroupList.SelectedIndex = 0;
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            //AddGroupButton.Command.Execute(AddGroupButton.CommandParameter);
            //GroupList.SelectedIndex = GroupList.Items.Count - 1;
        }

        private void RemoveGroup_Click(object sender, RoutedEventArgs e)
        {
            //RemoveGroupButton.Command.
            //GroupList.SelectedIndex = 0;
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            //TODO
            return;
            //MessageBox.Show((GroupList.SelectedItem as Group).ToString());

        }

        private void RemoveCategory_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
