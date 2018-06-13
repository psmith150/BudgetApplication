using BudgetApplication.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BudgetApplication.Base.Interfaces;
using BudgetApplication.Base.AbstractClasses;
using BudgetApplication.Base.EventArgs;

namespace BudgetApplication.Services
{
    public class SessionService : ObservableObject
    {
        #region Constructor
        public SessionService(IErrorHandler errorhandler, MessageViewerBase messageViewer)
        {
            //Sets event handlers to make sure all data is updated
            this.Categories.CollectionChanged += CategoryCollectionChanged;  //Used to add/remove rows
            this.Groups.CollectionChanged += GroupsCollectionChanged;  //Used to add/remove rows
            this.Groups.MemberChanged += GroupChanged;  //Used to update grouping
            this.BudgetValues.MemberChanged += UpdateBudgetTotals;  //Update respective totals
            this.Transactions.MemberChanged += UpdateSpendingValues;    //Update spending if transaction has been modified
            //TODO this.Transactions.MemberChanged += OnTransactionModified;   //Trigger event for view to handle
            this.Transactions.CollectionChanged += AddOrRemoveSpendingValues;   //Update spending if transaction has been added or removed
            //TODO this.Transactions.CollectionChanged += OnTransactionsChanged;   //Trigger event for view to handle
            //this.SpendingTotals.MemberChanged += ((o, a) => { Debug.WriteLine("Spending totals auto update"); UpdateComparisonValues(); });    //Update comparison values if a spending values was changed. Totals used to allow bulk modification.
            //this.BudgetTotals.MemberChanged += ((o, a) => { Debug.WriteLine("Budget totals auto update"); UpdateComparisonValues(); });  //Update comparison values if a budget value was changed. Totals used to allow bulk modification.

            this._errorhandler = errorhandler;
            this._messageViewer = messageViewer;
        }
        #endregion

        #region Private Fields
        IErrorHandler _errorhandler;
        MessageViewerBase _messageViewer;
        //Groups for income and expenditures in the Totals grids.
        private Group columnIncomeTotalsGroup = new Group(true, "Income Totals");
        private Group columnExpendituresTotalsGroup = new Group(false, "Expenditure totals");
        #endregion

        #region Public Properties
        private bool _IsBusy;
        public bool IsBusy
        {
            get
            {
                return this._IsBusy;
            }
            set
            {
                this.Set(ref this._IsBusy, value);
            }
        }

        private string _BusyMessage;
        public string BusyMessage
        {
            get
            {
                return this._BusyMessage;
            }
            set
            {
                this.Set(ref this._BusyMessage, value);
            }
        }
        private int _currentYear = DateTime.Now.Year;
        public int CurrentYear
        {
            get
            {
                return this._currentYear;
            }
            set
            {
                this.Set(ref this._currentYear, value);
            }
        }
        #region Budget Data
        private MyObservableCollection<Group> _groups = new MyObservableCollection<Group>();
        /// <summary>
        /// The collection of groups
        /// </summary>
        public MyObservableCollection<Group> Groups
        {
            get
            {
                return _groups;
            }
            private set
            {
                _groups = value;
            }
        }

        private MyObservableCollection<Category> _categories = new MyObservableCollection<Category>();
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

        private MyObservableCollection<Transaction> _transactions = new MyObservableCollection<Transaction>();
        /// <summary>
        /// The collection of transactions
        /// </summary>
        public MyObservableCollection<Transaction> Transactions
        {
            get
            {
                return _transactions;
            }
            private set
            {
                _transactions = value;
            }
        }

        private MyObservableCollection<PaymentMethod> _paymentMethods = new MyObservableCollection<PaymentMethod>();
        /// <summary>
        /// The collection of payment methods
        /// </summary>
        public MyObservableCollection<PaymentMethod> PaymentMethods
        {
            get
            {
                return _paymentMethods;
            }
            private set
            {
                _paymentMethods = value;
            }
        }

