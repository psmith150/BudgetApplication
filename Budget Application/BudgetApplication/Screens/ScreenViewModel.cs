using BudgetApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApplication.Screens
{
    public abstract class ScreenViewModel : BaseViewModel
    {
        public ScreenViewModel(SessionService session) : base(session) { }

        public abstract void Initialize();

        public abstract void Deinitialize();
    }
}
