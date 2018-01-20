
using BudgetApplication.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace BudgetApplication.Services
{
    public class SessionService : ObservableObject
    {
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
        #endregion
        #endregion
        #region Public Methods
        public void LoadDataFromFile(string filePath)
        {
            //Debug.WriteLine("Loading data");
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
            this.Groups.Add(testGroup);
            this.Categories.Add(testCategory);
            this.BudgetValues.Add(testRow);
            return;

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
                //TODO: InitNewFile();
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine($"Error reading XML from file {filePath}\n" + ex.Message);
            }
            //Process the data
            int index = 0;
            //Add groups, categories, and budget values
            List<Group> tempGroups = new List<Group>();
            List<Category> tempCategories = new List<Category>();
            Stopwatch runTimer;
            runTimer = Stopwatch.StartNew();
            // TODO BudgetValues.MemberChanged -= UpdateBudgetTotals;
            //Debug.WriteLine("Placeholder");
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
            // TODO_budgetValues.MemberChanged += UpdateBudgetTotals;
            // TODO RefreshBudgetTotals();
            runTimer.Stop();
            //Debug.WriteLine("Reading groups and categories: " + runTimer.ElapsedTicks);
            runTimer = Stopwatch.StartNew();
            //Add payment methods
            foreach (PaymentMethod payment in data.PaymentMethods)
            {
                this.PaymentMethods.Add(payment);
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
                string categoryName = transaction.Category.Name;
                //Debug.WriteLine(transaction.Item + " " + transaction.PaymentMethod.Name);
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
            runTimer.Stop();
            //Debug.WriteLine("Reading transactions: " + runTimer.ElapsedTicks);
            //Debug.WriteLine(_budgetValues.Count);
        }
        #endregion
    }
}