        private MyObservableCollection<MoneyGridRow> _budgetValues = new MyObservableCollection<MoneyGridRow>();
        /// <summary>
        /// The collection of values in the budget tab's Totals grid
        /// </summary>
        public MyObservableCollection<MoneyGridRow> BudgetValues
        {
            get
            {
                return _budgetValues;
            }
            private set
            {
                _budgetValues = value;
            }
        }

        private MyObservableCollection<MoneyGridRow> _budgetTotals = new MyObservableCollection<MoneyGridRow>();
        /// <summary>
        /// The collection of values in the budget tab's Totals grid
        /// </summary>
        public MyObservableCollection<MoneyGridRow> BudgetTotals
        {
            get
            {
                return _budgetTotals;
            }
            private set
            {
                _budgetTotals = value;
            }
        }

        private MyObservableCollection<MoneyGridRow> _spendingValues = new MyObservableCollection<MoneyGridRow>();
        /// <summary>
        /// The collection of values in the budget tab's Totals grid
        /// </summary>
        public MyObservableCollection<MoneyGridRow> SpendingValues
        {
            get
            {
                return _spendingValues;
            }
            private set
            {
                _spendingValues = value;
            }
        }

        private MyObservableCollection<MoneyGridRow> _spendingTotals = new MyObservableCollection<MoneyGridRow>();
        /// <summary>
        /// The collection of values in the budget tab's Totals grid
        /// </summary>
        public MyObservableCollection<MoneyGridRow> SpendingTotals
        {
            get
            {
                return _spendingTotals;
            }
            private set
            {
                _spendingTotals = value;
            }
        }

        private MyObservableCollection<MoneyGridRow> _comparisonValues = new MyObservableCollection<MoneyGridRow>();
        /// <summary>
        /// The collection of values in the budget tab's Totals grid
        /// </summary>
        public MyObservableCollection<MoneyGridRow> ComparisonValues
        {
            get
            {
                return _comparisonValues;
            }
            private set
            {
                _comparisonValues = value;
            }
        }

        private MyObservableCollection<MoneyGridRow> _comparisonTotals = new MyObservableCollection<MoneyGridRow>();
        /// <summary>
        /// The collection of values in the budget tab's Totals grid
        /// </summary>
        public MyObservableCollection<MoneyGridRow> ComparisonTotals
        {
            get
            {
                return _comparisonTotals;
            }
            private set
            {
                _comparisonTotals = value;
            }
        }

        private MyObservableCollection<MonthDetailRow> _monthDetails = new MyObservableCollection<MonthDetailRow>();
        public MyObservableCollection<MonthDetailRow> MonthDetails
        {
            get
            {
                return this._monthDetails;
            }
            set
            {
                this.Set(ref this._monthDetails, value);
            }
        }

        #region Events
        public delegate void UpdateMonthDetailsEventHandler(object source, EventArgs e);
        public UpdateMonthDetailsEventHandler RequestMonthDetailsUpdate;

        public delegate void RefreshListViewsEventHandler(object source, EventArgs e);
        public RefreshListViewsEventHandler RequestListViewUpdate;
        #endregion
        #endregion
        #endregion

        #region Public Methods
        /// <summary>
        /// Moves a row of values on all tabs
        /// </summary>
        /// <param name="start">Start index</param>
        /// <param name="end">End index</param>
        public void MoveValueRows(int start, int end)
        {
            this.BudgetValues.Move(start, end);
            this.SpendingValues.Move(start, end);
            this.ComparisonValues.Move(start, end);
        }

        /// <summary>
        /// Moves a row of totals on all tabs
        /// </summary>
        /// <param name="start">Start index</param>
        /// <param name="end">End index</param>
        public void MoveTotalRows(int start, int end)
        {
            this.BudgetTotals.Move(start, end);
            this.SpendingTotals.Move(start, end);
            this.ComparisonTotals.Move(start, end);
        }

