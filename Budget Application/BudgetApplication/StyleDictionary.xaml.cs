using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BudgetApplication
{
    public partial class StyleDictionary
    {
        public void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridColumnHeader parentHeader = ((Button)sender).TemplatedParent as DataGridColumnHeader;
            //MessageBox.Show(parent.Column.Header as string);
        }
    }
}
