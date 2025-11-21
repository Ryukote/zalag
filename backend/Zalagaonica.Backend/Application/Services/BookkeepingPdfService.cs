using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services
{
    public class BookkeepingPdfService
    {
        public BookkeepingPdfService()
        {
            // Set QuestPDF license
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerateMonthlyReportPdf(MonthlyBookkeepingReportDto report)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(content => ComposeMonthlyReportContent(content, report));
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Generirano: ");
                        x.Span(report.GeneratedAt.ToString("dd.MM.yyyy HH:mm")).Bold();
                    });
                });
            }).GeneratePdf();
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("ZALAGAONICA").FontSize(20).Bold();
                    column.Item().Text("Mjesečni izvještaj za knjigovođu").FontSize(14);
                });
            });
        }

        private void ComposeMonthlyReportContent(IContainer container, MonthlyBookkeepingReportDto report)
        {
            container.Column(column =>
            {
                column.Spacing(10);

                // Razdoblje
                column.Item().Text($"Razdoblje: {report.PeriodStart:dd.MM.yyyy} - {report.PeriodEnd:dd.MM.yyyy}").FontSize(12).Bold();
                column.Item().LineHorizontal(1);
                column.Item().PaddingTop(10);

                // Prodaja
                column.Item().Text("PRODAJA").FontSize(12).Bold().Underline();
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Ukupna prodaja (bez PDV-a):");
                    row.RelativeItem().AlignRight().Text($"{report.TotalSales:N2} €").Bold();
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("PDV (25%):");
                    row.RelativeItem().AlignRight().Text($"{report.TotalTax:N2} €");
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Ukupna prodaja (s PDV-om):");
                    row.RelativeItem().AlignRight().Text($"{report.TotalSalesWithTax:N2} €").Bold();
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Broj prodaja:");
                    row.RelativeItem().AlignRight().Text(report.SalesCount.ToString());
                });
                column.Item().PaddingTop(10);

                // Zalozi
                column.Item().Text("ZALOZI").FontSize(12).Bold().Underline();
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Ukupan iznos zajmova:");
                    row.RelativeItem().AlignRight().Text($"{report.TotalPledgeAmount:N2} €").Bold();
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Broj novih zaloga:");
                    row.RelativeItem().AlignRight().Text(report.PledgesCount.ToString());
                });
                column.Item().PaddingTop(10);

                // Otplate
                column.Item().Text("OTPLATE").FontSize(12).Bold().Underline();
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Ukupne otplate:");
                    row.RelativeItem().AlignRight().Text($"{report.TotalRepayments:N2} €").Bold();
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Ukupna kamata:");
                    row.RelativeItem().AlignRight().Text($"{report.TotalInterest:N2} €");
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Broj otplata:");
                    row.RelativeItem().AlignRight().Text(report.RepaymentsCount.ToString());
                });
                column.Item().PaddingTop(10);

                // Rashodi
                column.Item().Text("RASHODI").FontSize(12).Bold().Underline();
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Ukupni rashodi:");
                    row.RelativeItem().AlignRight().Text($"{report.TotalExpenses:N2} €").Bold();
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Broj stavki rashoda:");
                    row.RelativeItem().AlignRight().Text(report.ExpensesCount.ToString());
                });
                column.Item().PaddingTop(10);

                // Blagajna
                column.Item().Text("BLAGAJNA").FontSize(12).Bold().Underline();
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Ukupna gotovinska prodaja:");
                    row.RelativeItem().AlignRight().Text($"{report.TotalCashSales:N2} €");
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Gotovinski rashodi:");
                    row.RelativeItem().AlignRight().Text($"{report.TotalCashExpenses:N2} €");
                });
                column.Item().PaddingTop(10);

                // Sažetak
                column.Item().LineHorizontal(2);
                column.Item().PaddingTop(10);
                column.Item().Text("SAŽETAK").FontSize(12).Bold().Underline();
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("NETO DOHODAK:").FontSize(12).Bold();
                    row.RelativeItem().AlignRight().Text($"{report.NetIncome:N2} €").FontSize(12).Bold();
                });
            });
        }

        public byte[] GenerateKpoReportPdf(KpoReportDto report)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(1, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                    page.Header().Column(column =>
                    {
                        column.Item().Text("KNJIGA POPISA OBJEKATA (KPO)").FontSize(16).Bold();
                        column.Item().Text($"Razdoblje: {report.PeriodStart:dd.MM.yyyy} - {report.PeriodEnd:dd.MM.yyyy}").FontSize(11);
                    });

                    page.Content().Element(content => ComposeKpoTable(content, report));

                    page.Footer().AlignCenter().Text($"Generirano: {report.GeneratedAt:dd.MM.yyyy HH:mm}");
                });
            }).GeneratePdf();
        }

        private void ComposeKpoTable(IContainer container, KpoReportDto report)
        {
            container.Column(column =>
            {
                column.Spacing(5);

                // Summary
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text($"Ukupan ulaz: {report.TotalIn} kom");
                    row.RelativeItem().Text($"Ukupan izlaz: {report.TotalOut} kom");
                    row.RelativeItem().Text($"Završno stanje: {report.FinalBalance} kom").Bold();
                });
                column.Item().LineHorizontal(1);

                // Table
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(80); // Datum
                        columns.RelativeColumn(3); // Artikl
                        columns.RelativeColumn(2); // Dokument
                        columns.ConstantColumn(60); // Ulaz
                        columns.ConstantColumn(60); // Izlaz
                        columns.ConstantColumn(60); // Stanje
                        columns.RelativeColumn(2); // Napomena
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Datum").Bold();
                        header.Cell().Element(CellStyle).Text("Artikl").Bold();
                        header.Cell().Element(CellStyle).Text("Dokument").Bold();
                        header.Cell().Element(CellStyle).AlignRight().Text("Ulaz").Bold();
                        header.Cell().Element(CellStyle).AlignRight().Text("Izlaz").Bold();
                        header.Cell().Element(CellStyle).AlignRight().Text("Stanje").Bold();
                        header.Cell().Element(CellStyle).Text("Napomena").Bold();
                    });

                    // Rows
                    foreach (var entry in report.Entries)
                    {
                        table.Cell().Element(CellStyle).Text(entry.Date.ToString("dd.MM.yyyy"));
                        table.Cell().Element(CellStyle).Text(entry.ArticleName);
                        table.Cell().Element(CellStyle).Text(entry.DocumentNumber);
                        table.Cell().Element(CellStyle).AlignRight().Text(entry.InQuantity > 0 ? $"+{entry.InQuantity}" : "");
                        table.Cell().Element(CellStyle).AlignRight().Text(entry.OutQuantity > 0 ? $"-{entry.OutQuantity}" : "");
                        table.Cell().Element(CellStyle).AlignRight().Text(entry.Balance.ToString()).Bold();
                        table.Cell().Element(CellStyle).Text(entry.Notes ?? "");
                    }
                });
            });
        }

        public byte[] GeneratePledgeReportPdf(PledgeReportDto report)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                    page.Header().Column(column =>
                    {
                        column.Item().Text("IZVJEŠTAJ O ZALOZIMA").FontSize(16).Bold();
                        column.Item().Text($"Razdoblje: {report.PeriodStart:dd.MM.yyyy} - {report.PeriodEnd:dd.MM.yyyy}").FontSize(11);
                        column.Item().Text($"Sukladno Zakonu o zalagaonicama (NN 112/12)").FontSize(8).Italic();
                    });

                    page.Content().Element(content => ComposePledgeReportContent(content, report));

                    page.Footer().AlignCenter().Text($"Generirano: {report.GeneratedAt:dd.MM.yyyy HH:mm}");
                });
            }).GeneratePdf();
        }

        private void ComposePledgeReportContent(IContainer container, PledgeReportDto report)
        {
            container.Column(column =>
            {
                column.Spacing(8);

                // Statistics
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text($"Ukupno zaloga: {report.TotalPledges}").Bold();
                        c.Item().Text($"Aktivni: {report.ActivePledges}");
                        c.Item().Text($"Istekli: {report.ExpiredPledges}");
                    });
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text($"Otkupljeni: {report.RedeemedPledges}");
                        c.Item().Text($"Prodani: {report.ForfeitedPledges}");
                    });
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text($"Ukupan zajam: {report.TotalLoanAmount:N2} €").Bold();
                        c.Item().Text($"Ukupna procjena: {report.TotalEstimatedValue:N2} €");
                    });
                });

                column.Item().LineHorizontal(1);
                column.Item().PaddingTop(5);

                // Table
                column.Item().Text("Detalji zaloga:").FontSize(11).Bold();
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(70); // Datum
                        columns.RelativeColumn(2); // Klijent
                        columns.RelativeColumn(2); // Predmet
                        columns.ConstantColumn(70); // Procjena
                        columns.ConstantColumn(70); // Zajam
                        columns.ConstantColumn(50); // Kamata
                        columns.ConstantColumn(60); // Status
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Datum").Bold();
                        header.Cell().Element(CellStyle).Text("Klijent").Bold();
                        header.Cell().Element(CellStyle).Text("Predmet").Bold();
                        header.Cell().Element(CellStyle).AlignRight().Text("Procjena").Bold();
                        header.Cell().Element(CellStyle).AlignRight().Text("Zajam").Bold();
                        header.Cell().Element(CellStyle).AlignRight().Text("Kamata %").Bold();
                        header.Cell().Element(CellStyle).Text("Status").Bold();
                    });

                    // Rows
                    foreach (var pledge in report.Pledges)
                    {
                        table.Cell().Element(CellStyle).Text(pledge.StartDate.ToString("dd.MM.yy"));
                        table.Cell().Element(CellStyle).Text(pledge.ClientName).FontSize(8);
                        table.Cell().Element(CellStyle).Text(pledge.ItemName).FontSize(8);
                        table.Cell().Element(CellStyle).AlignRight().Text($"{pledge.EstimatedValue:N2} €");
                        table.Cell().Element(CellStyle).AlignRight().Text($"{pledge.LoanAmount:N2} €");
                        table.Cell().Element(CellStyle).AlignRight().Text($"{pledge.InterestRate:N1}%");
                        table.Cell().Element(CellStyle).Text(pledge.Status).FontSize(8);
                    }
                });
            });
        }

        public byte[] GenerateTaxReportPdf(TaxReportDto report)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    page.Header().Column(column =>
                    {
                        column.Item().Text("POREZNI IZVJEŠTAJ (PDV)").FontSize(18).Bold();
                        column.Item().Text($"Za razdoblje: {report.Month}/{report.Year}").FontSize(12);
                        column.Item().Text("Sukladno Zakonu o PDV-u (NN 115/16)").FontSize(8).Italic();
                    });

                    page.Content().Element(content => ComposeTaxReportContent(content, report));

                    page.Footer().AlignCenter().Text($"Generirano: {report.GeneratedAt:dd.MM.yyyy HH:mm}");
                });
            }).GeneratePdf();
        }

        private void ComposeTaxReportContent(IContainer container, TaxReportDto report)
        {
            container.Column(column =>
            {
                column.Spacing(10);

                column.Item().Text($"Razdoblje: {report.PeriodStart:dd.MM.yyyy} - {report.PeriodEnd:dd.MM.yyyy}").FontSize(11).Bold();
                column.Item().LineHorizontal(1);
                column.Item().PaddingTop(10);

                // Obračun PDV-a
                column.Item().Text("OBRAČUN PDV-A").FontSize(13).Bold().Underline();
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Osnovica (prodaja bez PDV-a):");
                    row.RelativeItem().AlignRight().Text($"{report.TotalSalesWithoutTax:N2} €").Bold();
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text($"PDV stopa ({report.TaxRate * 100}%):");
                    row.RelativeItem().AlignRight().Text($"{report.TaxRate * 100}%");
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Iznos PDV-a:");
                    row.RelativeItem().AlignRight().Text($"{report.TotalTax:N2} €").Bold().FontSize(12);
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Ukupna prodaja (s PDV-om):");
                    row.RelativeItem().AlignRight().Text($"{report.TotalSalesWithTax:N2} €").Bold();
                });
                column.Item().PaddingTop(15);

                // Otkup (nabava)
                column.Item().Text("OTKUP ROBE").FontSize(13).Bold().Underline();
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Ukupan otkup (nabava):");
                    row.RelativeItem().AlignRight().Text($"{report.TotalPurchases:N2} €").Bold();
                });
                column.Item().Text("* Otkup nije oporeziv PDV-om").FontSize(8).Italic();
                column.Item().PaddingTop(15);

                // Porezna osnovica
                column.Item().LineHorizontal(2);
                column.Item().PaddingTop(10);
                column.Item().Text("POREZNA OSNOVICA").FontSize(13).Bold().Underline();
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text("Prodaja - Nabava:");
                    row.RelativeItem().AlignRight().Text($"{report.TaxableBase:N2} €").FontSize(12).Bold();
                });
                column.Item().Text("* Ova osnovica se koristi za obračun prihoda za porez na dobit").FontSize(8).Italic();
            });
        }

        private IContainer CellStyle(IContainer container)
        {
            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(3).PaddingHorizontal(2);
        }
    }
}
