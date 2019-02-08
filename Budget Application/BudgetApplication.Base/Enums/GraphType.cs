using System.ComponentModel.DataAnnotations;
namespace BudgetApplication.Base.Enums
{
    public enum GraphType
    {
        [Display(Name = "Pie Chart")]
        Pie,
        [Display(Name = "Line Chart")]
        Line
    }
}