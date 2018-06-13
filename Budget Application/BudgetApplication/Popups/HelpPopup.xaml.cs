using System.Windows.Controls;

namespace BudgetApplication.Popups
{
    /// <summary>
    /// Interaction logic for SettingsPopup.xaml
    /// </summary>
    public partial class HelpPopup : UserControl
    {
        public HelpPopup(HelpViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }
    }
}
