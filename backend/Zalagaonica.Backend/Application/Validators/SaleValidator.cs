using Domain.Entities;
using FluentValidation;

namespace Application.Validators
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(x => x.ArticleId)
                .NotEmpty().WithMessage("Artikl je obavezan");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Količina mora biti veća od 0");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Jedinična cijena mora biti veća od 0");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("Ukupan iznos mora biti veći od 0")
                .Equal(x => x.Quantity * x.UnitPrice)
                .WithMessage("Ukupan iznos mora biti jednak količina * jedinična cijena");

            RuleFor(x => x.SaleDate)
                .NotEmpty().WithMessage("Datum prodaje je obavezan")
                .Must(BeValidDate).WithMessage("Datum prodaje ne može biti u budućnosti");

            RuleFor(x => x.PaymentMethod)
                .MaximumLength(50).WithMessage("Način plaćanja ne smije biti dulji od 50 znakova")
                .When(x => !string.IsNullOrEmpty(x.PaymentMethod));

            RuleFor(x => x.InvoiceNumber)
                .MaximumLength(50).WithMessage("Broj računa ne smije biti dulji od 50 znakova")
                .When(x => !string.IsNullOrEmpty(x.InvoiceNumber));

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Napomena ne smije biti dulja od 500 znakova")
                .When(x => !string.IsNullOrEmpty(x.Notes));
        }

        private bool BeValidDate(DateTime date)
        {
            return date <= DateTime.UtcNow;
        }
    }
}
