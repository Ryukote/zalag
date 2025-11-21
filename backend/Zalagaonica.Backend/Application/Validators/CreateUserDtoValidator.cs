using Application.Services;
using FluentValidation;

namespace Application.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Korisničko ime je obavezno")
                .MinimumLength(3).WithMessage("Korisničko ime mora imati najmanje 3 znaka")
                .MaximumLength(50).WithMessage("Korisničko ime ne smije biti dulje od 50 znakova")
                .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Korisničko ime može sadržavati samo slova, brojeve i podvlaku");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email je obavezan")
                .EmailAddress().WithMessage("Neispravan format email adrese")
                .MaximumLength(100).WithMessage("Email ne smije biti dulji od 100 znakova");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Lozinka je obavezna")
                .MinimumLength(6).WithMessage("Lozinka mora imati najmanje 6 znakova")
                .MaximumLength(100).WithMessage("Lozinka ne smije biti dulja od 100 znakova")
                .Matches(@"[A-Z]").WithMessage("Lozinka mora sadržavati barem jedno veliko slovo")
                .Matches(@"[a-z]").WithMessage("Lozinka mora sadržavati barem jedno malo slovo")
                .Matches(@"[0-9]").WithMessage("Lozinka mora sadržavati barem jedan broj");

            RuleFor(x => x.RoleIds)
                .NotEmpty().WithMessage("Morate dodijeliti barem jednu ulogu korisniku");
        }
    }
}
