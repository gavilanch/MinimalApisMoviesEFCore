using FluentValidation;
using MinimalAPIsMovies.DTOs;

namespace MinimalAPIsMovies.Validations
{
    public class UserCredentialsDTOValidator: AbstractValidator<UserCredentialsDTO>
    {
        public UserCredentialsDTOValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(ValidationUtilities.NonEmptyMessage)
                .MaximumLength(256).WithMessage(ValidationUtilities.MaximumLengthMessage)
                .EmailAddress().WithMessage(ValidationUtilities.EmailAddressMessage);

            RuleFor(x => x.Password).NotEmpty().WithMessage(ValidationUtilities.NonEmptyMessage);
        }
    }
}
