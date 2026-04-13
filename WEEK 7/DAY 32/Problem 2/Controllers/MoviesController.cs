using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{

    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var movies = _context.Movies.ToList();
            return View(movies);
        }

        //get

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // post 

        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Movies.Add(movie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _context.Movies.Find(id);
            return View(movie);
        }

        [HttpPost]
        public IActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Movies.Update(movie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }


        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.Find(id);
            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _context.Movies.Find(id);
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
