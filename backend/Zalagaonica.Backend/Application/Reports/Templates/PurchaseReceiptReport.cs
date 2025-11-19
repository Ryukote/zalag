using Application.DTOs.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates
{
    public class PurchaseReceiptReport : IDocument
    {
        private readonly PurchaseReceiptDto _data;

        public PurchaseReceiptReport(PurchaseReceiptDto data)
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
                            innerCol.Item().Text("KUPAC (ZALAGAONICA):").Bold().FontSize(10);
                            innerCol.Item().Text("PAWN SHOPS d.o.o.").FontSize(9);
                            innerCol.Item().Text("P.J. Horvačanska cesta 25, Zagreb").FontSize(9);
                            innerCol.Item().Text("Logorište 11a").FontSize(9);
                            innerCol.Item().Text("47000 Karlovac").FontSize(9);
                            innerCol.Item().Text("OIB: 51659874442").FontSize(9);
                            innerCol.Item().Text("Tel: 092 500 8000").FontSize(9);
                        });

                        row.RelativeItem().PaddingLeft(10);

                        // Seller box
                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("PRODAVAČ:").Bold().FontSize(10);
                            innerCol.Item().Text(_data.Seller.Name).FontSize(9);
                            innerCol.Item().Text(_data.Seller.Address).FontSize(9);
                            innerCol.Item().Text(_data.Seller.City).FontSize(9);
                            if (!string.IsNullOrEmpty(_data.Seller.Oib))
                            {
                                innerCol.Item().Text($"OIB: {_data.Seller.Oib}").FontSize(9);
                            }
                        });
                    });

                    col.Item().PaddingTop(10).AlignRight().Column(innerCol =>
                    {
                        innerCol.Item().Text($"Broj dokumenta: {_data.DocumentNumber}").FontSize(9);
                        innerCol.Item().Text($"Datum: {_data.DocumentDate:dd.MM.yyyy}").FontSize(9);
                    });

                    col.Item().PaddingTop(20).Text("OTKUPNI BLOK ZA OTKUP RABLJENOG DOBRA")
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
                            columns.RelativeColumn(3);   // Naziv artikla
                            columns.RelativeColumn(2);   // Oznaka/šifra
                            columns.ConstantColumn(50);  // Jed. mjere
                            columns.ConstantColumn(50);  // Količina
                            columns.ConstantColumn(60);  // MPC
                            columns.ConstantColumn(70);  // MPV (EUR)
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3)
                                .Padding(4).Text("").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3)
                                .Padding(4).Text("naziv artikla").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3)
                                .Padding(4).Text("oznaka / šifra").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3)
                                .Padding(4).Text("jed. mjere").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3)
                                .Padding(4).Text("količina").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3)
                                .Padding(4).Text("MPC").Bold().FontSize(9);
                            header.Cell().Border(1).Background(Colors.Grey.Lighten3)
                                .Padding(4).Text("MPV (EUR)").Bold().FontSize(9);
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
                            table.Cell().Border(1).Padding(4).AlignRight().Text($"{item.Mpc:F2}").FontSize(9);
                            table.Cell().Border(1).Padding(4).AlignRight().Text($"{item.PurchasePrice:F2} €").FontSize(9);
                            index++;
                        }
                    });

                    // Total
                    var total = _data.Items.Sum(i => i.PurchasePrice);
                    col.Item().PaddingTop(10).AlignRight()
                        .Text($"Ukupno: {total:F2} €").Bold().FontSize(12);

                    // Statement
                    col.Item().PaddingTop(20).Column(innerCol =>
                    {
                        innerCol.Item().Text("Izjava prodavatelja:").Bold().FontSize(10);
                        innerCol.Item().PaddingTop(5).Text(
                            "Izjavljujem da je predmet isključivo moje vlasništvo. Suglasan sam da: PAWN SHOPS d.o.o. " +
                            "ne odgovara za dokumente, podatke, informacije, autorska i vlasnička prava trećih osoba " +
                            "te bilo koji drugi sadržaj, kao ni moguću štetu prema prodavatelju ili bilo kojoj trećoj " +
                            "strani koja bi mogla nastati zbog upotrebe istih."
                        ).FontSize(9).LineHeight(1.4f);
                        innerCol.Item().PaddingTop(10).Text(
                            "Izjavljujem da nisam porezni obveznik po čl. 6. Zakona o porezu na dodanu vrijednost, " +
                            "niti sam obveznik fiskalizacije."
                        ).FontSize(9).LineHeight(1.4f);
                    });

                    // Signatures
                    col.Item().PaddingTop(40).Row(row =>
                    {
                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("Potpis prodavača:").FontSize(9);
                            innerCol.Item().PaddingTop(5).BorderBottom(1).Height(1);
                        });

                        row.RelativeItem().PaddingLeft(20);

                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("Potpis kupca:").FontSize(9);
                            innerCol.Item().PaddingTop(5).BorderBottom(1).Height(1);
                            innerCol.Item().PaddingTop(3).Text("m.p.").FontSize(8);
                        });
                    });

                    col.Item().PaddingTop(10).Text($"Skladište: {_data.Warehouse}").FontSize(8);
                    col.Item().Text($"Djelatnik: {_data.EmployeeName}").FontSize(8);
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
