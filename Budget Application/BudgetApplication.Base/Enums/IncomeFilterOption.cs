using System.ComponentModel.DataAnnotations;
namespace BudgetApplication.Base.Enums
{
    public enum IncomeFilterOption
    {
        [Display(Description = "Both")]
        Both,
        [Display(Description = "Income")]
        Income,
        [Display(Description = "Expenditures")]
        Expenditures
    }
}