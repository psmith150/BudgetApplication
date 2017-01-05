using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Wrapper class for saving and loading data
    /// </summary>
    [Serializable]
    [XmlRoot("Data")]
    public class DataWrapper
    {
        public MyObservableCollection<Group> Groups { get; set; }   //Collection of groups
        public MyObservableCollection<PaymentMethod> PaymentMethods { get; set; }   //Collection of payment methods
        public MyObservableCollection<Transaction> Transactions { get; set; }   //Collection of transactions
        public List<decimal[]> BudgetValues { get; set; }   //Collection of budget values
    }
}
