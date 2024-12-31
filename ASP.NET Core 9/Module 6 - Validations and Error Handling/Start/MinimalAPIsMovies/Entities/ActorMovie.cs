namespace MinimalAPIsMovies.Entities
{
    public class ActorMovie
    {
        public int ActorId { get; set; }
        public int MovieId { get; set; }
        public Actor Actor { get; set; } = null!;
        public Movie Movie { get; set; } = null!;
        public int Order { get; set; }
        public string Character { get; set; } = null!;
    }
}
