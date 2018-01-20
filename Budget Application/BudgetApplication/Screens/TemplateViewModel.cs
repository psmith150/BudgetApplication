using System.Windows.Input;
using BudgetApplication.Popups;
using BudgetApplication.Services;
using GalaSoft.MvvmLight.CommandWpf;

namespace BudgetApplication.Screens
{
    public class TemplateViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;

        private bool _IsRunning;
        public bool IsRunning
        {
            get
            {
                return this._IsRunning;
            }
            set
            {
                this.Set(ref this._IsRunning, value);
            }
        }

        private string _Message;
        public string Message
        {
            get
            {
                return this._Message;
            }
            set
            {
                this.Set(ref this._Message, value);
            }
        }

        public ICommand TestButtonCommand { get; private set; }
        public ICommand OpenSettingsCommand { get; private set; }
        public ICommand AskQuestionCommand { get; private set; }
        public ICommand NavigateToChartCommand { get; private set; }

        public TemplateViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;
            this.TestButtonCommand = new RelayCommand(() => this.TestButton());
            this.OpenSettingsCommand = new RelayCommand(() => this.OpenSettings());
            this.AskQuestionCommand = new RelayCommand(() => this.AskQuestion());
            this.NavigateToChartCommand = new RelayCommand(() => this.NavigateToChart());
        }

        public override void Initialize()
        {
        }

        public override void Deinitialize()
        {
        }

        private async void TestButton()
        {
            this.Session.IsBusy = true;

            this.Message = "Long Task Started";
            this.Message = "Long Task Finished";

            this.Session.IsBusy = false;
            CommandManager.InvalidateRequerySuggested();
        }

        private async void OpenSettings()
        {
            //await this.navigationService.OpenPopup<SettingsViewModel>();
        }

        private async void AskQuestion()
        {
            //var e = await this.navigationService.OpenPopup<YesNoViewModel>("Do you want to be my friend?") as YesNoEventArgs;
            //if (e != null)
            //{
            //    this.Message = e.YesChosen ? ":)" : ":(";
            //}
        }

        private void NavigateToChart()
        {
            //this.navigationService.NavigateTo<PortalManagementViewModel>();
        }
    }
}