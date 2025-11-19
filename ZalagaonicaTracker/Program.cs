using HtmlAgilityPack;
using Microsoft.Playwright;
using System.Collections.Concurrent;
using System.Text.Json;
using ZalagaonicaTracker.Scrapers;

namespace ZalagaonicaTracker
{
    public class Program
    {
        static HashSet<string> usedPhoneNumbers = new HashSet<string>();
        public static string jsonPath = "geonames-postal-code.json";
        public static JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static string jsonData = "";
        public static List<Region> postalCodeRegions = JsonSerializer.Deserialize<List<Region>>(jsonData, options);


        private static List<string> locationCodebook = new List<string>()
        {
            "",
            ""
        };

        #region Glazbala
        #region Trzalacki instrumenti
        static List<AdData> trzalackiCrnomerec = new List<AdData>();
        static List<AdData> trzalackiKnezija = new List<AdData>();
        static List<AdData> trzalackiKvatric = new List<AdData>();
        static List<AdData> trzalackiMesnicka = new List<AdData>();
        static List<AdData> trzalackiKarlovac = new List<AdData>();
        static List<AdData> trzalackiRijeka = new List<AdData>();
        static List<AdData> trzalackiZadar = new List<AdData>();
        static List<AdData> trzalackiDubrovnik = new List<AdData>();
        #endregion

        #region Klavijature
        static List<AdData> klavijatureCrnomerec = new List<AdData>();
        static List<AdData> klavijatureKnezija = new List<AdData>();
        static List<AdData> klavijatureKvatric = new List<AdData>();
        static List<AdData> klavijatureMesnicka = new List<AdData>();
        static List<AdData> klavijatureKarlovac = new List<AdData>();
        static List<AdData> klavijatureRijeka = new List<AdData>();
        static List<AdData> klavijatureZadar = new List<AdData>();
        static List<AdData> klavijatureDubrovnik = new List<AdData>();
        #endregion

        #region Razglasi i studio oprema
        static List<AdData> razglasiCrnomerec = new List<AdData>();
        static List<AdData> razglasiKnezija = new List<AdData>();
        static List<AdData> razglasiKvatric = new List<AdData>();
        static List<AdData> razglasiMesnicka = new List<AdData>();
        static List<AdData> razglasiKarlovac = new List<AdData>();
        static List<AdData> razglasiRijeka = new List<AdData>();
        static List<AdData> razglasiZadar = new List<AdData>();
        static List<AdData> razglasiDubrovnik = new List<AdData>();
        #endregion
        #endregion

        #region Strojevi alati
        #region Rucni alati
        static List<AdData> rucniAlatiCrnomerec = new List<AdData>();
        static List<AdData> rucniAlatiKnezija = new List<AdData>();
        static List<AdData> rucniAlatiKvatric = new List<AdData>();
        static List<AdData> rucniAlatiMesnicka = new List<AdData>();
        static List<AdData> rucniAlatiKarlovac = new List<AdData>();
        static List<AdData> rucniAlatiRijeka = new List<AdData>();
        static List<AdData> rucniAlatiZadar = new List<AdData>();
        static List<AdData> rucniAlatiDubrovnik = new List<AdData>();
        #endregion
        #endregion

        #region Od glave do pete
        #region Modni dodaci
        static List<AdData> modniDodaciCrnomerec = new List<AdData>();
        static List<AdData> modniDodaciKnezija = new List<AdData>();
        static List<AdData> modniDodaciKvatric = new List<AdData>();
        static List<AdData> modniDodaciMesnicka = new List<AdData>();
        static List<AdData> modniDodaciKarlovac = new List<AdData>();
        static List<AdData> modniDodaciRijeka = new List<AdData>();
        static List<AdData> modniDodaciZadar = new List<AdData>();
        static List<AdData> modniDodaciDubrovnik = new List<AdData>();
        #endregion
        #endregion

        #region Ostalo
        #region Investicijsko zlato i srebro
        static List<AdData> investicijskoZlatoISrebroCrnomerec = new List<AdData>();
        static List<AdData> investicijskoZlatoISrebroKnezija = new List<AdData>();
        static List<AdData> investicijskoZlatoISrebroKvatric = new List<AdData>();
        static List<AdData> investicijskoZlatoISrebroMesnicka = new List<AdData>();
        static List<AdData> investicijskoZlatoISrebroKarlovac = new List<AdData>();
        static List<AdData> investicijskoZlatoISrebroRijeka = new List<AdData>();
        static List<AdData> investicijskoZlatoISrebroZadar = new List<AdData>();
        static List<AdData> investicijskoZlatoISrebroDubrovnik = new List<AdData>();
        #endregion
        #endregion

