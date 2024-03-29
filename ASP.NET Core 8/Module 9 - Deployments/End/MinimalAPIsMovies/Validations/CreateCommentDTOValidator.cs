using FluentValidation;
using MinimalAPIsMovies.DTOs;

namespace MinimalAPIsMovies.Validations
{
    public class CreateCommentDTOValidator: AbstractValidator<CreateCommentDTO>
    {
        public CreateCommentDTOValidator()
        {
            RuleFor(x => x.Body).NotEmpty().WithMessage(ValidationUtilities.NonEmptyMessage);
        }
    }
}
