using MinimalAPIsMovies.Utilities;

namespace MinimalAPIsMovies.DTOs
{
    public class MoviesFilterDTO
    {
        public int Page { get; set; }
        public int RecordsPerPage { get; set; }
        public PaginationDTO PaginationDTO
        {
            get
            {
                return new PaginationDTO { Page = Page, RecordsPerPage = RecordsPerPage };
            }
        }
        public string? Title { get; set; }
        public int GenreId { get; set; }
        public bool InTheaters { get; set; }
        public bool FutureReleases { get; set; }
        public string? OrderByField { get; set; }
        public bool OrderByAscending { get; set; } = true;

        public static ValueTask<MoviesFilterDTO> BindAsync(HttpContext context)
        {
            var page = context.ExtractValueOrDefault(nameof(Page), 
                PaginationDTO.pageInitialValue);
            var recordsPerPage = context.ExtractValueOrDefault(nameof(RecordsPerPage), 
                PaginationDTO.recordsPerPageInitialValue);

            var title = context.ExtractValueOrDefault(nameof(Title), string.Empty);
            var genreId = context.ExtractValueOrDefault(nameof(GenreId), 0);
            var inTheaters = context.ExtractValueOrDefault(nameof(InTheaters), false);
            var futureReleases = context.ExtractValueOrDefault(nameof(FutureReleases), false);
            var orderByField = context.ExtractValueOrDefault(nameof(OrderByField), string.Empty);
            var orderByAscending = context.ExtractValueOrDefault(nameof(OrderByAscending), true);

            var response = new MoviesFilterDTO
            {
                Page = page,
                RecordsPerPage = recordsPerPage,
                Title = title,
                GenreId = genreId,
                InTheaters = inTheaters,
                FutureReleases = futureReleases,
                OrderByField = orderByField,
                OrderByAscending = orderByAscending
            };

            return ValueTask.FromResult(response);

        }
    }
}
