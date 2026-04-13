using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EmpController : Controller
    {
        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>
        {
            new Employee { Empno = 101, Ename = "Amit", Job = "Manager", Salary = 50000, Deptno = 10 },
            new Employee { Empno = 102, Ename = "Neha", Job = "Developer", Salary = 40000, Deptno = 20 },
            new Employee { Empno = 103, Ename = "Raj", Job = "Tester", Salary = 35000, Deptno = 20 },
            new Employee { Empno = 104, Ename = "Sneha", Job = "HR", Salary = 30000, Deptno = 30 },
            new Employee { Empno = 105, Ename = "Karan", Job = "Analyst", Salary = 45000, Deptno = 10 }
        };
            return View(employees);
        }
    }
}
