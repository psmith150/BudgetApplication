using System;
using System.Collections.ObjectModel;
using System.Collections;

namespace BudgetApplication.Model
{
    /// <summary>
    /// Class for comparing MoneyGridRow objects
    /// </summary>
    public class RowCompare : IComparer
    {
        private ObservableCollection<Group> _groups;    //The collection of existing groups

        /// <summary>
        /// Instantiates a new instance for comparing.
        /// </summary>
        /// <param name="groups">The collection of groups</param>
        public RowCompare(ObservableCollection<Group> groups)
        {
            _groups = groups;
        }

        /// <summary>
        /// The main comparison. Operates by comparing the index of the rows' groups inside the group collection.
        /// </summary>
        /// <param name="x">Row 1</param>
        /// <param name="y">Row 2</param>
        /// <returns>A value representing the difference between the indices.</returns>
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
