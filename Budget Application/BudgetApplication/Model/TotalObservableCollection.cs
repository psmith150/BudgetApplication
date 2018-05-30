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
    /// <summary>
    /// A container for the NetSum and NetBudget rows of the budget and spending views.
    /// </summary>
    public class TotalObservableCollection : MyObservableCollection<MoneyGridRow>
    {
        private Group _group;   //The group of the rows
        private MoneyGridRow netBudgetRow;
        private MoneyGridRow netSumRow;
        private MyObservableCollection<MoneyGridRow> _totals;   //The collection of rows that represent the group totals.
        public TotalObservableCollection(MyObservableCollection<MoneyGridRow> totals) : base()
        {
            _group = new Group(false, "Budget And Sum");    //Creates a new group
            netBudgetRow = new MoneyGridRow(_group, new Category("Net Budget"));
            netSumRow = new MoneyGridRow(_group, new Category("Net Sum"));
            netSumRow.IsSum = true;
            this.Add(netBudgetRow);
            this.Add(netSumRow);
            _totals = totals;
            _totals.MemberChanged += UpdateBudgetAndSum;    //Updates the two rows when any of the totals have been changed.
            IsComparison = false;
        }

        /// <summary>
        /// Updates the two rows with new values.
        /// </summary>
        /// <param name="sender">The member of totals that was changed.</param>
        /// <param name="e">The arguments</param>
        public void UpdateBudgetAndSum(object sender, PropertyChangedEventArgs e)
        {
            netBudgetRow.Values.Values = new decimal[12];
            for (int i=0; i< _totals.Count; i++)
            {
                MoneyGridRow row = _totals.ElementAt(i);
                for (int j=0; j<row.Values.Count; j++)
                {
                    if (IsComparison)   //Checks if object is part of comparison view.
                    {
                        netBudgetRow.Values[j] += row.Values[j];
                        continue;
                    }
                    //Values have sign changed if it is from an expenditure group.
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

        /// <summary>
        /// Field that determines if it is part of the comparison view.
        /// Comparison rows are always added, regardless of income state.
        /// </summary>
        public bool IsComparison
        {
            get; set;
        }
    }
}
