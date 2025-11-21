using Domain.Entities;
using FluentValidation;

namespace Application.Validators
{
    public class DailyClosingValidator : AbstractValidator<DailyClosing>
    {
        public DailyClosingValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Datum je obavezan")
                .Must(BeValidDate).WithMessage("Datum ne može biti u budućnosti");

            RuleFor(x => x.CashierName)
                .NotEmpty().WithMessage("Ime blagajnika je obavezno")
                .MaximumLength(100).WithMessage("Ime blagajnika ne smije biti dulje od 100 znakova");

            RuleFor(x => x.StartingCash)
                .GreaterThanOrEqualTo(0).WithMessage("Početno stanje ne može biti negativno");

            RuleFor(x => x.TotalSales)
                .GreaterThanOrEqualTo(0).WithMessage("Ukupna prodaja ne može biti negativna");

            RuleFor(x => x.TotalExpenses)
                .GreaterThanOrEqualTo(0).WithMessage("Ukupni rashodi ne mogu biti negativni");

            RuleFor(x => x.CashInRegister)
                .GreaterThanOrEqualTo(0).WithMessage("Stanje u blagajni ne može biti negativno");

            RuleFor(x => x)
                .Must(HaveBalancedCash)
                .WithMessage("Razlika između očekivanog i stvarnog stanja je prevelika (> 10 €)")
                .When(x => x.IsClosed);

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Napomena ne smije biti dulja od 1000 znakova")
                .When(x => !string.IsNullOrEmpty(x.Notes));
        }

        private bool BeValidDate(DateTime date)
        {
            return date.Date <= DateTime.UtcNow.Date;
        }

        private bool HaveBalancedCash(DailyClosing closing)
        {
            var expectedCash = closing.StartingCash + closing.TotalSales - closing.TotalExpenses;
            var difference = Math.Abs(closing.CashInRegister - expectedCash);
            return difference <= 10m; // Allow max 10 EUR difference
        }
    }
}
