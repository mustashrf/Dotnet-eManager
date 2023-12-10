using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class EmployeeController : Controller
{

    ApplicationDbContext dbContext = new ApplicationDbContext();

    public IActionResult Index(string SortField, string CurrentSortField, string SortDirection, string SearchByName)
    {
        List<Employee> employees = dbContext.Employees.Include(e => e.Department).ToList();

        if (!string.IsNullOrEmpty(SearchByName))
        {
            employees = employees.Where(e => e.EmployeeName.Contains(SearchByName, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        employees = SortEmployees(employees, SortField, CurrentSortField, SortDirection);

        return View(employees);
    }

    public IActionResult Create()
    {
        ViewBag.Departments = dbContext.Departments.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        ViewBag.Departments = dbContext.Departments.ToList();

        ModelState.Remove("Department");
        ModelState.Remove("DepartmentName");
        ModelState.Remove("EmployeeID");
    
        if (ModelState.IsValid)
        {
            dbContext.Employees.Add(employee);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        return View();
    }

    public IActionResult Update(int ID)
    {
        Employee data = dbContext.Employees.Where(emp => emp.EmployeeID == ID).FirstOrDefault();
        ViewBag.Departments = dbContext.Departments.ToList();
        return View("Create", data);
    }

    [HttpPost]
    public IActionResult Update(Employee employee)
    {
        ViewBag.Departments = dbContext.Departments.ToList();

        ModelState.Remove("Department");
        ModelState.Remove("DepartmentName");
    
        if (ModelState.IsValid)
        {
            dbContext.Employees.Update(employee);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        return View("Create", employee);
    }

    public IActionResult Delete(int ID)
    {
        Employee data = dbContext.Employees.Where(emp => emp.EmployeeID == ID).FirstOrDefault();
        if (data != null)
        {
            dbContext.Employees.Remove(data);
            dbContext.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    private List<Employee> SortEmployees(List<Employee> employees, string SortField, string CurrentSortField, string SortDirection)
    {
        if (string.IsNullOrEmpty(SortField))
        {
            ViewBag.SortField = "EmployeeName";
            ViewBag.SortDirection = "ASC";
        }
        else 
        {
            if (CurrentSortField == SortField)
                ViewBag.SortDirection = SortDirection == "ASC" ? "DSC" : "ASC";
            else
                // set to default SortDirection
                ViewBag.SortDirection = "ASC";

            ViewBag.SortField = SortField;
        }

        // Get the selected SortField property name
        var propertyInfo = typeof(Employee).GetProperty(ViewBag.SortField);

        // Sort the list according to SortDirection
        if (ViewBag.SortDirection == "ASC")
            employees = employees.OrderBy(e => propertyInfo.GetValue(e, null)).ToList();
        else
            employees = employees.OrderByDescending(e => propertyInfo.GetValue(e, null)).ToList();

        return employees;

    }

}