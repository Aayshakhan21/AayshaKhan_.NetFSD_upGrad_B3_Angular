using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;

        public MovieService(IMovieRepository repository)
        {
            _repository = repository;
        }

        public List<Movie> GetMovies()
        {
            return _repository.GetAll();
        }

        public Movie GetMovie(int id)
        {
            return _repository.GetById(id);
        }

        public void AddMovie(Movie movie)
        {
            if (string.IsNullOrEmpty(movie.Title))
                throw new Exception("Title is required");

            _repository.Add(movie);
        }

        public void UpdateMovie(Movie movie)
        {
            _repository.Update(movie);
        }

        public void DeleteMovie(int id)
        {
            _repository.Delete(id);
        }
    }
}
