using System.ComponentModel.DataAnnotations;
namespace BudgetApplication.Base.Enums
{
    public enum BudgetGraphGrouping
    {
        [Display(Description = "Month")]
        Month,
        [Display(Description = "Category")]
        Category,
        [Display(Description = "Group")]
        Group,
        [Display(Description = "Income vs Expeditures")]
        IsIncome
    }
}