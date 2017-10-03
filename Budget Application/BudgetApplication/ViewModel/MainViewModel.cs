using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using BudgetApplication.Model;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Xml.Serialization;

namespace BudgetApplication.ViewModel
{
    /// <summary>
    /// The view model for all the data in the application
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        //Data collections
        private MyObservableCollection<Transaction> _transactions;  //Collection of transactions
        private MyObservableCollection<Category> _categories;   //Collection of categories
        private MyObservableCollection<Group> _groups;  //collection of groups
        private MyObservableCollection<PaymentMethod> _paymentMethods;  //Collection of payment methods
        private MyObservableCollection<MoneyGridRow> _budgetValues; //Values used in the value grid of the budget view
        private MyObservableCollection<MoneyGridRow> _budgetTotals; //Values used in the total grid of the budget view
        private TotalObservableCollection _budgetBudgetAndSum; //Values used in the budget and sum grid of the budget view
        private MyObservableCollection<MoneyGridRow> _spendingValues; //Values used in the value grid of the spending view
        private MyObservableCollection<MoneyGridRow> _spendingTotals; //Values used in the total grid of the spending view
        private TotalObservableCollection _spendingBudgetAndSum; //Values used in the budget and sum grid of the spending view
        private MyObservableCollection<MoneyGridRow> _comparisonValues; //Values used in the value grid of the comparison view
        private MyObservableCollection<MoneyGridRow> _comparisonTotals; //Values used in the total grid of the spending view
        private TotalObservableCollection _comparisonBudgetAndSum; //Values used in the budget and sum grid of the comparison view
        private MyObservableCollection<MonthDetailRow> _monthDetails; //Values for showing the percentage spent in each category

        //List collection views used to display data for Values grids. Defined here to control grouping.
        private ListCollectionView _budgetValueView;
        private ListCollectionView _spendingValueView;
        private ListCollectionView _comparisonValueView;
        private ListCollectionView _monthDetailsView;

        //Groups for income and expenditures in the Totals grids.
        private Group columnIncomeTotalsGroup;
        private Group columnExpendituresTotalsGroup;

        //Data used for file management
        private ObservableCollection<String> _yearList; //List of all years that files have been created for.
        private String fileName = "data";   //Name of the data files: <fileName>_<_currentYear>.xml
        private String filePath = "";   //File path of the data files.
        private String completeFilePath;    //Complete file path
        private String _currentYear;    //The year of data currently loaded.
        private bool _validYear;    //Returns if the supplied year is valid.

        //Payment method to allow filtering by all payment methods
        private CheckingAccount _allPayments;
        private ObservableCollection<PaymentMethod> _allPaymentsCollection;

        //Data used for display month details
        private int _selectedMonth;

        /// <summary>
        /// Instantiates a new MainViewModel object. Run when the application is launched. Initializes variables and loads data
        /// </summary>
        public MainViewModel()
        {
            columnIncomeTotalsGroup = new Group(true, "Income Totals");
            columnExpendituresTotalsGroup = new Group(false, "Expenditure totals");

            //Default to showing all transactions in the last month
            _allPayments = new CheckingAccount("All");
            _allPayments.StartDate = DateTime.Now.AddMonths(-1);
            _allPayments.EndDate = DateTime.Now;
            _allPaymentsCollection = new ObservableCollection<PaymentMethod>();
            _allPaymentsCollection.Add(_allPayments);

            //Initialize groups
            _groups = new MyObservableCollection<Group>();
            _categories = new MyObservableCollection<Category>();
            _transactions = new MyObservableCollection<Transaction>();
            _paymentMethods = new MyObservableCollection<PaymentMethod>();
            _budgetValues = new MyObservableCollection<MoneyGridRow>();
            _budgetTotals = new MyObservableCollection<MoneyGridRow>();
            _budgetBudgetAndSum = new TotalObservableCollection(_budgetTotals);
            _spendingValues = new MyObservableCollection<MoneyGridRow>();
            _spendingTotals = new MyObservableCollection<MoneyGridRow>();
            _spendingBudgetAndSum = new TotalObservableCollection(_spendingTotals);
            _comparisonValues = new MyObservableCollection<MoneyGridRow>();
            _comparisonTotals = new MyObservableCollection<MoneyGridRow>();
            _comparisonBudgetAndSum = new TotalObservableCollection(_comparisonTotals);
            _comparisonBudgetAndSum.IsComparison = true;

            _monthDetails = new MyObservableCollection<MonthDetailRow>();

            InitListViews();    //Creates the Values grid ListCollectionViews.

            //Sets event handlers to make sure all data is updated
            _categories.CollectionChanged += CategoryCollectionChanged;  //Used to add/remove rows
            _groups.CollectionChanged += GroupsCollectionChanged;  //Used to add/remove rows
            _groups.MemberChanged += GroupChanged;  //Used to update grouping
            _budgetValues.MemberChanged += UpdateBudgetTotals;  //Update respective totals
            _transactions.MemberChanged += UpdateSpendingValues;    //Update spending if transaction has been modified
            _transactions.MemberChanged += OnTransactionModified;   //Trigger event for view to handle
            _transactions.CollectionChanged += AddOrRemoveSpendingValues;   //Update spending if transaction has been added or removed
            _transactions.CollectionChanged += OnTransactionsChanged;   //Trigger event for view to handle
            //_spendingTotals.MemberChanged += UpdateComparisonValues;    //Update comparison values if a spending values was changed. Totals used to allow bulk modification.
            //_budgetTotals.MemberChanged += UpdateComparisonValues;  //Update comparison values if a budget value was changed. Totals used to allow bulk modification.

            //Set initial month to current month
            _selectedMonth = DateTime.Now.Month - 1;

            _yearList = new ObservableCollection<string>();
            GetYears(); //Finds all the existing data files
            if (_yearList.Count == 0)   //No data found; create empty file and save it.
            {
                MessageBox.Show("No data found; creating blank file");
                _currentYear = DateTime.Today.Year.ToString();
                completeFilePath = filePath + fileName + "_" + _currentYear + ".xml";
                SaveData();
            }
            else    //Select most recent data. TODO: allow user to choose year
            {
                _currentYear = _yearList.Last();
                completeFilePath = filePath + fileName + "_" + _currentYear + ".xml";
                LoadData();
            }

            //Commands to allow binding to View
            SaveDataCommand = new RelayCommand(() => SaveData());
            LoadDataCommand = new RelayCommand(() => LoadData());
            AddGroupCommand = new RelayCommand<Group>((group) => AddGroup(new Group()));
            RemoveGroupCommand = new RelayCommand<Group>((group) => RemoveGroup(group));
            AddCategoryCommand = new RelayCommand<Group>((group) => AddCategory(group));
            RemoveCategoryCommand = new RelayCommand<Category>((category) => RemoveCategory(category));
            MoveGroupUpCommand = new RelayCommand<Group>((group) => MoveGroupUp(group));
            MoveGroupDownCommand = new RelayCommand<Group>((group) => MoveGroupDown(group));
            MoveCategoryUpCommand = new RelayCommand<Category>((category) => MoveCategoryUp(category));
            MoveCategoryDownCommand = new RelayCommand<Category>((category) => MoveCategoryDown(category));
            AddPaymentMethodCommand = new RelayCommand<PaymentMethod>((paymentMethod) => AddPaymentMethod(paymentMethod));
            RemovePaymentMethodCommand = new RelayCommand<PaymentMethod>((paymentMethod) => RemovePaymentMethod(paymentMethod));
            AddYearCommand = new RelayCommand<String>((year) => AddYear(year));
            CopyDataCommand = new RelayCommand<string>((year) => CopyData(year));

        }

