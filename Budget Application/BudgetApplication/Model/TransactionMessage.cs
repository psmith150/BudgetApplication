using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApplication.Model
{
    class TransactionMessage
    {
        public TransactionMessage(Transaction transaction)
        {
            this.Transaction = transaction;
        }
        public Transaction Transaction { get; private set; }
    }
}
