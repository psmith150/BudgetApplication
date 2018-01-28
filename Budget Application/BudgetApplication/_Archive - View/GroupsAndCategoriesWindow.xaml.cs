using System.Windows;
using System.Windows.Controls;
using BudgetApplication.Model;
using System.Collections.Specialized;


namespace BudgetApplication.View
{
    /// <summary>
    /// A window used to manage the groups and categories.
    /// </summary>
    public partial class GroupsAndCategoriesWindow : Window
    {
        private int lastGroupIndex; //Used to store the index of the last selected group
        private int lastCategoryIndex;  //Used to store the index of the last selected category
        private Group lastGroup;    //The last selected group
        private Category lastCategory;  //The last selected category

        /// <summary>
        /// Initializes a new window.
        /// </summary>
        public GroupsAndCategoriesWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new window with the specified parent window.
        /// </summary>
        /// <param name="owner">The parent window</param>
        public GroupsAndCategoriesWindow(Window owner) : this()
        {
            this.Owner = owner;
        }

        /// <summary>
        /// Run when the window is loaded. Sets the source for the list of groups.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GroupList.SelectedIndex = 0;
            (GroupList.ItemsSource as MyObservableCollection<Group>).CollectionChanged += GroupModified;
            //(CategoryList.ItemsSource as MyObservableCollection<Category>).CollectionChanged += CategoryModified;
        }

        /// <summary>
        /// Selects the just-added group when a new group has been added.
        /// </summary>
        /// <param name="sender">The sending button</param>
        /// <param name="e">The arguments</param>
        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            GroupList.SelectedIndex = GroupList.Items.Count;
        }

        /// <summary>
        /// Run when the list of groups is changed. Used to keep the cursor on the relevant group.
        /// </summary>
        /// <param name="sender">The collection that was changed</param>
        /// <param name="e">The arguments</param>
        private void GroupModified(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Moves the cursor to the previous group.
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
            //Keeps the cursor on the group being moved.
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                GroupList.SelectedItem = lastGroup;
            }
        }

        /// <summary>
        /// Run when the list of categories is changed. Used to keep the cursor on the relevant category.
        /// </summary>
        /// <param name="sender">The collection that was changed</param>
        /// <param name="e">The arguments</param>
        private void CategoryModified(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Moves the cursor to the previous group.
            if (e.OldItems != null)
            {
                if (lastCategoryIndex > 0 && e.Action == NotifyCollectionChangedAction.Remove)
                {
                    CategoryList.SelectedIndex = lastCategoryIndex - 1;
                }
                else
                {
                    CategoryList.SelectedIndex = 0;
                }
            }
            //Keeps the cursor on the group being moved.
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                CategoryList.SelectedItem = lastCategory;
            }
        }
        /// <summary>
        /// Selects the just-added category when a new category has been added.
        /// </summary>
        /// <param name="sender">The sending button</param>
        /// <param name="e">The arguments</param>
        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            CategoryList.SelectedIndex = CategoryList.Items.Count;
        }

        /// <summary>
        /// Run when the user selects a different group.
        /// Changes the category list source.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((GroupList.SelectedItem as Group) == null)
                return;

            Group selectedGroup = (GroupList.SelectedItem as Group);
            if ((CategoryList.ItemsSource as MyObservableCollection<Category>) != null)
                (CategoryList.ItemsSource as MyObservableCollection<Category>).CollectionChanged -= CategoryModified;
            CategoryList.ItemsSource = selectedGroup.Categories;
            CategoryList.SelectedIndex = 0;
            (CategoryList.ItemsSource as MyObservableCollection<Category>).CollectionChanged += CategoryModified;
        }

        /// <summary>
        /// Saves the current index for when a group is removed or moved
        /// </summary>
        /// <param name="sender">The sending button</param>
        /// <param name="e">The arguments</param>
        private void SaveGroupIndex_Click(object sender, RoutedEventArgs e)
        {
            lastGroupIndex = GroupList.SelectedIndex;
            lastGroup = GroupList.SelectedItem as Group;
        }

        /// <summary>
        /// Saves the current index for when a category is removed or moved
        /// </summary>
        /// <param name="sender">The sending button</param>
        /// <param name="e">The arguments</param>
        private void SaveCategoryIndex_Click(object sender, RoutedEventArgs e)
        {
            lastCategoryIndex = CategoryList.SelectedIndex;
            lastCategory = CategoryList.SelectedItem as Category;
        }
    }
}
