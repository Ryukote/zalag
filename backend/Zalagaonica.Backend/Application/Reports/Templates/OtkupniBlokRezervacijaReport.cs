using System;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates
{
    public class OtkupniBlokRezervacijaReport : IDocument
    {
        private readonly Client _client;
        private readonly Article _article;
        private readonly string _brojDokumenta;
        private readonly DateTime _datum;
        private readonly decimal _kapara;
        private readonly decimal _cijena;

        public OtkupniBlokRezervacijaReport(Client client, Article article, string brojDokumenta, DateTime datum, decimal kapara, decimal cijena)
        {
            _client = client;
            _article = article;
            _brojDokumenta = brojDokumenta;
            _datum = datum;
            _kapara = kapara;
            _cijena = cijena;
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
                    header.Item().Text($"OTKUPNI BLOK - REZERVACIJA {_brojDokumenta}")
                        .Bold().FontSize(16).AlignCenter();
                    header.Item().Text($"Datum: {_datum:dd.MM.yyyy}").FontSize(10).AlignRight();
                });

                // BODY
                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    // Klijent
                    col.Item().Column(c =>
                    {
                        c.Item().Text("KUPAC:").Bold();
                        c.Item().Text($"{_client.Name}");
                        c.Item().Text($"{_client.Address}");
                        if (!string.IsNullOrEmpty(_client.PhoneNumber))
                            c.Item().Text($"Kontakt: {_client.PhoneNumber}");
                    });

                    col.Item().LineHorizontal(0.8f);

                    // Artikl tablica
                    col.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(2f);   // Naziv
                            cols.RelativeColumn(1f);   // Oznaka
                            cols.RelativeColumn(0.8f); // Količina
                            cols.RelativeColumn(1f);   // Cijena
                            cols.RelativeColumn(1f);   // Kapara
                            cols.RelativeColumn(1f);   // Za platiti
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Naziv artikla").Bold();
                            header.Cell().Text("Oznaka / Šifra").Bold();
                            header.Cell().Text("Količina").Bold();
                            header.Cell().Text("Cijena (€)").Bold();
                            header.Cell().Text("Kapara (€)").Bold();
                            header.Cell().Text("Za platiti (€)").Bold();
                        });

                        table.Cell().Text(_article.Name);
                        table.Cell().Text(_article.Id.ToString().Substring(0, 6));
                        table.Cell().Text("1");
                        table.Cell().Text($"{_cijena:F2}");
                        table.Cell().Text($"{_kapara:F2}");
                        table.Cell().Text($"{_cijena - _kapara:F2}");
                    });

                    // Napomena
                    col.Item().PaddingTop(15).Column(c =>
                    {
                        c.Item().Text("NAPOMENA:").Bold();
                        c.Item().Text("Kupac se obvezuje da ostatak iznosa mora biti plaćen u roku od 30 dana. " +
                                      "Ukoliko to ne učini, smatra se da je odustao od kupnje i gubi pravo povrata kapare.");
                    });

                    // Izjava
                    col.Item().PaddingTop(20).Column(c =>
                    {
                        c.Item().Text("IZJAVA PRODAVATELJA:").Bold();
                        c.Item().Text("Izjavljujem da je predmet rezervacije isključivo moje vlasništvo i da nisam obveznik PDV-a. " +
                                      "Navedeni predmet je ostavljen u prodaji do isteka roka rezervacije.");
                    });

                    // Potpisi
                    col.Item().PaddingTop(30).Row(r =>
                    {
                        r.RelativeItem().Text("Izradio: ____________________");
                        r.RelativeItem().AlignRight().Text("Komitent: ____________________");
                    });

                    // Ukupno
                    col.Item().PaddingTop(20).AlignRight().Text($"UKUPNO ZA PLATITI: {_cijena - _kapara:F2} €").Bold().FontSize(13);
                });

                // FOOTER
                page.Footer().AlignCenter().Text($"Automatski generirano {DateTime.Now:dd.MM.yyyy HH:mm}").FontSize(9);
            });
        }
    }
}
