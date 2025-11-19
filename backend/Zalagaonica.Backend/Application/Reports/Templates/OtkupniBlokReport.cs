using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates
{
    public class OtkupniBlokReport : IDocument
    {
        private readonly Client _client;
        private readonly Article _article;
        private readonly decimal _mpc;
        private readonly string _brojDokumenta;
        private readonly DateTime _datum;

        public OtkupniBlokReport(Client client, Article article, decimal mpc, string brojDokumenta, DateTime datum)
        {
            _client = client;
            _article = article;
            _mpc = mpc;
            _brojDokumenta = brojDokumenta;
            _datum = datum;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);

                page.Header().Text($"OTKUPNI BLOK ZA OTKUP RABLJENOG DOBRA: {_brojDokumenta}")
                    .Bold().FontSize(16).AlignCenter();

                page.Content().Column(col =>
                {
                    col.Item().Text($"Kupac: {_client.Name} ({_client.Address})");
                    col.Item().Text($"Datum: {_datum:dd.MM.yyyy}");
                    col.Item().Text($"Artikl: {_article.Name} ({_article.Description})");
                    col.Item().Text($"Cijena (MPC): {_mpc:F2} €");
                    col.Item().Text("");
                    col.Item().Text("Izjava prodavatelja:")
                        .Bold().FontSize(12);
                    col.Item().Text("Izjavljujem da je predmet isključivo moje vlasništvo te da nisam obveznik PDV-a...");
                    col.Spacing(10);
                });

                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span("Izradio: ").Bold();
                    t.Span("Automatski sustav Zalagaonica Backend");
                });
            });
        }
    }
}
