using System;
using System.Collections.Generic;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates
{
    public class IzvjesceOProdajiArtiklaReport : IDocument
    {
        private readonly string _naziv;
        private readonly string _oznaka;
        private readonly DateTime _datumOtkupa;
        private readonly string _skladiste;
        private readonly decimal _nabavna;
        private readonly decimal? _prodajna;
        private readonly decimal? _zarada;
        private readonly List<(string datum, string opis, string tip)> _dogadjaji;

        public IzvjesceOProdajiArtiklaReport(
            string naziv,
            string oznaka,
            DateTime datumOtkupa,
            string skladiste,
            decimal nabavna,
            decimal? prodajna,
            decimal? zarada,
            List<(string datum, string opis, string tip)> dogadjaji)
        {
            _naziv = naziv;
            _oznaka = oznaka;
            _datumOtkupa = datumOtkupa;
            _skladiste = skladiste;
            _nabavna = nabavna;
            _prodajna = prodajna;
            _zarada = zarada;
            _dogadjaji = dogadjaji;
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
                    header.Item().Text("IZVJEŠĆE O PRODAJI ARTIKLA")
                        .Bold().FontSize(16).AlignCenter();
                    header.Item().Text($"Datum generiranja: {DateTime.Now:dd.MM.yyyy HH:mm}")
                        .AlignRight().FontSize(10);
                });

                // BODY
                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    col.Item().Text("PODACI O ARTIKLU:").Bold().FontSize(13);
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(2);
                            c.RelativeColumn(3);
                        });

                        table.Cell().Text("Naziv").Bold();
                        table.Cell().Text(_naziv);

                        table.Cell().Text("Oznaka / Šifra").Bold();
                        table.Cell().Text(_oznaka);

                        table.Cell().Text("Datum otkupa").Bold();
                        table.Cell().Text($"{_datumOtkupa:dd.MM.yyyy}");

                        table.Cell().Text("Trenutno skladište").Bold();
                        table.Cell().Text(_skladiste);

                        table.Cell().Text("Nabavna cijena (€)").Bold();
                        table.Cell().Text($"{_nabavna:F2}");

                        table.Cell().Text("Prodajna cijena (€)").Bold();
                        table.Cell().Text(_prodajna.HasValue ? $"{_prodajna:F2}" : "—");

                        table.Cell().Text("Zarada (€)").Bold();
                        table.Cell().Text(_zarada.HasValue ? $"{_zarada:F2}" : "—");
                    });

                    col.Item().PaddingTop(15).LineHorizontal(0.8f);
                    col.Item().Text("POVIJEST DOGAĐAJA:").Bold().FontSize(13);

                    // Tablica događaja
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(1.5f); // Datum
                            c.RelativeColumn(4);   // Opis
                            c.RelativeColumn(1.5f); // Tip
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Datum").Bold();
                            header.Cell().Text("Opis događaja").Bold();
                            header.Cell().Text("Vrsta").Bold();
                        });

                        foreach (var d in _dogadjaji)
                        {
                            table.Cell().Text(d.datum);
                            table.Cell().Text(d.opis);
                            table.Cell().Text(d.tip);
                        }
                    });

                    // Zaključak
                    col.Item().PaddingTop(20).Column(c =>
                    {
                        c.Item().Text("ZAKLJUČAK:").Bold();
                        c.Item().Text(_prodajna.HasValue
                            ? $"Artikl je prodan po cijeni od {_prodajna:F2} € uz zaradu od {_zarada:F2} €."
                            : "Artikl je još uvijek aktivan i nije prodan.");
                    });

                    // Potpisi
                    col.Item().PaddingTop(30).Row(r =>
                    {
                        r.RelativeItem().Text("Izradio: ____________________");
                        r.RelativeItem().AlignRight().Text("Odobrio: ____________________");
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
