using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections;

namespace BudgetApplication.Model
{
    public class RowCompare : IComparer
    {
        private ObservableCollection<Group> _groups;
        public RowCompare(ObservableCollection<Group> groups)
        {
            _groups = groups;
        }
        public int Compare(Object x, Object y)
        {
            if (x as MoneyGridRow == null && y as MoneyGridRow == null)
            {
                throw new ArgumentException("RowComparer can only sort MoneyGridRow objects");
            }
            Group group1 = (x as MoneyGridRow).Group;
            Group group2 = (y as MoneyGridRow).Group;
            int index1 = _groups.IndexOf(group1);
            int index2 = _groups.IndexOf(group2);
            return (index1 - index2);
        }
    }
}
