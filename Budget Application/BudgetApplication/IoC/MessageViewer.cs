using BudgetApplication.Base.Enums;
using BudgetApplication.Base.EventArgs;
using BudgetApplication.Base.AbstractClasses;
using BudgetApplication.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BudgetApplication.IoC
{
    class MessageViewer : MessageViewerBase
    {
        #region Constructors

        public MessageViewer(NavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.CloseOkCommand = new RelayCommand(() => this.MessageClosed_Ok());
            this.CloseCancelCommand = new RelayCommand(() => this.MessageClosed_Cancel());
        }

        #endregion

        #region Private Fields
        private TaskCompletionSource<MessageViewerEventArgs> messageTaskCompletionSource;
        #endregion

        #region Public Properties
        
        #endregion

        #region Private Fields

        private NavigationService NavigationService;

        #endregion

        #region Public Methods

        public override Task<MessageViewerEventArgs> DisplayMessage(string message)
        {
            return this.DisplayMessage(message, MessageViewerButton.Ok);
        }

        public override Task<MessageViewerEventArgs> DisplayMessage(string message, MessageViewerButton button)
        {
            return this.DisplayMessage(message, button, MessageViewerIcon.Information);
        }

        public override Task<MessageViewerEventArgs> DisplayMessage(string message, MessageViewerButton button, MessageViewerIcon icon)
        {
            this.ActiveMessage = message;
            this.IsMessageActive = true;
            this.messageTaskCompletionSource = new TaskCompletionSource<MessageViewerEventArgs>();
            return this.messageTaskCompletionSource.Task;
        }

        #endregion

        #region Private Methods
        private void CloseMessageViewer(MessageViewerEventArgs e)
        {
            this.ActiveMessage = "";
            this.IsMessageActive = false;
            this.messageTaskCompletionSource.SetResult(e);
        }

        private void MessageClosed_Ok()
        {
            this.CloseMessageViewer(new MessageViewerEventArgs(MessageViewerResult.Ok));
        }

        private void MessageClosed_Cancel()
        {
            this.CloseMessageViewer(new MessageViewerEventArgs(MessageViewerResult.Cancel));
        }
        #endregion
    }
}
