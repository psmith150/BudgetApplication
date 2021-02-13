using System.ComponentModel.DataAnnotations;
namespace BudgetApplication.Base.Enums
{
    public enum PieGraphType
    {
        [Display(Description = "Transactions")]
        Transaction,
        [Display(Description = "Budget")]
        Budget
    }
}