namespace MinimalAPIsMovies.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Body { get; set; } = null!;
        public int MovieId { get; set; }
    }
}
