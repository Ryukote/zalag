using Application.DTOs.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates
{
    public class ReservationReceiptReport : IDocument
    {
        private readonly ReservationReceiptDto _data;

        public ReservationReceiptReport(ReservationReceiptDto data)
        {
            _data = data;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);

                page.Header().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        // Company info
                        row.RelativeItem().Border(1).Padding(8).Column(innerCol =>
                        {
                            innerCol.Item().Text("PAWN SHOPS d.o.o.").Bold().FontSize(11);
                            innerCol.Item().Text("P.J. Horvačanska cesta 25, Zagreb").FontSize(9);
                            innerCol.Item().Text("Logorište 11a, 47000 Karlovac").FontSize(9);
                            innerCol.Item().Text("OIB: 51659874442").FontSize(9);
                            innerCol.Item().Text("Tel: 092 500 8000").FontSize(9);
                        });

                        row.RelativeItem().PaddingLeft(10);

                        // Client info
                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("KUPAC:").Bold().FontSize(10);
                            innerCol.Item().Text(_data.Client.Name).FontSize(9);
                            if (!string.IsNullOrEmpty(_data.Client.Address))
                            {
                                innerCol.Item().Text(_data.Client.Address).FontSize(9);
                            }
                            if (!string.IsNullOrEmpty(_data.Client.City))
                            {
                                innerCol.Item().Text(_data.Client.City).FontSize(9);
                            }
                        });
                    });

                    col.Item().PaddingTop(10).AlignRight().Column(innerCol =>
                    {
                        innerCol.Item().Text($"Broj rezervacije: {_data.DocumentNumber}").FontSize(9);
                        innerCol.Item().Text($"Datum: {_data.DocumentDate:dd.MM.yyyy}").FontSize(9);
                        innerCol.Item().Text($"Rezervacija do: {_data.ReservationUntil:dd.MM.yyyy}").FontSize(9);
                    });

                    col.Item().PaddingTop(20).Text("POTVRDA O REZERVACIJI")
                        .Bold().FontSize(16).AlignCenter();
                });

                page.Content().PaddingTop(20).Column(col =>
                {
                    // Items table
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);  // #
                            columns.RelativeColumn(3);   // Naziv
                            columns.RelativeColumn(2);   // Opis
                            columns.ConstantColumn(60);  // Cijena
                            columns.ConstantColumn(50);  // Količina
                            columns.ConstantColumn(80);  // Ukupno
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(4).Text("#").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(4).Text("Naziv").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(4).Text("Opis").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(4).Text("Cijena").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(4).Text("Količina").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(4).Text("Ukupno").Bold().FontSize(9);
                        });

                        // Items
                        int index = 1;
                        foreach (var item in _data.Items)
                        {
                            var total = item.Price * item.Quantity;
                            table.Cell().Border(1).Padding(4).AlignCenter().Text(index.ToString()).FontSize(9);
                            table.Cell().Border(1).Padding(4).Text(item.Name).FontSize(9);
                            table.Cell().Border(1).Padding(4).Text(item.Description).FontSize(9);
                            table.Cell().Border(1).Padding(4).AlignRight().Text($"{item.Price:F2} €").FontSize(9);
                            table.Cell().Border(1).Padding(4).AlignCenter().Text(item.Quantity.ToString()).FontSize(9);
                            table.Cell().Border(1).Padding(4).AlignRight().Text($"{total:F2} €").FontSize(9);
                            index++;
                        }
                    });

                    // Totals
                    var itemsTotal = _data.Items.Sum(i => i.Price * i.Quantity);
                    col.Item().PaddingTop(10).Column(innerCol =>
                    {
                        innerCol.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Ukupna vrijednost artikala:").FontSize(10);
                            row.ConstantItem(100).AlignRight().Text($"{itemsTotal:F2} €").FontSize(10);
                        });
                        innerCol.Item().PaddingTop(5).Row(row =>
                        {
                            row.RelativeItem().Text("Polog za rezervaciju:").Bold().FontSize(10);
                            row.ConstantItem(100).AlignRight().Text($"{_data.ReservationDeposit:F2} €").Bold().FontSize(10);
                        });
                        innerCol.Item().PaddingTop(5).Row(row =>
                        {
                            row.RelativeItem().Text("Preostalo za uplatu:").Bold().FontSize(10);
                            row.ConstantItem(100).AlignRight().Text($"{(itemsTotal - _data.ReservationDeposit):F2} €").Bold().FontSize(10);
                        });
                    });

                    // Reservation terms
                    col.Item().PaddingTop(20).Column(innerCol =>
                    {
                        innerCol.Item().Text("UVJETI REZERVACIJE").Bold().FontSize(11);
                        innerCol.Item().PaddingTop(5).Text(
                            $"1. Rezervacija je važeća do {_data.ReservationUntil:dd.MM.yyyy}."
                        ).FontSize(9).LineHeight(1.4f);
                        innerCol.Item().Text(
                            "2. Polog za rezervaciju nije povratljiv ako se artikal ne preuzme do datuma isteka rezervacije."
                        ).FontSize(9).LineHeight(1.4f);
                        innerCol.Item().Text(
                            "3. Preostali iznos mora biti uplaćen prilikom preuzimanja artikala."
                        ).FontSize(9).LineHeight(1.4f);
                        innerCol.Item().Text(
                            "4. Zalagaonica zadržava pravo prodaje artikala drugom kupcu nakon isteka rezervacije."
                        ).FontSize(9).LineHeight(1.4f);
                    });

                    // Signatures
                    col.Item().PaddingTop(30).Row(row =>
                    {
                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("ZALAGAONICA:").FontSize(9);
                            innerCol.Item().Text($"Djelatnik: {_data.EmployeeName}").FontSize(9);
                            innerCol.Item().PaddingTop(30).BorderBottom(1).Height(1);
                            innerCol.Item().PaddingTop(3).Text("m.p.").FontSize(8);
                        });

                        row.RelativeItem().PaddingLeft(20);

                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("KUPAC:").FontSize(9);
                            innerCol.Item().Text(_data.Client.Name).FontSize(9);
                            innerCol.Item().PaddingTop(30).BorderBottom(1).Height(1);
                            innerCol.Item().PaddingTop(3).Text("(potpis)").FontSize(8);
                        });
                    });
                });

                page.Footer().AlignCenter().Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text($"Datum ispisa: {DateTime.Now:dd.MM.yyyy}").FontSize(7);
                    row.RelativeItem().AlignRight().Text("1 od 1").FontSize(7);
                });
            });
        }
    }
}
