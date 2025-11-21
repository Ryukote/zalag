using Domain.Entities;
using FluentValidation;

namespace Application.Validators
{
    public class ArticleValidator : AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Naziv artikla je obavezan")
                .MaximumLength(200).WithMessage("Naziv ne smije biti dulji od 200 znakova");

            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage("Šifra ne smije biti dulja od 50 znakova")
                .When(x => !string.IsNullOrEmpty(x.Code));

            RuleFor(x => x.Barcode)
                .MaximumLength(50).WithMessage("Barkod ne smije biti dulji od 50 znakova")
                .When(x => !string.IsNullOrEmpty(x.Barcode));

            RuleFor(x => x.PurchasePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Nabavna cijena ne može biti negativna");

            RuleFor(x => x.SellingPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Prodajna cijena ne može biti negativna");

            RuleFor(x => x.MinimumStock)
                .GreaterThanOrEqualTo(0).WithMessage("Minimalna zaliha ne može biti negativna");

            RuleFor(x => x.CurrentStock)
                .GreaterThanOrEqualTo(0).WithMessage("Trenutna zaliha ne može biti negativna");

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Težina mora biti veća od 0")
                .When(x => x.Weight.HasValue);

            RuleFor(x => x.Fineness)
                .GreaterThan(0).WithMessage("Finoća mora biti veća od 0")
                .LessThanOrEqualTo(999).WithMessage("Finoća ne može biti veća od 999")
                .When(x => x.Fineness.HasValue);
        }
    }
}
