using System.ComponentModel.DataAnnotations;
namespace BudgetApplication.Base.Enums
{
    public enum GraphType
    {
        [Display(Description = "Pie Chart")]
        Pie,
        [Display(Description = "Line Chart")]
        Line
    }
}