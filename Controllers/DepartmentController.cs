using System.Reflection;
using Microsoft.AspNetCore.Mvc;

public class DepartmentController : Controller
{

    ApplicationDbContext dbContext = new ApplicationDbContext();

    public IActionResult Index(string SortField, string CurrentSortField, string SortDirection, string SearchByName)
    {
        List<Department> Departments = dbContext.Departments.ToList();

        if(!string.IsNullOrEmpty(SearchByName))
            Departments = Departments.Where(d => d.DepartmentName.Contains(SearchByName, StringComparison.OrdinalIgnoreCase)).ToList();

        Departments = SortDepartments(Departments, SortField, CurrentSortField, SortDirection);

        return View(Departments);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Department department)
    {
        ModelState.Remove("DepartmentID");
        ModelState.Remove("Employees");

        if (ModelState.IsValid)
        {
            dbContext.Add(department);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Update(int ID)
    {
        Department department = dbContext.Departments.Where(d => d.DepartmentID == ID).FirstOrDefault();
        return View("Create", department);
    }

    [HttpPost]
    public IActionResult Update(Department department)
    {
        ModelState.Remove("Employees");

        if (ModelState.IsValid)
        {
            dbContext.Update(department);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Delete(int ID)
    {
        Department department = dbContext.Departments.Where(d => d.DepartmentID == ID).FirstOrDefault();
        if (department != null)
        {
            dbContext.Remove(department);
            dbContext.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    public List<Department> SortDepartments(List<Department> departments, string SortField, string CurrentSortField, string SortDirection)
    {
        if (string.IsNullOrEmpty(SortField))
        {
            ViewBag.SortField = "DepartmentName";
            ViewBag.SortDirection = "ASC";
        }
        else
        {
            if (CurrentSortField == SortField)
                ViewBag.SortDirection = SortDirection == "ASC" ? "DSC" : "ASC";
            else
                ViewBag.SortDirection = SortDirection;

            ViewBag.SortField = SortField;
        }

        PropertyInfo propertyInfo = typeof(Department).GetProperty(ViewBag.SortField);

        if (ViewBag.SortDirection == "ASC")
            departments = departments.OrderBy(d => propertyInfo.GetValue(d)).ToList();
        else
            departments = departments.OrderByDescending(d => propertyInfo.GetValue(d)).ToList();

        return departments;
    }

}