using BudgetApplication.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BudgetApplication.Base.AbstractClasses;
using BudgetApplication.Base.Enums;
using BudgetApplication.Base.EventArgs;

namespace BudgetApplication.Popups
{
    public class ChangeYearViewModel : PopupViewModel
    {
        #region Commands
        public ICommand SaveCommand { get; private set; }
        public ICommand ExitPopupCommand {get; private set;}
        #endregion

        public ChangeYearViewModel(SessionService session, MessageViewerBase messageViewer) : base(session)
        {
            this.SaveCommand = new RelayCommand(() => this.Save());
            this.ExitPopupCommand = new RelayCommand(() => this.Exit());

            this._messageViewer = messageViewer;
        }

        public override void Initialize(object param)
        {
            this.CurrentYear = this.Session.CurrentYear.ToString();
            this.savingNeeded = false;
        }

        public override void Deinitialize()
        {
        }

        #region Public Properties
        private string _currentYear;
        public string CurrentYear
        {
            get
            {
                return this._currentYear;
            }
            set
            {
                this.Set(ref this._currentYear, value);
                this.savingNeeded = true;
            }
        }
        #endregion

        #region Private Fields
        private bool savingNeeded = false;
        private MessageViewerBase _messageViewer;
        #endregion

        #region Private Methods
        private async void Exit()
        {
            if (this.savingNeeded)
            {
                MessageViewerEventArgs result = await this._messageViewer.DisplayMessage("Settings have not been saved, are you sure you want to exit?", "Discard Changes?", MessageViewerButton.OkCancel);
                if (result.Result == MessageViewerResult.Ok)
                    this.ClosePopup(null);
            }
            else
            {
                this.ClosePopup(null);
            }
        }

        private async void Save()
        {
            int testYear = 0;
            bool result = Int32.TryParse(this.CurrentYear, out testYear);
            if (result && testYear > 1950 && testYear < 2100)
            {
                this.Session.CurrentYear = testYear;
                this.ClosePopup(null);
            }
            else
            {
                await this._messageViewer.DisplayMessage("Error: please enter a valid year!", "Invalid Year", MessageViewerButton.Ok, MessageViewerIcon.Warning);
            }
        }
        #endregion
    }
}
