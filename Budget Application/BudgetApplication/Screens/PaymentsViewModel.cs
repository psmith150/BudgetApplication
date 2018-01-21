using System.Windows.Input;
using BudgetApplication.Popups;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.CommandWpf;

namespace BudgetApplication.Screens
{
    public class PaymentsViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;

        public PaymentsViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;
        }

        public override void Initialize()
        {
        }

        public override void Deinitialize()
        {
        }
    }
}