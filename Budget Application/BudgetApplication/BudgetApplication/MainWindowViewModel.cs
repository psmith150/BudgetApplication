using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetApplication.HelperClasses;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace BudgetApplication
{
    public class MainWindowViewModel
    {
        private List<Transaction> _transactions;
        public MainWindowViewModel()
        {
            _transactions = new List<Transaction>();
            CreditCard testCard = new CreditCard("testcard1");
            _transactions.Add(new HelperClasses.Transaction("test1", "test1", -10.11M, "Food", testCard, DateTime.Today, "this is a test"));
        }
        public List<Transaction> Transactions
        {
            get
            {
                return _transactions;
            }
            set
            {

            }
        }
    }
}
