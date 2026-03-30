using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {

        List<Product> products = new List<Product>
{
    new Product { ProductId = 1, ProductName = "Laptop", Category = "Electronics", Price = 60000, Stock = 10 },
    new Product { ProductId = 2, ProductName = "Mobile", Category = "Electronics", Price = 25000, Stock = 25 },
    new Product { ProductId = 3, ProductName = "Headphones", Category = "Accessories", Price = 2000, Stock = 50 },
    new Product { ProductId = 4, ProductName = "Keyboard", Category = "Accessories", Price = 1500, Stock = 30 },
    new Product { ProductId = 5, ProductName = "Chair", Category = "Furniture", Price = 5000, Stock = 15 }
};
        public IActionResult Index()
        {

            return View(products);
        }
        public IActionResult Details(int id)
        {
            var product = products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}
