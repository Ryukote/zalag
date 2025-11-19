using Application.DTOs.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates
{
    public class AppraisalRequestReport : IDocument
    {
        private readonly AppraisalRequestDto _data;

        public AppraisalRequestReport(AppraisalRequestDto data)
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
                            innerCol.Item().Text("ZALAGAONICA:").Bold().FontSize(10);
                            innerCol.Item().Text("PAWN SHOPS d.o.o.").FontSize(9);
                            innerCol.Item().Text("P.J. Horvačanska cesta 25, Zagreb").FontSize(9);
                            innerCol.Item().Text("Logorište 11a, 47000 Karlovac").FontSize(9);
                            innerCol.Item().Text("OIB: 51659874442").FontSize(9);
                            innerCol.Item().Text("Tel: 092 500 8000").FontSize(9);
                        });

                        row.RelativeItem().PaddingLeft(10);

                        // Client info
                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("KLIJENT:").Bold().FontSize(10);
                            innerCol.Item().Text(_data.Client.Name).FontSize(9);
                            if (!string.IsNullOrEmpty(_data.Client.Address))
                            {
                                innerCol.Item().Text(_data.Client.Address).FontSize(9);
                            }
                            if (!string.IsNullOrEmpty(_data.Client.City))
                            {
                                innerCol.Item().Text(_data.Client.City).FontSize(9);
                            }
                            if (!string.IsNullOrEmpty(_data.Client.Oib))
                            {
                                innerCol.Item().Text($"OIB: {_data.Client.Oib}").FontSize(9);
                            }
                        });
                    });

                    col.Item().PaddingTop(10).AlignRight().Column(innerCol =>
                    {
                        innerCol.Item().Text($"Broj zahtjeva: {_data.DocumentNumber}").FontSize(9);
                        innerCol.Item().Text($"Datum: {_data.DocumentDate:dd.MM.yyyy}").FontSize(9);
                    });

                    col.Item().PaddingTop(20).Text("ZAHTJEV ZA PROCJENU")
                        .Bold().FontSize(16).AlignCenter();
                });

                page.Content().PaddingTop(20).Column(col =>
                {
                    col.Item().Text("PODACI O PREDMETU").Bold().FontSize(11);

                    col.Item().PaddingTop(10).Row(row =>
                    {
                        row.ConstantItem(150).Text("Naziv predmeta:").Bold().FontSize(9);
                        row.RelativeItem().Text(_data.Item.Name).FontSize(9);
                    });

                    col.Item().PaddingTop(5).Row(row =>
                    {
                        row.ConstantItem(150).Text("Opis i stanje:").Bold().FontSize(9);
                        row.RelativeItem().Text(_data.Item.Description).FontSize(9);
                    });

                    col.Item().PaddingTop(5).Row(row =>
                    {
                        row.ConstantItem(150).Text("Procijenjena vrijednost:").Bold().FontSize(9);
                        row.RelativeItem().Text($"{_data.Item.EstimatedValue:F2} €").FontSize(9);
                    });

                    col.Item().PaddingTop(5).Row(row =>
                    {
                        row.ConstantItem(150).Text("Svrha procjene:").Bold().FontSize(9);
                        row.RelativeItem().Text(_data.Purpose).FontSize(9);
                    });

                    col.Item().PaddingTop(30).Text("NAPOMENA").Bold().FontSize(11);
                    col.Item().PaddingTop(5).Text(
                        "Molimo da izvršite stručnu procjenu navedenog predmeta. Procjena će biti korištena u svrhu " +
                        "određivanja tržišne vrijednosti predmeta i utvrđivanja visine eventualnog zajma ili otkupa."
                    ).FontSize(9).LineHeight(1.4f);

                    col.Item().PaddingTop(10).Text(
                        "Procjena treba uključiti:\n" +
                        "- Identifikaciju i autentifikaciju predmeta\n" +
                        "- Procjenu trenutnog stanja predmeta\n" +
                        "- Utvrđivanje tržišne vrijednosti\n" +
                        "- Eventualnu potrebu za dodatnim ispitivanjima"
                    ).FontSize(9).LineHeight(1.4f);

                    // Space for appraisal
                    col.Item().PaddingTop(30).Border(1).Padding(10).MinHeight(150).Column(innerCol =>
                    {
                        innerCol.Item().Text("Prostor za procjenu:").Bold().FontSize(9);
                        innerCol.Item().PaddingTop(5).Text(txt =>
                        {
                            txt.Span(
                                "_".PadRight(100, '_') + "\n\n" +
                                "_".PadRight(100, '_') + "\n\n" +
                                "_".PadRight(100, '_') + "\n\n" +
                                "_".PadRight(100, '_')
                            ).FontSize(9).LineHeight(1.4f);
                        });
                    });

                    // Signatures
                    col.Item().PaddingTop(30).Row(row =>
                    {
                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("ZALAGAONICA:").FontSize(9);
                            innerCol.Item().PaddingTop(30).BorderBottom(1).Height(1);
                            innerCol.Item().PaddingTop(3).Text("m.p.").FontSize(8);
                        });

                        row.RelativeItem().PaddingLeft(20);

                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("PROCJENITELJ:").FontSize(9);
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
