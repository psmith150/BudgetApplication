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
        private Group columnTotalsGroup;
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

            columnTotalsGroup = new Group(false, "Totals");

            _categories.CollectionChanged += UpdateCategories;
            _groups.CollectionChanged += UpdateGroups;
            _budgetValues.CollectionChanged += UpdateBudgetTotals;
            _transactions.CollectionChanged += UpdateSpendingValues;
            _transactions.CollectionChanged += UpdateSpendingTotals;
            _budgetValues.CollectionChanged += UpdateComparisonValues;
            _spendingValues.CollectionChanged += UpdateComparisonValues;
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

            _canExecute = true;
        }
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

        public bool AddPaymentMethod(PaymentMethod paymentMethod)
        {
            _paymentMethods.Add(paymentMethod);
            return true;
        }

        public MyObservableCollection<MoneyGridRow> BudgetRows
        {
            get
            {
                return _budgetValues;
            }
            set
            {
                _budgetValues = value;
            }
        }

        public MyObservableCollection<MoneyGridRow> BudgetTotals
        {
            get
            {
                return _budgetTotals;
            }
            set
            {
                _budgetTotals = value;
            }
        }

        public MyObservableCollection<MoneyGridRow> SpendingRows
        {
            get
            {
                return _spendingValues;
            }
            set
            {
                _spendingValues = value;
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

        public MyObservableCollection<MoneyGridRow> ComparisonRows
        {
            get
            {
                return _comparisonValues;
            }
            set
            {
                _comparisonValues = value;
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
                    throw new ArgumentException("Could not find corresponding total row", ex);
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
                        Debug.WriteLine(columnValues.ElementAt(0).Group + " " + columnValues.ElementAt(0).Category);
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
            Debug.WriteLine("Removed group " + group.Name);

            foreach (Category category in group.Categories)
            {
                Debug.WriteLine("Removed category " + category.Name);

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
                _budgetTotals.Move(index, index - 1);
                _spendingTotals.Move(index, index - 1);
                _comparisonTotals.Move(index, index - 1);
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
            if (index < _groups.Count-1)
            {
                _groups.Move(index, index + 1);
                _budgetTotals.Move(index, index + 1);
                _spendingTotals.Move(index, index + 1);
                _comparisonTotals.Move(index, index + 1);
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
                throw new ArgumentException("Could not find row with category " + group.Categories.ElementAt(previousCategoryIndex));
            }
            int previousRowIndex = _budgetValues.IndexOf(previousRow);
            _categories.Move(_budgetValues.Count - 1, previousRowIndex + 1);
            _budgetValues.Move(_budgetValues.Count - 1, previousRowIndex + 1);
            _spendingValues.Move(_budgetValues.Count - 1, previousRowIndex + 1);
            _comparisonValues.Move(_budgetValues.Count - 1, previousRowIndex + 1);
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
            Debug.WriteLine("Removed category " + category.Name);

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
                group.Categories.Move(index, index-1);
                MoneyGridRow budgetRow = _budgetValues.Single(x => x.Category == category);
                int rowIndex = _budgetValues.IndexOf(budgetRow);
                Debug.WriteLine("index is " + rowIndex);
                _budgetValues.Move(rowIndex, rowIndex-1);
                Debug.WriteLine("new index is " + _budgetValues.IndexOf(budgetRow));
                _spendingValues.Move(rowIndex, rowIndex - 1);
                _comparisonValues.Move(rowIndex, rowIndex - 1);
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
                _budgetValues.Move(rowIndex, rowIndex + 1);
                _spendingValues.Move(rowIndex, rowIndex + 1);
                _comparisonValues.Move(rowIndex, rowIndex + 1);
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
            if (e.NewItems != null && e.Action != NotifyCollectionChangedAction.Move)
            {
                foreach (Category newCategory in e.NewItems)
                {
                    //MessageBox.Show(newCategory.Group.Name);
                    Group group = GetCategoryGroup(newCategory);
                    if (group == null)
                    {

                    }
                    try
                    {
                        _budgetValues.Add(new MoneyGridRow(group, newCategory));
                        _spendingValues.Add(new MoneyGridRow(group, newCategory));
                        _comparisonValues.Add(new MoneyGridRow(group, newCategory));
                    }
                    catch (ArgumentException ex)
                    {
                        Debug.WriteLine("Could not match group to category " + newCategory.Name + ", " + GetCategoryGroup(newCategory).Name);
                        throw new ArgumentException("Could not match group to category " + newCategory.Name, ex);
                    }
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
                    Debug.WriteLine("New group" + newGroup.Name);
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
                    Debug.WriteLine("Group to be deleted: " + oldGroup.Name);
                    foreach (MoneyGridRow row in _budgetValues)
                    {
                        Debug.WriteLine("Budget row: " + row.Category);
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

        public void TestGroup(Group group)
        {
            Debug.WriteLine(group.Name);
        }

        public RelayCommand<Group> RemoveGroupCommand
        {
            get; set;
        }

        public RelayCommand<Group> AddCategoryCommand
        {
            get;set;
        }

        public RelayCommand<Category> RemoveCategoryCommand
        {
            get;set;
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
                MoneyGridRow row;
                try
                {
                    row = _spendingValues.Single(x => x.Category == transaction.Category);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Cannot find category for transaction category " + transaction.Category, ex);
                }
                //TODO: check year
                int month = transaction.Date.Month - 1;
                row.Values[month] += transaction.Amount;
            }

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

        public void UpdateSpendingTotals(Object sender, NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine("Spending Total Updated");
            CalculateColumnTotals(_spendingValues, _spendingTotals, "SpendingTotals");
        }

        private void CalculateSpendingColumnTotals()
        {
            CalculateColumnTotals(_spendingValues, _spendingTotals, "SpendingTotals");
        }
        #endregion

        #region Comparison tab

        public void UpdateComparisonValues(Object sender, NotifyCollectionChangedEventArgs e)
        {
            //bug.WriteLine("Number of categories: " + _categories.Count + "Number of rows: " + _budgetValues.Count + " " + _spendingValues.Count + " " + _comparisonValues.Count);
            for (int i=0;i<_comparisonValues.Count; i++)
            {
                for(int j=0;j<12;j++)
                {
                    _comparisonValues.ElementAt(i).Values[j] = _budgetValues.ElementAt(i).Values[j] - _spendingValues.ElementAt(i).Values[j];
                }
            }
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
            using (FileStream file = new FileStream("data.xml", FileMode.Create))
            {
                using (StreamWriter stream = new StreamWriter(file))
                {
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
                            if(payment.PaymentType() == PaymentMethod.Type.CreditCard)
                            {
                                CreditCard card = payment as CreditCard;
                                writer.WriteStartElement("CreditCard");
                                writer.WriteElementString("Name", card.Name);
                                writer.WriteElementString("CreditLimit", card.CreditLimit.ToString());
                            }
                            else if (payment.PaymentType() == PaymentMethod.Type.CheckingAccount)
                            {
                                CheckingAccount account = payment as CheckingAccount;
                                writer.WriteStartElement("CheckingAccount");
                                writer.WriteElementString("Name", account.Name);
                            }
                            else
                            {
                                throw new XmlException("Invalid payment method: " + payment.Name);
                            }
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
                }

            }
        }

        public void LoadData()
        {
            _groups.Clear();
            _categories.Clear();
            _transactions.Clear();
            _paymentMethods.Clear();
            //MessageBox.Show("Gets here 2");
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("data.xml");

                foreach(XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    String nodeName = node.Name;

                    if (nodeName.Equals("Groups"))
                    {
                        foreach(XmlNode groupNode in node.ChildNodes)
                        {
                            String name = null;
                            bool isIncome = false;
                            MyObservableCollection<Category> categories = new MyObservableCollection<Category>();
                            foreach(XmlNode property in groupNode.ChildNodes)
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
                                                for (int i=0; i<12; i++)
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
                            if (paymentNode.Name.Equals("CreditCard"))
                            {
                                foreach (XmlNode property in paymentNode.ChildNodes)
                                {
                                    decimal creditLimit = 300;
                                    if (property.Name.Equals("Name"))
                                    {
                                        name = property.InnerText;
                                    }
                                    else if (property.Name.Equals("CreditLimit"))
                                    {
                                        creditLimit = Decimal.Parse(property.InnerText);
                                    }
                                    else
                                    {
                                        throw new XmlException("Unknown credit card property: " + property.Name);
                                    }
                                    payment = new CreditCard(name, creditLimit);
                                }
                            }
                            else if (paymentNode.Name.Equals("CheckingAccount"))
                            {
                                foreach (XmlNode property in paymentNode.ChildNodes)
                                {
                                    if (property.Name.Equals("Name"))
                                    {
                                        name = property.InnerText;
                                    }
                                    else
                                    {
                                        throw new XmlException("Unknown checking account property: " + property.Name);
                                    }
                                    payment = new CheckingAccount(name);
                                }
                            }
                            else
                            {
                                throw new XmlException("Unknown payment type: " + paymentNode.Name);
                            }
                            _paymentMethods.Add(payment);
                        }
                    }
                    else if (nodeName.Equals("Transactions"))
                    {
                        foreach (XmlNode transactionNode in node.ChildNodes)
                        {
                            DateTime date = DateTime.Today;
                            String item=null;
                            String payee=null;
                            Decimal amount=0;
                            String comment=null;
                            String categoryName=null;
                            Category category=null;
                            String paymentName=null;
                            PaymentMethod payment=null;

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
}
