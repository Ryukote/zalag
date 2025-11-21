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

            RuleFor(x => x.SalePrice)
                .GreaterThan(0).WithMessage("Prodajna cijena mora biti veća od 0");

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("Datum prodaje je obavezan")
                .Must(BeValidDate).WithMessage("Datum prodaje ne može biti u budućnosti");
        }

        private bool BeValidDate(DateTime date)
        {
            return date <= DateTime.UtcNow;
        }
    }
}
