using Domain.Entities;
using FluentValidation;

namespace Application.Validators
{
    public class ClientValidator : AbstractValidator<Client>
    {
        public ClientValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ime klijenta je obavezno")
                .MaximumLength(200).WithMessage("Ime ne smije biti duže od 200 znakova");

            RuleFor(x => x.IdCardNumber)
                .NotEmpty().WithMessage("Broj osobne / OIB je obavezan")
                .MaximumLength(50).WithMessage("Broj osobne / OIB ne smije biti dulji od 50 znakova")
                .Must(oib => oib.Length == 11 && oib.All(char.IsDigit) || oib.Length <= 50)
                .WithMessage("OIB mora imati točno 11 brojčanih znakova ili unesite broj osobne");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adresa je obavezna")
                .MaximumLength(200).WithMessage("Adresa ne smije biti duža od 200 znakova");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Grad je obavezan")
                .MaximumLength(100).WithMessage("Grad ne smije biti dulji od 100 znakova");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Neispravan format email adrese")
                .MaximumLength(255).WithMessage("Email ne smije biti dulji od 255 znakova")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Iban)
                .MaximumLength(50).WithMessage("IBAN ne smije biti dulji od 50 znakova")
                .When(x => !string.IsNullOrEmpty(x.Iban));

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Tip klijenta je obavezan")
                .Must(type => type == "individual" || type == "legal")
                .WithMessage("Tip mora biti 'individual' ili 'legal'");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status je obavezan")
                .Must(status => status == "active" || status == "inactive")
                .WithMessage("Status mora biti 'active' ili 'inactive'");
        }
    }
}
