using Application.DTOs.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates
{
    public class InboundCalculationReport : IDocument
    {
        private readonly InboundCalculationDto _data;

        public InboundCalculationReport(InboundCalculationDto data)
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
                    // Company and Seller info boxes
                    col.Item().Row(row =>
                    {
                        // Company box
                        row.RelativeItem().Border(1).Padding(8).Column(innerCol =>
                        {
                            innerCol.Item().Text("KUPAC:").Bold().FontSize(10);
                            innerCol.Item().Text("PAWN SHOPS d.o.o.").FontSize(9);
                            innerCol.Item().Text("P.J. Horvačanska cesta 25, Zagreb").FontSize(9);
                            innerCol.Item().Text("Logorište 11a, 47000 Karlovac").FontSize(9);
                            innerCol.Item().Text("OIB: 51659874442").FontSize(9);
                        });

                        row.RelativeItem().PaddingLeft(10);

                        // Seller box
                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("PRODAVATELJ:").Bold().FontSize(10);
                            innerCol.Item().Text(_data.Seller.Name).FontSize(9);
                            innerCol.Item().Text($"OIB: {_data.Seller.Oib}").FontSize(9);
                        });
                    });

                    col.Item().PaddingTop(10).Row(row =>
                    {
                        row.RelativeItem().Text($"Broj dokumenta: {_data.DocumentNumber}").FontSize(9);
                        row.RelativeItem().AlignRight().Text($"Datum: {_data.DocumentDate:dd.MM.yyyy}").FontSize(9);
                    });

                    col.Item().Text($"Ulazni dokument: {_data.IncomingDocumentNumber} od {_data.IncomingDocumentDate:dd.MM.yyyy}").FontSize(9);
                    col.Item().Text($"Skladište: {_data.Warehouse}").FontSize(9);

                    col.Item().PaddingTop(15).Text("ULAZNA KALKULACIJA")
                        .Bold().FontSize(16).AlignCenter();
                });

                page.Content().PaddingTop(20).Column(col =>
                {
                    // Items table
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(25);   // #
                            columns.RelativeColumn(2);    // Naziv
                            columns.RelativeColumn(1);    // Opis
                            columns.ConstantColumn(40);   // Jed.
                            columns.ConstantColumn(35);   // Kol.
                            columns.ConstantColumn(55);   // R-cijena
                            columns.ConstantColumn(40);   // Rabat%
                            columns.ConstantColumn(55);   // Rabat€
                            columns.ConstantColumn(55);   // N-cijena
                            columns.ConstantColumn(40);   // Mar%
                            columns.ConstantColumn(55);   // Mar€
                            columns.ConstantColumn(40);   // Por%
                            columns.ConstantColumn(55);   // Por€
                            columns.ConstantColumn(60);   // MPC
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(2).Text("#").Bold().FontSize(7);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(2).Text("Naziv").Bold().FontSize(7);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(2).Text("Opis").Bold().FontSize(7);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(2).Text("Jed.").Bold().FontSize(7);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(2).Text("Kol.").Bold().FontSize(7);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(2).Text("R-cij").Bold().FontSize(7);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(2).Text("Rab%").Bold().FontSize(7);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(2).Text("Rab€").Bold().FontSize(7);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(2).Text("N-cij").Bold().FontSize(7);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(2).Text("Mar%").Bold().FontSize(7);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(2).Text("Mar€").Bold().FontSize(7);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(2).Text("Por%").Bold().FontSize(7);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(2).Text("Por€").Bold().FontSize(7);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(2).Text("MPC").Bold().FontSize(7);
                        });

                        // Items
                        int index = 1;
                        foreach (var item in _data.Items)
                        {
                            table.Cell().Border(1).Padding(2).AlignCenter().Text(index.ToString()).FontSize(7);
                            table.Cell().Border(1).Padding(2).Text(item.Name).FontSize(7);
                            table.Cell().Border(1).Padding(2).Text(item.Description).FontSize(7);
                            table.Cell().Border(1).Padding(2).AlignCenter().Text(item.UnitOfMeasure).FontSize(7);
                            table.Cell().Border(1).Padding(2).AlignCenter().Text(item.Quantity.ToString()).FontSize(7);
                            table.Cell().Border(1).Padding(2).AlignRight().Text($"{item.InvoicePrice:F2}").FontSize(7);
                            table.Cell().Border(1).Padding(2).AlignRight().Text($"{item.DiscountPercent:F1}").FontSize(7);
                            table.Cell().Border(1).Padding(2).AlignRight().Text($"{item.DiscountAmount:F2}").FontSize(7);
                            table.Cell().Border(1).Padding(2).AlignRight().Text($"{item.PurchasePrice:F2}").FontSize(7);
                            table.Cell().Border(1).Padding(2).AlignRight().Text($"{item.MarginPercent:F1}").FontSize(7);
                            table.Cell().Border(1).Padding(2).AlignRight().Text($"{item.MarginAmount:F2}").FontSize(7);
                            table.Cell().Border(1).Padding(2).AlignRight().Text($"{item.TaxPercent:F1}").FontSize(7);
                            table.Cell().Border(1).Padding(2).AlignRight().Text($"{item.TaxAmount:F2}").FontSize(7);
                            table.Cell().Border(1).Padding(2).AlignRight().Text($"{item.RetailPrice:F2}").FontSize(7);
                            index++;
                        }
                    });

                    // Totals
                    col.Item().PaddingTop(10).Column(innerCol =>
                    {
                        innerCol.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Ukupna račun cijena:").FontSize(9);
                            row.ConstantItem(100).AlignRight().Text($"{_data.TotalInvoicePrice:F2} €").FontSize(9);
                        });
                        innerCol.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Ukupna nabavna cijena:").Bold().FontSize(9);
                            row.ConstantItem(100).AlignRight().Text($"{_data.TotalPurchasePrice:F2} €").Bold().FontSize(9);
                        });
                        innerCol.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Ukupna marža:").FontSize(9);
                            row.ConstantItem(100).AlignRight().Text($"{_data.TotalMargin:F2} €").FontSize(9);
                        });
                        innerCol.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Ukupna porez:").FontSize(9);
                            row.ConstantItem(100).AlignRight().Text($"{_data.TotalTax:F2} €").FontSize(9);
                        });
                        innerCol.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Ukupna MPC:").Bold().FontSize(9);
                            row.ConstantItem(100).AlignRight().Text($"{_data.TotalRetailPrice:F2} €").Bold().FontSize(9);
                        });
                    });

                    // VAT on added value
                    col.Item().PaddingTop(10).Text("PDV na dodanu vrijednost:").Bold().FontSize(9);
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("Osnovica:").FontSize(9);
                        row.ConstantItem(100).AlignRight().Text($"{_data.VatOnAddedValue.Base:F2} €").FontSize(9);
                    });
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("Iznos PDV-a:").FontSize(9);
                        row.ConstantItem(100).AlignRight().Text($"{_data.VatOnAddedValue.Amount:F2} €").FontSize(9);
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
