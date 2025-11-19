using System;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates
{
    public class RacunOIsplatiReport : IDocument
    {
        private readonly Client _client;
        private readonly Article _article;
        private readonly string _brojDokumenta;
        private readonly DateTime _datum;
        private readonly decimal _iznos;

        public RacunOIsplatiReport(Client client, Article article, string brojDokumenta, DateTime datum, decimal iznos)
        {
            _client = client;
            _article = article;
            _brojDokumenta = brojDokumenta;
            _datum = datum;
            _iznos = iznos;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);

                // HEADER
                page.Header().Column(header =>
                {
                    header.Item().Text($"RAČUN O ISPLATI: {_brojDokumenta}")
                        .Bold().FontSize(16).AlignCenter();
                    header.Item().Text($"Datum izdavanja: {_datum:dd.MM.yyyy}")
                        .FontSize(10).AlignRight();
                });

                // BODY
                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    // Podaci o kupcu
                    col.Item().Column(c =>
                    {
                        c.Item().Text("KUPAC / ISPLATITELJ:").Bold();
                        c.Item().Text("PAWN SHOPS d.o.o.");
                        c.Item().Text("Logorište 11a, Karlovac");
                        c.Item().Text("OIB: 10046718846");
                    });

                    // Podaci o primatelju (klijent)
                    col.Item().PaddingTop(10).Column(c =>
                    {
                        c.Item().Text("PRIMATELJ ISPLATE:").Bold();
                        c.Item().Text($"{_client.Name}");
                        c.Item().Text($"{_client.Address}");
                        if (!string.IsNullOrEmpty(_client.PhoneNumber))
                            c.Item().Text($"Kontakt: {_client.PhoneNumber}");
                    });

                    // Separator
                    col.Item().LineHorizontal(0.8f);

                    // Podaci o artiklu
                    col.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(2f);
                            cols.RelativeColumn(1f);
                            cols.RelativeColumn(1f);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Naziv artikla").Bold();
                            header.Cell().Text("Šifra").Bold();
                            header.Cell().Text("Iznos (€)").Bold();
                        });

                        table.Cell().Text(_article.Name);
                        table.Cell().Text(_article.Id.ToString().Substring(0, 6));
                        table.Cell().Text($"{_iznos:F2}");
                    });

                    // Ukupni iznos
                    col.Item().PaddingTop(10).AlignRight()
                        .Text($"UKUPNO ISPLAĆENO: {_iznos:F2} €").Bold().FontSize(13);

                    // Napomena
                    col.Item().PaddingTop(15).Column(c =>
                    {
                        c.Item().Text("NAPOMENA:").Bold();
                        c.Item().Text("Isplata izvršena u gotovini. Ovaj dokument služi kao potvrda o preuzimanju novca i prodaji navedenog predmeta.");
                    });

                    // Izjava prodavatelja
                    col.Item().PaddingTop(20).Column(c =>
                    {
                        c.Item().Text("IZJAVA PRODAVATELJA:").Bold();
                        c.Item().Text("Izjavljujem da sam navedeni iznos primio u gotovini i da sam time u cijelosti namirio svoje obveze prema kupcu.");
                    });

                    // Potpisi
                    col.Item().PaddingTop(30).Row(r =>
                    {
                        r.RelativeItem().Text("Izradio: ____________________");
                        r.RelativeItem().AlignRight().Text("Prodavatelj: ____________________");
                    });
                });

                // FOOTER
                page.Footer().AlignCenter()
                    .Text($"Automatski generirano {DateTime.Now:dd.MM.yyyy HH:mm}")
                    .FontSize(9);
            });
        }
    }
}
