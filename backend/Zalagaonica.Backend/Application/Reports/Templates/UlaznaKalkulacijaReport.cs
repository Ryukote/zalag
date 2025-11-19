using System;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates
{
    public class UlaznaKalkulacijaReport : IDocument
    {
        private readonly Client _client;
        private readonly Article _article;
        private readonly string _brojDokumenta;
        private readonly DateTime _datum;
        private readonly decimal _nabavna;
        private readonly decimal _prodajna;

        public UlaznaKalkulacijaReport(Client client, Article article, string brojDokumenta, DateTime datum, decimal nabavna, decimal prodajna)
        {
            _client = client;
            _article = article;
            _brojDokumenta = brojDokumenta;
            _datum = datum;
            _nabavna = nabavna;
            _prodajna = prodajna;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);

                // Header
                page.Header().Column(header =>
                {
                    header.Item().Text($"ULAZNA KALKULACIJA: {_brojDokumenta}")
                        .Bold().FontSize(16).AlignCenter();
                    header.Item().Text($"Datum: {_datum:dd.MM.yyyy}").AlignRight().FontSize(10);
                });

                // Content
                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    // Klijent i komitent
                    col.Item().Row(row =>
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
                            c.Item().Text("KOMITENT:").Bold();
                            c.Item().Text("Šut Ivana");
                            c.Item().Text("Zagreb, Balokovićeva 63");
                            c.Item().Text("OIB: 10046718846");
                        });
                    });

                    // Separator
                    col.Item().LineHorizontal(0.8f);

                    // Tablica artikala
                    col.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(2.0f);
                            cols.RelativeColumn(1.0f);
                            cols.RelativeColumn(0.8f);
                            cols.RelativeColumn(1.0f);
                            cols.RelativeColumn(1.0f);
                            cols.RelativeColumn(1.0f);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Naziv artikla").Bold();
                            header.Cell().Text("Oznaka / Šifra").Bold();
                            header.Cell().Text("Količina").Bold();
                            header.Cell().Text("Nabavna cijena (€)").Bold();
                            header.Cell().Text("Marža (€)").Bold();
                            header.Cell().Text("MPC (€)").Bold();
                        });

                        table.Cell().Text(_article.Name);
                        table.Cell().Text(_article.Id.ToString().Substring(0, 6));
                        table.Cell().Text("1");
                        table.Cell().Text($"{_nabavna:F2}");
                        table.Cell().Text($"{_prodajna - _nabavna:F2}");
                        table.Cell().Text($"{_prodajna:F2}");
                    });

                    // Ukupno
                    col.Item().PaddingTop(10).AlignRight().Text($"UKUPNO: {_prodajna:F2} €").Bold();

                    // Napomena
                    col.Item().PaddingTop(15).Column(c =>
                    {
                        c.Item().Text("Porez na dodanu vrijednost").Bold();
                        c.Item().Text($"Osnovica: {_prodajna:F2} € | Iznos: 0,00 €");
                    });

                    // Potpisi
                    col.Item().PaddingTop(25).Row(r =>
                    {
                        r.RelativeItem().Text("Izradio: ____________________");
                        r.RelativeItem().AlignRight().Text("m.p.");
                    });
                });

                // Footer
                page.Footer().AlignCenter().Text($"Automatski generirano {DateTime.Now:dd.MM.yyyy HH:mm}").FontSize(9);
            });
        }
    }
}
