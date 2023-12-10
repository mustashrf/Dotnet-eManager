
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Department")]
public class Department
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DepartmentID { get; set; }

    [Required]
    [DataType("varchar(150)")]
    public string DepartmentName { get; set; }


    [Required]
    [DataType("varchar(10)")]
    public string DepartmentAbbrev { get; set; }

    public ICollection<Employee> Employees { get; set; }

}