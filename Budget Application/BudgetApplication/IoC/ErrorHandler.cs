using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetApplication.Base.Interfaces;
using BudgetApplication.Services;
using BudgetApplication.Popups;

namespace BudgetApplication.IoC
{
    public class ErrorHandler : IErrorHandler
    {

        #region Constructors

        public ErrorHandler(NavigationService navigationService)
        {
            this.NavigationService = navigationService;
        }

        #endregion

        #region Private Fields

        private NavigationService NavigationService;

        #endregion

        #region Public Methods

        public async Task DisplayError(string message, Action callback = null)
        {
            var result = await this.NavigationService.OpenPopup<ErrorViewModel>(message).ConfigureAwait(false);
            callback?.Invoke();
        }

        public async Task DisplayError(Exception e, Action callback = null)
        {
            await this.DisplayError(e.Message, callback).ConfigureAwait(false);
        }

        #endregion

    }
}
