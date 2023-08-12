using BudgetApplication.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;

namespace BudgetApplication
{
    public abstract class BaseViewModel : ObservableRecipient
    {
        private SessionService _Session;
        public SessionService Session
        {
            get
            {
                return this._Session;
            }
            private set
            {
                this.SetProperty(ref this._Session, value);
            }
        }

        protected BaseViewModel(SessionService session)
        {
            this.Session = session;
        }

        public string AppVersion
        {
            get
            {
                return Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
        }
    }
}