        #region Informatička oprema
        #region Dronovi
        static List<AdData> dronoviCrnomerec = new List<AdData>();
        static List<AdData> dronoviKnezija = new List<AdData>();
        static List<AdData> dronoviKvatric = new List<AdData>();
        static List<AdData> dronoviMesnicka = new List<AdData>();
        static List<AdData> dronoviKarlovac = new List<AdData>();
        static List<AdData> dronoviRijeka = new List<AdData>();
        static List<AdData> dronoviZadar = new List<AdData>();
        static List<AdData> dronoviDubrovnik = new List<AdData>();
        #endregion

        #region Igre i igraće konzole
        static List<AdData> igreIKonzoleCrnomerec = new List<AdData>();
        static List<AdData> igreIKonzoleKnezija = new List<AdData>();
        static List<AdData> igreIKonzoleKvatric = new List<AdData>();
        static List<AdData> igreIKonzoleMesnicka = new List<AdData>();
        static List<AdData> igreIKonzoleKarlovac = new List<AdData>();
        static List<AdData> igreIKonzoleRijeka = new List<AdData>();
        static List<AdData> igreIKonzoleZadar = new List<AdData>();
        static List<AdData> igreIKonzoleDubrovnik = new List<AdData>();
        #endregion

        #region Tableti
        static List<AdData> tabletiCrnomerec = new List<AdData>();
        static List<AdData> tabletiKnezija = new List<AdData>();
        static List<AdData> tabletiKvatric = new List<AdData>();
        static List<AdData> tabletiMesnicka = new List<AdData>();
        static List<AdData> tabletiKarlovac = new List<AdData>();
        static List<AdData> tabletiRijeka = new List<AdData>();
        static List<AdData> tabletiZadar = new List<AdData>();
        static List<AdData> tabletiDubrovnik = new List<AdData>();
        #endregion

        #region Laptopi
        static List<AdData> laptopiCrnomerec = new List<AdData>();
        static List<AdData> laptopiKnezija = new List<AdData>();
        static List<AdData> laptopiKvatric = new List<AdData>();
        static List<AdData> laptopiMesnicka = new List<AdData>();
        static List<AdData> laptopiKarlovac = new List<AdData>();
        static List<AdData> laptopiRijeka = new List<AdData>();
        static List<AdData> laptopiZadar = new List<AdData>();
        static List<AdData> laptopiDubrovnik = new List<AdData>();
        #endregion
        #endregion

        #region Mobiteli
        #region Oprema https://www.njuskalo.hr/mobiteli-oprema
        static List<AdData> mobiteliOpremaCrnomerec = new List<AdData>();
        static List<AdData> mobiteliOpremaKnezija = new List<AdData>();
        static List<AdData> mobiteliOpremaKvatric = new List<AdData>();
        static List<AdData> mobiteliOpremaMesnicka = new List<AdData>();
        static List<AdData> mobiteliOpremaKarlovac = new List<AdData>();
        static List<AdData> mobiteliOpremaRijeka = new List<AdData>();
        static List<AdData> mobiteliOpremaZadar = new List<AdData>();
        static List<AdData> mobiteliOpremaDubrovnik = new List<AdData>();
        #endregion

        #region Dijelovi https://www.njuskalo.hr/mobiteli-dijelovi
        static List<AdData> mobiteliDijeloviCrnomerec = new List<AdData>();
        static List<AdData> mobiteliDijeloviKnezija = new List<AdData>();
        static List<AdData> mobiteliDijeloviKvatric = new List<AdData>();
        static List<AdData> mobiteliDijeloviMesnicka = new List<AdData>();
        static List<AdData> mobiteliDijeloviKarlovac = new List<AdData>();
        static List<AdData> mobiteliDijeloviRijeka = new List<AdData>();
        static List<AdData> mobiteliDijeloviZadar = new List<AdData>();
        static List<AdData> mobiteliDijeloviDubrovnik = new List<AdData>();
        #endregion

        #region Sim kartice https://www.njuskalo.hr/sim-kartice
        static List<AdData> mobiteliSIMKarticeCrnomerec = new List<AdData>();
        static List<AdData> mobiteliSIMKarticeKnezija = new List<AdData>();
        static List<AdData> mobiteliSIMKarticeKvatric = new List<AdData>();
        static List<AdData> mobiteliSIMKarticeMesnicka = new List<AdData>();
        static List<AdData> mobiteliSIMKarticeKarlovac = new List<AdData>();
        static List<AdData> mobiteliSIMKarticeRijeka = new List<AdData>();
        static List<AdData> mobiteliSIMKarticeZadar = new List<AdData>();
        static List<AdData> mobiteliSIMKarticeDubrovnik = new List<AdData>();
        #endregion

        #region Sami mobiteli https://www.njuskalo.hr/mobiteli
        static List<AdData> mobiteliCrnomerec = new List<AdData>();
        static List<AdData> mobiteliKnezija = new List<AdData>();
        static List<AdData> mobiteliKvatric = new List<AdData>();
        static List<AdData> mobiteliMesnicka = new List<AdData>();
        static List<AdData> mobiteliKarlovac = new List<AdData>();
        static List<AdData> mobiteliRijeka = new List<AdData>();
        static List<AdData> mobiteliZadar = new List<AdData>();
        static List<AdData> mobiteliDubrovnik = new List<AdData>();
        #endregion
        #endregion