        #region Private helpers

        /// <summary>
        /// Initialize the ListCollectionViews with grouping. Used in the Value grids
        /// </summary>
        private void InitListViews()
        {
            _budgetValueView = new ListCollectionView(_budgetValues);
            _budgetValueView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
            _spendingValueView = new ListCollectionView(_spendingValues);
            _spendingValueView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
            _comparisonValueView = new ListCollectionView(_comparisonValues);
            _comparisonValueView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
            _monthDetailsView = new ListCollectionView(_monthDetails);
            _monthDetailsView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        }

        /// <summary>
        /// Refresh the ListCollectionViews. Used to update grouping.
        /// </summary>
        private void RefreshListViews()
        {
            _budgetValueView.Refresh();
            _spendingValueView.Refresh();
            _comparisonValueView.Refresh();
        }

        /// <summary>
        /// Moves a row of values on all tabs
        /// </summary>
        /// <param name="start">Start index</param>
        /// <param name="end">End index</param>
        private void MoveValueRows(int start, int end)
        {
            _budgetValues.Move(start, end);
            _spendingValues.Move(start, end);
            _comparisonValues.Move(start, end);
        }

        /// <summary>
        /// Moves a row of totals on all tabs
        /// </summary>
        /// <param name="start">Start index</param>
        /// <param name="end">End index</param>
        private void MoveTotalRows(int start, int end)
        {
            _budgetTotals.Move(start, end);
            _spendingTotals.Move(start, end);
            _comparisonTotals.Move(start, end);
        }
        #endregion

        #region Fields

        /// <summary>
        /// The collection of groups
        /// </summary>
        public MyObservableCollection<Group> Groups
        {
            get
            {
                return _groups;
            }
        }

        /// <summary>
        /// The collection of categories
        /// </summary>
        public MyObservableCollection<Category> Categories
        {
            get
            {
                return _categories;
            }
            private set
            {
                _categories = value;
            }
        }

        /// <summary>
        /// The collection of transactions
        /// </summary>
        public ObservableCollection<Transaction> Transactions
        {
            get
            {
                return _transactions;
            }
        }

        /// <summary>
        /// The collection of payment methods
        /// </summary>
        public MyObservableCollection<PaymentMethod> PaymentMethods
        {
            get
            {
                return _paymentMethods;
            }
        }

        /// <summary>
        /// The collection of values in the budget tab's Values grid
        /// </summary>
        public ListCollectionView BudgetRows
        {
            get
            {
                return _budgetValueView;
            }
        }

        /// <summary>
        /// The collection of values in the budget tab's Totals grid
        /// </summary>
        public MyObservableCollection<MoneyGridRow> BudgetTotals
        {
            get
            {
                return _budgetTotals;
            }
        }

        /// <summary>
        /// The collection of values in the budget tab's Budget and Sum grid
        /// </summary>
        public TotalObservableCollection BudgetBudgetAndSum
        {
            get
            {
                return _budgetBudgetAndSum;
            }
        }

        /// <summary>
        /// The collection of values in the spending tab's Values grid
        /// </summary>
        public ListCollectionView SpendingRows
        {
            get
            {
                return _spendingValueView;
            }
        }

        /// <summary>
        /// The collection of values in the spending tab's Totals grid
        /// </summary>
        public MyObservableCollection<MoneyGridRow> SpendingTotals
        {
            get
            {
                return _spendingTotals;
            }
            set
            {
                _spendingTotals = value;
            }
        }

        /// <summary>
        /// The collection of values in the spending tab's Budget and Sum grid
        /// </summary>
        public TotalObservableCollection SpendingBudgetAndSum
        {
            get
            {
                return _spendingBudgetAndSum;
            }
        }

        /// <summary>
        /// The collection of values in the comparison tab's Values grid
        /// </summary>
        public ListCollectionView ComparisonRows
        {
            get
            {
                return _comparisonValueView;
            }
        }

        /// <summary>
        /// The collection of values in the comparison tab's Totals grid
        /// </summary>
        public MyObservableCollection<MoneyGridRow> ComparisonTotals
        {
            get
            {
                return _comparisonTotals;
            }
            set
            {
                _comparisonTotals = value;
            }
        }

        /// <summary>
        /// The collection of values in the comparison tab's Budget and Sum grid
        /// </summary>
        public TotalObservableCollection ComparisonBudgetAndSum
        {
            get
            {
                return _comparisonBudgetAndSum;
            }
        }

        /// <summary>
        /// The list of available years
        /// </summary>
        public ObservableCollection<String> YearList
        {
            get
            {
                return _yearList;
            }
        }

