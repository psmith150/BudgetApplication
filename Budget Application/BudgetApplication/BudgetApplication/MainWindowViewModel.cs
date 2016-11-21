using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetApplication.Model;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace BudgetApplication
{
    public class MainWindowViewModel
    {
        private ObservableCollection<Transaction> _transactions;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Group> _groups;
        public MainWindowViewModel()
        {
            _categories = new ObservableCollection<Category>();
            _groups = new ObservableCollection<Group>();
            _transactions = new ObservableCollection<Transaction>();
            CreditCard testCard = new CreditCard("testcard1");
            Group testGroup = new Model.Group(false, "Test Group");
            _groups.Add(testGroup);
            Category testCategory = new Model.Category(testGroup, "test Category");
            _categories.Add(testCategory);
            Transaction testTransaction = new Transaction();
            testTransaction.Date = DateTime.Today;
            testTransaction.Item = "test item";
            testTransaction.Payee = "test payee";
            testTransaction.Amount = 10;
            testTransaction.Comment = "test #1";
            testTransaction.Category = testCategory;
            testTransaction.PaymentMethod = testCard;
            AddTransaction(testTransaction);
        }
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

        public ObservableCollection<Category> Categories
        {
            get
            {
                return _categories;
            }
            private set
            {

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
    }
}
