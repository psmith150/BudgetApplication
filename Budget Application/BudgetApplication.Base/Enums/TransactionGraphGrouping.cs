using System.ComponentModel.DataAnnotations;
namespace BudgetApplication.Base.Enums
{
    public enum TransactionGraphGrouping
    {
        [Display(Description = "Month")]
        Month,
        [Display(Description = "Category")]
        Category,
        [Display(Description = "Group")]
        Group,
        [Display(Description = "Payment Method")]
        PaymentMethod,
        [Display(Description = "Income/Expeditures")]
        IsIncome,
        [Display(Description = "Payee")]
        Payee,
        [Display(Description = "Item")]
        Item
    }
}