using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BudgetApplication.Screens
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class PaymentsScreen : UserControl
    {
        public PaymentsScreen(PaymentsViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }
        /// <summary>
        /// Handles the RequestBringIntoView event of the category combobox. Prevents annoying autoscroll.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            //Allows the keyboard to bring the items into view as expected:
            if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.Up))
                return;

            e.Handled = true;
        }
    }
}
