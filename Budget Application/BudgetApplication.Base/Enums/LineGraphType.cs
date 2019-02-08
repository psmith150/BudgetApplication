using System.ComponentModel.DataAnnotations;
namespace BudgetApplication.Base.Enums
{
    public enum LineGraphType
    {
        [Display(Name = "Transactions")]
        Transactions,
        [Display(Name = "Budget")]
        Budget,
        [Display(Name = "Comparison")]
        Comparison
    }
}