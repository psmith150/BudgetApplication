using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace BudgetApplication.Model
{
    public class TotalObservableCollection : MyObservableCollection<MoneyGridRow>
    {
        private Group _group;
        private MoneyGridRow netBudgetRow;
        private MoneyGridRow netSumRow;
        private MyObservableCollection<MoneyGridRow> _totals;
        public TotalObservableCollection(MyObservableCollection<MoneyGridRow> totals) : base()
        {
            _group = new Group(false, "Budget And Sum");
            netBudgetRow = new MoneyGridRow(_group, new Category("Net Budget"));
            netSumRow = new MoneyGridRow(_group, new Category("Net Sum"));
            netSumRow.IsSum = true;
            this.Add(netBudgetRow);
            this.Add(netSumRow);
            _totals = totals;
            _totals.MemberChanged += UpdateBudgetAndSum;
            IsComparison = false;
        }

        public void UpdateBudgetAndSum(object sender, PropertyChangedEventArgs e)
        {
            netBudgetRow.Values.Values = new decimal[12];
            for (int i=0; i< _totals.Count; i++)
            {
                MoneyGridRow row = _totals.ElementAt(i);
                for (int j=0; j<row.Values.Count; j++)
                {
                    if (IsComparison)
                    {
                        netBudgetRow.Values[j] += row.Values[j];
                        continue;
                    }
                    if (row.Group.IsIncome)
                        netBudgetRow.Values[j] += row.Values[j];
                    else
                        netBudgetRow.Values[j] -= row.Values[j];

                }
            }
            netSumRow.Values[0] = netBudgetRow.Values[0];
            
            for (int i=1; i< netBudgetRow.Values.Count; i++)
            {
                netSumRow.Values[i] = netSumRow.Values[i - 1] + netBudgetRow.Values[i];
            }
        }

        public bool IsComparison
        {
            get; set;
        }
    }
}
