using System.ComponentModel.DataAnnotations;

namespace MinimalAPIsMovies.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<GenreMovie> GenresMovies { get; set; } = new List<GenreMovie>();
    }
}
