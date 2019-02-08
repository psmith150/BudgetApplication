using System.ComponentModel.DataAnnotations;
namespace BudgetApplication.Base.Enums
{
    public enum PieGraphType
    {
        [Display(Name = "Transactions")]
        Transaction,
        [Display(Name = "Budget")]
        Budget
    }
}