        #region Audio video i foto
        #region TV https://www.njuskalo.hr/tv
        static List<AdData> tvCrnomerec = new List<AdData>();
        static List<AdData> tvaKnezija = new List<AdData>();
        static List<AdData> tvKvatric = new List<AdData>();
        static List<AdData> tvMesnicka = new List<AdData>();
        static List<AdData> tvKarlovac = new List<AdData>();
        static List<AdData> tvRijeka = new List<AdData>();
        static List<AdData> tvZadar = new List<AdData>();
        static List<AdData> tvDubrovnik = new List<AdData>();
        #endregion

        #region Video kamere https://www.njuskalo.hr/video-kamere
        static List<AdData> videoKamereCrnomerec = new List<AdData>();
        static List<AdData> videoKamereKnezija = new List<AdData>();
        static List<AdData> videoKamereKvatric = new List<AdData>();
        static List<AdData> videoKamereMesnicka = new List<AdData>();
        static List<AdData> videoKamereKarlovac = new List<AdData>();
        static List<AdData> videoKamereRijeka = new List<AdData>();
        static List<AdData> videoKamereZadar = new List<AdData>();
        static List<AdData> videoKamereDubrovnik = new List<AdData>();
        #endregion

        #region Foto aparati https://www.njuskalo.hr/foto
        static List<AdData> fotoaparatiCrnomerec = new List<AdData>();
        static List<AdData> fotoaparatiKnezija = new List<AdData>();
        static List<AdData> fotoaparatiKvatric = new List<AdData>();
        static List<AdData> fotoaparatiMesnicka = new List<AdData>();
        static List<AdData> fotoaparatiKarlovac = new List<AdData>();
        static List<AdData> fotoaparatiRijeka = new List<AdData>();
        static List<AdData> fotoaparatiZadar = new List<AdData>();
        static List<AdData> fotoaparatiDubrovnik = new List<AdData>();
        #endregion

        #region Audio oprema https://www.njuskalo.hr/audio-oprema
        static List<AdData> audioOpremaCrnomerec = new List<AdData>();
        static List<AdData> audioOpremaKnezija = new List<AdData>();
        static List<AdData> audioOpremaKvatric = new List<AdData>();
        static List<AdData> audioOpremaMesnicka = new List<AdData>();
        static List<AdData> audioOpremaKarlovac = new List<AdData>();
        static List<AdData> audioOpremaRijeka = new List<AdData>();
        static List<AdData> audioOpremaZadar = new List<AdData>();
        static List<AdData> audioOpremaDubrovnik = new List<AdData>();
        #endregion
        #endregion

        private static List<string> authList = new List<string>
        {
            "brd-customer-hl_4ba5ca77-zone-scraping_browser1:qfv9ujrb4m64",
            "brd-customer-hl_bf875d04-zone-scraping_browser1:ffnbmakvbvl9", //tinkrenic@gmail.com
            "brd-customer-hl_afaa2a27-zone-scraping_browser1:h4m9wzgxiakq" //milictomislav506@gmail.com
        };

        //private static List<AdData> scrapedAds = new List<AdData>(); // Global list to store results
        private static ConcurrentBag<AdData> scrapedAds = new ConcurrentBag<AdData>();

