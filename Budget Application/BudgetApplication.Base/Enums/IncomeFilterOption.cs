using System.ComponentModel.DataAnnotations;
namespace BudgetApplication.Base.Enums
{
    public enum IncomeFilterOption
    {
        [Display(Name = "Both")]
        Both,
        [Display(Name = "Income")]
        Income,
        [Display(Name = "Expenditures")]
        Expenditures
    }
}