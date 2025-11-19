using System;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates
{
    public class ZahtjevZaProcjenuReport : IDocument
    {
        private readonly Client _client;
        private readonly Article _article;
        private readonly string _opis;
        private readonly string _brojDokumenta;
        private readonly DateTime _datum;

        public ZahtjevZaProcjenuReport(Client client, Article article, string opis, string brojDokumenta, DateTime datum)
        {
            _client = client;
            _article = article;
            _opis = opis;
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

                // Gornji naslov i broj dokumenta
                page.Header().Column(header =>
                {
                    header.Item().Text($"ZAHTJEV ZA PROCJENOM PREDMETA: {_brojDokumenta}")
                        .Bold().FontSize(16).AlignCenter();
                    header.Spacing(5);
                });

                // Sadržaj dokumenta
                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    // Kupac i prodavatelj info
                    col.Item().BorderBottom(1).PaddingBottom(5).Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("KUPAC:").Bold();
                            c.Item().Text($"{_client.Name}");
                            c.Item().Text($"{_client.Address}");
                            c.Item().Text($"OIB: {_client?.PhoneNumber ?? "—"}");
                        });
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("PRODAVATELJ:").Bold();
                            c.Item().Text(_article?.Description ?? "Nepoznato");
                        });
                    });

                    // Podaci o dokumentu
                    col.Item().PaddingVertical(5).Column(c =>
                    {
                        c.Item().Text($"Datum: {_datum:dd.MM.yyyy}");
                        c.Item().Text("Skladište: Zalagaonica (ZG)");
                    });

                    // Linija i naslov artikla
                    col.Item().PaddingTop(10).Text("PREDMET PROCJENE:").Bold().FontSize(13);

                    // Tablica artikla
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(1);
                            cols.RelativeColumn(2);
                            cols.RelativeColumn(1);
                            cols.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Naziv artikla").Bold();
                            header.Cell().Text("Oznaka / Šifra").Bold();
                            header.Cell().Text("Jedinica").Bold();
                            header.Cell().Text("Količina").Bold();
                        });

                        table.Cell().Text(_article.Name);
                        table.Cell().Text(_article.Id.ToString().Substring(0, 6));
                        table.Cell().Text("KOM");
                        table.Cell().Text("1");
                    });

                    // Napomena
                    col.Item().PaddingTop(15).Text($"Napomena: {_opis}");

                    // Izjava vlasnika
                    col.Item().PaddingTop(25).Column(c =>
                    {
                        c.Item().Text("IZJAVA VLASNIKA:").Bold();
                        c.Item().Text("Izjavljujem da je predmet koji prodajem isključivo moje vlasništvo, da nisam obveznik PDV-a niti fiskalizacije.");
                        c.Item().Text("Kupac se obvezuje da procijenjeni predmet mora biti preuzet u roku od 30 dana, u suprotnom se gubi pravo na podizanje robe.");
                    });

                    // Potpis i datum
                    col.Item().PaddingTop(20).Row(r =>
                    {
                        r.RelativeItem().Text($"Izradio: ____________________").AlignLeft();
                        r.RelativeItem().Text($"Komitent: ____________________").AlignRight();
                    });
                });

                // Footer
                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span("Stranica generirana automatski – ");
                    t.Span($"{DateTime.Now:dd.MM.yyyy HH:mm}");
                });
            });
        }
    }
}
