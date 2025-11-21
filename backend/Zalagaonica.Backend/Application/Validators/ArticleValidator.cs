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
                .MaximumLength(300).WithMessage("Naziv ne smije biti dulji od 300 znakova");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Opis ne smije biti dulji od 1000 znakova")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.PurchasePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Nabavna cijena ne može biti negativna");

            RuleFor(x => x.RetailPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Maloprodajna cijena ne može biti negativna");

            RuleFor(x => x.SalePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Prodajna cijena ne može biti negativna")
                .When(x => x.SalePrice.HasValue);

            RuleFor(x => x.TaxRate)
                .GreaterThanOrEqualTo(0).WithMessage("Porezna stopa ne može biti negativna")
                .LessThanOrEqualTo(100).WithMessage("Porezna stopa ne može biti veća od 100%");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Zaliha ne može biti negativna");

            RuleFor(x => x.UnitOfMeasureCode)
                .NotEmpty().WithMessage("Mjerna jedinica je obavezna")
                .MaximumLength(20).WithMessage("Mjerna jedinica ne smije biti dulja od 20 znakova");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status je obavezan")
                .Must(status => status == "available" || status == "sold")
                .WithMessage("Status mora biti 'available' ili 'sold'");

            RuleFor(x => x.WarehouseType)
                .NotEmpty().WithMessage("Tip skladišta je obavezan")
                .Must(type => type == "main" || type == "pledge")
                .WithMessage("Tip skladišta mora biti 'main' ili 'pledge'");
        }
    }
}
