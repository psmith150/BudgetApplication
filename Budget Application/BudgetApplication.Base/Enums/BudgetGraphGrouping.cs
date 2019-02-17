using System.ComponentModel.DataAnnotations;
namespace BudgetApplication.Base.Enums
{
    public enum BudgetGraphGrouping
    {
        [Display(Name = "Month")]
        Month,
        [Display(Name = "Category")]
        Category,
        [Display(Name = "Group")]
        Group,
        [Display(Name = "Income vs Expeditures")]
        IsIncome
    }
}