        /// <summary>
        /// The year currently loaded. Reloads data when set.
        /// </summary>
        public String CurrentYear
        {
            get
            {
                return _currentYear;
            }
            set
            {
                _currentYear = value;
                //Debug.WriteLine("Loading data for year: " + _currentYear);
                completeFilePath = filePath + fileName + "_" + _currentYear + ".xml";
                //LoadData();
                RaisePropertyChanged("CurrentYear");
            }
        }

        //Returns if a year is valid or not
        public Boolean ValidYear
        {
            get
            {
                return _validYear;
            }
        }

        //The collection that contains the All Payments payment method
        public ObservableCollection<PaymentMethod> AllPayments
        {
            get
            {
                return _allPaymentsCollection;
            }
        }

        public int SelectedMonth
        {
            get
            {
                return _selectedMonth;
            }
            set
            {
                _selectedMonth = value;
                RaisePropertyChanged("SelectedMonth");
                UpdateMonthDetails();
                Debug.WriteLine("Updating month to " + _selectedMonth);
                RaisePropertyChanged("MonthDetails");
                RaisePropertyChanged("PercentMonth");
            }
        }

        public ListCollectionView MonthDetails
        {
            get
            {
                return _monthDetailsView;
            }
        }

        public double PercentMonth
        {
            get
            {
                if (_selectedMonth != DateTime.Now.Month - 1)
                    return 1.0;
                return (double)DateTime.Now.Day / DateTime.DaysInMonth(Int32.Parse(_currentYear), _selectedMonth);
            }
        }


        #endregion

        #region Methods common to all tabs

        /// <summary>
        /// Calculates the sum of the columns in a value grid.
        /// </summary>
        /// <param name="columnValues">The values to be summed</param>
        /// <param name="columnTotals">The values to hold the sums</param>
        /// <param name="propertyName">The property name to trigger a notification</param>
        private void CalculateColumnTotals(ObservableCollection<MoneyGridRow> columnValues, ObservableCollection<MoneyGridRow> columnTotals, String propertyName)
        {
            //Don't do anything if not all the rows have been loaded yet
            if (columnValues.Count < _categories.Count || columnTotals.Count < _groups.Count)
                return;
            double totalGridIncomeTotal = 0.0;
            double totalGridExpenditureTotal = 0.0;
            foreach (Group group in _groups) //For each group, find its total row and then sum all the category rows that are part of the group
            {
                double groupTotal = 0.0;
                decimal[] groupSum = new decimal[12];
                MoneyGridRow total;
                try
                {
                    total = columnTotals.Single(x => x.Category.Name.Equals(group.Name));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Could not find corresponding total row: " + group.Name);
                    continue;
                    //throw new ArgumentException("Could not find corresponding total row: " + group.Name, ex);
                }
                foreach (Category category in group.Categories)
                {
                    try
                    {
                        MoneyGridRow row = columnValues.Single(x => x.Group == group && x.Category == category);
                        for (int i = 0; i < row.Values.Count; i++)
                        {
                            groupSum[i] += row.Values[i];
                        }
                        groupTotal += (double)row.Sum;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(propertyName + " does not contain row for group " + group + " and category " + category);
                        continue;
                        //Debug.WriteLine(columnValues.ElementAt(0).Group + " " + columnValues.ElementAt(0).Category);
                        throw new ArgumentException("Could not find corresponding row", ex);
                    }
                }
                total.Values.Values = groupSum;
                groupTotal = (double)total.Sum;
                if (group.IsIncome)
                    totalGridIncomeTotal += (double)total.Sum;
                else
                    totalGridExpenditureTotal += (double)total.Sum;
                foreach (Category category in group.Categories)
                {
                    try
                    {
                        MoneyGridRow row = columnValues.Single(x => x.Group == group && x.Category == category);
                        row.Percentage = (double)row.Sum / groupTotal;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(propertyName + " does not contain row for group " + group + " and category " + category);
                        continue;
                        //Debug.WriteLine(columnValues.ElementAt(0).Group + " " + columnValues.ElementAt(0).Category);
                        throw new ArgumentException("Could not find corresponding row", ex);
                    }
                }
                //RaisePropertyChanged(propertyName);
            }
            foreach (MoneyGridRow row in columnTotals)
            {
                if (row.Group.IsIncome)
                    row.Percentage = (double)row.Sum / totalGridIncomeTotal;
                else
                    row.Percentage = (double)row.Sum / totalGridExpenditureTotal;
            }
        }

        #endregion

        #region Methods to modify group and category collections

