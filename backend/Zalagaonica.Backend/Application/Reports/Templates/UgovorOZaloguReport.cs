using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates
{
    public class UgovorOZaloguReport : IDocument
    {
        public class PledgeAgreementData
        {
            public string PledgeNumber { get; set; } = string.Empty;
            public DateTime PledgeDate { get; set; }
            public string ClientName { get; set; } = string.Empty;
            public string? ClientAddress { get; set; }
            public string? ClientCity { get; set; }
            public string? ClientOib { get; set; }
            public string ItemName { get; set; } = string.Empty;
            public string ItemDescription { get; set; } = string.Empty;
            public decimal EstimatedValue { get; set; }
            public decimal LoanAmount { get; set; }
            public decimal ReturnAmount { get; set; }
            public int Period { get; set; }
            public DateTime RedeemDeadline { get; set; }
        }

        private readonly PledgeAgreementData _data;

        public UgovorOZaloguReport(PledgeAgreementData data)
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

                page.Header().Column(header =>
                {
                    header.Spacing(15);

                    // Company and Client info side by side
                    header.Item().Row(row =>
                    {
                        // Company info
                        row.RelativeItem().Border(1).Padding(8).Column(col =>
                        {
                            col.Item().Text("ZALAGAONICA:").Bold().FontSize(10);
                            col.Item().Text("PAWN SHOPS d.o.o.").FontSize(8);
                            col.Item().Text("P.J. Horvačanska cesta 25, Zagreb").FontSize(8);
                            col.Item().Text("Logorište 11a").FontSize(8);
                            col.Item().Text("47000 Karlovac").FontSize(8);
                            col.Item().Text("OIB: 51659874442").FontSize(8);
                            col.Item().Text("Tel: 092 500 8000").FontSize(8);
                        });

                        // Client info
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Zaloglitelj:").Bold().FontSize(10);
                            col.Item().Text(_data.ClientName).FontSize(8);
                            if (!string.IsNullOrEmpty(_data.ClientAddress))
                                col.Item().Text(_data.ClientAddress).FontSize(8);
                            if (!string.IsNullOrEmpty(_data.ClientCity))
                                col.Item().Text(_data.ClientCity).FontSize(8);
                            if (!string.IsNullOrEmpty(_data.ClientOib))
                                col.Item().Text($"OIB: {_data.ClientOib}").FontSize(8);
                        });
                    });

                    // Document metadata
                    header.Item().AlignRight().Column(col =>
                    {
                        col.Item().Text($"Broj ugovora: {_data.PledgeNumber}").FontSize(8);
                        col.Item().Text($"Datum: {_data.PledgeDate:dd.MM.yyyy}").FontSize(8);
                        col.Item().Text($"Rok isplate: {_data.RedeemDeadline:dd.MM.yyyy}").FontSize(8);
                    });
                });

                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    // Title
                    col.Item().Text($"UGOVOR O ZALOGU - {_data.PledgeNumber}")
                        .Bold().FontSize(14).AlignCenter();

                    // Introduction
                    col.Item().Text("Ovaj ugovor o zalogu zaključen je između tvrtke PAWN SHOPS d.o.o. (dalje u tekstu: Zalagaonica) i gore navedenog zaloglitelja u skladu sa Zakonom o zalagaonicama i Općim uvjetima poslovanja.")
                        .FontSize(8).Justify();

                    // Section 1: Pledged Item
                    col.Item().PaddingTop(10).Text("ČLANAK 1. - PREDMET ZALOGA").Bold().FontSize(9);
                    col.Item().Column(c =>
                    {
                        c.Item().Row(r =>
                        {
                            r.RelativeItem(3).Text("Naziv predmeta:").Bold().FontSize(8);
                            r.RelativeItem(7).Text(_data.ItemName).FontSize(8);
                        });
                        c.Item().Row(r =>
                        {
                            r.RelativeItem(3).Text("Opis i stanje:").Bold().FontSize(8);
                            r.RelativeItem(7).Text(_data.ItemDescription).FontSize(8);
                        });
                        c.Item().Row(r =>
                        {
                            r.RelativeItem(3).Text("Procijenjena vrijednost:").Bold().FontSize(8);
                            r.RelativeItem(7).Text($"{_data.EstimatedValue:N2} €").FontSize(8);
                        });
                    });

                    // Section 2: Loan Terms
                    col.Item().PaddingTop(10).Text("ČLANAK 2. - UVJETI ZAJMA").Bold().FontSize(9);
                    col.Item().Column(c =>
                    {
                        c.Item().Row(r =>
                        {
                            r.RelativeItem(3).Text("Iznos pozajmice:").Bold().FontSize(8);
                            r.RelativeItem(7).Text($"{_data.LoanAmount:N2} €").FontSize(8);
                        });
                        c.Item().Row(r =>
                        {
                            r.RelativeItem(3).Text("Iznos za isplatu:").Bold().FontSize(8);
                            r.RelativeItem(7).Text($"{_data.ReturnAmount:N2} €").FontSize(8);
                        });
                        c.Item().Row(r =>
                        {
                            r.RelativeItem(3).Text("Razdoblje:").Bold().FontSize(8);
                            r.RelativeItem(7).Text($"{_data.Period} dana").FontSize(8);
                        });
                        c.Item().Row(r =>
                        {
                            r.RelativeItem(3).Text("Rok isplate:").Bold().FontSize(8);
                            r.RelativeItem(7).Text($"{_data.RedeemDeadline:dd.MM.yyyy}").FontSize(8);
                        });
                    });

                    // Section 3: Terms and Conditions
                    col.Item().PaddingTop(10).Text("ČLANAK 3. - OPĆI UVJETI I PRAVA STRANAKA").Bold().FontSize(9);
                    col.Item().Column(c =>
                    {
                        c.Spacing(5);
                        c.Item().Text("1. Zaloglitelj izjavljuje da je isključivi vlasnik predmeta zaloga i da predmet nije opterećen pravima trećih osoba.").FontSize(8).Justify();
                        c.Item().Text("2. Zaloglitelj se obvezuje da će vratiti pozajmljeni iznos zajedno s kamatom u dogovorenom roku.").FontSize(8).Justify();
                        c.Item().Text("3. Zalagaonica se obvezuje da će čuvati predmet zaloga s dužnom pažnjom i osigurati ga od uništenja i oštećenja.").FontSize(8).Justify();
                        c.Item().Text("4. Zaloglitelj ima pravo otkupiti predmet zaloga u bilo kojem trenutku tijekom trajanja ugovora plaćanjem iznosa za isplatu.").FontSize(8).Justify();
                        c.Item().Text("5. U slučaju neisplate u dogovorenom roku, Zalagaonica stječe pravo prodaje predmeta zaloga radi naplate potraživanja.").FontSize(8).Justify();
                        c.Item().Text("6. Ako vrijednost prodaje predmeta prelazi iznos duga, razlika pripada zaloglitelju i može se isplatiti na zahtjev.").FontSize(8).Justify();
                        c.Item().Text("7. Ako vrijednost prodaje ne pokriva dug, Zalagaonica zadržava pravo naplate ostatka duga od zaloglitelja.").FontSize(8).Justify();
                        c.Item().Text("8. Zaloglitelj može produljiti rok isplate ugovorom o obnovi uz plaćanje dodatnih kamata.").FontSize(8).Justify();
                    });

                    // Section 4: Special Provisions
                    col.Item().PaddingTop(10).Text("ČLANAK 4. - POSEBNE ODREDBE").Bold().FontSize(9);
                    col.Item().Text("Zaloglitelj je upoznat s Općim uvjetima poslovanja Zalagaonice koji čine sastavni dio ovog ugovora. Zaloglitelj potvrđuje da je primio kopiju ovog ugovora te da su mu objašnjena sva prava i obveze.")
                        .FontSize(8).Justify();

                    // Signatures
                    col.Item().PaddingTop(15).Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("ZALAGAONICA:").FontSize(8);
                            c.Item().Text("Izradio:").FontSize(8);
                            c.Item().PaddingTop(40).BorderBottom(1);
                            c.Item().Text("m.p.").FontSize(7);
                        });

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("ZALOGLITELJ:").FontSize(8);
                            c.Item().Text(_data.ClientName).FontSize(8);
                            c.Item().PaddingTop(40).BorderBottom(1);
                            c.Item().Text("(potpis)").FontSize(7);
                        });
                    });
                });

                page.Footer().Row(footer =>
                {
                    footer.RelativeItem().Text($"Datum ispisa: {DateTime.Now:dd.MM.yyyy}").FontSize(7);
                    footer.RelativeItem().AlignRight().Text("1 od 1").FontSize(7);
                });
            });
        }
    }
}
