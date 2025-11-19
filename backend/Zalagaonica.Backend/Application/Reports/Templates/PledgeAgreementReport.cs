using Application.DTOs.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates
{
    public class PledgeAgreementReport : IDocument
    {
        private readonly PledgeAgreementDto _data;

        public PledgeAgreementReport(PledgeAgreementDto data)
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
                    // Company and Client info boxes
                    col.Item().Row(row =>
                    {
                        // Company box
                        row.RelativeItem().Border(1).Padding(8).Column(innerCol =>
                        {
                            innerCol.Item().Text("ZALAGAONICA:").Bold().FontSize(10);
                            innerCol.Item().Text("PAWN SHOPS d.o.o.").FontSize(9);
                            innerCol.Item().Text("P.J. Horvačanska cesta 25, Zagreb").FontSize(9);
                            innerCol.Item().Text("Logorište 11a").FontSize(9);
                            innerCol.Item().Text("47000 Karlovac").FontSize(9);
                            innerCol.Item().Text("OIB: 51659874442").FontSize(9);
                            innerCol.Item().Text("Tel: 092 500 8000").FontSize(9);
                        });

                        row.RelativeItem().PaddingLeft(10);

                        // Client box
                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("Zaloglitelj:").Bold().FontSize(10);
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
                        innerCol.Item().Text($"Broj ugovora: {_data.PledgeNumber}").FontSize(9);
                        innerCol.Item().Text($"Datum: {_data.PledgeDate:dd.MM.yyyy}").FontSize(9);
                        innerCol.Item().Text($"Rok isplate: {_data.RedeemDeadline:dd.MM.yyyy}").FontSize(9);
                    });

                    col.Item().PaddingTop(20).Text($"UGOVOR O ZALOGU - {_data.PledgeNumber}")
                        .Bold().FontSize(14).AlignCenter();
                });

                page.Content().PaddingTop(20).Column(col =>
                {
                    // Introduction
                    col.Item().Text(
                        "Ovaj ugovor o zalogu zaključen je između tvrtke PAWN SHOPS d.o.o. (dalje u tekstu: Zalagaonica) " +
                        "i gore navedenog zaloglitelja u skladu sa Zakonom o zalagaonicama i Općim uvjetima poslovanja."
                    ).FontSize(9).LineHeight(1.4f);

                    // Article 1: Pledged Item
                    col.Item().PaddingTop(15).Text("ČLANAK 1. - PREDMET ZALOGA").Bold().FontSize(10);
                    col.Item().PaddingTop(5).Row(row =>
                    {
                        row.ConstantItem(150).Text("Naziv predmeta:").Bold().FontSize(9);
                        row.RelativeItem().Text(_data.Item.Name).FontSize(9);
                    });
                    col.Item().PaddingTop(3).Row(row =>
                    {
                        row.ConstantItem(150).Text("Opis i stanje:").Bold().FontSize(9);
                        row.RelativeItem().Text(_data.Item.Description).FontSize(9);
                    });
                    col.Item().PaddingTop(3).Row(row =>
                    {
                        row.ConstantItem(150).Text("Procijenjena vrijednost:").Bold().FontSize(9);
                        row.RelativeItem().Text($"{_data.Item.EstimatedValue:F2} €").FontSize(9);
                    });

                    // Article 2: Loan Terms
                    col.Item().PaddingTop(15).Text("ČLANAK 2. - UVJETI ZAJMA").Bold().FontSize(10);
                    col.Item().PaddingTop(5).Row(row =>
                    {
                        row.ConstantItem(150).Text("Iznos pozajmice:").Bold().FontSize(9);
                        row.RelativeItem().Text($"{_data.LoanAmount:F2} €").FontSize(9);
                    });
                    col.Item().PaddingTop(3).Row(row =>
                    {
                        row.ConstantItem(150).Text("Iznos za isplatu:").Bold().FontSize(9);
                        row.RelativeItem().Text($"{_data.ReturnAmount:F2} €").FontSize(9);
                    });
                    col.Item().PaddingTop(3).Row(row =>
                    {
                        row.ConstantItem(150).Text("Razdoblje:").Bold().FontSize(9);
                        row.RelativeItem().Text($"{_data.Period} dana").FontSize(9);
                    });
                    col.Item().PaddingTop(3).Row(row =>
                    {
                        row.ConstantItem(150).Text("Rok isplate:").Bold().FontSize(9);
                        row.RelativeItem().Text($"{_data.RedeemDeadline:dd.MM.yyyy}").FontSize(9);
                    });

                    // Article 3: Terms and Conditions
                    col.Item().PaddingTop(15).Text("ČLANAK 3. - OPĆI UVJETI I PRAVA STRANAKA").Bold().FontSize(10);
                    col.Item().PaddingTop(5).Text(
                        "1. Zaloglitelj izjavljuje da je isključivi vlasnik predmeta zaloga i da predmet nije opterećen pravima trećih osoba."
                    ).FontSize(9).LineHeight(1.4f);
                    col.Item().PaddingTop(3).Text(
                        "2. Zaloglitelj se obvezuje da će vratiti pozajmljeni iznos zajedno s kamatom u dogovorenom roku."
                    ).FontSize(9).LineHeight(1.4f);
                    col.Item().PaddingTop(3).Text(
                        "3. Zalagaonica se obvezuje da će čuvati predmet zaloga s dužnom pažnjom i osigurati ga od uništenja i oštećenja."
                    ).FontSize(9).LineHeight(1.4f);
                    col.Item().PaddingTop(3).Text(
                        "4. Zaloglitelj ima pravo otkupiti predmet zaloga u bilo kojem trenutku tijekom trajanja ugovora plaćanjem iznosa za isplatu."
                    ).FontSize(9).LineHeight(1.4f);
                    col.Item().PaddingTop(3).Text(
                        "5. U slučaju neisplate u dogovorenom roku, Zalagaonica stječe pravo prodaje predmeta zaloga radi naplate potraživanja."
                    ).FontSize(9).LineHeight(1.4f);
                    col.Item().PaddingTop(3).Text(
                        "6. Ako vrijednost prodaje predmeta prelazi iznos duga, razlika pripada zaloglitelju i može se isplatiti na zahtjev."
                    ).FontSize(9).LineHeight(1.4f);
                    col.Item().PaddingTop(3).Text(
                        "7. Ako vrijednost prodaje ne pokriva dug, Zalagaonica zadržava pravo naplate ostatka duga od zaloglitelja."
                    ).FontSize(9).LineHeight(1.4f);
                    col.Item().PaddingTop(3).Text(
                        "8. Zaloglitelj može produljiti rok isplate ugovorom o obnovi uz plaćanje dodatnih kamata."
                    ).FontSize(9).LineHeight(1.4f);

                    // Article 4: Special Provisions
                    col.Item().PaddingTop(15).Text("ČLANAK 4. - POSEBNE ODREDBE").Bold().FontSize(10);
                    col.Item().PaddingTop(5).Text(
                        "Zaloglitelj je upoznat s Općim uvjetima poslovanja Zalagaonice koji čine sastavni dio ovog ugovora. " +
                        "Zaloglitelj potvrđuje da je primio kopiju ovog ugovora te da su mu objašnjena sva prava i obveze."
                    ).FontSize(9).LineHeight(1.4f);

                    // Signatures
                    col.Item().PaddingTop(30).Row(row =>
                    {
                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("ZALAGAONICA:").FontSize(9);
                            innerCol.Item().Text("Izradio:").FontSize(9);
                            innerCol.Item().PaddingTop(30).BorderBottom(1).Height(1);
                            innerCol.Item().PaddingTop(3).Text("m.p.").FontSize(8);
                        });

                        row.RelativeItem().PaddingLeft(20);

                        row.RelativeItem().Column(innerCol =>
                        {
                            innerCol.Item().Text("ZALOGLITELJ:").FontSize(9);
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
