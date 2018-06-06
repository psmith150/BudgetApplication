using System.Windows.Input;
using BudgetApplication.Services;
using BudgetApplication.Screens;
using Type = System.Type;

namespace BudgetApplication.Popups
{
    public class HelpViewModel : PopupViewModel
    {
        public HelpViewModel(SessionService session) : base(session)
        {
        }

        public override void Initialize(object param)
        {
            Type paramType = param.GetType();
            if(paramType == typeof(BudgetViewModel))
            {
                this.HelpMessage = budgetHelpMessage;
            }
            else if (paramType == typeof(SpendingViewModel))
            {
                this.HelpMessage = spendingHelpMessage;
            }
            else if (paramType == typeof(ComparisonViewModel))
            {
                this.HelpMessage = comparisonHelpMessage;
            }
            else if (paramType == typeof(TransactionsViewModel))
            {
                this.HelpMessage = transactionsHelpMessage;
            }
            else if (paramType == typeof(PaymentsViewModel))
            {
                this.HelpMessage = paymentsHelpMessage;
            }
            else
            {
                this.HelpMessage = defaultHelpMessage;
            }
        }

        public override void Deinitialize()
        {
        }

        #region Public Properties
        private string _HelpMessage;
        public string HelpMessage
        {
            get
            {
                return this._HelpMessage;
            }
            set
            {
                this.Set(ref this._HelpMessage, value);
            }
        }
        #endregion

        #region Help Messages
        private const string budgetHelpMessage = "This screen is where you can create your budget. Simply enter the budget amount into the box for the appropriate category and month. Use Ctrl+Enter when entering " +
            "a value to apply it to all months of that category.\n" +
            "The totals for each group are shown at the bottom. The Net Budget row will show the over/under of income vs expenditures for that month. The Net Sum row is cumulative across all months. The final " +
            "total of the Net Sum will be the over/under for the entire year. This number should be as close to $0 as possible.\n" +
            "Remember to keep your budget up to date througout the year!";
        private const string spendingHelpMessage = "This screen shows a summary of all the money you have spent. Each transaction updates the approriate category and month. Each month, category, and group is " +
            "summed for reference";
        private const string comparisonHelpMessage = "This screen shows the difference between what you budgeted and what you actually spent. Each box is color coded to show how you did for that category and month. " +
            "Ideally, all these numbers will be $0, indicating that you stuck perfectly to your budget. In reality, keep a close eye on your Net Budget and Net Sum; these are your best indicators of how well you " +
            "are sticking to your budget.\n" +
            "If there are certain categories that you are constantly over or under on, you should take a look at your budgeted values and your actual spending and see if you need to adjust your budget or your " +
            "spending habits.";
        private const string monthDetailsHelpMessage = "This screen shows the progress you've made on spending categories over one month (default is the current month). Each category will have a progress bar " +
            "comparing how much you've spent to how much you've budgeted for the month. The yellow line is today's date. Categories are colored by what percent has been spent compared to the current day of the month; " +
            "red indicates that you are spending faster than you should, and may be at risk of exceeding your budget.";
        private const string transactionsHelpMessage = "This screen is where all your transactions are recorded. Each transaction has a date, a note of what was purchased, the person or business purchased from, " +
            "the amount, an appropriate category for tracking, how it was paid for, and any additional comments. Transactions can be sorted based on any of these criteria.";
        private const string paymentsHelpMessage = "This screen filters all the transactions by payment method. You can choose a payment method as well as a start and end date. Transactions can also be added, " +
            "deleted, and modified from this screen.\n" +
            "Selecting a credit card as the payment method will show additional details to break down a billing cycle. This can be useful to quickly see if your credit card statements match up to your spending.";
        private const string defaultHelpMessage = "Help not defined for this screen. Please check https://github.com/psmith150/BudgetApplication for information";
        #endregion
    }
}