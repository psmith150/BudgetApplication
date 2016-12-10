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

        public event Action<Group> AddGroup;
        public event Action<Category> AddCategory;

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
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Group> groups = GroupList.ItemsSource as ObservableCollection<Group>;
            groups.Add(new Group());
        }

        private void RemoveGroup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show((GroupList.SelectedItem as Group).ToString());
            ObservableCollection<Category> categories = CategoryList.ItemsSource as ObservableCollection<Category>;
            categories.Add(new Category((GroupList.SelectedItem as Group)));
        }

        private void RemoveCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((GroupList.SelectedItem as Group) == null)
                return;

            String selectedGroup = (GroupList.SelectedItem as Group).Name;
            CollectionViewSource.GetDefaultView(CategoryList.ItemsSource).Refresh();
        }
    }
}
