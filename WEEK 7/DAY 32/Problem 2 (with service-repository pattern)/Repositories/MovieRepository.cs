using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        // db is injected in repository  
        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public Movie GetById(int id)
        {
            return _context.Movies.Find(id);
        }

        public void Add(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        public void Update(Movie movie)
        {
            _context.Movies.Update(movie);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
        }
    }
}
