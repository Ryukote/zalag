using Domain.Entities;
using FluentValidation;

namespace Application.Validators
{
    public class PledgeValidator : AbstractValidator<Pledge>
    {
        public PledgeValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("Klijent je obavezan");

            RuleFor(x => x.ItemName)
                .NotEmpty().WithMessage("Naziv predmeta je obavezan")
                .MaximumLength(200).WithMessage("Naziv ne smije biti dulji od 200 znakova");

            RuleFor(x => x.ItemDescription)
                .MaximumLength(1000).WithMessage("Opis ne smije biti dulji od 1000 znakova")
                .When(x => !string.IsNullOrEmpty(x.ItemDescription));

            RuleFor(x => x.EstimatedValue)
                .GreaterThan(0).WithMessage("Procjena vrijednosti mora biti veća od 0");

            RuleFor(x => x.LoanAmount)
                .GreaterThan(0).WithMessage("Iznos zajma mora biti veći od 0")
                .LessThanOrEqualTo(x => x.EstimatedValue)
                .WithMessage("Iznos zajma ne može biti veći od procijenjene vrijednosti");

            RuleFor(x => x.InterestRate)
                .GreaterThanOrEqualTo(0).WithMessage("Kamatna stopa ne može biti negativna")
                .LessThanOrEqualTo(100).WithMessage("Kamatna stopa ne može biti veća od 100%");

            RuleFor(x => x.DurationDays)
                .GreaterThan(0).WithMessage("Trajanje mora biti veće od 0 dana")
                .LessThanOrEqualTo(365).WithMessage("Trajanje ne može biti dulje od 365 dana");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Datum početka je obavezan")
                .Must(BeValidDate).WithMessage("Datum početka ne može biti u budućnosti");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).WithMessage("Datum kraja mora biti nakon datuma početka");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status je obavezan")
                .MaximumLength(20).WithMessage("Status ne smije biti dulji od 20 znakova");

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Težina mora biti veća od 0")
                .When(x => x.Weight.HasValue);

            RuleFor(x => x.Fineness)
                .GreaterThan(0).WithMessage("Finoća mora biti veća od 0")
                .LessThanOrEqualTo(999).WithMessage("Finoća ne može biti veća od 999")
                .When(x => x.Fineness.HasValue);
        }

        private bool BeValidDate(DateTime date)
        {
            return date <= DateTime.UtcNow;
        }
    }
}
