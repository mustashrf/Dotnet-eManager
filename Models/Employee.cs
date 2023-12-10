
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Employee")]
public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeID { get; set; }

    [Required]
    [Display(Name = "Employee Name")]
    [DataType("varchar(150)")]
    public string EmployeeName { get; set; }

    [Required]
    [Display(Name = "Date Of Birth")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly DOB { get; set; }

    [Required]
    [Display(Name = "Hiring Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateOnly HiringDate { get; set; }

    [Required]
    [DataType("decimal(12, 2)")]
    public decimal Salary { get; set; }

    [Required]
    [DataType("varchar(50)")]
    public string Country { get; set; }

    [ForeignKey("Department")]
    [Required]
    public int DepartmentID { get; set; }

    public Department Department { get; set; }

    public string DepartmentName
    {
        get
        {
            return Department?.DepartmentName;
        }
    }

}