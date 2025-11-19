using System;
using System.Collections.Generic;
using System.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates
{
    public class MjesecniIzvjestajReport : IDocument
    {
        private readonly int _mjesec;
        private readonly int _godina;
        private readonly int _brojOtkupa;
        private readonly int _brojProdaja;
        private readonly decimal _ukupnaNabavna;
        private readonly decimal _ukupnaProdajna;
        private readonly Dictionary<string, (int artikala, decimal vrijednost)> _poSkladistima;

        public MjesecniIzvjestajReport(int mjesec, int godina, int brojOtkupa, int brojProdaja,
            decimal ukupnaNabavna, decimal ukupnaProdajna,
            Dictionary<string, (int artikala, decimal vrijednost)> poSkladistima)
        {
            _mjesec = mjesec;
            _godina = godina;
            _brojOtkupa = brojOtkupa;
            _brojProdaja = brojProdaja;
            _ukupnaNabavna = ukupnaNabavna;
            _ukupnaProdajna = ukupnaProdajna;
            _poSkladistima = poSkladistima;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            var zarada = _ukupnaProdajna - _ukupnaNabavna;

            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);

                // HEADER
                page.Header().Column(header =>
                {
                    header.Item().Text("MJesečni izvještaj o poslovanju")
                        .Bold().FontSize(18).AlignCenter();
                    header.Item().Text($"Za razdoblje: {_mjesec:D2}/{_godina}")
                        .FontSize(12).AlignCenter();
                    header.Item().LineHorizontal(0.8f);
                });

                // BODY
                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    // Opći podaci
                    col.Item().Text("SAŽETAK:").Bold().FontSize(13);
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(2);
                            c.RelativeColumn(1);
                        });

                        table.Cell().Text("Broj otkupa").Bold();
                        table.Cell().Text($"{_brojOtkupa}");

                        table.Cell().Text("Broj prodaja").Bold();
                        table.Cell().Text($"{_brojProdaja}");

                        table.Cell().Text("Ukupna nabavna vrijednost (€)").Bold();
                        table.Cell().Text($"{_ukupnaNabavna:F2}");

                        table.Cell().Text("Ukupna prodajna vrijednost (€)").Bold();
                        table.Cell().Text($"{_ukupnaProdajna:F2}");

                        table.Cell().Text("Ukupna zarada (€)").Bold();
                        table.Cell().Text($"{zarada:F2}");
                    });

                    col.Item().PaddingTop(15).LineHorizontal(0.8f);

                    // Podaci po skladištima
                    col.Item().Text("PREGLED PO SKLADIŠTIMA:").Bold().FontSize(13);
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(2);
                            c.RelativeColumn(1);
                            c.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Skladište").Bold();
                            header.Cell().Text("Broj artikala").Bold();
                            header.Cell().Text("Ukupna vrijednost (€)").Bold();
                        });

                        foreach (var s in _poSkladistima)
                        {
                            table.Cell().Text(s.Key);
                            table.Cell().Text($"{s.Value.artikala}");
                            table.Cell().Text($"{s.Value.vrijednost:F2}");
                        }
                    });

                    // Zaključak
                    col.Item().PaddingTop(20).Text("Zaključak:").Bold();
                    col.Item().Text("Poslovanje u ovom razdoblju pokazuje stabilan promet i održivu maržu.");

                    // Potpisi
                    col.Item().PaddingTop(30).Row(r =>
                    {
                        r.RelativeItem().Text("Izradio: ____________________");
                        r.RelativeItem().AlignRight().Text("Odobrio: ____________________");
                    });
                });

                // FOOTER
                page.Footer().AlignCenter().Text($"Automatski generirano {DateTime.Now:dd.MM.yyyy HH:mm}").FontSize(9);
            });
        }
    }
}