        /// <summary>
        /// Refresh the ListCollectionViews. Used to update grouping.
        /// </summary>
        public void RefreshListViews()
        {
            if (this.RequestListViewUpdate != null)
            {
                this.RequestListViewUpdate(this, null);
            }
        }
        #region File Handling
        public async Task LoadDataFromFile(string filePath)
        {
            //Clears all existing data
            this.Groups.Clear();
            this.Categories.Clear();
            this.Transactions.Clear();
            this.PaymentMethods.Clear();
            this.BudgetValues.Clear();
            this.BudgetTotals.Clear();
            this.SpendingValues.Clear();
            this.SpendingTotals.Clear();
            this.ComparisonValues.Clear();
            this.ComparisonTotals.Clear();

            Group testGroup = new Model.Group();
            Category testCategory = new Category();
            MoneyGridRow testRow = new MoneyGridRow(testGroup, testCategory);
            //this.Groups.Add(testGroup);
            //this.Categories.Add(testCategory);
            //this.BudgetValues.Add(testRow);
            //return;

            //Retrieves the data using the serialize attributes
            DataWrapper data = new DataWrapper();
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                    data = (DataWrapper)dataSerializer.Deserialize(file);
                }
            }
            catch (IOException ex) //File does not exist; set everything to defaults
            {
                await CreateNewFile(Properties.Settings.Default.DefaultDirectory + "\\data_new.xml");
            }
            catch (InvalidOperationException ex)
            {
                this._errorhandler.DisplayError($"Error reading XML from file {filePath}\n{ex.Message}").Wait();
            }
            catch (System.Xml.XmlException ex)
            {
                this._errorhandler.DisplayError($"Error in XML file\n{ex.Message}").Wait();
            }
            //Process the data
            this.CurrentYear = data.Year;
            int index = 0;
            //Add groups, categories, and budget values
            List<Group> tempGroups = new List<Group>();
            List<Category> tempCategories = new List<Category>();
            this.BudgetValues.MemberChanged -= UpdateBudgetTotals;
            foreach (Group group in data.Groups)
            {
                Group newGroup = new Group(group.IsIncome, group.Name);
                this.Groups.Add(newGroup);
                foreach (Category category in group.Categories)
                {
                    newGroup.Categories.Add(category);
                    this.Categories.Add(category);
                    MoneyGridRow row = this.BudgetValues.Single(x => x.Category == category);
                    row.Values.Values = data.BudgetValues.ElementAt(index);
                    index++;
                }
            }
            this.BudgetValues.MemberChanged += UpdateBudgetTotals;
            RefreshBudgetTotals();
            //Add payment methods
            foreach (PaymentMethod payment in data.PaymentMethods)
            {
                this.PaymentMethods.Add(payment);
            }
            //Adds the transactions
            //Transactions are stored with different instances of the category and payment method objects. These need to 
            //be matched to the data loaded above. Matching is done using the name of each.
            List<Transaction> tempTransactions = new List<Transaction>();
            foreach (Transaction transaction in data.Transactions)
            {
                string categoryName = transaction.Category.Name;
                string paymentName = transaction.PaymentMethod.Name;
                try
                {
                    transaction.Category = this.Categories.Single(x => x.Name.Equals(categoryName));
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Cannot find matching category " + transaction.Category.Name + " in categories list");
                }
                try
                {
                    transaction.PaymentMethod = this.PaymentMethods.Single(x => x.Name.Equals(paymentName));
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Cannot find matching payment method " + transaction.PaymentMethod.Name + " in payment methods list");
                }
                tempTransactions.Add(transaction);
            }
            this.Transactions.InsertRange(tempTransactions);
        }

        public async Task SaveDataToFile(string filePath)
        {
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open))
                {
                    using (StreamWriter stream = new StreamWriter(file))
                    {
                        //Create the DataWrapper object and add the apprpriate data
                        XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                        DataWrapper data = new DataWrapper();
                        data.Year = this.CurrentYear;
                        data.Groups = this.Groups;
                        data.PaymentMethods = this.PaymentMethods;
                        data.Transactions = this.Transactions;
                        List<decimal[]> budgetData = new List<decimal[]>(); //Easiest to just store values in order
                        foreach (MoneyGridRow row in this.BudgetValues)
                        {
                            budgetData.Add(row.Values.Values);
                        }
                        data.BudgetValues = budgetData;

                        await Task.Run(() => dataSerializer.Serialize(stream, data)); //Saves the data using the attributes defined in each class
                    }
                }
            }
            catch (IOException ex)
            {
                this._errorhandler.DisplayError($"Error saving to file {filePath}\n{ex.Message}").Wait();
            }
            catch (InvalidOperationException ex)
            {
                this._errorhandler.DisplayError($"Error writing XML to file {filePath}\n{ex.Message}").Wait();
            }
        }

        /// <summary>
        /// Creates a new data file with no data in the default directory
        /// </summary>
        public async Task CreateNewFile(string filePath)
        {
            using (FileStream file = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter stream = new StreamWriter(file))
                {
                    //Create the DataWrapper object and add the apprpriate data
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                    DataWrapper data = new DataWrapper();
                    data.Year = DateTime.Now.Year;
                    data.Groups = new MyObservableCollection<Group>();
                    data.PaymentMethods = new MyObservableCollection<PaymentMethod>();
                    data.Transactions = new MyObservableCollection<Transaction>();
                    List<decimal[]> budgetData = new List<decimal[]>(); //Easiest to just store values in order
                    foreach (MoneyGridRow row in this.BudgetValues)
                    {
                        budgetData.Add(new decimal[12]);
                    }
                    data.BudgetValues = budgetData;

                    await Task.Run(() => dataSerializer.Serialize(stream, data)); //Saves the data using the attributes defined in each class
                }
            }
        }
        #endregion
        #endregion

        #region Private Methods
        private void CategoryChanged(Object sender, PropertyChangedEventArgs e)
        {
            RefreshListViews();
        }

        /// <summary>
        /// Called when a group has its properties modified. Used to update the groupings.
        /// </summary>
        /// <param name="sender">The modified object</param>
        /// <param name="e">The arguments</param>
        private void GroupChanged(Object sender, PropertyChangedEventArgs e)
        {
            RefreshListViews();
        }

        /// <summary>
        /// Called when the category collection is changed. Used to add/remove Values rows.
        /// </summary>
        /// <param name="sender">The modified collection</param>
        /// <param name="e">The arguments</param>
        private void CategoryCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            //Adds Values rows
            if (e.NewItems != null && e.Action != NotifyCollectionChangedAction.Move)
            {
                foreach (Category newCategory in e.NewItems)
                {
                    Group group = GetCategoryGroup(newCategory);
                    if (group == null)
                    {
                        throw new ArgumentException("Could not match group to category " + newCategory.Name);
                    }
                    this.BudgetValues.Add(new MoneyGridRow(group, newCategory));
                    this.SpendingValues.Add(new MoneyGridRow(group, newCategory));
                    this.ComparisonValues.Add(new MoneyGridRow(group, newCategory));
                }
            }
            //Removes Values rows. Order is important to avoid triggering data that doesn't exist. (1/4/2017: May be fixed now)
            if (e.OldItems != null && e.Action != NotifyCollectionChangedAction.Move)
            {
                foreach (Category oldCategory in e.OldItems)
                {
                    MoneyGridRow oldRow = this.ComparisonValues.Where(row => row.Category == oldCategory).ElementAt(0);
                    if (oldRow == null)
                        throw new ArgumentException("Cannot locate deleted row");
                    this.ComparisonValues.Remove(oldRow);

                    oldRow = this.SpendingValues.Where(row => row.Category == oldCategory).ElementAt(0);
                    if (oldRow == null)
                        throw new ArgumentException("Cannot locate deleted row");
                    this.SpendingValues.Remove(oldRow);

                    oldRow = this.BudgetValues.Where(row => row.Category == oldCategory).ElementAt(0);
                    if (oldRow == null)
                        throw new ArgumentException("Cannot locate deleted row");
                    this.BudgetValues.Remove(oldRow);
                }
            }
        }

        /// <summary>
        /// Called when the group collection is changed. Used to add/remove Totals rows.
        /// </summary>
        /// <param name="sender">The modified collection</param>
        /// <param name="e">The arguments</param>
        private void GroupsCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
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
                        this.BudgetTotals.Add(new MoneyGridRow(totalGroup, newCategory));
                        this.SpendingTotals.Add(new MoneyGridRow(totalGroup, newCategory));
                        this.ComparisonTotals.Add(new MoneyGridRow(totalGroup, newCategory));
                    }
                    catch (ArgumentException ex)
                    {
                        this._errorhandler.DisplayError($"Could not match group to category {newCategory.Name}").Wait();
                        throw new ArgumentException("Could not match group to category " + newGroup.Name, ex);
                    }
                }
            }

            //Removes totals rows
            if (e.OldItems != null && e.Action != NotifyCollectionChangedAction.Move)
            {
                foreach (Group oldGroup in e.OldItems)
                {
                    MoneyGridRow oldRow = this.BudgetTotals.Where(row => row.Category.Name.Equals(oldGroup.Name)).ElementAt(0);
                    if (oldRow == null)
                        throw new ArgumentException("Cannot locate deleted row");
                    this.BudgetTotals.Remove(oldRow);

                    oldRow = this.SpendingTotals.Where(row => row.Category.Name.Equals(oldGroup.Name)).ElementAt(0);
                    if (oldRow == null)
                        throw new ArgumentException("Cannot locate deleted row");
                    this.SpendingTotals.Remove(oldRow);

                    oldRow = this.ComparisonTotals.Where(row => row.Category.Name.Equals(oldGroup.Name)).ElementAt(0);
                    if (oldRow == null)
                        throw new ArgumentException("Cannot locate deleted row");
                    this.ComparisonTotals.Remove(oldRow);
                }
            }
        }
        /// <summary>
        /// Finds the group corresponding to the specified category
        /// </summary>
        /// <param name="category">The category to locate</param>
        /// <returns>The mathching group</returns>
        private Group GetCategoryGroup(Category category)
        {
            foreach (Group group in this.Groups)
            {
                if (group.Categories.Contains(category))
                {
                    return group;
                }
            }
            return null;
        }
        private void CalculateColumnTotals(ObservableCollection<MoneyGridRow> columnValues, ObservableCollection<MoneyGridRow> columnTotals, String propertyName)
        {
            //Don't do anything if not all the rows have been loaded yet
            if (columnValues.Count < this.Categories.Count || columnTotals.Count < this.Groups.Count)
                return;
            double totalGridIncomeTotal = 0.0;
            double totalGridExpenditureTotal = 0.0;
            foreach (Group group in this.Groups) //For each group, find its total row and then sum all the category rows that are part of the group
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
                    this._errorhandler.DisplayError($"Could not find corresponding total row: {group.Name}").Wait();
                    throw new ArgumentException("Could not find corresponding total row: " + group.Name, ex);
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
                        this._errorhandler.DisplayError($"{propertyName} does not contain row for group {group} and category {category}").Wait();
                        continue;
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
                        this._errorhandler.DisplayError($"{propertyName} does not contain row for group {group} and category {category}").Wait();
                        continue;
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

        /// <summary>
        /// Updates the values in the budget tab's Totals grid
        /// </summary>
        /// <param name="sender">The Values grid row property that was modified</param>
        /// <param name="e">The arguments</param>
        private void UpdateBudgetTotals(Object sender, PropertyChangedEventArgs e)
        {
            //Debug.WriteLine("Updating budget totals");
            if (e.PropertyName.Equals("Values"))
            {
                CalculateColumnTotals(this.BudgetValues, this.BudgetTotals, "BudgetTotals");
                UpdateComparisonValues();
                UpdateMonthDetails();
            }
        }

        /// <summary>
        /// This function is used to force a refresh of the budget totals
        /// </summary>
        private void RefreshBudgetTotals()
        {
            CalculateColumnTotals(this.BudgetValues, this.BudgetTotals, "BudgetTotals");
            UpdateComparisonValues();
            UpdateMonthDetails();
        }


        //Possible update: add oldValue to transaction, find transaction being modified, and update the appropriate fields
        /// <summary>
        /// Updates the values in the Spending tab when a transaction is modified. Resets the values and recalculates from the transactions.
        /// Note: not very efficient and should be updated.
        /// </summary>
        /// <param name="sender">The changed Transaction property</param>
        /// <param name="e">The arguments</param>
        private void UpdateSpendingValues(Object sender, PropertyChangedEventArgs e)
        {
            //Only category, date, and amount will change the spending data
            if (!e.PropertyName.Equals("Category") && !e.PropertyName.Equals("Amount") && !e.PropertyName.Equals("Date"))
                return;
            //Reset values
            foreach (MoneyGridRow row in this.SpendingValues)
            {
                row.Values.Values = new decimal[12];
            }
            //Loop through each transaction and add the values
            foreach (Transaction transaction in this.Transactions)
            {
                if (transaction.Category != null)
                {
                    MoneyGridRow row;
                    try
                    {
                        row = this.SpendingValues.Single(x => x.Category == transaction.Category);
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
            UpdateSpendingTotals(); //Called here so that it is only updated once all the values are recalculated
        }

        /// <summary>
        /// Updates the values in the Spending tab when a transaction is added or removed. Adds or subtracts the appropriate amount.
        /// </summary>
        /// <param name="sender">The collection that was changed</param>
        /// <param name="e">The arguments</param>
        private void AddOrRemoveSpendingValues(Object sender, NotifyCollectionChangedEventArgs e)
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
                            row = this.SpendingValues.Single(x => x.Category == transaction.Category);
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
                        row = this.SpendingValues.Single(x => x.Category == transaction.Category);
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
                foreach (Transaction transaction in this.Transactions)
                {
                    if (transaction.Category != null)
                    {
                        MoneyGridRow row;
                        try
                        {
                            row = this.SpendingValues.Single(x => x.Category == transaction.Category);
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
        private void UpdateSpendingTotals()
        {
            CalculateColumnTotals(this.SpendingValues, this.SpendingTotals, "SpendingTotals");
            UpdateComparisonValues();
            UpdateMonthDetails();
        }

        private void UpdateComparisonValues()
        {
            for (int i = 0; i < this.ComparisonValues.Count; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    //Checks if value needs to be updated - reduces redraw time
                    //Sign adjusted for expenditure categories
                    if (this.ComparisonValues.ElementAt(i).Group.IsIncome)
                    {
                        if (this.ComparisonValues.ElementAt(i).Values[j] == this.SpendingValues.ElementAt(i).Values[j] - this.BudgetValues.ElementAt(i).Values[j])
                            continue;
                        this.ComparisonValues.ElementAt(i).Values[j] = this.SpendingValues.ElementAt(i).Values[j] - this.BudgetValues.ElementAt(i).Values[j];
                    }
                    else
                    {
                        if (this.ComparisonValues.ElementAt(i).Values[j] == this.BudgetValues.ElementAt(i).Values[j] - this.SpendingValues.ElementAt(i).Values[j])
                            continue;
                        this.ComparisonValues.ElementAt(i).Values[j] = this.BudgetValues.ElementAt(i).Values[j] - this.SpendingValues.ElementAt(i).Values[j];
                    }
                }
            }
            //Update the Totals grid if it has all the groups
            if (this.ComparisonTotals.Count == this.Groups.Count)
                CalculateColumnTotals(this.ComparisonValues, this.ComparisonTotals, "Comparison Totals");
            //_comparisonValues.MemberPropertyChanged(null, null);
        }

        private void UpdateMonthDetails()
        {
            if (this.RequestMonthDetailsUpdate != null)
            {
                this.RequestMonthDetailsUpdate(this, null);
            }
        }
        #endregion
    }
}