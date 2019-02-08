using System.ComponentModel.DataAnnotations;
namespace BudgetApplication.Base.Enums
{
    public enum TransactionGraphGrouping
    {
        [Display(Name = "Month")]
        Month,
        [Display(Name = "Category")]
        Category,
        [Display(Name = "Group")]
        Group,
        [Display(Name = "Payment Method")]
        PaymentMethod,
        [Display(Name = "Income vs Expeditures")]
        IsIncome
    }
}