using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetApplication.Model;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Xml;
using System.Collections.Specialized;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections;
using System.Xml.Serialization;

namespace BudgetApplication.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private MyObservableCollection<Transaction> _transactions;
        private MyObservableCollection<Category> _categories;
        private MyObservableCollection<Group> _groups;
        private MyObservableCollection<PaymentMethod> _paymentMethods;
        private MyObservableCollection<MoneyGridRow> _budgetValues;
        private MyObservableCollection<MoneyGridRow> _budgetTotals;
        private MyObservableCollection<MoneyGridRow> _spendingValues;
        private MyObservableCollection<MoneyGridRow> _spendingTotals;
        private MyObservableCollection<MoneyGridRow> _comparisonValues;
        private MyObservableCollection<MoneyGridRow> _comparisonTotals;
        private ListCollectionView _budgetValueView;
        private ListCollectionView _spendingValueView;
        private ListCollectionView _comparisonValueView;
        private Group columnTotalsGroup;
        private String filepath = "data.xml";
        public MainViewModel()
        {
            _groups = new MyObservableCollection<Group>();
            _categories = new MyObservableCollection<Category>();
            _transactions = new MyObservableCollection<Transaction>();
            _paymentMethods = new MyObservableCollection<PaymentMethod>();
            _budgetValues = new MyObservableCollection<MoneyGridRow>();
            _budgetTotals = new MyObservableCollection<MoneyGridRow>();
            _spendingValues = new MyObservableCollection<MoneyGridRow>();
            _spendingTotals = new MyObservableCollection<MoneyGridRow>();
            _comparisonValues = new MyObservableCollection<MoneyGridRow>();
            _comparisonTotals = new MyObservableCollection<MoneyGridRow>();

            InitListViews();

            columnTotalsGroup = new Group(false, "Totals");

            _categories.CollectionChanged += UpdateCategories;
            _groups.CollectionChanged += UpdateGroups;
            _budgetValues.CollectionChanged += UpdateBudgetTotals;
            _transactions.CollectionChanged += UpdateSpendingValues;
            _spendingTotals.CollectionChanged += UpdateComparisonValues;
            _budgetTotals.CollectionChanged += UpdateComparisonValues;
            LoadData();

            BudgetRefreshCommand = new RelayCommand(OnBudgetValueUpdate, () => _canExecute);
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

            _canExecute = true;
        }

        #region Private helpers

        private void InitListViews()
        {
            _budgetValueView = new ListCollectionView(_budgetValues);
            _budgetValueView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
            _spendingValueView = new ListCollectionView(_spendingValues);
            _spendingValueView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
            _comparisonValueView = new ListCollectionView(_comparisonValues);
            _comparisonValueView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        }

        private void RefreshListViews()
        {
            _budgetValueView.Refresh();
            _spendingValueView.Refresh();
            _comparisonValueView.Refresh();
        }

        private void MoveValueRows(int start, int end)
        {
            _budgetValues.Move(start, end);
            _spendingValues.Move(start, end);
            _comparisonValues.Move(start, end);
        }

        private void MoveTotalRows(int start, int end)
        {
            _budgetTotals.Move(start, end);
            _spendingTotals.Move(start, end);
            _comparisonTotals.Move(start, end);
        }
        #endregion

        #region Common to all tabs

        public MyObservableCollection<Group> Groups
        {
            get
            {
                return _groups;
            }
            private set
            {

            }
        }

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

        public MyObservableCollection<PaymentMethod> PaymentMethods
        {
            get
            {
                return _paymentMethods;
            }
            set
            {

            }
        }

        public ListCollectionView BudgetRows
        {
            get
            {
                return _budgetValueView;
            }
        }

        public MyObservableCollection<MoneyGridRow> BudgetTotals
        {
            get
            {
                return _budgetTotals;
            }
        }

        public ListCollectionView SpendingRows
        {
            get
            {
                return _spendingValueView;
            }
        }

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

        public ListCollectionView ComparisonRows
        {
            get
            {
                return _comparisonValueView;
            }
        }

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

        private void CalculateColumnTotals(ObservableCollection<MoneyGridRow> columnValues, ObservableCollection<MoneyGridRow> columnTotals, String propertyName)
        {
            if (columnValues.Count < _categories.Count || columnTotals.Count < _groups.Count)
                return;
            //MessageBox.Show(propertyName);
            foreach (Group group in _groups)
            {
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
                        for (int i = 0; i < row.Values.Length; i++)
                        {
                            groupSum[i] += row.Values[i];
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(propertyName + " does not contain row for group " + group + " and category " + category);
                        continue;
                        //Debug.WriteLine(columnValues.ElementAt(0).Group + " " + columnValues.ElementAt(0).Category);
                        throw new ArgumentException("Could not find corresponding row", ex);
                    }
                }
                total.Values = groupSum;
                RaisePropertyChanged(propertyName);
            }
        }

        private void OnBudgetValueUpdate()
        {
            CalculateBudgetColumnTotals();
            UpdateComparisonValues(null, null);
            //RefreshListViews();
        }

        #endregion

        #region Methods to modify group and category collections

        public bool AddGroup(Group group)
        {
            if (String.IsNullOrEmpty(group.Name))
            {
                AddGroup(new Group());
            }
            int index = 0;
            bool nameExists = true;
            String name = "";
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
            return true;
        }

        public void RemoveGroup(Group group)
        {
            if (!_groups.Remove(group))
            {
                throw new ArgumentException("Group " + group.Name + " does not exist");
            }
            //Debug.WriteLine("Removed group " + group.Name);

            foreach (Category category in group.Categories)
            {
                //Debug.WriteLine("Removed category " + category.Name);

                _categories.Remove(category);
            }
        }

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
                _groups.Move(index, index - 1);
                MoveTotalRows(index, index - 1);
                //TODO: move entire group in budgetValues, etc
            }
        }

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
                //Debug.WriteLine("Start at " + startIndex + "; end at " + targetIndex + "; offset is " + offset);
                //Debug.WriteLine("Line 3 is category " + _budgetValues.ElementAt(3).Category.Name);
                //_budgetValues.Move(3, 7);
                //_budgetValueView.Refresh();
                //Debug.WriteLine("Line 3 is now category " + _budgetValues.ElementAt(3).Category.Name);
                //return;
                for (int i = 0; i <= endIndex-startIndex; i++)
                {
                    MoveValueRows(startIndex, startIndex+offset);
                    //Debug.WriteLine("Row moved from " + startIndex + " to " + (i + offset));
                }
                RefreshListViews();
            }
        }

        /// <summary>
        /// Adds a category with the specified name to the category list
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public bool AddCategory(Group group, Category category = null)
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
            group.Categories.Add(category);
            _categories.Add(category);
            int previousCategoryIndex = group.Categories.IndexOf(category) - 1;
            if (previousCategoryIndex < 0)
                return true;
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
            return true;
        }

        public void RemoveCategory(Category category)
        {
            Group currGroup = GetCategoryGroup(category);
            if (currGroup == null)
            {
                throw new ArgumentException("Category" + category.Name + " is not part of a group");
            }
            currGroup.Categories.Remove(category);
            if (!_categories.Remove(category))
            {
                throw new ArgumentException("Category " + category.Name + " does not exist");
            }
            //Debug.WriteLine("Removed category " + category.Name);

        }

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
                group.Categories.Move(index, index - 1);
                MoneyGridRow budgetRow = _budgetValues.Single(x => x.Category == category);
                int rowIndex = _budgetValues.IndexOf(budgetRow);
                //Debug.WriteLine("index is " + rowIndex);
                MoveValueRows(rowIndex, rowIndex - 1);
                //Debug.WriteLine("new index is " + _budgetValues.IndexOf(budgetRow));
            }
        }

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
                group.Categories.Move(index, index + 1);
                MoneyGridRow budgetRow = _budgetValues.Single(x => x.Category == category);
                int rowIndex = _budgetValues.IndexOf(budgetRow);
                MoveValueRows(rowIndex, rowIndex + 1);
            }
        }

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

        public void UpdateCategories(Object sender, NotifyCollectionChangedEventArgs e)
        {
            //Debug.WriteLine("Adding category");
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
                    //TODO: remove from other collections
                }
            }
        }
        public void UpdateGroups(Object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.Action != NotifyCollectionChangedAction.Move)
            {
                foreach (Group newGroup in e.NewItems)
                {
                    //Debug.WriteLine("New group" + newGroup.Name);
                    //Debug.WriteLine("Event: " + e.Action.ToString());

                    //MessageBox.Show(newCategory.Group.Name);
                    Category newCategory = new Category(newGroup.Name);
                    try
                    {
                        _budgetTotals.Add(new MoneyGridRow(columnTotalsGroup, newCategory));
                        _spendingTotals.Add(new MoneyGridRow(columnTotalsGroup, newCategory));
                        _comparisonTotals.Add(new MoneyGridRow(columnTotalsGroup, newCategory));
                    }
                    catch (ArgumentException ex)
                    {
                        Debug.WriteLine("Could not match group to category " + newCategory.Name);
                        throw new ArgumentException("Could not match group to category " + newGroup.Name, ex);
                    }
                }
            }

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
        public RelayCommand<Group> AddGroupCommand
        {
            get; set;
        }

        public RelayCommand<Group> RemoveGroupCommand
        {
            get; set;
        }

        public RelayCommand<Group> AddCategoryCommand
        {
            get; set;
        }

        public RelayCommand<Category> RemoveCategoryCommand
        {
            get; set;
        }

        public RelayCommand<Group> MoveGroupUpCommand
        {
            get; set;
        }

        public RelayCommand<Group> MoveGroupDownCommand
        {
            get; set;
        }

        public RelayCommand<Category> MoveCategoryUpCommand
        {
            get; set;
        }

        public RelayCommand<Category> MoveCategoryDownCommand
        {
            get; set;
        }
        #endregion

        #region Methods to modify payment method collection
        public void AddPaymentMethod(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
                return;
            _paymentMethods.Add(paymentMethod);
        }

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
        public RelayCommand<PaymentMethod> AddPaymentMethodCommand
        {
            get; set;
        }

        public RelayCommand<PaymentMethod> RemovePaymentMethodCommand
        {
            get; set;
        }
        #endregion

        #region Budget tab


        public void UpdateBudgetTotals(Object sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateColumnTotals(_budgetValues, _budgetTotals, "BudgetTotals");
        }

        public RelayCommand BudgetRefreshCommand
        {
            get;
            private set;
        }

        private void CalculateBudgetColumnTotals()
        {
            CalculateColumnTotals(_budgetValues, _budgetTotals, "BudgetTotals");
        }


        #endregion

        #region Spending tab
        public void UpdateSpendingValues(Object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (MoneyGridRow row in _spendingValues)
            {
                row.Values = new decimal[12];
            }
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
            UpdateSpendingTotals();

            //if (e.NewItems != null)
            //{
            //    foreach (Transaction newTransaction in _transactions)
            //    {
            //        MoneyGridRow row;
            //        try
            //        {
            //            row = _spendingValues.Single(x => x.Category == newTransaction.Category);
            //        }
            //        catch (ArgumentException ex)
            //        {
            //            throw new ArgumentException("Cannot find category for transaction category " + newTransaction.Category, ex);
            //        }
            //        //TODO: check year
            //        int month = newTransaction.Date.Month - 1;
            //        row.Values[month] += newTransaction.Amount;
            //    }
            //}

            //if (e.OldItems != null)
            //{
            //    foreach (Transaction oldTransaction in e.OldItems)
            //    {
            //        MoneyGridRow row;
            //        try
            //        {
            //            row = _spendingValues.Single(x => x.Category == oldTransaction.Category);
            //        }
            //        catch (ArgumentException ex)
            //        {
            //            throw new ArgumentException("Cannot find category for transaction category " + oldTransaction.Category, ex);
            //        }
            //        //TODO: check year
            //        int month = oldTransaction.Date.Month - 1;
            //        row.Values[month] -= oldTransaction.Amount;
            //    }
            //}
        }

        public void UpdateSpendingTotals()
        {
            CalculateColumnTotals(_spendingValues, _spendingTotals, "SpendingTotals");
            //Debug.WriteLine("Spending Total Updated");
        }

        private void CalculateSpendingColumnTotals()
        {
            CalculateColumnTotals(_spendingValues, _spendingTotals, "SpendingTotals");
        }
        #endregion

        #region Comparison tab

        public void UpdateComparisonValues(Object sender, NotifyCollectionChangedEventArgs e)
        {
            //Debug.WriteLine("Number of categories: " + _categories.Count + "Number of rows: " + _budgetValues.Count + " " + _spendingValues.Count + " " + _comparisonValues.Count);
            for (int i = 0; i < _comparisonValues.Count; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    _comparisonValues.ElementAt(i).Values[j] = _budgetValues.ElementAt(i).Values[j] - _spendingValues.ElementAt(i).Values[j];
                    if (_spendingValues.ElementAt(i).Values[j] > 0)
                    {
                        //MessageBox.Show(i + " " + j);
                    }
                }
            }
            if (_comparisonTotals.Count == _groups.Count)
                CalculateColumnTotals(_comparisonValues, _comparisonTotals, "Comparison Totals");
            _comparisonValues.MemberPropertyChanged(null, null);
        }

        #endregion

        #region Transaction tab

        public ObservableCollection<Transaction> Transactions
        {
            get
            {
                return _transactions;
            }
            private set
            {

            }
        }

        public bool AddTransaction(Transaction transaction)
        {
            //TODO: verify valid transaction
            _transactions.Add(transaction);
            return true;
        }
        #endregion

        #region Command Helpers
        private ICommand _saveData_ClickCommand;
        private ICommand _loadData_ClickCommand;
        public ICommand SaveData_ClickCommand
        {
            get
            {
                return _saveData_ClickCommand ?? (_saveData_ClickCommand = new CommandHandler(() => SaveData(), _canExecute));
            }
        }

        public ICommand LoadData_ClickCommand
        {
            get
            {
                return _loadData_ClickCommand ?? (_loadData_ClickCommand = new CommandHandler(() => LoadData(), _canExecute));
            }
        }
        private bool _canExecute;
        #endregion

        #region Saving and Opening files
        public void SaveData()
        {           
            using (FileStream file = new FileStream(filepath, FileMode.Create))
            {
                using (StreamWriter stream = new StreamWriter(file))
                {
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                    DataWrapper data = new DataWrapper();
                    data.Groups = _groups;
                    data.PaymentMethods = _paymentMethods;
                    data.Transactions = _transactions;
                    List<decimal[]> budgetData = new List<decimal[]>();
                    foreach (MoneyGridRow row in _budgetValues)
                    {
                        budgetData.Add(row.Values);
                    }
                    data.BudgetValues = budgetData;
                    dataSerializer.Serialize(stream, data);
                    return;
                    #region deprecated
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    using (XmlWriter writer = XmlWriter.Create(stream, settings))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("Data");

                        writer.WriteStartElement("Groups");
                        foreach (Group group in _groups)
                        {
                            writer.WriteStartElement("Group");
                            writer.WriteElementString("Name", group.Name);
                            writer.WriteElementString("IsIncome", group.IsIncome.ToString());

                            writer.WriteStartElement("Categories");
                            foreach (Category category in group.Categories)
                            {
                                writer.WriteStartElement("Category");
                                writer.WriteElementString("Name", category.Name);

                                writer.WriteStartElement("BudgetValues");
                                MoneyGridRow row;
                                try
                                {
                                    row = _budgetValues.Single(x => x.Group == group && x.Category == category);
                                }
                                catch
                                {
                                    throw new XmlException("Cannot find budget row for category " + category.Name + " and group " + group.Name);
                                }
                                foreach (decimal value in row.Values)
                                {
                                    writer.WriteElementString("Value", value.ToString());
                                }
                                writer.WriteEndElement();

                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();

                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();

                        writer.WriteStartElement("PaymentMethods");
                        foreach (PaymentMethod payment in _paymentMethods)
                        {
                            //MessageBox.Show(String.Format("{0}", category.Name));
                            if (payment.PaymentType == PaymentMethod.Type.CreditCard)
                            {
                                CreditCard card = payment as CreditCard;
                                writer.WriteStartElement("CreditCard");
                                writer.WriteElementString("Name", card.Name);
                                writer.WriteElementString("CreditLimit", card.CreditLimit.ToString());
                            }
                            else if (payment.PaymentType == PaymentMethod.Type.CheckingAccount)
                            {
                                CheckingAccount account = payment as CheckingAccount;
                                writer.WriteStartElement("CheckingAccount");
                                writer.WriteElementString("Name", account.Name);
                                writer.WriteElementString("Bank", account.Bank);
                                writer.WriteElementString("AccountNumber", account.AccountNumber.ToString());
                            }
                            else
                            {
                                throw new XmlException("Invalid payment method: " + payment.Name);
                            }
                            writer.WriteElementString("StartDate", payment.StartDate.ToString());
                            writer.WriteElementString("EndDate", payment.EndDate.ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();


                        writer.WriteStartElement("Transactions");
                        foreach (Transaction transaction in _transactions)
                        {
                            //MessageBox.Show(String.Format("{0}", category.Name));
                            writer.WriteStartElement("Transaction");
                            writer.WriteElementString("Date", transaction.Date.ToString());
                            writer.WriteElementString("Item", transaction.Item);
                            writer.WriteElementString("Payee", transaction.Payee);
                            writer.WriteElementString("Amount", transaction.Amount.ToString());
                            writer.WriteElementString("Comment", transaction.Comment);
                            writer.WriteElementString("Category", transaction.Category.Name);
                            writer.WriteElementString("PaymentMethod", transaction.PaymentMethod.Name);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();

                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                    #endregion
                }
            }
        }

        public void LoadData()
        {
            _groups.Clear();
            _categories.Clear();
            _transactions.Clear();
            _paymentMethods.Clear();
            DataWrapper data = new DataWrapper();
            try
            {
                using (FileStream file = new FileStream(filepath, FileMode.Open))
                {
                    XmlSerializer dataSerializer = new XmlSerializer(typeof(DataWrapper));
                    data = (DataWrapper)dataSerializer.Deserialize(file);
                }
            }
            catch (IOException ex) //File does not exist; set everything to defaults
            {

            }
            int index = 0;
            foreach (Group group in data.Groups)
            {
                Group newGroup = new Group(group.IsIncome, group.Name);
                _groups.Add(newGroup);
                foreach (Category category in group.Categories)
                {
                    newGroup.Categories.Add(category);
                    _categories.Add(category);
                    MoneyGridRow row = _budgetValues.Single(x => x.Category == category);
                    row.Values = data.BudgetValues.ElementAt(index);
                    index++;
                }
            }
            foreach (PaymentMethod payment in data.PaymentMethods)
            {
                _paymentMethods.Add(payment);
            }
            //Need to match transaction categories and payment methods
            foreach (Transaction transaction in data.Transactions)
            {
                String categoryName = transaction.Category.Name;
                string paymentName = transaction.PaymentMethod.Name;
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
                _transactions.Add(transaction);
            }
            Debug.WriteLine(_budgetValues.Count);
            return;


            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    String nodeName = node.Name;

                    if (nodeName.Equals("Groups"))
                    {
                        foreach (XmlNode groupNode in node.ChildNodes)
                        {
                            String name = null;
                            bool isIncome = false;
                            MyObservableCollection<Category> categories = new MyObservableCollection<Category>();
                            foreach (XmlNode property in groupNode.ChildNodes)
                            {
                                if (property.Name.Equals("Name"))
                                {
                                    name = property.InnerText;
                                }
                                else if (property.Name.Equals("IsIncome"))
                                {
                                    isIncome = Boolean.Parse(property.InnerText);
                                }
                                else if (property.Name.Equals("Categories"))
                                {
                                    Group newGroup = new Group(isIncome, name);
                                    _groups.Add(newGroup);
                                    foreach (XmlNode categoryNode in property.ChildNodes)
                                    {
                                        String categoryName = null;
                                        foreach (XmlNode categoryProperty in categoryNode.ChildNodes)
                                        {
                                            if (categoryProperty.Name.Equals("Name"))
                                            {
                                                categoryName = categoryProperty.InnerText;
                                            }
                                            else if (categoryProperty.Name.Equals("BudgetValues"))
                                            {
                                                Category newCategory = new Category(categoryName);
                                                newGroup.Categories.Add(newCategory);
                                                _categories.Add(newCategory);
                                                MoneyGridRow row;
                                                try
                                                {
                                                    row = _budgetValues.Single(x => x.Group == newGroup && x.Category == newCategory);
                                                }
                                                catch
                                                {
                                                    throw new XmlException("Cannot find budget row for category " + newCategory.Name + " and group " + newGroup.Name);
                                                }
                                                decimal[] values = new decimal[12];
                                                for (int i = 0; i < 12; i++)
                                                {
                                                    values[i] = Decimal.Parse(categoryProperty.ChildNodes[i].InnerText);
                                                }
                                                row.Values = values;
                                            }
                                            else
                                            {
                                                throw new XmlException("Unknown category property: " + categoryProperty.Name);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    throw new XmlException("Unknown group property: " + property.Name);
                                }
                            }
                        }
                    }
                    else if (nodeName.Equals("PaymentMethods"))
                    {
                        foreach (XmlNode paymentNode in node.ChildNodes)
                        {
                            String name = null;
                            PaymentMethod payment = null;
                            DateTime startDate = new DateTime();
                            DateTime endDate = DateTime.Today;
                            if (paymentNode.Name.Equals("CreditCard"))
                            {
                                decimal creditLimit = 300;
                                foreach (XmlNode property in paymentNode.ChildNodes)
                                {
                                    if (property.Name.Equals("Name"))
                                    {
                                        name = property.InnerText;
                                    }
                                    else if (property.Name.Equals("CreditLimit"))
                                    {
                                        creditLimit = Decimal.Parse(property.InnerText);
                                    }
                                    else if (property.Name.Equals("StartDate"))
                                    {
                                        startDate = DateTime.Parse(property.InnerText);
                                    }
                                    else if (property.Name.Equals("EndDate"))
                                    {
                                        endDate = DateTime.Parse(property.InnerText);
                                    }
                                    else
                                    {
                                        throw new XmlException("Unknown credit card property: " + property.Name);
                                    }
                                }
                                payment = new CreditCard(name, creditLimit);
                            }
                            else if (paymentNode.Name.Equals("CheckingAccount"))
                            {
                                string bank = "";
                                int accountNumber = 0;
                                foreach (XmlNode property in paymentNode.ChildNodes)
                                {
                                    if (property.Name.Equals("Name"))
                                    {
                                        name = property.InnerText;
                                    }
                                    else if (property.Name.Equals("Bank"))
                                    {
                                        bank = property.InnerText;
                                    }
                                    else if (property.Name.Equals("AccountNumber"))
                                    {
                                        accountNumber = int.Parse(property.InnerText);
                                    }
                                    else if (property.Name.Equals("StartDate"))
                                    {
                                        startDate = DateTime.Parse(property.InnerText);
                                    }
                                    else if (property.Name.Equals("EndDate"))
                                    {
                                        endDate = DateTime.Parse(property.InnerText);
                                    }
                                    else
                                    {
                                        throw new XmlException("Unknown checking account property: " + property.Name);
                                    }
                                }
                                payment = new CheckingAccount(name);
                                (payment as CheckingAccount).Bank = bank;
                                (payment as CheckingAccount).AccountNumber = accountNumber;
                            }
                            else
                            {
                                throw new XmlException("Unknown payment type: " + paymentNode.Name);
                            }
                            payment.StartDate = startDate;
                            payment.EndDate = endDate;
                            _paymentMethods.Add(payment);
                        }
                    }
                    else if (nodeName.Equals("Transactions"))
                    {
                        foreach (XmlNode transactionNode in node.ChildNodes)
                        {
                            DateTime date = DateTime.Today;
                            String item = null;
                            String payee = null;
                            Decimal amount = 0;
                            String comment = null;
                            String categoryName = null;
                            Category category = null;
                            String paymentName = null;
                            PaymentMethod payment = null;

                            foreach (XmlNode property in transactionNode.ChildNodes)
                            {
                                if (property.Name.Equals("Date"))
                                {
                                    date = DateTime.Parse(property.InnerText);
                                }
                                else if (property.Name.Equals("Item"))
                                {
                                    item = property.InnerText;
                                }
                                else if (property.Name.Equals("Payee"))
                                {
                                    payee = property.InnerText;
                                }
                                else if (property.Name.Equals("Amount"))
                                {
                                    amount = Decimal.Parse(property.InnerText);
                                }
                                else if (property.Name.Equals("Comment"))
                                {
                                    comment = property.InnerText;
                                }
                                else if (property.Name.Equals("Category"))
                                {
                                    categoryName = property.InnerText;
                                    try
                                    {
                                        category = _categories.Single(x => x.Name.Equals(categoryName));
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new XmlException("Invalid category name: " + categoryName, ex);
                                    }
                                }
                                else if (property.Name.Equals("PaymentMethod"))
                                {
                                    paymentName = property.InnerText;
                                    try
                                    {
                                        payment = _paymentMethods.Single(x => x.Name.Equals(paymentName));
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new XmlException("Invalid payment method name: " + paymentName, ex);
                                    }
                                }
                                else
                                {
                                    throw new XmlException("Unknown transaction property: " + property.Name);
                                }
                            }
                            Transaction transaction = new Transaction();
                            transaction.Date = date;
                            transaction.Item = item;
                            transaction.Payee = payee;
                            transaction.Amount = amount;
                            transaction.Comment = comment;
                            transaction.Category = category;
                            transaction.PaymentMethod = payment;
                            _transactions.Add(transaction);
                        }
                    }
                    else
                    {
                        throw new XmlException("Unexpected node: " + nodeName);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException("Could not load xml file from " + filepath, ex);
            }
        }
        #endregion

    }

    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }

    [Serializable]
    [XmlRoot("Data")]
    public class DataWrapper
    {
        public MyObservableCollection<Group> Groups { get; set; }
        public MyObservableCollection<PaymentMethod> PaymentMethods { get; set; }
        public MyObservableCollection<Transaction> Transactions { get; set; }
        public List<decimal[]> BudgetValues { get; set; }
    }
}
