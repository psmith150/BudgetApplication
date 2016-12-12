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
        private Group budgetTotalGroup;
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

            budgetTotalGroup = new Group(false, "Totals");

            _categories.CollectionChanged += UpdateCategories;
            _groups.CollectionChanged += UpdateGroups;
            _budgetValues.CollectionChanged += UpdateBudgetTotals;
            _transactions.CollectionChanged += UpdateSpendingValues;
            LoadData();

            BudgetRefreshCommand = new RelayCommand(CalculateBudgetColumnTotals, () => _canExecute);

            _canExecute = true;
        }
        #region Common to all tabs

        public ObservableCollection<Group> Groups
        {
            get
            {
                return _groups;
            }
            private set
            {

            }
        }

        public bool AddGroup(Group group)
        {
            foreach (Group currGroup in _groups)
            {
                if (currGroup.Name.Equals(group))
                {
                    return false;
                }
            }
            if (String.IsNullOrEmpty(group.Name))
            {
                return false;
            }
            _groups.Add(group);
            return true;
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

        /// <summary>
        /// Adds a category with the specified name to the category list
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public bool AddCategory(Category category)
        {
            foreach (Category currCategory in _categories)
            {
                if (currCategory.Name.Equals(category))
                {
                    return false;
                }
            }
            if (String.IsNullOrEmpty(category.Name))
            {
                return false;
            }
            _categories.Add(category);
            return true;
        }

        public bool IsValidCategory(string categoryName)
        {
            foreach (Category currCategory in _categories)
            {
                if (currCategory.Name.Equals(categoryName))
                {
                    return true;
                }
            }
            return false;
        }

        private Group GetCategoryGroup(Category category)
        {
            foreach (Group group in _groups)
            {
                if(group.Categories.Contains(category))
                {
                    return group;
                }
            }
            return null;
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
                return _spendingValues;
            }
            set
            {
                _spendingValues = value;
            }
        }

        public MyObservableCollection<MoneyGridRow> ComparisonTotals
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

        private void CalculateColumnTotals(ObservableCollection<MoneyGridRow> columnTotals, String propertyName)
        {
            MessageBox.Show(propertyName);
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
                        MoneyGridRow row = columnTotals.Single(x => x.Group == group && x.Category == category);
                        for (int i = 0; i < row.Values.Length; i++)
                        {
                            groupSum[i] += row.Values[i];
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("Could not find corresponding row", ex);
                    }
                }
                total.Values = groupSum;
                RaisePropertyChanged(propertyName);
            }
        }

        #endregion

        #region Budget tab
        public void UpdateCategories(Object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Category newCategory in e.NewItems)
                {
                    //MessageBox.Show(newCategory.Group.Name);
                    try
                    {
                        _budgetValues.Add(new MoneyGridRow(GetCategoryGroup(newCategory), newCategory));
                        //_spendingValues.Add(new MoneyGridRow(GetCategoryGroup(newCategory), newCategory));
                        //_comparisonValues.Add(new MoneyGridRow(GetCategoryGroup(newCategory), newCategory));
                    }
                    catch (ArgumentException ex)
                    {
                        //Debug.WriteLine("Could not match group to category " + newCategory.Name);
                        throw new ArgumentException("Could not match group to category " + newCategory.Name, ex);
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (Category oldCategory in e.OldItems)
                {
                    MoneyGridRow oldRow = _budgetValues.Where(row => row.Category == oldCategory).ElementAt(0);
                    if (oldRow == null)
                        throw new ArgumentException("Cannot locate deleted row");
                    _budgetValues.Remove(oldRow);
                    //TODO: remove from other collections
                }
            }
        }
        public void UpdateGroups(Object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Group newGroup in e.NewItems)
                {
                    //MessageBox.Show(newCategory.Group.Name);
                    Category newCategory = new Category(newGroup.Name);
                    try
                    {
                        _budgetTotals.Add(new MoneyGridRow(budgetTotalGroup, newCategory));
                    }
                    catch (ArgumentException ex)
                    {
                        //Debug.WriteLine("Could not match group to category " + newCategory.Name);
                        throw new ArgumentException("Could not match group to category " + newGroup.Name, ex);
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (Category oldCategory in e.OldItems)
                {
                    MoneyGridRow oldRow = _budgetValues.Where(row => row.Category == oldCategory).ElementAt(0);
                    if (oldRow == null)
                        throw new ArgumentException("Cannot locate deleted row");
                    _budgetValues.Remove(oldRow);
                }
            }
        }

        public void UpdateBudgetTotals(Object sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateColumnTotals(_budgetTotals, "BudgetTotals");
        }

        public RelayCommand BudgetRefreshCommand
        {
            get;
            private set;
        }

        private void CalculateBudgetColumnTotals()
        {
            CalculateColumnTotals(_budgetTotals, "BudgetTotals");
        }


        #endregion

        #region Spending tab
        public void UpdateSpendingValues(Object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Transaction newTransaction in e.NewItems)
                {
                    MoneyGridRow row;
                    try
                    {
                        row = _spendingValues.Single(x => x.Category == newTransaction.Category);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new ArgumentException("Cannot find category for transaction category " + newTransaction.Category, ex);
                    }
                    //TODO: check year
                    int month = newTransaction.Date.Month - 1;
                    row.Values[month] += newTransaction.Amount;
                }
            }

            if (e.OldItems != null)
            {
                foreach (Transaction oldTransaction in e.OldItems)
                {
                    MoneyGridRow row;
                    try
                    {
                        row = _spendingValues.Single(x => x.Category == oldTransaction.Category);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new ArgumentException("Cannot find category for transaction category " + oldTransaction.Category, ex);
                    }
                    //TODO: check year
                    int month = oldTransaction.Date.Month - 1;
                    row.Values[month] -= oldTransaction.Amount;
                }
            }
        }
        #endregion

        #region Comparison tab
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
                                    Debug.WriteLine("gets here");
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
