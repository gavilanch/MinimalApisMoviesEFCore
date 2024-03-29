using Microsoft.IdentityModel.Tokens;
using MinimalAPIsMovies.Utilities;

namespace MinimalAPIsMovies.DTOs
{
    public class PaginationDTO
    {
        public const int pageInitialValue = 1;
        public const int recordsPerPageInitialValue = 10;
        public int Page { get; set; } = 1;
        private int recordsPerPage = 10;
        private readonly int recordsPerPageMax = 50;

        public int RecordsPerPage
        {
            get
            {
                return recordsPerPage;
            }
            set
            {
                if (value > recordsPerPageMax)
                {
                    recordsPerPage = recordsPerPageMax;
                }
                else
                {
                    recordsPerPage = value;
                }
            }
        }

        public static ValueTask<PaginationDTO> BindAsync(HttpContext context)
        {
            // nameof(Page) = "Page"
            var page = context.ExtractValueOrDefault(nameof(Page), pageInitialValue);
            var recordsPerPage = context.ExtractValueOrDefault(nameof(RecordsPerPage), 
                    recordsPerPageInitialValue);

            var response = new PaginationDTO
            {
                Page = page,
                RecordsPerPage = recordsPerPage
            };

            return ValueTask.FromResult(response);
        }
    }
}