        public static async Task<ConcurrentBag<AdData>> GetNjuskaloData(string baseUrl, int page = 1, int minPrice = 0)
        {
            if (authList.Count == 0)
            {
                Console.WriteLine("No AUTH values left to use.");
                return scrapedAds; // Return already scraped data
            }

            string auth = authList.First();
            string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";

            try
            {
                Console.WriteLine($"Connecting with AUTH: {auth}...");
                using var pw = await Playwright.CreateAsync();
                await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
                Console.WriteLine("Connected! Navigating...");

                var pageInstance = await browser.NewPageAsync();
                bool hasRecentAds = true;
                bool checkedNextPage = false;
                DateTime now = DateTime.Now;

                while (true)
                {
                    string url = $"{baseUrl}?page={page}";
                    await pageInstance.GotoAsync(url, new() { Timeout = 120000 });

                    Console.WriteLine($"Scraping page {page}...");
                    var html = await pageInstance.ContentAsync();
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    var ads = htmlDoc.DocumentNode.SelectNodes("//article");
                    if (ads == null)
                    {
                        if (checkedNextPage) break;
                        checkedNextPage = true;
                        page++;
                        continue;
                    }

                    hasRecentAds = false;
                    foreach (var ad in ads)
                    {

                        var userIcon = ad.SelectSingleNode(".//span[contains(@class, 'icon icon--action icon--s icon--user')]");
                        if (userIcon == null)
                        {
                            Console.WriteLine("Skipping ad without user icon.");
                            continue;
                        }

                        var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'entity-pub-date')]/time");

                        string datePostedStr = dateNode.GetAttributeValue("datetime", "");
                        if (!DateTime.TryParse(datePostedStr, out DateTime datePosted))
                        {
                            Console.WriteLine("Skipping ad with invalid date format.");
                            continue;
                        }

                        if ((now - datePosted).TotalHours > 24)
                        {
                            Console.WriteLine("Skipping old ad.");
                            continue;
                        }

                        var titleNode = ad.SelectSingleNode(".//h3/a");
                        var priceNode = ad.SelectSingleNode(".//strong[contains(@class, 'price price--hrk')]");
                        var descNode = ad.SelectSingleNode(".//div[contains(@class, 'entity-description')]");
                        var urlNode = ad.SelectSingleNode(".//h3/a");

                        string title = titleNode?.InnerText.Trim() ?? "N/A";
                        string price = priceNode?.InnerText.Trim() ?? "N/A";
                        string description = descNode?.InnerText.Trim() ?? "N/A";
                        string link = urlNode?.GetAttributeValue("href", "N/A");

                        var numericPrice = Convert.ToInt32(price.Replace(" €", ""));

                        if (numericPrice >= minPrice)
                        {
                            scrapedAds.Add(new AdData()
                            {
                                Title = title,
                                Price = price.Replace("&nbsp;", ""),
                                Description = description,
                                URL = "https://www.njuskalo.hr" + link,
                                AdPostedOn = datePosted
                            });

                            hasRecentAds = true;
                        }
                    }

                    if (!hasRecentAds)
                    {
                        if (checkedNextPage) break;
                        checkedNextPage = true;
                        page++;
                    }
                    else
                    {
                        checkedNextPage = false;
                        page++; // Move to next page
                    }
                }

