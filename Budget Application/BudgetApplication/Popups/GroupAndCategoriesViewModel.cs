using BudgetApplication.Model;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace BudgetApplication.Popups
{
    public class GroupAndCategoriesViewModel : PopupViewModel
    {
        #region Commands
        public ICommand AddGroupCommand { get; private set; }
        public ICommand RemoveGroupCommand { get; private set; }
        public ICommand AddCategoryCommand { get; private set; }
        public ICommand RemoveCategoryCommand { get; private set; }
        public ICommand MoveGroupUpCommand { get; private set; }
        public ICommand MoveGroupDownCommand { get; private set; }
        public ICommand MoveCategoryUpCommand { get; private set; }
        public ICommand MoveCategoryDownCommand { get; private set; }
        #endregion
        #region Private Fields
        SessionService sessionService;
        #endregion

        public GroupAndCategoriesViewModel(SessionService session) : base(session)
        {
            this.Groups = session.Groups;
            this.categories = session.Categories;
            this.CategoriesView = new ListCollectionView(this.categories);
            this.CategoriesView.Filter = ((category) => CategoryView_Filter(category as Category));
            this.sessionService = session;

            AddGroupCommand = new RelayCommand(() => AddGroup(new Group()));
            RemoveGroupCommand = new RelayCommand(() => RemoveGroup(this.SelectedGroupItem));
            AddCategoryCommand = new RelayCommand(() => AddCategory(this.SelectedGroupItem));
            RemoveCategoryCommand = new RelayCommand(() => RemoveCategory(this.SelectedCategoryItem));
            MoveGroupUpCommand = new RelayCommand(() => MoveGroupUp(this.SelectedGroupItem));
            MoveGroupDownCommand = new RelayCommand(() => MoveGroupDown(this.SelectedGroupItem));
            MoveCategoryUpCommand = new RelayCommand(() => MoveCategoryUp(this.SelectedCategoryItem));
            MoveCategoryDownCommand = new RelayCommand(() => MoveCategoryDown(this.SelectedCategoryItem));
        }

        public override void Initialize(object param)
        {
            this.SelectedGroupIndex = 0;
        }

        public override void Deinitialize()
        {
        }

        #region Public Properties
        private MyObservableCollection<Group> _groups;
        public MyObservableCollection<Group> Groups
        {
            get
            {
                return _groups;
            }
            private set
            {
                this._groups = value;
            }
        }

        private ListCollectionView _categories;
        public ListCollectionView CategoriesView
        {
            get
            {
                return _categories;
            }
            private set
            {
                this._categories = value;
            }
        }

        private int _selectedGroupIndex;
        public int SelectedGroupIndex
        {
            get
            {
                return _selectedGroupIndex;
            }
            set
            {
                this.Set(ref this._selectedGroupIndex, value);
            }
        }

        private Group _selectedGroupItem;
        public Group SelectedGroupItem
        {
            get
            {
                return _selectedGroupItem;
            }
            set
            {
                this.Set(ref this._selectedGroupItem, value);
                this.CategoriesView.Refresh();
            }
        }

        private int _selectedCategoryIndex;
        public int SelectedCategoryIndex
        {
            get
            {
                return _selectedCategoryIndex;
            }
            set
            {
                this.Set(ref this._selectedCategoryIndex, value);
            }
        }

        private Category _selectedCategoryItem;
        public Category SelectedCategoryItem
        {
            get
            {
                return _selectedCategoryItem;
            }
            set
            {
                this.Set(ref this._selectedCategoryItem, value);
            }
        }
        #endregion

        #region Private Fields
        MyObservableCollection<Category> categories;
            #endregion

        #region Private Methods

        /// <summary>
        /// Adds the specified group to the collection
        /// </summary>
        /// <param name="group">The group to add</param>
        private void AddGroup(Group group)
        {
            //If the group has no name, create a default group
            if (String.IsNullOrEmpty(group.Name))
            {
                AddGroup(new Group());
            }
            int index = 0;
            bool nameExists = true;
            String name = "";
            //Check if the group name already exists. If so, create it with a number on the end.
            while (nameExists)
            {
                nameExists = false;
                if (index == 0)
                {
                    name = String.Copy(group.Name);
                }
                else
                {
                    name = String.Copy(group.Name) + index.ToString();
                }
                foreach (Group currGroup in _groups)
                {
                    if (name.Equals(currGroup.Name))
                    {
                        nameExists = true;
                    }
                }
                index++;
            }
            group.Name = name;
            _groups.Add(group);
            this.SelectedGroupIndex = this.Groups.Count;
        }

        /// <summary>
        /// Remove the specified group from the collection.
        /// </summary>
        /// <param name="group">The group to remove</param>
        private void RemoveGroup(Group group)
        {
            if (!_groups.Remove(group))
            {
                throw new ArgumentException("Group " + group.Name + " does not exist");
            }
            //Remove the group's categories
            foreach (Category category in group.Categories)
            {
                _categories.Remove(category);
            }
        }

        /// <summary>
        /// Moves the specified group closer to the front (0-index) of the collection.
        /// </summary>
        /// <param name="group">The group to move</param>
        private void MoveGroupUp(Group group)
        {
            if (group == null)
                return;
            int index = _groups.IndexOf(group);
            if (index < 0)
            {
                throw new ArgumentException("Group " + group.Name + " does not exist");
            }
            if (index > 0)
            {
                int startIndex = this.sessionService.BudgetValues.IndexOf(this.sessionService.BudgetValues.First(x => x.Group == group));
                int targetIndex = this.sessionService.BudgetValues.IndexOf(this.sessionService.BudgetValues.First(x => x.Group == _groups.ElementAt(index - 1)));
                int endIndex = this.sessionService.BudgetValues.IndexOf(this.sessionService.BudgetValues.Last(x => x.Group == group));
                _groups.Move(index, index - 1);
                this.sessionService.MoveTotalRows(index, index - 1);
                int offset = targetIndex - startIndex;
                //Move each row in the Values grids
                for (int i = 0; i <= endIndex - startIndex; i++)
                {
                    this.sessionService.MoveValueRows(startIndex + i, startIndex + i + offset);
                }
                this.sessionService.RefreshListViews(); //Refresh grouping
                this.SelectedGroupItem = group;
            }
        }

        /// <summary>
        /// Moves the specified group closer to the back (N-index) of the collection.
        /// </summary>
        /// <param name="group">The group to move</param>
        private void MoveGroupDown(Group group)
        {
            if (group == null)
                return;
            int index = _groups.IndexOf(group);
            if (index < 0)
            {
                throw new ArgumentException("Group " + group.Name + " does not exist");
            }
            if (index < _groups.Count - 1)
            {
                int startIndex = this.sessionService.BudgetValues.IndexOf(this.sessionService.BudgetValues.First(x => x.Group == group));
                int targetIndex = this.sessionService.BudgetValues.IndexOf(this.sessionService.BudgetValues.Last(x => x.Group == _groups.ElementAt(index + 1)));
                int endIndex = this.sessionService.BudgetValues.IndexOf(this.sessionService.BudgetValues.Last(x => x.Group == group));
                _groups.Move(index, index + 1);
                this.sessionService.MoveTotalRows(index, index + 1);
                int offset = targetIndex - startIndex;
                //Move each row in the Values grids
                for (int i = 0; i <= endIndex - startIndex; i++)
                {
                    this.sessionService.MoveValueRows(startIndex, startIndex + offset);
                }
                this.sessionService.RefreshListViews(); //Refresh grouping
                this.SelectedGroupItem = group;
            }
        }

        /// <summary>
        /// Adds a category with the specified name to the category list.
        /// </summary>
        /// <param name="category">The category to add</param>
        /// <param name="group">The group to associate the category to</param>
        private void AddCategory(Group group, Category category = null)
        {
            if (category == null)
            {
                category = new Category();
            }
            if (String.IsNullOrEmpty(category.Name))
            {
                AddCategory(group, new Category());
            }

            int index = 0;
            bool nameExists = true;
            String name = "";
            //Checks if name is already used. Adds a number to the end if so.
            while (nameExists)
            {
                nameExists = false;
                if (index == 0)
                {
                    name = String.Copy(category.Name);
                }
                else
                {
                    name = String.Copy(category.Name) + index.ToString();
                }
                foreach (Category currCategory in _categories)
                {
                    if (name.Equals(currCategory.Name))
                    {
                        nameExists = true;
                    }
                }
                index++;
            }
            category.Name = name;
            group.Categories.Add(category); //Add the category to its group
            this.categories.Add(category);

            //Inserts the category in the correct position (after other categories in its group)
            int previousCategoryIndex = group.Categories.IndexOf(category) - 1;
            if (previousCategoryIndex >= 0)
            {
                MoneyGridRow previousRow;
                try
                {
                    previousRow = this.sessionService.BudgetValues.Single(x => x.Category == group.Categories.ElementAt(previousCategoryIndex));
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Could not find row with category " + group.Categories.ElementAt(previousCategoryIndex), ex);
                }
                int previousRowIndex = this.sessionService.BudgetValues.IndexOf(previousRow);
                this.categories.Move(this.sessionService.BudgetValues.Count - 1, previousRowIndex + 1);
                this.sessionService.MoveValueRows(this.sessionService.BudgetValues.Count - 1, previousRowIndex + 1);
            }
        }

        /// <summary>
        /// Removes the specified category.
        /// </summary>
        /// <param name="category">The category to remove</param>
        private void RemoveCategory(Category category)
        {
            Group currGroup = GetCategoryGroup(category);
            if (currGroup == null)
            {
                throw new ArgumentException("Category" + category.Name + " is not part of a group");
            }
            currGroup.Categories.Remove(category);  //Remove from the group
            if (!this.categories.Remove(category))
            {
                throw new ArgumentException("Category " + category.Name + " does not exist");
            }
        }

        /// <summary>
        /// Move a category closer to the front (0-index) of its group's collection
        /// </summary>
        /// <param name="category">The category to move up</param>
        private void MoveCategoryUp(Category category)
        {
            if (category == null)
                return;
            Group group = GetCategoryGroup(category);
            if (group == null)
            {
                throw new ArgumentException("Category " + category.Name + " does not belong to a group");
            }
            int index = group.Categories.IndexOf(category);
            if (index < 0)
            {
                throw new ArgumentException("Group " + category.Name + " does not exist");
            }
            if (index > 0)
            {
                //Move all Value rows up
                group.Categories.Move(index, index - 1);
                MoneyGridRow budgetRow = this.sessionService.BudgetValues.Single(x => x.Category == category);
                int rowIndex = this.sessionService.BudgetValues.IndexOf(budgetRow);
                this.sessionService.MoveValueRows(rowIndex, rowIndex - 1);
            }
        }

        /// <summary>
        /// Move a category closer to the back (N-index) of its group's collection
        /// </summary>
        /// <param name="category">The category to move up</param>
        private void MoveCategoryDown(Category category)
        {
            if (category == null)
                return;
            Group group = GetCategoryGroup(category);
            if (group == null)
            {
                throw new ArgumentException("Category " + category.Name + " does not belong to a group");
            }
            int index = group.Categories.IndexOf(category);
            if (index < 0)
            {
                throw new ArgumentException("Category " + category.Name + " does not exist");
            }
            if (index < group.Categories.Count - 1)
            {
                //Move all the Values rows down
                group.Categories.Move(index, index + 1);
                MoneyGridRow budgetRow = this.sessionService.BudgetValues.Single(x => x.Category == category);
                int rowIndex = this.sessionService.BudgetValues.IndexOf(budgetRow);
                this.sessionService.MoveValueRows(rowIndex, rowIndex + 1);
            }
        }

        /// <summary>
        /// Finds the group corresponding to the specified category
        /// </summary>
        /// <param name="category">The category to locate</param>
        /// <returns>The mathching group</returns>
        private Group GetCategoryGroup(Category category)
        {
            foreach (Group group in _groups)
            {
                if (group.Categories.Contains(category))
                {
                    return group;
                }
            }
            return null;
        }

        private bool CategoryView_Filter(Category category)
        {
            return this.SelectedGroupItem.Categories.Contains(category);
        }
        #endregion
    }
}
