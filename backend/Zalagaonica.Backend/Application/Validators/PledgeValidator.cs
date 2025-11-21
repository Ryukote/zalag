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

            RuleFor(x => x.ClientName)
                .NotEmpty().WithMessage("Ime klijenta je obavezno")
                .MaximumLength(200).WithMessage("Ime klijenta ne smije biti dulje od 200 znakova");

            RuleFor(x => x.ItemName)
                .NotEmpty().WithMessage("Naziv predmeta je obavezan")
                .MaximumLength(300).WithMessage("Naziv ne smije biti dulji od 300 znakova");

            RuleFor(x => x.ItemDescription)
                .NotEmpty().WithMessage("Opis predmeta je obavezan")
                .MaximumLength(1000).WithMessage("Opis ne smije biti dulji od 1000 znakova");

            RuleFor(x => x.EstimatedValue)
                .GreaterThan(0).WithMessage("Procjena vrijednosti mora biti veća od 0");

            RuleFor(x => x.LoanAmount)
                .GreaterThan(0).WithMessage("Iznos zajma mora biti veći od 0")
                .LessThanOrEqualTo(x => x.EstimatedValue)
                .WithMessage("Iznos zajma ne može biti veći od procijenjene vrijednosti");

            RuleFor(x => x.ReturnAmount)
                .GreaterThanOrEqualTo(x => x.LoanAmount)
                .WithMessage("Iznos povrata mora biti veći ili jednak iznosu zajma");

            RuleFor(x => x.Period)
                .GreaterThan(0).WithMessage("Trajanje mora biti veće od 0 dana")
                .LessThanOrEqualTo(365).WithMessage("Trajanje ne može biti dulje od 365 dana");

            RuleFor(x => x.PledgeDate)
                .NotEmpty().WithMessage("Datum zaloga je obavezan")
                .Must(BeValidDate).WithMessage("Datum zaloga ne može biti u budućnosti");

            RuleFor(x => x.RedeemDeadline)
                .GreaterThan(x => x.PledgeDate).WithMessage("Datum isteka mora biti nakon datuma zaloga");
        }

        private bool BeValidDate(DateTime date)
        {
            return date <= DateTime.UtcNow;
        }
    }
}
