using BudgetApplication.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BudgetApplication.Popups
{
    public class ErrorViewModel : PopupViewModel
    {
        private readonly NavigationService navigationService;
        public ICommand AcknowledgeErrorCommand { get; private set; }
        private bool _busyStatus;
        private string _busyMessage;
        #region Constructor

        public ErrorViewModel(SessionService session, NavigationService navigation) : base(session)
        {
            this.navigationService = navigation;
            this.AcknowledgeErrorCommand = new RelayCommand(() => AcknowledgeError());
        }

        #endregion

        #region Public Properties

        private string _ErrorMessage;
        public string ErrorMessage
        {
            get
            {
                return this._ErrorMessage;
            }
            set
            {
                this.Set(ref this._ErrorMessage, value);
            }
        }
        
        #endregion

        public override void Deinitialize()
        {
        }

        public override void Initialize(object param)
        {
            if (param is string)
            {
                this.ErrorMessage = param as string;
            }
            else
            {
                this.ErrorMessage = "";
            }
            _busyStatus = this.Session.IsBusy;
            _busyMessage = this.Session.BusyMessage;
            if (_busyStatus)
                this.Session.IsBusy = false; //Hide busy indicator so error can be acknowledged
        }

        public void AcknowledgeError()
        {
            this.Session.IsBusy = _busyStatus; //Set busy indicator to its previous state
            this.Session.BusyMessage = _busyMessage;
            this.ClosePopup(null);
        }
    }
}