                return scrapedAds;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with AUTH {auth}: {ex.Message}");
                return await GetNjuskaloData(baseUrl, page, minPrice); // Retry with next AUTH, keeping the page number
            }
        }

        static async Task Main(string[] args)
        {
            HashSet<string> sentNumbers = new HashSet<string>();


            var totalList = new List<AdData>();
            //#region Glazbala Trzalačka
            ////totalList.AddRange(await TrzalackaGlazbala.GetNjuskaloTrzalackaGlazbalaBjelovarskaMedimurskaVarazdinska());
            ////totalList.AddRange(await TrzalackaGlazbala.GetNjuskaloTrzalackaGlazbalaDubrovnikSplit());
            ////totalList.AddRange(await TrzalackaGlazbala.GetNjuskaloTrzalackaGlazbalaKarlovac());
            ////totalList.AddRange(await TrzalackaGlazbala.GetNjuskaloTrzalackaGlazbalaKrapinskoZagorska());
            ////totalList.AddRange(await TrzalackaGlazbala.GetNjuskaloTrzalackaGlazbalaRijekaIstra());
            ////totalList.AddRange(await TrzalackaGlazbala.GetNjuskaloTrzalackaGlazbalaSisackoMoslavacka());
            ////totalList.AddRange(await TrzalackaGlazbala.GetNjuskaloTrzalackaGlazbalaZadarLika());
            ////totalList.AddRange(await TrzalackaGlazbala.GetNjuskaloTrzalackaGlazbalaZagreb());

            totalList.AddRange(await TrzalackaGlazbala.TrzalackaGlazbalaIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));
            totalList.AddRange(await TrzalackaGlazbala.TrzalackaGlazbalaIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await TrzalackaGlazbala.TrzalackaGlazbalaIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await TrzalackaGlazbala.TrzalackaGlazbalaIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await TrzalackaGlazbala.TrzalackaGlazbalaIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await TrzalackaGlazbala.TrzalackaGlazbalaIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await TrzalackaGlazbala.TrzalackaGlazbalaIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await TrzalackaGlazbala.TrzalackaGlazbalaIndexZagreb(totalList, sentNumbers));
            //#endregion

            //#region Glazbala Klavijature
            ////totalList.AddRange(await Klavijatura.GetNjuskaloKlavijaturaBjelovarskaMedimurskaVarazdinska());
            ////totalList.AddRange(await Klavijatura.GetNjuskaloKlavijaturaDubrovnikSplit());
            ////totalList.AddRange(await Klavijatura.GetNjuskaloKlavijaturaKarlovac());
            ////totalList.AddRange(await Klavijatura.GetNjuskaloKlavijaturaKrapinskoZagorska());
            ////totalList.AddRange(await Klavijatura.GetNjuskaloKlavijaturaRijekaIstra());
            ////totalList.AddRange(await Klavijatura.GetNjuskaloKlavijaturaSisackoMoslavacka());
            ////totalList.AddRange(await Klavijatura.GetNjuskaloKlavijaturaZadarLika());
            ////totalList.AddRange(await Klavijatura.GetNjuskaloKlavijaturaZagreb());

            totalList.AddRange(await Klavijatura.KlavijaturaIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));
            totalList.AddRange(await Klavijatura.KlavijaturaIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await Klavijatura.KlavijaturaIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await Klavijatura.KlavijaturaIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await Klavijatura.KlavijaturaIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await Klavijatura.KlavijaturaIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await Klavijatura.KlavijaturaIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await Klavijatura.KlavijaturaIndexZagreb(totalList, sentNumbers));
            //#endregion

            //#region Razglasi
            ////totalList.AddRange(await Razglas.GetNjuskaloRazglasBjelovarskaMedimurskaVarazdinska());
            ////totalList.AddRange(await Razglas.GetNjuskaloRazglasDubrovnikSplit());
            ////totalList.AddRange(await Razglas.GetNjuskaloRazglasKarlovac());
            ////totalList.AddRange(await Razglas.GetNjuskaloRazglasKrapinskoZagorska());
            ////totalList.AddRange(await Razglas.GetNjuskaloRazglasRijekaIstra());
            ////totalList.AddRange(await Razglas.GetNjuskaloRazglasSisackoMoslavacka());
            ////totalList.AddRange(await Razglas.GetNjuskaloRazglasZadarLika());
            ////totalList.AddRange(await Razglas.GetNjuskaloRazglasZagreb());

            totalList.AddRange(await Razglas.RazglasIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));
            totalList.AddRange(await Razglas.RazglasIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await Razglas.RazglasIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await Razglas.RazglasIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await Razglas.RazglasIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await Razglas.RazglasIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await Razglas.RazglasIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await Razglas.RazglasIndexZagreb(totalList, sentNumbers));
            //#endregion

            //#region Ručni alati
            ////totalList.AddRange(await RucniAlati.GetNjuskaloRucniAlatiBjelovarskaMedimurskaVarazdinska());
            ////totalList.AddRange(await RucniAlati.GetNjuskaloRucniAlatiDubrovnikSplit());
            ////totalList.AddRange(await RucniAlati.GetNjuskaloRucniAlatiKarlovac());
            ////totalList.AddRange(await RucniAlati.GetNjuskaloRucniAlatiKrapinskoZagorska());
            ////totalList.AddRange(await RucniAlati.GetNjuskaloRucniAlatiRijekaIstra());
            ////totalList.AddRange(await RucniAlati.GetNjuskaloRucniAlatiSisackoMoslavacka());
            ////totalList.AddRange(await RucniAlati.GetNjuskaloRucniAlatiZadarLika());
            ////totalList.AddRange(await RucniAlati.GetNjuskaloRucniAlatiZagreb());

            totalList.AddRange(await RucniAlati.RucniAlatiIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));
            totalList.AddRange(await RucniAlati.RucniAlatiIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await RucniAlati.RucniAlatiIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await RucniAlati.RucniAlatiIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await RucniAlati.RucniAlatiIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await RucniAlati.RucniAlatiIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await RucniAlati.RucniAlatiIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await RucniAlati.RucniAlatiIndexZagreb(totalList, sentNumbers));
            //#endregion

            //#region Modni dodaci
            ////totalList.AddRange(await ModniDodaci.GetNjuskaloModniDodaciBjelovarskaMedimurskaVarazdinska());
            ////totalList.AddRange(await ModniDodaci.GetNjuskaloModniDodaciDubrovnikSplit());
            ////totalList.AddRange(await ModniDodaci.GetNjuskaloModniDodaciKarlovac());
            ////totalList.AddRange(await ModniDodaci.GetNjuskaloModniDodaciKrapinskoZagorska());
            ////totalList.AddRange(await ModniDodaci.GetNjuskaloModniDodaciRijekaIstra());
            ////totalList.AddRange(await ModniDodaci.GetNjuskaloModniDodaciSisackoMoslavacka());
            ////totalList.AddRange(await ModniDodaci.GetNjuskaloModniDodaciZadarLika());
            ////totalList.AddRange(await ModniDodaci.GetNjuskaloModniDodaciZagreb());

            totalList.AddRange(await ModniDodaci.ModniDodaciIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));
            totalList.AddRange(await ModniDodaci.ModniDodaciIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await ModniDodaci.ModniDodaciIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await ModniDodaci.ModniDodaciIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await ModniDodaci.ModniDodaciIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await ModniDodaci.ModniDodaciIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await ModniDodaci.ModniDodaciIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await ModniDodaci.ModniDodaciIndexZagreb(totalList, sentNumbers));
            //#endregion

            //#region Investicijsko zlato i srebro
            ////totalList.AddRange(await InvesticijskoZlatoSrebro.GetNjuskaloInvesticijskoZlatoSrebroBjelovarskaMedimurskaVarazdinska());
            ////totalList.AddRange(await InvesticijskoZlatoSrebro.GetNjuskaloInvesticijskoZlatoSrebroDubrovnikSplit());
            ////totalList.AddRange(await InvesticijskoZlatoSrebro.GetNjuskaloInvesticijskoZlatoSrebroKarlovac());
            ////totalList.AddRange(await InvesticijskoZlatoSrebro.GetNjuskaloInvesticijskoZlatoSrebroKrapinskoZagorska());
            ////totalList.AddRange(await InvesticijskoZlatoSrebro.GetNjuskaloInvesticijskoZlatoSrebroRijekaIstra());
            ////totalList.AddRange(await InvesticijskoZlatoSrebro.GetNjuskaloInvesticijskoZlatoSrebroSisackoMoslavacka());
            ////totalList.AddRange(await InvesticijskoZlatoSrebro.GetNjuskaloInvesticijskoZlatoSrebroZadarLika());
            ////totalList.AddRange(await InvesticijskoZlatoSrebro.GetNjuskaloInvesticijskoZlatoSrebroZagreb());
            //#endregion

            //#region Dronovi
            ////totalList.AddRange(await Dronovi.GetNjuskaloDronoviBjelovarskaMedimurskaVarazdinska());
            ////totalList.AddRange(await Dronovi.GetNjuskaloDronoviDubrovnikSplit());
            ////totalList.AddRange(await Dronovi.GetNjuskaloDronoviKarlovac());
            ////totalList.AddRange(await Dronovi.GetNjuskaloDronoviKrapinskoZagorska());
            ////totalList.AddRange(await Dronovi.GetNjuskaloDronoviRijekaIstra());
            ////totalList.AddRange(await Dronovi.GetNjuskaloDronoviSisackoMoslavacka());
            ////totalList.AddRange(await Dronovi.GetNjuskaloDronoviZadarLika());
            ////totalList.AddRange(await Dronovi.GetNjuskaloDronoviZagreb());

            totalList.AddRange(await Dronovi.DronoviIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));
            totalList.AddRange(await Dronovi.DronoviIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await Dronovi.DronoviIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await Dronovi.DronoviIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await Dronovi.DronoviIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await Dronovi.DronoviIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await Dronovi.DronoviIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await Dronovi.DronoviIndexZagreb(totalList, sentNumbers));
            //#endregion

            //#region Igre i igrače konzole
            ////totalList.AddRange(await Igre.GetNjuskaloIgreBjelovarskaMedimurskaVarazdinska());
            ////totalList.AddRange(await Igre.GetNjuskaloIgreDubrovnikSplit());
            ////totalList.AddRange(await Igre.GetNjuskaloIgreKarlovac());
            ////totalList.AddRange(await Igre.GetNjuskaloIgreKrapinskoZagorska());
            ////totalList.AddRange(await Igre.GetNjuskaloIgreRijekaIstra());
            ////totalList.AddRange(await Igre.GetNjuskaloIgreSisackoMoslavacka());
            ////totalList.AddRange(await Igre.GetNjuskaloIgreZadarLika());
            ////totalList.AddRange(await Igre.GetIgreLaptopiZagreb());

            totalList.AddRange(await Igre.IgreIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));
            totalList.AddRange(await Igre.IgreIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await Igre.IgreIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await Igre.IgreIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await Igre.IgreIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await Igre.IgreIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await Igre.IgreIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await Igre.IgreIndexZagreb(totalList, sentNumbers));
            ////#endregion

            ////#region Tableti
            //////totalList.AddRange(await Tableti.GetNjuskaloTabletiBjelovarskaMedimurskaVarazdinska());
            //////totalList.AddRange(await Tableti.GetNjuskaloTabletiDubrovnikSplit());
            //////totalList.AddRange(await Tableti.GetNjuskaloTabletiKarlovac());
            //////totalList.AddRange(await Tableti.GetNjuskaloTabletiKrapinskoZagorska());
            //////totalList.AddRange(await Tableti.GetNjuskaloTabletiRijekaIstra());
            //////totalList.AddRange(await Tableti.GetNjuskaloTabletiSisackoMoslavacka());
            //////totalList.AddRange(await Tableti.GetNjuskaloTabletiZadarLika());
            //////totalList.AddRange(await Tableti.GetNjuskaloTabletiZagreb());

            totalList.AddRange(await Tableti.TabletiIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));
            totalList.AddRange(await Tableti.TabletiIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await Tableti.TabletiIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await Tableti.TabletiIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await Tableti.TabletiIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await Tableti.TabletiIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await Tableti.TabletiIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await Tableti.TabletiIndexZagreb(totalList, sentNumbers));
            //////#endregion

            //////#region Laptopi
            ////////totalList.AddRange(await Laptopi.GetNjuskaloLaptopiBjelovarskaMedimurskaVarazdinska());
            ////////totalList.AddRange(await Laptopi.GetNjuskaloLaptopiDubrovnikSplit());
            ////////totalList.AddRange(await Laptopi.GetNjuskaloLaptopiKarlovac());
            ////////totalList.AddRange(await Laptopi.GetNjuskaloLaptopiKrapinskoZagorska());
            ////////totalList.AddRange(await Laptopi.GetNjuskaloLaptopiRijekaIstra());
            ////////totalList.AddRange(await Laptopi.GetNjuskaloLaptopiSisackoMoslavacka());
            ////////totalList.AddRange(await Laptopi.GetNjuskaloLaptopiZadarLika());
            ////////totalList.AddRange(await Laptopi.GetNjuskaloLaptopiZagreb());

            totalList.AddRange(await Laptopi.LaptopiIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));
            totalList.AddRange(await Laptopi.LaptopiIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await Laptopi.LaptopiIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await Laptopi.LaptopiIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await Laptopi.LaptopiIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await Laptopi.LaptopiIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await Laptopi.LaptopiIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await Laptopi.LaptopiIndexZagreb(totalList, sentNumbers));
            ////#endregion

            ////#region Mobiteli
            //////totalList.AddRange(await Mobiteli.GetNjuskaloMobiteliBjelovarskaMedimurskaVarazdinska());
            //////totalList.AddRange(await Mobiteli.GetNjuskaloMobiteliDubrovnikSplit());
            //////totalList.AddRange(await Mobiteli.GetNjuskaloMobiteliKarlovac());
            //////totalList.AddRange(await Mobiteli.GetNjuskaloMobiteliKrapinskoZagorska());
            //////totalList.AddRange(await Mobiteli.GetNjuskaloMobiteliRijekaIstra());
            //////totalList.AddRange(await Mobiteli.GetNjuskaloMobiteliSisackoMoslavacka());
            //////totalList.AddRange(await Mobiteli.GetNjuskaloMobiteliZadarLika());
            //////totalList.AddRange(await Mobiteli.GetNjuskaloMobiteliZagreb());

            //#region Samsung mobiteli index

            totalList.AddRange(await Mobiteli.MobiteliIPhoneIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await Mobiteli.MobiteliSamsungIndexKarlovac(totalList, sentNumbers));

            totalList.AddRange(await Mobiteli.MobiteliIPhoneIndexZagreb(totalList, sentNumbers));
            totalList.AddRange(await Mobiteli.MobiteliSamsungIndexZagreb(totalList, sentNumbers));

            totalList.AddRange(await Mobiteli.MobiteliIPhoneIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await Mobiteli.MobiteliSamsungIndexDubrovnikSplit(totalList, sentNumbers));

            totalList.AddRange(await Mobiteli.MobiteliIPhoneIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await Mobiteli.MobiteliSamsungIndexKrapinskoZagorska(totalList, sentNumbers));

            totalList.AddRange(await Mobiteli.MobiteliIPhoneIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await Mobiteli.MobiteliSamsungIndexRijekaIstra(totalList, sentNumbers));

            totalList.AddRange(await Mobiteli.MobiteliIPhoneIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await Mobiteli.MobiteliSamsungIndexSisackoMoslavacka(totalList, sentNumbers));

            totalList.AddRange(await Mobiteli.MobiteliIPhoneIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await Mobiteli.MobiteliSamsungIndexZadarLika(totalList, sentNumbers));

            totalList.AddRange(await Mobiteli.MobiteliIPhoneIndexZagreb(totalList, sentNumbers));
            totalList.AddRange(await Mobiteli.MobiteliSamsungIndexZagreb(totalList, sentNumbers));

            //#endregion
            //#endregion

            //#region TV
            ////totalList.AddRange(await TV.GetNjuskaloTVBjelovarskaMedimurskaVarazdinska());
            ////totalList.AddRange(await TV.GetNjuskaloTVDubrovnikSplit());
            ////totalList.AddRange(await TV.GetNjuskaloTVKarlovac());
            ////totalList.AddRange(await TV.GetNjuskaloTVKrapinskoZagorska());
            ////totalList.AddRange(await TV.GetNjuskaloTVRijekaIstra());
            ////totalList.AddRange(await TV.GetNjuskaloTVSisackoMoslavacka());
            ////totalList.AddRange(await TV.GetNjuskaloTVZadarLika());
            ////totalList.AddRange(await TV.GetNjuskaloTVZagreb());

            totalList.AddRange(await TV.TVIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));
            totalList.AddRange(await TV.TVIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await TV.TVIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await TV.TVIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await TV.TVIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await TV.TVIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await TV.TVIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await TV.TVIndexZagreb(totalList, sentNumbers));


            #region Video kamere
            //totalList.AddRange(await VideoCameras.GetNjuskaloVideoCameraBjelovarskaMedimurskaVarazdinska());
            //totalList.AddRange(await VideoCameras.GetNjuskaloVideoCameraDubrovnikSplit());
            //totalList.AddRange(await VideoCameras.GetNjuskaloVideoCameraKarlovac());
            //totalList.AddRange(await VideoCameras.GetNjuskaloVideoCameraKrapinskoZagorska());
            //totalList.AddRange(await VideoCameras.GetNjuskaloVideoCameraRijekaIstra());
            //totalList.AddRange(await VideoCameras.GetNjuskaloVideoCameraSisackoMoslavacka());
            //totalList.AddRange(await VideoCameras.GetNjuskaloVideoCameraZadarLika());
            //totalList.AddRange(await VideoCameras.GetNjuskaloVideoCameraZagreb());

            totalList.AddRange(await VideoCameras.VideoKameraIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));
            totalList.AddRange(await VideoCameras.VideoKameraIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await VideoCameras.VideoKameraIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await VideoCameras.VideoKameraIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await VideoCameras.VideoKameraIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await VideoCameras.VideoKameraIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await VideoCameras.VideoKameraIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await VideoCameras.VideKameraIndexZagreb(totalList, sentNumbers));
            #endregion

            #region Foto aparati
            //await Task.Run(() =>
            //{
            //    Parallel.Invoke(
            //        async () => totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiBjelovarskaMedimurskaVarazdinska()),
            //        async () => totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiDubrovnikSplit()),
            //        async () => totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiKarlovac()));
            //    //async () => totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiKrapinskoZagorska()),
            //    //async () => totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiRijekaIstra()),
            //    //async () => totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiSisackoMoslavacka()),
            //    //async () => totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiZadarLika()),
            //    //async () => totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiZagreb()));
            //});

            //totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiBjelovarskaMedimurskaVarazdinska());
            //return list;
            //totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiDubrovnikSplit());
            //await Task.Delay(200);
            //totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiKarlovac());
            //totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiKrapinskoZagorska());
            //totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiRijekaIstra());
            //totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiSisackoMoslavacka());
            //totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiZadarLika());
            //totalList.AddRange(await Fotoaparati.GetNjuskaloFotoaparatiZagreb());

            totalList.AddRange(await Fotoaparati.FotoaparatiIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));
            totalList.AddRange(await Fotoaparati.FotoaparatiIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await Fotoaparati.FotoaparatiIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await Fotoaparati.FotoaparatiIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await Fotoaparati.FotoaparatiIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await Fotoaparati.FotoaparatiIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await Fotoaparati.FotoaparatiIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await Fotoaparati.FotoaparatiIndexZagreb(totalList, sentNumbers));
            //#endregion

            //#region Foto aparati
            //totalList.AddRange(await AudioOprema.GetNjuskaloAudioBjelovarskaMedimurskaVarazdinska());
            //totalList.AddRange(await AudioOprema.GetNjuskaloAudioOpremaDubrovnikSplit());
            //totalList.AddRange(await AudioOprema.GetNjuskaloAudioOpremaKarlovac());
            //totalList.AddRange(await AudioOprema.GetNjuskaloAudioOpremaKrapinskoZagorska());
            //totalList.AddRange(await AudioOprema.GetNjuskaloAudioOpremaRijekaIstra());
            //totalList.AddRange(await AudioOprema.GetNjuskaloAudioOpremaSisackoMoslavacka());
            //totalList.AddRange(await AudioOprema.GetNjuskaloAudioOpremaZadarLika());
            //totalList.AddRange(await AudioOprema.GetNjuskaloAudioOpremaZagreb());

            totalList.AddRange(await AudioOprema.MikrofoniIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.SlusaliceIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.ZvucniciIndexBjelovarskaMedimurskaVarazdinska(totalList, sentNumbers));

            totalList.AddRange(await AudioOprema.MikrofonIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.SlusaliceIndexDubrovnikSplit(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.ZvucniciIndexDubrovnikSplit(totalList, sentNumbers));

            totalList.AddRange(await AudioOprema.MikrofonIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.SlusaliceIndexKarlovac(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.ZvucniciIndexKarlovac(totalList, sentNumbers));

            totalList.AddRange(await AudioOprema.MikrofonIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.SlusaliceIndexKrapinskoZagorska(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.ZvucniciIndexKrapinskoZagorska(totalList, sentNumbers));

            totalList.AddRange(await AudioOprema.MikrofonIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.SlusaliceIndexRijekaIstra(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.ZvucniciIndexRijekaIstra(totalList, sentNumbers));

            totalList.AddRange(await AudioOprema.MikrofoniIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.SlusaliceIndexSisackoMoslavacka(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.ZvucniciIndexSisackoMoslavacka(totalList, sentNumbers));

            totalList.AddRange(await AudioOprema.MikrofonIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.SlusaliceIndexZadarLika(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.ZvucniciIndexZadarLika(totalList, sentNumbers));

            totalList.AddRange(await AudioOprema.MikrofonIndexZagreb(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.SlusaliceIndexZagreb(totalList, sentNumbers));
            totalList.AddRange(await AudioOprema.ZvucniciIndexZagreb(totalList, sentNumbers));
            #endregion

            var count = totalList.DistinctBy(x => x.PhoneNumber).Count();

            var result = totalList.DistinctBy(x => x.PhoneNumber);

            Console.WriteLine("Hello, World!");
            Console.ReadKey();
        }
    }
}
