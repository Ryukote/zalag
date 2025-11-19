using Application.DTOs.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates
{
    public class WarehouseTransferReport : IDocument
    {
        private readonly WarehouseTransferDto _data;

        public WarehouseTransferReport(WarehouseTransferDto data)
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
                    col.Item().Text("PAWN SHOPS d.o.o.").Bold().FontSize(14).AlignCenter();
                    col.Item().Text("Logorište 11a, 47000 Karlovac").FontSize(9).AlignCenter();
                    col.Item().Text("OIB: 51659874442").FontSize(9).AlignCenter();

                    col.Item().PaddingTop(15).Text("MEĐUSKLADIŠNICA")
                        .Bold().FontSize(16).AlignCenter();

                    col.Item().PaddingTop(10).Row(row =>
                    {
                        row.RelativeItem().Text($"Broj dokumenta: {_data.DocumentNumber}").FontSize(9);
                        row.RelativeItem().AlignRight().Text($"Datum: {_data.DocumentDate:dd.MM.yyyy}").FontSize(9);
                    });

                    col.Item().PaddingTop(5).Row(row =>
                    {
                        row.RelativeItem().Text($"Iz skladišta: {_data.FromWarehouse}").FontSize(9);
                        row.RelativeItem().AlignRight().Text($"U skladište: {_data.ToWarehouse}").FontSize(9);
                    });
                });

                page.Content().PaddingTop(20).Column(col =>
                {
                    // Items table
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);   // #
                            columns.RelativeColumn(3);    // Naziv
                            columns.RelativeColumn(2);    // Šifra
                            columns.ConstantColumn(60);   // Jed. mjere
                            columns.ConstantColumn(60);   // Količina
                            columns.ConstantColumn(80);   // Jed. cijena
                            columns.ConstantColumn(90);   // Ukupno
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(4).Text("#").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(4).Text("Naziv artikla").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(4).Text("Šifra").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(4).Text("Jed. mjere").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(4).Text("Količina").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(4).Text("Jed. cijena").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(4).Text("Ukupno (EUR)").Bold().FontSize(9);
                        });

                        // Items
                        int index = 1;
                        foreach (var item in _data.Items)
                        {
                            table.Cell().Border(1).Padding(4).AlignCenter().Text(index.ToString()).FontSize(9);
                            table.Cell().Border(1).Padding(4).Text(item.Name).FontSize(9);
                            table.Cell().Border(1).Padding(4).Text(item.Code).FontSize(9);
                            table.Cell().Border(1).Padding(4).AlignCenter().Text(item.UnitOfMeasure).FontSize(9);
                            table.Cell().Border(1).Padding(4).AlignCenter().Text(item.Quantity.ToString()).FontSize(9);
                            table.Cell().Border(1).Padding(4).AlignRight().Text($"{item.UnitPrice:F2}").FontSize(9);
                            table.Cell().Border(1).Padding(4).AlignRight().Text($"{item.TotalPrice:F2} €").FontSize(9);
                            index++;
                        }
                    });

                    // Total
                    var total = _data.Items.Sum(i => i.TotalPrice);
                    col.Item().PaddingTop(10).AlignRight()
                        .Text($"UKUPNO: {total:F2} €").Bold().FontSize(12);

                    // Notes
                    if (!string.IsNullOrEmpty(_data.Notes))
                    {
                        col.Item().PaddingTop(20).Column(innerCol =>
                        {
                            innerCol.Item().Text("Napomena:").Bold().FontSize(10);
                            innerCol.Item().PaddingTop(5).Text(_data.Notes).FontSize(9).LineHeight(1.4f);
                        });
                    }

                    col.Item().PaddingTop(20).Text($"Djelatnik: {_data.EmployeeName}").FontSize(9);

                    // Signatures
                    col.Item().PaddingTop(40).Row(row =>
                    {
                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("Predao:").FontSize(9);
                            innerCol.Item().PaddingTop(30).BorderBottom(1).Height(1);
                            innerCol.Item().PaddingTop(3).Text("(potpis)").FontSize(8);
                        });

                        row.RelativeItem().PaddingLeft(20);

                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("Primio:").FontSize(9);
                            innerCol.Item().PaddingTop(30).BorderBottom(1).Height(1);
                            innerCol.Item().PaddingTop(3).Text("(potpis)").FontSize(8);
                        });

                        row.RelativeItem().PaddingLeft(20);

                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("Ovjerio:").FontSize(9);
                            innerCol.Item().PaddingTop(30).BorderBottom(1).Height(1);
                            innerCol.Item().PaddingTop(3).Text("(potpis i pečat)").FontSize(8);
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