        /// <summary>
        /// Adds the specified group to the collection
        /// </summary>
        /// <param name="group">The group to add</param>
        public void AddGroup(Group group)
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
        }

        /// <summary>
        /// Remove the specified group from the collection.
        /// </summary>
        /// <param name="group">The group to remove</param>
        public void RemoveGroup(Group group)
        {
            if (!_groups.Remove(group))
            {
                throw new ArgumentException("Group " + group.Name + " does not exist");
            }
            //Debug.WriteLine("Removed group " + group.Name);

            //Remove the group's categories
            foreach (Category category in group.Categories)
            {
                //Debug.WriteLine("Removed category " + category.Name);

                _categories.Remove(category);
            }
        }

        /// <summary>
        /// Moves the specified group closer to the front (0-index) of the collection.
        /// </summary>
        /// <param name="group">The group to move</param>
        public void MoveGroupUp(Group group)
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
                int startIndex = _budgetValues.IndexOf(_budgetValues.First(x => x.Group == group));
                int targetIndex = _budgetValues.IndexOf(_budgetValues.First(x => x.Group == _groups.ElementAt(index - 1)));
                int endIndex = _budgetValues.IndexOf(_budgetValues.Last(x => x.Group == group));
                _groups.Move(index, index - 1);
                //Debug.WriteLine("Moved group " + group.Name + " one row up");
                MoveTotalRows(index, index - 1);
                int offset = targetIndex - startIndex;
                //Debug.WriteLine("Offset is " + offset);
                //Move each row in the Values grids
                for (int i = 0; i <= endIndex - startIndex; i++)
                {
                    MoveValueRows(startIndex + i, startIndex + i + offset);
                    //Debug.WriteLine("Row moved from " + (startIndex+i) + " to " + (startIndex + i + offset));
                }
                RefreshListViews(); //Refresh grouping
            }
        }

        /// <summary>
        /// Moves the specified group closer to the back (N-index) of the collection.
        /// </summary>
        /// <param name="group">The group to move</param>
        public void MoveGroupDown(Group group)
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
                int startIndex = _budgetValues.IndexOf(_budgetValues.First(x => x.Group == group));
                int targetIndex = _budgetValues.IndexOf(_budgetValues.Last(x => x.Group == _groups.ElementAt(index + 1)));
                int endIndex = _budgetValues.IndexOf(_budgetValues.Last(x => x.Group == group));
                _groups.Move(index, index + 1);
                MoveTotalRows(index, index + 1);
                int offset = targetIndex - startIndex;
                //Move each row in the Values grids
                for (int i = 0; i <= endIndex - startIndex; i++)
                {
                    MoveValueRows(startIndex, startIndex + offset);
                    //Debug.WriteLine("Row moved from " + startIndex + " to " + (i + offset));
                }
                RefreshListViews(); //Refresh grouping
            }
        }

        /// <summary>
        /// Adds a category with the specified name to the category list.
        /// </summary>
        /// <param name="category">The category to add</param>
        /// <param name="group">The group to associate the category to</param>
        public void AddCategory(Group group, Category category = null)
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
            _categories.Add(category);

            //Inserts the category in the correct position (after other categories in its group)
            int previousCategoryIndex = group.Categories.IndexOf(category) - 1;
            if (previousCategoryIndex >= 0)
            {
                MoneyGridRow previousRow;
                try
                {
                    previousRow = _budgetValues.Single(x => x.Category == group.Categories.ElementAt(previousCategoryIndex));
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Could not find row with category " + group.Categories.ElementAt(previousCategoryIndex), ex);
                }
                int previousRowIndex = _budgetValues.IndexOf(previousRow);
                _categories.Move(_budgetValues.Count - 1, previousRowIndex + 1);
                MoveValueRows(_budgetValues.Count - 1, previousRowIndex + 1);
            }
        }

        /// <summary>
        /// Removes the specified category.
        /// </summary>
        /// <param name="category">The category to remove</param>
        public void RemoveCategory(Category category)
        {
            Group currGroup = GetCategoryGroup(category);
            if (currGroup == null)
            {
                throw new ArgumentException("Category" + category.Name + " is not part of a group");
            }
            currGroup.Categories.Remove(category);  //Remove from the group
            if (!_categories.Remove(category))
            {
                throw new ArgumentException("Category " + category.Name + " does not exist");
            }
            //Debug.WriteLine("Removed category " + category.Name);

        }

        /// <summary>
        /// Move a category closer to the front (0-index) of its group's collection
        /// </summary>
        /// <param name="category">The category to move up</param>
        public void MoveCategoryUp(Category category)
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
                MoneyGridRow budgetRow = _budgetValues.Single(x => x.Category == category);
                int rowIndex = _budgetValues.IndexOf(budgetRow);
                //Debug.WriteLine("index is " + rowIndex);
                MoveValueRows(rowIndex, rowIndex - 1);
                //Debug.WriteLine("new index is " + _budgetValues.IndexOf(budgetRow));
            }
        }

        /// <summary>
        /// Move a category closer to the back (N-index) of its group's collection
        /// </summary>
        /// <param name="category">The category to move up</param>
        public void MoveCategoryDown(Category category)
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
                MoneyGridRow budgetRow = _budgetValues.Single(x => x.Category == category);
                int rowIndex = _budgetValues.IndexOf(budgetRow);
                MoveValueRows(rowIndex, rowIndex + 1);
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

        /// <summary>
        /// Called when a category has its properties modified
        /// </summary>
        /// <param name="sender">The modified object</param>
        /// <param name="e">The arguments</param>
        public void CategoryChanged(Object sender, PropertyChangedEventArgs e)
        {
            //RefreshListViews();
        }

        /// <summary>
        /// Called when a group has its properties modified. Used to update the groupings.
        /// </summary>
        /// <param name="sender">The modified object</param>
        /// <param name="e">The arguments</param>
        public void GroupChanged(Object sender, PropertyChangedEventArgs e)
        {
            RefreshListViews();
        }

        /// <summary>
        /// Called when the category collection is changed. Used to add/remove Values rows.
        /// </summary>
        /// <param name="sender">The modified collection</param>
        /// <param name="e">The arguments</param>
        public void CategoryCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            //Debug.WriteLine("Adding category");
            //Adds Values rows
            if (e.NewItems != null && e.Action != NotifyCollectionChangedAction.Move)
            {
                foreach (Category newCategory in e.NewItems)
                {
                    //MessageBox.Show(newCategory.Group.Name);
                    Group group = GetCategoryGroup(newCategory);
                    if (group == null)
                    {
                        throw new ArgumentException("Could not match group to category " + newCategory.Name);
                    }
                    _budgetValues.Add(new MoneyGridRow(group, newCategory));
                    _spendingValues.Add(new MoneyGridRow(group, newCategory));
                    _comparisonValues.Add(new MoneyGridRow(group, newCategory));

                    //Debug.WriteLine(_spendingValues.Count);
                    //Debug.WriteLine("Current group " + group.Name);
                    //Debug.WriteLine("Could not match group to category " + newCategory.Name + ", " + group.Name);
                    //throw new ArgumentException("Could not match group to category " + newCategory.Name, ex);
                }
            }
            //Removes Values rows. Order is important to avoid triggering data that doesn't exist. (1/4/2017: May be fixed now)
            if (e.OldItems != null && e.Action != NotifyCollectionChangedAction.Move)
            {
                foreach (Category oldCategory in e.OldItems)
                {
                    MoneyGridRow oldRow = _comparisonValues.Where(row => row.Category == oldCategory).ElementAt(0);
                    if (oldRow == null)
                        throw new ArgumentException("Cannot locate deleted row");
                    _comparisonValues.Remove(oldRow);

                    oldRow = _spendingValues.Where(row => row.Category == oldCategory).ElementAt(0);
                    if (oldRow == null)
                        throw new ArgumentException("Cannot locate deleted row");
                    _spendingValues.Remove(oldRow);

                    oldRow = _budgetValues.Where(row => row.Category == oldCategory).ElementAt(0);
                    if (oldRow == null)
                        throw new ArgumentException("Cannot locate deleted row");
                    _budgetValues.Remove(oldRow);
                }
            }
        }

        /// <summary>
        /// Called when the group collection is changed. Used to add/remove Totals rows.
        /// </summary>
        /// <param name="sender">The modified collection</param>
        /// <param name="e">The arguments</param>
        public void GroupsCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            //Adds Totals rows
            if (e.NewItems != null && e.Action != NotifyCollectionChangedAction.Move)
            {
                foreach (Group newGroup in e.NewItems)
                {
                    Category newCategory = new Category(newGroup.Name);
                    try
                    {
                        Group totalGroup;
                        //Gets the income state
                        if (newGroup.IsIncome)
                        {
                            totalGroup = columnIncomeTotalsGroup;
                        }
                        else
                        {
                            totalGroup = columnExpendituresTotalsGroup;
                        }
                        _budgetTotals.Add(new MoneyGridRow(totalGroup, newCategory));
                        _spendingTotals.Add(new MoneyGridRow(totalGroup, newCategory));
                        _comparisonTotals.Add(new MoneyGridRow(totalGroup, newCategory));
                    }
                    catch (ArgumentException ex)
                    {
                        Debug.WriteLine("Could not match group to category " + newCategory.Name);
                        throw new ArgumentException("Could not match group to category " + newGroup.Name, ex);
                    }
                }
            }

            //Removes totals rows
            if (e.OldItems != null && e.Action != NotifyCollectionChangedAction.Move)
            {
                foreach (Group oldGroup in e.OldItems)
                {
                    //Debug.WriteLine("Group to be deleted: " + oldGroup.Name);
                    foreach (MoneyGridRow row in _budgetValues)
                    {
                        //Debug.WriteLine("Budget row: " + row.Category);
                    }
                    MoneyGridRow oldRow = _budgetTotals.Where(row => row.Category.Name.Equals(oldGroup.Name)).ElementAt(0);
                    if (oldRow == null)
                        throw new ArgumentException("Cannot locate deleted row");
                    _budgetTotals.Remove(oldRow);

                    oldRow = _spendingTotals.Where(row => row.Category.Name.Equals(oldGroup.Name)).ElementAt(0);
                    if (oldRow == null)
                        throw new ArgumentException("Cannot locate deleted row");
                    _spendingTotals.Remove(oldRow);

                    oldRow = _comparisonTotals.Where(row => row.Category.Name.Equals(oldGroup.Name)).ElementAt(0);
                    if (oldRow == null)
                        throw new ArgumentException("Cannot locate deleted row");
                    _comparisonTotals.Remove(oldRow);
                }
            }
        }

        #endregion

        #region Group and Category window
        /// <summary>
        /// Command for adding a new group
        /// </summary>
        public RelayCommand<Group> AddGroupCommand
        {
            get; set;
        }

        /// <summary>
        /// Command for removing an existing group
        /// </summary>
        public RelayCommand<Group> RemoveGroupCommand
        {
            get; set;
        }

        /// <summary>
        /// Command for adding a new category
        /// </summary>
        public RelayCommand<Group> AddCategoryCommand
        {
            get; set;
        }

        /// <summary>
        /// Command for removing an existing category
        /// </summary>
        public RelayCommand<Category> RemoveCategoryCommand
        {
            get; set;
        }

        /// <summary>
        /// Command for moving a group up (0-index)
        /// </summary>
        public RelayCommand<Group> MoveGroupUpCommand
        {
            get; set;
        }

        /// <summary>
        /// Command for moving a group down (N-index)
        /// </summary>
        public RelayCommand<Group> MoveGroupDownCommand
        {
            get; set;
        }

        /// <summary>
        /// Command for moving a category up (0-index)
        /// </summary>
        public RelayCommand<Category> MoveCategoryUpCommand
        {
            get; set;
        }

        /// <summary>
        /// Command for moving a category down (N-index)
        /// </summary>
        public RelayCommand<Category> MoveCategoryDownCommand
        {
            get; set;
        }
        #endregion

        #region Methods to modify payment method collection

        /// <summary>
        /// Adds a new payment method to the collection
        /// </summary>
        /// <param name="paymentMethod">The payment method to add</param>
        public void AddPaymentMethod(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
                return;
            PaymentMethod checkMethod = _paymentMethods.First(x => x.Name.Equals(paymentMethod.Name));
            if (checkMethod != null)
            {
                MessageBox.Show("Payment method of the same name already exists; please choose another.");
            }
            else
                _paymentMethods.Add(paymentMethod);
        }

        /// <summary>
        /// Removes an existing payment method from the collection
        /// </summary>
        /// <param name="paymentMethod">The payment method to remove</param>
        public void RemovePaymentMethod(PaymentMethod paymentMethod)
        {
            try
            {
                _paymentMethods.Remove(paymentMethod);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not find specified payment method " + paymentMethod.Name, ex);
            }
        }
        #endregion

        #region Payment Methods window
        /// <summary>
        /// Command to add a new payment method
        /// </summary>
        public RelayCommand<PaymentMethod> AddPaymentMethodCommand
        {
            get; set;
        }

        /// <summary>
        /// Command to remove an existing payment method
        /// </summary>
        public RelayCommand<PaymentMethod> RemovePaymentMethodCommand
        {
            get; set;
        }
        #endregion

        #region Budget tab

        /// <summary>
        /// Updates the values in the budget tab's Totals grid
        /// </summary>
        /// <param name="sender">The Values grid row property that was modified</param>
        /// <param name="e">The arguments</param>
        public void UpdateBudgetTotals(Object sender, PropertyChangedEventArgs e)
        {
            //Debug.WriteLine("Updating budget totals");
            if (e.PropertyName.Equals("Values"))
            {
                CalculateColumnTotals(_budgetValues, _budgetTotals, "BudgetTotals");
                UpdateComparisonValues();
                UpdateMonthDetails();
            }
        }

        /// <summary>
        /// This function is used to force a refresh of the budget totals
        /// </summary>
        private void RefreshBudgetTotals()
        {
            CalculateColumnTotals(_budgetValues, _budgetTotals, "BudgetTotals");
            UpdateComparisonValues();
            UpdateMonthDetails();
        }

        #endregion

        #region Spending tab

        //Possible update: add oldValue to transaction, find transaction being modified, and update the appropriate fields
        /// <summary>
        /// Updates the values in the Spending tab when a transaction is modified. Resets the values and recalculates from the transactions.
        /// Note: not very efficient and should be updated.
        /// </summary>
        /// <param name="sender">The changed Transaction property</param>
        /// <param name="e">The arguments</param>
        public void UpdateSpendingValues(Object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("Transaction modified!");
            //Only category, date, and amount will change the spending data
            if (!e.PropertyName.Equals("Category") && !e.PropertyName.Equals("Amount") && !e.PropertyName.Equals("Date"))
                return;
            //Reset values
            foreach (MoneyGridRow row in _spendingValues)
            {
                row.Values.Values = new decimal[12];
            }
            //Loop through each transaction and add the values
            foreach (Transaction transaction in _transactions)
            {
                if (transaction.Category != null)
                {
                    MoneyGridRow row;
                    try
                    {
                        row = _spendingValues.Single(x => x.Category == transaction.Category);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("Cannot find category for transaction category " + transaction.Category, ex);
                    }
                    //TODO: check year
                    int month = transaction.Date.Month - 1;
                    row.Values[month] += transaction.Amount;
                }
            }
            //Debug.WriteLine("Spending Values Updated");
            UpdateSpendingTotals(); //Called here so that it is only updated once all the values are recalculated
        }

        /// <summary>
        /// Updates the values in the Spending tab when a transaction is added or removed. Adds or subtracts the appropriate amount.
        /// </summary>
        /// <param name="sender">The collection that was changed</param>
        /// <param name="e">The arguments</param>
        public void AddOrRemoveSpendingValues(Object sender, NotifyCollectionChangedEventArgs e)
        {
            //Add a new value
            if (e.NewItems != null && e.Action != NotifyCollectionChangedAction.Move)
            {
                foreach (Transaction transaction in e.NewItems)
                {
                    if (transaction.Category != null)
                    {
                        MoneyGridRow row;
                        try
                        {
                            row = _spendingValues.Single(x => x.Category == transaction.Category);
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException("Cannot find category for transaction category " + transaction.Category, ex);
                        }
                        //TODO: check year
                        int month = transaction.Date.Month - 1;
                        row.Values[month] += transaction.Amount;
                    }
                }
                UpdateSpendingTotals();
            }
            //Remove an old value
            if (e.OldItems != null && e.Action != NotifyCollectionChangedAction.Move)
            {
                foreach (Transaction transaction in e.OldItems)
                {
                    MoneyGridRow row;
                    try
                    {
                        row = _spendingValues.Single(x => x.Category == transaction.Category);
                    }
                    catch (Exception ex)    //Category doesn't exist, so no need to remove amount from total
                    {
                        return;
                        throw new ArgumentException("Cannot find category for transaction category " + transaction.Category, ex);
                    }
                    //TODO: check year
                    int month = transaction.Date.Month - 1;
                    row.Values[month] -= transaction.Amount;
                }
                UpdateSpendingTotals();
            }
            //Recalculate all values due to reset
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (Transaction transaction in _transactions)
                {
                    if (transaction.Category != null)
                    {
                        MoneyGridRow row;
                        try
                        {
                            row = _spendingValues.Single(x => x.Category == transaction.Category);
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException("Cannot find category for transaction category " + transaction.Category, ex);
                        }
                        //TODO: check year
                        int month = transaction.Date.Month - 1;
                        row.Values[month] += transaction.Amount;
                    }
                }
                UpdateSpendingTotals();
            }
        }

        /// <summary>
        /// Updates the values in the spending tab's Totals grid
        /// </summary>
        public void UpdateSpendingTotals()
        {
            //Debug.WriteLine("Updating spending totals");
            CalculateColumnTotals(_spendingValues, _spendingTotals, "SpendingTotals");
            //Debug.WriteLine("Spending Total Updated");
            UpdateComparisonValues();
            UpdateMonthDetails();
        }
        #endregion

        #region Comparison tab

        /// <summary>
        /// Updates the values in the Comparison tab's Values grid when the spending or budget totals have been updated.
        /// Iterates through each row and gets the difference.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateComparisonValues()
        {
            //Debug.WriteLine("Number of categories: " + _categories.Count + "Number of rows: " + _budgetValues.Count + " " + _spendingValues.Count + " " + _comparisonValues.Count);
            for (int i = 0; i < _comparisonValues.Count; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    //Checks if value needs to be updated - reduces redraw time
                    //Sign adjusted for expenditure categories
                    if (_comparisonValues.ElementAt(i).Group.IsIncome)
                    {
                        if (_comparisonValues.ElementAt(i).Values[j] == _spendingValues.ElementAt(i).Values[j] - _budgetValues.ElementAt(i).Values[j])
                            continue;
                        _comparisonValues.ElementAt(i).Values[j] = _spendingValues.ElementAt(i).Values[j] - _budgetValues.ElementAt(i).Values[j];
                    }
                    else
                    {
                        if (_comparisonValues.ElementAt(i).Values[j] == _budgetValues.ElementAt(i).Values[j] - _spendingValues.ElementAt(i).Values[j])
                            continue;
                        _comparisonValues.ElementAt(i).Values[j] = _budgetValues.ElementAt(i).Values[j] - _spendingValues.ElementAt(i).Values[j];
                    }

                    if (_spendingValues.ElementAt(i).Values[j] > 0)
                    {
                        //MessageBox.Show(i + " " + j);
                    }
                }
            }
            //Update the Totals grid if it has all the groups
            if (_comparisonTotals.Count == _groups.Count)
                CalculateColumnTotals(_comparisonValues, _comparisonTotals, "Comparison Totals");
            //_comparisonValues.MemberPropertyChanged(null, null);
        }

        #endregion

        #region Month Details tab
        private void UpdateMonthDetails()
        {
            _monthDetails.Clear();
            int numRows = _budgetValues.Count;
            List<MonthDetailRow> monthDetails = new List<MonthDetailRow>();
            for (int i=0; i < numRows; i++)
            {
                Group currentGroup = _budgetValues.ElementAt(i).Group;
                Category currentCategory = _budgetValues.ElementAt(i).Category;
                MonthDetailRow currentDetail = new MonthDetailRow(currentGroup, currentCategory, _selectedMonth, Int32.Parse(_currentYear));
                currentDetail.BudgetedAmount = _budgetValues.ElementAt(i).Values[_selectedMonth];
                currentDetail.SpentAmount = _spendingValues.ElementAt(i).Values[_selectedMonth];
                monthDetails.Add(currentDetail);
            }
            numRows = _budgetTotals.Count;
            for (int i = 0; i < numRows; i++)
            {
                Group currentGroup = _budgetTotals.ElementAt(i).Group;
                Category currentCategory = _budgetTotals.ElementAt(i).Category;
                MonthDetailRow currentDetail = new MonthDetailRow(currentGroup, currentCategory, _selectedMonth, Int32.Parse(_currentYear));
                currentDetail.BudgetedAmount = _budgetTotals.ElementAt(i).Values[_selectedMonth];
                currentDetail.SpentAmount = _spendingTotals.ElementAt(i).Values[_selectedMonth];
                monthDetails.Add(currentDetail);
            }

            numRows = _budgetBudgetAndSum.Count;
            for (int i = 0; i < numRows; i++)
            {
                Group currentGroup = _budgetBudgetAndSum.ElementAt(i).Group;
                Category currentCategory = _budgetBudgetAndSum.ElementAt(i).Category;
                MonthDetailRow currentDetail = new MonthDetailRow(currentGroup, currentCategory, _selectedMonth, Int32.Parse(_currentYear));
                currentDetail.BudgetedAmount = _budgetBudgetAndSum.ElementAt(i).Values[_selectedMonth];
                currentDetail.SpentAmount = _spendingBudgetAndSum.ElementAt(i).Values[_selectedMonth];
                monthDetails.Add(currentDetail);
            }
            _monthDetails.InsertRange(monthDetails);
            //Debug.WriteLine("Days so far: " + DateTime.Now.Day + "; total in month: " + DateTime.DaysInMonth(Int32.Parse(_currentYear), _selectedMonth + 1));
        }
        #endregion

        #region Transaction tab

        /// <summary>
        /// Adds a new transaction to the collection
        /// </summary>
        /// <param name="transaction">The transaction to add</param>
        public void AddTransaction(Transaction transaction)
        {
            //TODO: verify valid transaction
            _transactions.Add(transaction);
        }

        /// <summary>
        /// Event for the transactions collection being modified
        /// </summary>
        public event NotifyCollectionChangedEventHandler TransactionsChangedEvent;

        /// <summary>
        /// Raises the TransactionsChangedEvent event
        /// </summary>
        /// <param name="sender">The changed collection</param>
        /// <param name="e">The arguments</param>
        private void OnTransactionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (TransactionsChangedEvent != null)
            {
                TransactionsChangedEvent(sender, e);
                //Debug.WriteLine("Transaction collection changed");
            }
        }

        /// <summary>
        /// Event for a transaction property being modified
        /// </summary>
        public event PropertyChangedEventHandler TransactionModifiedEvent;

        /// <summary>
        /// Raises the TransactionsModifiedEvent event
        /// </summary>
        /// <param name="sender">The changed Transaction property</param>
        /// <param name="e">The arguments</param>
        private void OnTransactionModified(object sender, PropertyChangedEventArgs e)
        {
            if (TransactionModifiedEvent != null)
                TransactionModifiedEvent(sender, e);
        }
        #endregion

        #region Saving and Opening files

        /// <summary>
        /// Command to save the current data.
        /// </summary>
        public RelayCommand SaveDataCommand
        {
            get; set;
        }

        /// <summary>
        /// Command to load data from the current filepath.
        /// </summary>
        public RelayCommand LoadDataCommand
        {
            get; set;
        }

        /// <summary>
        /// Command to add a new year of data.
        /// </summary>
        public RelayCommand<String> AddYearCommand
        {
            get; set;
        }

        public RelayCommand<String> CopyDataCommand
        {
            get; set;
        }

        /// <summary>
        /// Search for all data files in the save directory.
        /// </summary>
        private void GetYears()
        {
            string[] files;
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(fileName + @"_[0-9]{4}\.xml$"); //Regular expression for matching <fileName>_<year>.xml

            files = Directory.GetFiles("./", "*.xml");
            foreach (string file in files)
            {
                if (reg.IsMatch(file))
                {
                    System.Text.RegularExpressions.Match match = reg.Match(file);
                    string year = match.Captures[0].Value;
                    int index = year.IndexOf("_") + 1;
                    int length = year.IndexOf(".") - index;
                    year = year.Substring(index, length);
                    //Debug.WriteLine("Found a file for year: " + year);
                    _yearList.Add(year);
                }
            }
        }

        /// <summary>
        /// Adds a new data file for the given year, if it is a valid year.
        /// </summary>
        /// <param name="year"></param>
        private void AddYear(String year)
        {
            Debug.WriteLine("Attempting to add year: " + year);
            //Check if year already exists
            foreach (String checkYear in _yearList)
            {
                if (checkYear.Equals(year))
                {
                    _validYear = false;
                    return;
                }
            }
            //Check if the year is a valid year (evaluates to an integer, has 4 digits)
            int yearInt = 0;
            bool test = int.TryParse(year, out yearInt);
            if (!test || year.Length != 4)
            {
                _validYear = false;
                return;
            }

            _yearList.Add(year);
            _currentYear = year;
            completeFilePath = filePath + fileName + "_" + _currentYear + ".xml";
            InitNewFile();

            _validYear = true;
        }

        /// <summary>
        /// Creates a new data file with no data in the current filepath
        /// </summary>
        private void InitNewFile()
        {
            Debug.WriteLine("Creating new file at " + completeFilePath);
            using (FileStream file = new FileStream(completeFilePath, FileMode.Create))
            {
                using (StreamWriter stream = new StreamWriter(file))
                {
                    //Create the DataWrapper object and add the apprpriate data
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                    DataWrapper data = new DataWrapper();
                    data.Groups = new MyObservableCollection<Group>();
                    data.PaymentMethods = new MyObservableCollection<PaymentMethod>();
                    data.Transactions = new MyObservableCollection<Transaction>();
                    List<decimal[]> budgetData = new List<decimal[]>(); //Easiest to just store values in order
                    foreach (MoneyGridRow row in _budgetValues)
                    {
                        budgetData.Add(new decimal[12]);
                    }
                    data.BudgetValues = budgetData;

                    dataSerializer.Serialize(stream, data); //Saves the data using the attributes defined in each class
                }
            }
        }

        /// <summary>
        /// Saves the current data in the current filepath
        /// </summary>
        public void SaveData()
        {
            using (FileStream file = new FileStream(completeFilePath, FileMode.Create))
            {
                using (StreamWriter stream = new StreamWriter(file))
                {
                    //Create the DataWrapper object and add the apprpriate data
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                    DataWrapper data = new DataWrapper();
                    data.Groups = _groups;
                    data.PaymentMethods = _paymentMethods;
                    data.Transactions = _transactions;
                    List<decimal[]> budgetData = new List<decimal[]>(); //Easiest to just store values in order
                    foreach (MoneyGridRow row in _budgetValues)
                    {
                        budgetData.Add(row.Values.Values);
                    }
                    data.BudgetValues = budgetData;

                    dataSerializer.Serialize(stream, data); //Saves the data using the attributes defined in each class
                }
            }
        }

        /// <summary>
        /// Loads data from the current filepath
        /// </summary>
        public void LoadData()
        {
            //Debug.WriteLine("Loading data");
            //Clears all existing data
            _groups.Clear();
            _categories.Clear();
            _transactions.Clear();
            _paymentMethods.Clear();
            _budgetValues.Clear();
            _budgetTotals.Clear();
            _spendingValues.Clear();
            _spendingTotals.Clear();
            _comparisonValues.Clear();
            _comparisonTotals.Clear();

            //Retrieves the data using the serialize attributes
            DataWrapper data = new DataWrapper();
            try
            {
                using (FileStream file = new FileStream(completeFilePath, FileMode.Open))
                {
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                    data = (DataWrapper)dataSerializer.Deserialize(file);
                }
            }
            catch (IOException ex) //File does not exist; set everything to defaults
            {
                InitNewFile();
            }
            //Process the data
            int index = 0;
            //Add groups, categories, and budget values
            List<Group> tempGroups = new List<Group>();
            List<Category> tempCategories = new List<Category>();
            Stopwatch runTimer;
            runTimer = Stopwatch.StartNew();
            _budgetValues.MemberChanged -= UpdateBudgetTotals;
            //Debug.WriteLine("Placeholder");
            foreach (Group group in data.Groups)
            {
                Group newGroup = new Group(group.IsIncome, group.Name);
                _groups.Add(newGroup);
                foreach (Category category in group.Categories)
                {
                    newGroup.Categories.Add(category);
                    _categories.Add(category);
                    MoneyGridRow row = _budgetValues.Single(x => x.Category == category);
                    row.Values.Values = data.BudgetValues.ElementAt(index);
                    index++;
                }
            }
            _budgetValues.MemberChanged += UpdateBudgetTotals;
            RefreshBudgetTotals();
            runTimer.Stop();
            //Debug.WriteLine("Reading groups and categories: " + runTimer.ElapsedTicks);
            runTimer = Stopwatch.StartNew();
            //Add payment methods
            foreach (PaymentMethod payment in data.PaymentMethods)
            {
                _paymentMethods.Add(payment);
            }
            runTimer.Stop();
            //Debug.WriteLine("Reading payment methods: " + runTimer.ElapsedTicks);
            runTimer = Stopwatch.StartNew();
            //Adds the transactions
            //Transactions are stored with different instances of the category and payment method objects. These need to 
            //be matched to the data loaded above. Matching is done using the name of each.
            List<Transaction> tempTransactions = new List<Transaction>();
            foreach (Transaction transaction in data.Transactions)
            {
                String categoryName = transaction.Category.Name;
                //Debug.WriteLine(transaction.Item + " " + transaction.PaymentMethod.Name);
                String paymentName = transaction.PaymentMethod.Name;
                try
                {
                    transaction.Category = _categories.Single(x => x.Name.Equals(categoryName));
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Cannot find matching category " + transaction.Category.Name + " in categories list");
                }
                try
                {
                    transaction.PaymentMethod = _paymentMethods.Single(x => x.Name.Equals(paymentName));
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Cannot find matching payment method " + transaction.PaymentMethod.Name + " in payment methods list");
                }
                tempTransactions.Add(transaction);
            }
            _transactions.InsertRange(tempTransactions);
            runTimer.Stop();
            //Debug.WriteLine("Reading transactions: " + runTimer.ElapsedTicks);
            //Debug.WriteLine(_budgetValues.Count);
        }

        /// <summary>
        /// Copies groups, categories, and payment methods from an existing configuration
        /// </summary>
        /// <param name="yearSource">The year to copy from</param>
        private void CopyData(String yearSource)
        {
            //Clears all existing data
            _groups.Clear();
            _categories.Clear();
            _transactions.Clear();
            _paymentMethods.Clear();
            _budgetValues.Clear();
            _budgetTotals.Clear();
            _spendingValues.Clear();
            _spendingTotals.Clear();
            _comparisonValues.Clear();
            _comparisonTotals.Clear();

            //Retrieves the data using the serialize attributes
            DataWrapper data = new DataWrapper();
            String tempCompleteFilePath = filePath + fileName + "_" + yearSource + ".xml";
            try
            {
                using (FileStream file = new FileStream(tempCompleteFilePath, FileMode.Open))
                {
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                    data = (DataWrapper)dataSerializer.Deserialize(file);
                }
            }
            catch (IOException ex) //File does not exist; set everything to defaults
            {
                InitNewFile();
            }

            //Process the data
            int index = 0;
            //Add groups, categories, and budget values
            foreach (Group group in data.Groups)
            {
                Group newGroup = new Group(group.IsIncome, group.Name);
                _groups.Add(newGroup);
                foreach (Category category in group.Categories)
                {
                    newGroup.Categories.Add(category);
                    _categories.Add(category);
                    MoneyGridRow row = _budgetValues.Single(x => x.Category == category);
                    row.Values.Values = new decimal[12];
                    index++;
                }
            }
            //Add payment methods
            foreach (PaymentMethod payment in data.PaymentMethods)
            {
                _paymentMethods.Add(payment);
            }
        }
        #endregion
    }
}
