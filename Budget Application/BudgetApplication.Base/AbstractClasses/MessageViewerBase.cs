using BudgetApplication.Base.Enums;
using BudgetApplication.Base.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;

namespace BudgetApplication.Base.AbstractClasses
{
    public abstract class MessageViewerBase : ObservableObject
    {
        #region Commands
        public ICommand CloseOkCommand { get; protected set; }
        public ICommand CloseCancelCommand { get; protected set; }
        #endregion

        #region Public Properties
        private bool _IsMessageActive;
        public bool IsMessageActive
        {
            get
            {
                return this._IsMessageActive;
            }
            set
            {
                this.Set(ref this._IsMessageActive, value);
            }
        }

        private string _ActiveMessage;
        public string ActiveMessage
        {
            get
            {
                return this._ActiveMessage;
            }
            set
            {
                this.Set(ref this._ActiveMessage, value);
            }
        }
        #endregion

        #region Public Methods
        public abstract Task<MessageViewerEventArgs> DisplayMessage(string message);
        public abstract Task<MessageViewerEventArgs> DisplayMessage(string message, MessageViewerButton button);
        public abstract Task<MessageViewerEventArgs> DisplayMessage(string message, MessageViewerButton button, MessageViewerIcon icon);
        #endregion
    }
}
