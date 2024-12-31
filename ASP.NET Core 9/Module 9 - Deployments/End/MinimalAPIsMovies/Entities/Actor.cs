namespace MinimalAPIsMovies.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string? Picture { get; set; }
        public List<ActorMovie> ActorsMovies { get; set; } = new List<ActorMovie>();
    }
}
