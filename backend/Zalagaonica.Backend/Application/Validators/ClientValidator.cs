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

            RuleFor(x => x.Oib)
                .NotEmpty().WithMessage("OIB je obavezan")
                .Length(11).WithMessage("OIB mora imati točno 11 znakova")
                .Matches(@"^\d{11}$").WithMessage("OIB mora sadržavati samo brojeve");

            RuleFor(x => x.Address)
                .MaximumLength(300).WithMessage("Adresa ne smije biti duža od 300 znakova")
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(x => x.City)
                .MaximumLength(100).WithMessage("Grad ne smije biti dulji od 100 znakova")
                .When(x => !string.IsNullOrEmpty(x.City));

            RuleFor(x => x.PostalCode)
                .MaximumLength(10).WithMessage("Poštanski broj ne smije biti dulji od 10 znakova")
                .When(x => !string.IsNullOrEmpty(x.PostalCode));

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("Telefon ne smije biti dulji od 20 znakova")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Neispravan format email adrese")
                .MaximumLength(100).WithMessage("Email ne smije biti dulji od 100 znakova")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.IdCardNumber)
                .MaximumLength(20).WithMessage("Broj osobne ne smije biti dulji od 20 znakova")
                .When(x => !string.IsNullOrEmpty(x.IdCardNumber));
        }
    }
}
