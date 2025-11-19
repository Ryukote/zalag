//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using HtmlAgilityPack;
//using MailKit;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using Microsoft.Playwright;
//using MimeKit;


//namespace NjuskaloTracker
//{
//    internal class Program
//    {
//        static IPManager ipManager = new IPManager();

//        static List<AdData> kuceZagreb = new List<AdData>();
//        static List<AdData> stanoviZagreb = new List<AdData>();
//        static List<AdData> zemljistaZagreb = new List<AdData>();
//        static List<AdData> poslovniProstoriZagreb = new List<AdData>();

//        static List<AdData> najamKucaZagrebLista = new List<AdData>();
//        static List<AdData> najamStanovaZagrebLista = new List<AdData>();
//        static List<AdData> najamZemljistaZagrebLista = new List<AdData>();
//        static List<AdData> najamPoslovnihProstoraZagrebLista = new List<AdData>();

//        static List<AdData> kuceNoviVinodolski = new List<AdData>();
//        static List<AdData> stanoviNoviVinodolski = new List<AdData>();
//        static List<AdData> zemljistaNoviVinodolski = new List<AdData>();
//        static List<AdData> poslovniProstoriNoviVinodolski = new List<AdData>();

//        static List<AdData> najamKucaNoviVinodolskiLista = new List<AdData>();
//        static List<AdData> najamStanovaNoviVinodolskiLista = new List<AdData>();
//        static List<AdData> najamZemljistaNoviVinodolskiLista = new List<AdData>();
//        static List<AdData> najamPoslovnihProstoraNoviVinodolskiLista = new List<AdData>();

//        static List<AdData> kuceKarlovac = new List<AdData>();
//        static List<AdData> stanoviKarlovac = new List<AdData>();
//        static List<AdData> zemljistaKarlovac = new List<AdData>();
//        static List<AdData> poslovniProstoriKarlovac = new List<AdData>();

//        static List<AdData> najamKucaKarlovacLista = new List<AdData>();
//        static List<AdData> najamStanovaKarlovacLista = new List<AdData>();
//        static List<AdData> najamZemljistaKarlovacLista = new List<AdData>();
//        static List<AdData> najamPoslovnihProstoraKarlovacLista = new List<AdData>();

//        static List<AdData> kuceCrikvenica = new List<AdData>();
//        static List<AdData> stanoviCrikvenica = new List<AdData>();
//        static List<AdData> zemljistaCrikvenica = new List<AdData>();
//        static List<AdData> poslovniProstoriCrikvenica = new List<AdData>();

//        static List<AdData> najamKucaCrikvenicaLista = new List<AdData>();
//        static List<AdData> najamStanovaCrikvenicaLista = new List<AdData>();
//        static List<AdData> najamZemljistaCrikvenicaLista = new List<AdData>();
//        static List<AdData> najamPoslovnihProstoraCrikvenicaLista = new List<AdData>();

//        static List<AdData> kuceKlenovica = new List<AdData>();
//        static List<AdData> stanoviKlenovica = new List<AdData>();
//        static List<AdData> zemljistaKlenovica = new List<AdData>();

//        static List<AdData> kuceZadarIOkolica = new List<AdData>();
//        static List<AdData> stanoviZadarIOkolica = new List<AdData>();
//        static List<AdData> zemljistaZadarIOkolica = new List<AdData>();
//        static List<AdData> poslovniProstoriZadarIOkolica = new List<AdData>();

//        static List<AdData> autiGeneralnoLista = new List<AdData>();

//        static string prodajaKucaZagreb = "https://www.njuskalo.hr/prodaja-kuca/zagreb";
//        static string prodajaStanovaZagreb = "https://www.njuskalo.hr/prodaja-stanova/zagreb";
//        static string prodajaZemljistaZagreb = "https://www.njuskalo.hr/prodaja-zemljista/zagreb";
//        static string prodajaPoslovnihProstoraZagreb = "https://www.njuskalo.hr/prodaja-poslovnih-prostora/zagreb";
//        static string prodajaKucaNoviVinodolski = "https://www.njuskalo.hr/prodaja-kuca/novi-vinodolski";
//        static string prodajaStanovaNoviVinodolski = "https://www.njuskalo.hr/prodaja-stanova/novi-vinodolski";
//        static string prodajaZemljistaNoviVinodolski = "https://www.njuskalo.hr/prodaja-zemljista/novi-vinodolski";
//        static string prodajaPoslovnihProstoraNoviVinodolski = "https://www.njuskalo.hr/prodaja-poslovnih-prostora/novi-vinodolski";
//        static string prodajaKucaKarlovac = "https://www.njuskalo.hr/prodaja-kuca/karlovacka";
//        static string prodajaStanovaKarlovac = "https://www.njuskalo.hr/prodaja-stanova/karlovacka";
//        static string prodajaZemljistaKarlovac = "https://www.njuskalo.hr/prodaja-zemljista/karlovacka";
//        static string prodajaPoslovnihProstoraKarlovac = "https://www.njuskalo.hr/prodaja-poslovnih-prostora/karlovacka";
//        static string prodajaKucaCrikvenica = "https://www.njuskalo.hr/prodaja-kuca/crikvenica";
//        static string prodajaStanovaCrikvenica = "https://www.njuskalo.hr/prodaja-stanova/crikvenica";
//        static string prodajaZemljistaCrikvenica = "https://www.njuskalo.hr/prodaja-zemljista/crikvenica";
//        static string prodajaPoslovnihProstoraCrikvenica = "https://www.njuskalo.hr/prodaja-poslovnih-prostora/crikvenica";
//        static string prodajaKuceZadarIOkolica = "https://www.njuskalo.hr/prodaja-kuca/zadarska";
//        static string prodajaStanovaZadarIOkolica = "https://www.njuskalo.hr/prodaja-stanova/zadarska";
//        static string prodajaZemljistaZadarIOkolica = "https://www.njuskalo.hr/prodaja-zemljista/zadarska";
//        static string prodajaPoslovnihProstoraZadarIOkolica = "https://www.njuskalo.hr/prodaja-poslovnih-prostora/zadarska";
//        static string prodajaKucaKlenovica = "https://www.njuskalo.hr/search/?keywords=klenovica+kuca";
//        static string prodajaStanovaKlenovica = "https://www.njuskalo.hr/search/?keywords=klenovica%2Bstan\r\n";
//        static string prodajaZemljistaKlenovica = "https://www.njuskalo.hr/search/?keywords=klenovica+zemljiste";

//        static string najamKucaZagreb = "https://www.njuskalo.hr/iznajmljivanje-kuca/zagreb";
//        static string najamStanovaZagreb = "https://www.njuskalo.hr/iznajmljivanje-stanova/zagreb";
//        static string najamZemljistaZagreb = "https://www.njuskalo.hr/zakup-zemljista/zagreb";
//        static string najamPoslovnihProstoraZagreb = "https://www.njuskalo.hr/iznajmljivanje-poslovnih-prostora/zagreb";

//        static string najamKucaNoviVinodolski = "https://www.njuskalo.hr/iznajmljivanje-kuca/novi-vinodolski";
//        static string najamStanovaNoviVinodolski = "https://www.njuskalo.hr/iznajmljivanje-stanova/novi-vinodolski";
//        static string najamZemljistaNoviVinodolski = "https://www.njuskalo.hr/zakup-zemljista/novi-vinodolski";
//        static string najamPoslovnihProstoraNoviVinodolski = "https://www.njuskalo.hr/iznajmljivanje-poslovnih-prostora/novi-vinodolski";

//        static string najamKucaKarlovac = "https://www.njuskalo.hr/iznajmljivanje-kuca/karlovacka";
//        static string najamStanovaKarlovac = "https://www.njuskalo.hr/iznajmljivanje-stanova/karlovacka";
//        static string najamZemljistaKarlovac = "https://www.njuskalo.hr/zakup-zemljista/karlovacka";
//        static string najamPoslovnihProstoraKarlovac = "https://www.njuskalo.hr/iznajmljivanje-poslovnih-prostora/karlovacka";

//        static string najamKucaCrikvenica = "https://www.njuskalo.hr/iznajmljivanje-kuca/crikvenica";
//        static string najamStanovaCrikvenica = "https://www.njuskalo.hr/iznajmljivanje-stanova/crikvenica";
//        static string najamZemljistaCrikvenica = "https://www.njuskalo.hr/zakup-zemljista/crikvenica";
//        static string najamPoslovnihProstoraCrikvenica = "https://www.njuskalo.hr/iznajmljivanje-poslovnih-prostora/crikvenica";

//        public Program()
//        {

//        }

//        private static List<string> authList = new List<string>
//        {
//            "brd-customer-hl_615a7fb5-zone-scraping_browser1:x609rxea4tb3"
//            //"brd-customer-hl_bf875d04-zone-scraping_browser1:ffnbmakvbvl9", //tinkrenic@gmail.com
//            //"brd-customer-hl_afaa2a27-zone-scraping_browser1:h4m9wzgxiakq" //milictomislav506@gmail.com
//        };

//        public static async Task<List<AdData>> GetNjuskaloData(string baseUrl, int page = 1)
//        {
//            List<AdData> scrapedAds = new List<AdData>(); // Global list to store results

//            if (authList.Count == 0)
//            {
//                Console.WriteLine("No AUTH values left to use.");
//                return scrapedAds; // Return already scraped data
//            }

//            string auth = authList.First();
//            string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";

//            try
//            {
//                Console.WriteLine($"Connecting with AUTH: {auth}...");
//                using var pw = await Playwright.CreateAsync();
//                await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
//                Console.WriteLine("Connected! Navigating...");

//                var pageInstance = await browser.NewPageAsync();
//                bool hasRecentAds = true;
//                bool checkedNextPage = false;
//                DateTime now = DateTime.Now;

//                while (true)
//                {
//                    string url = $"{baseUrl}?page={page}";
//                    await pageInstance.GotoAsync(url, new() { Timeout = 120000 });

//                    Console.WriteLine($"Scraping page {page}...");
//                    var html = await pageInstance.ContentAsync();
//                    var htmlDoc = new HtmlDocument();
//                    htmlDoc.LoadHtml(html);

//                    var ads = htmlDoc.DocumentNode.SelectNodes("//article");
//                    if (ads == null)
//                    {
//                        if (checkedNextPage) break;
//                        checkedNextPage = true;
//                        page++;
//                        continue;
//                    }

//                    hasRecentAds = false;
//                    foreach (var ad in ads)
//                    {

//                        var userIcon = ad.SelectSingleNode(".//span[contains(@class, 'icon icon--action icon--s icon--user')]");
//                        if (userIcon == null)
//                        {
//                            Console.WriteLine("Skipping ad without user icon.");
//                            continue;
//                        }

//                        var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'entity-pub-date')]/time");

//                        string datePostedStr = dateNode.GetAttributeValue("datetime", "");
//                        if (!DateTime.TryParse(datePostedStr, out DateTime datePosted))
//                        {
//                            Console.WriteLine("Skipping ad with invalid date format.");
//                            continue;
//                        }

//                        if ((now - datePosted).TotalHours > 24)
//                        {
//                            Console.WriteLine("Skipping old ad.");
//                            continue;
//                        }

//                        var titleNode = ad.SelectSingleNode(".//h3/a");
//                        var priceNode = ad.SelectSingleNode(".//strong[contains(@class, 'price price--hrk')]");
//                        var descNode = ad.SelectSingleNode(".//div[contains(@class, 'entity-description')]");
//                        var urlNode = ad.SelectSingleNode(".//h3/a");

//                        string title = titleNode?.InnerText.Trim() ?? "N/A";
//                        string price = priceNode?.InnerText.Trim() ?? "N/A";
//                        string description = descNode?.InnerText.Trim() ?? "N/A";
//                        string link = urlNode?.GetAttributeValue("href", "N/A");

//                        var adData = new AdData();
//                        adData.Title = title;
//                        adData.Price = price.Replace("&nbsp;", "");
//                        adData.Description = description;
//                        adData.URL = "https://www.njuskalo.hr" + link;
//                        adData.AdPostedOn = datePosted;

//                        adData = await GetNjuskaloData2(adData.URL, adData);

//                        if (adData.Views < 500)
//                        {
//                            scrapedAds.Add(adData);
//                            hasRecentAds = true;
//                        }
//                    }

//                    if (!hasRecentAds)
//                    {
//                        if (checkedNextPage) break;
//                        checkedNextPage = true;
//                        page++;
//                    }
//                    else
//                    {
//                        checkedNextPage = false;
//                        page++; // Move to next page
//                    }
//                }

//                await browser.CloseAsync();

//                await pageInstance.CloseAsync();

//                return scrapedAds;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error with AUTH {auth}: {ex.Message}");
//                return await GetNjuskaloData(baseUrl, page); // Retry with next AUTH, keeping the page number
//            }
//        }

//        //public static async Task<List<AdData>> GetNjuskaloData(string baseUrl, int page = 1)
//        //{
//        //    if (authList.Count == 0)
//        //    {
//        //        Console.WriteLine("No AUTH values left to use.");
//        //        return scrapedAds; // Return already scraped data
//        //    }

//        //    string auth = authList.First();
//        //    string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";

//        //    try
//        //    {
//        //        Console.WriteLine($"Connecting with AUTH: {auth}...");
//        //        using var pw = await Playwright.CreateAsync();
//        //        await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
//        //        Console.WriteLine("Connected! Navigating...");

//        //        var pageInstance = await browser.NewPageAsync();
//        //        bool hasRecentAds = true;
//        //        bool checkedNextPage = false;
//        //        DateTime now = DateTime.UtcNow;

//        //        while (true)
//        //        {
//        //            string url = $"{baseUrl}?page={page}";
//        //            await pageInstance.GotoAsync(url, new() { Timeout = 120000 });

//        //            Console.WriteLine($"Scraping page {page}...");
//        //            var html = await pageInstance.ContentAsync();
//        //            var htmlDoc = new HtmlDocument();
//        //            htmlDoc.LoadHtml(html);

//        //            var ads = htmlDoc.DocumentNode.SelectNodes("//article");
//        //            if (ads == null)
//        //            {
//        //                if (checkedNextPage) break;
//        //                checkedNextPage = true;
//        //                page++;
//        //                continue;
//        //            }

//        //            hasRecentAds = false;
//        //            foreach (var ad in ads)
//        //            {
//        //                if (ad.GetAttributeValue("class", "").Contains("EntityList-item-VauVau"))
//        //                {
//        //                    Console.WriteLine("Skipping VauVau ad.");
//        //                    continue;
//        //                }

//        //                var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'entity-pub-date')]/time");
//        //                if (dateNode == null)
//        //                {
//        //                    Console.WriteLine("Skipping ad without date.");
//        //                    continue;
//        //                }

//        //                string datePostedStr = dateNode.GetAttributeValue("datetime", "");
//        //                if (!DateTime.TryParse(datePostedStr, out DateTime datePosted))
//        //                {
//        //                    Console.WriteLine("Skipping ad with invalid date format.");
//        //                    continue;
//        //                }

//        //                if ((now - datePosted).TotalHours > 24)
//        //                {
//        //                    Console.WriteLine("Skipping old ad.");
//        //                    continue;
//        //                }

//        //                var titleNode = ad.SelectSingleNode(".//h3/a");
//        //                var priceNode = ad.SelectSingleNode(".//strong[contains(@class, 'price price--hrk')]");
//        //                var descNode = ad.SelectSingleNode(".//div[contains(@class, 'entity-description')]");
//        //                var urlNode = ad.SelectSingleNode(".//h3/a");
//        //                string title = titleNode?.InnerText.Trim() ?? "N/A";
//        //                string price = priceNode?.InnerText.Trim() ?? "N/A";
//        //                string description = descNode?.InnerText.Trim() ?? "N/A";
//        //                string link = urlNode?.GetAttributeValue("href", "N/A");

//        //                string fullUrl = "https://www.njuskalo.hr" + link;
//        //                if (await IsValidAd(fullUrl, pageInstance))
//        //                {
//        //                    scrapedAds.Add(new AdData()
//        //                    {
//        //                        Title = title,
//        //                        Price = price.Replace("&nbsp;", ""),
//        //                        Description = description,
//        //                        URL = fullUrl,
//        //                        AdPostedOn = datePosted
//        //                    });
//        //                    hasRecentAds = true;
//        //                }
//        //            }

//        //            if (!hasRecentAds)
//        //            {
//        //                if (checkedNextPage) break;
//        //                checkedNextPage = true;
//        //                page++;
//        //            }
//        //            else
//        //            {
//        //                checkedNextPage = false;
//        //                page++; // Move to next page
//        //            }
//        //        }

//        //        return scrapedAds;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Console.WriteLine($"Error with AUTH {auth}: {ex.Message}");
//        //        authList.Remove(auth); // Remove failing AUTH
//        //        return await GetNjuskaloData(baseUrl, page); // Retry with next AUTH, keeping the page number
//        //    }
//        //}

//        private static async Task<bool> IsValidAd(string url, IPage pageInstance)
//        {
//            await pageInstance.GotoAsync(url, new() { Timeout = 120000 });
//            var html = await pageInstance.ContentAsync();
//            var htmlDoc = new HtmlDocument();
//            htmlDoc.LoadHtml(html);

//            string pattern = @"(info|nekret|agen)";
//            string websitePattern = @"(https?://|www\\.)";

//            var ownerDetails = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'ClassifiedDetailsOwnerDetails')]")?.InnerText ?? "";
//            var descriptionDetails = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'ClassifiedDetailDescription-text')]")?.InnerText ?? "";

//            if (Regex.IsMatch(ownerDetails, pattern, RegexOptions.IgnoreCase) || Regex.IsMatch(ownerDetails, websitePattern, RegexOptions.IgnoreCase))
//                return false;
//            if (Regex.IsMatch(descriptionDetails, pattern, RegexOptions.IgnoreCase) || Regex.IsMatch(descriptionDetails, websitePattern, RegexOptions.IgnoreCase))
//                return false;

//            return true;
//        }

//        //private static List<string> authList = new List<string>
//        //{
//        //    "brd-customer-hl_b82cdcad-zone-scraping_browser1:im6tirhq6cgq",
//        //    "brd-customer-hl_bd91bc72-zone-scraping_browser1:5lsqm9su9f3a",
//        //    "brd-customer-hl_7f44d60e-zone-scraping_browser1:1cj16b8jwp9u"
//        //};

//        //public static async Task<List<AdData>> GetNjuskaloData(string baseUrl, int page = 1)
//        //{
//        //    var list = new List<AdData>();

//        //    if (authList.Count == 0)
//        //    {
//        //        Console.WriteLine("No AUTH values left to use.");
//        //        return list; // Return already scraped data
//        //    }

//        //    string auth = authList.First();
//        //    string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";

//        //    try
//        //    {
//        //        Console.WriteLine($"Connecting with AUTH: {auth}...");
//        //        using var pw = await Playwright.CreateAsync();
//        //        await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
//        //        Console.WriteLine("Connected! Navigating...");

//        //        var pageInstance = await browser.NewPageAsync();
//        //        bool hasRecentAds = true;

//        //        while (hasRecentAds)
//        //        {
//        //            string url = $"{baseUrl}?page={page}";
//        //            await pageInstance.GotoAsync(url, new() { Timeout = 120000 });

//        //            Console.WriteLine($"Scraping page {page}...");
//        //            var html = await pageInstance.ContentAsync();
//        //            var htmlDoc = new HtmlDocument();
//        //            htmlDoc.LoadHtml(html);

//        //            var ads = htmlDoc.DocumentNode.SelectNodes("//li[contains(@class, 'EntityList-item')]");
//        //            if (ads == null)
//        //            {
//        //                Console.WriteLine("No ads found on page.");
//        //                return list;
//        //            }

//        //            string today = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
//        //            hasRecentAds = false;

//        //            foreach (var ad in ads)
//        //            {
//        //                var titleNode = ad.SelectSingleNode(".//h3/a");
//        //                var priceNode = ad.SelectSingleNode(".//strong[contains(@class, 'price price--hrk')]");
//        //                var descNode = ad.SelectSingleNode(".//div[contains(@class, 'entity-description')]");
//        //                var urlNode = ad.SelectSingleNode(".//h3/a/@href");
//        //                var dateNode = ad.SelectSingleNode(".//time");

//        //                string title = titleNode?.InnerText.Trim() ?? "N/A";
//        //                string price = priceNode?.InnerText.Trim() ?? "N/A";
//        //                string description = descNode?.InnerText.Trim() ?? "N/A";
//        //                string link = urlNode?.GetAttributeValue("href", "N/A");
//        //                string datePosted = dateNode?.GetAttributeValue("datetime", "N/A") ?? dateNode?.InnerText.Trim() ?? "N/A";

//        //                if (!string.IsNullOrEmpty(link) && title != "No title" && datePosted.Contains(today))
//        //                {
//        //                    hasRecentAds = true;
//        //                    list.Add(new AdData()
//        //                    {
//        //                        Title = title,
//        //                        Price = price.Replace("&nbsp;", ""),
//        //                        Description = description,
//        //                        URL = "https://www.njuskalo.hr" + link,
//        //                        AdPostedOn = Convert.ToDateTime(datePosted),
//        //                    });
//        //                }
//        //            }

//        //            page++; // Move to next page
//        //        }

//        //        return list;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Console.WriteLine($"Error with AUTH {auth}: {ex.Message}");
//        //        authList.Remove(auth); // Remove failing AUTH
//        //        return await GetNjuskaloData(baseUrl, page); // Retry with next AUTH, keeping the page number
//        //    }
//        //}

//        //public static async Task<List<AdData>> GetNjuskaloData(string url)
//        //{
//        //    try
//        //    {
//        //        string html = "";
//        //        var list = new List<AdData>();

//        //        var AUTH = "brd-customer-hl_b82cdcad-zone-scraping_browser1:im6tirhq6cgq";
//        //        var SBR_CDP = $"wss://{AUTH}@brd.superproxy.io:9222";

//        //        Console.WriteLine("Connecting to Scraping Browser...");
//        //        using var pw = await Playwright.CreateAsync();
//        //        await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
//        //        Console.WriteLine("Connected! Navigating...");
//        //        var page = await browser.NewPageAsync();
//        //        var headers = new Dictionary<string, string>
//        //        {
//        //            { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36" },
//        //            { "Accept-Language", "en-US,en;q=0.9" }
//        //        };
//        //        //await page.SetExtraHTTPHeadersAsync(headers);
//        //        await page.GotoAsync(url, new()
//        //        {
//        //            Timeout = 2 * 60 * 1000,
//        //        });

//        //        Console.WriteLine("Navigated! Scraping page content...");

//        //        html = await page.ContentAsync();
//        //        Console.WriteLine("HTML content is scrapped");

//        //        // Parse with HtmlAgilityPack
//        //        var htmlDoc = new HtmlDocument();
//        //        htmlDoc.LoadHtml(html);
//        //        Console.WriteLine("HTML content is loaded");

//        //        // Extract data (e.g., all links)
//        //        string today = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

//        //        var ads = htmlDoc.DocumentNode.SelectNodes("//li[contains(@class, 'EntityList-item')]");

//        //        if (ads != null)
//        //        {
//        //            foreach (var ad in ads)
//        //            {
//        //                var titleNode = ad.SelectSingleNode(".//h3/a");
//        //                var priceNode = ad.SelectSingleNode(".//strong[contains(@class, 'price price--hrk')]");
//        //                var descNode = ad.SelectSingleNode(".//div[contains(@class, 'entity-description')]");
//        //                var urlNode = ad.SelectSingleNode(".//h3/a/@href");
//        //                var dateNode = ad.SelectSingleNode(".//time");

//        //                string title = titleNode?.InnerText.Trim() ?? "N/A";
//        //                string price = priceNode?.InnerText.Trim() ?? "N/A";
//        //                string description = descNode?.InnerText.Trim() ?? "N/A";
//        //                string link = urlNode?.GetAttributeValue("href", "N/A");
//        //                string datePosted = dateNode?.GetAttributeValue("datetime", "N/A") ?? dateNode?.InnerText.Trim() ?? "N/A";

//        //                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(?!outlook\.com$|gmail\.com$|yahoo\.com$)[a-zA-Z]{2,}$";
//        //                //if (!html.Contains("agent") && Regex.IsMatch(html, pattern))
//        //                //{
//        //                    if (!string.IsNullOrEmpty(link) && title != "No title" && datePosted.Contains(today))
//        //                    {
//        //                        list.Add(new AdData()
//        //                        {
//        //                            Title = title,
//        //                            Price = price.Replace("&nbsp;", ""),
//        //                            Description = description,
//        //                            URL = "https://www.njuskalo.hr" + link,
//        //                            AdPostedOn = Convert.ToDateTime(datePosted),
//        //                            //ImageUrls = lista
//        //                        });
//        //                    }
//        //                //}
//        //            }
//        //        }
//        //        else
//        //        {
//        //            Console.WriteLine("No ads found.");
//        //        }

//        //        return list;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        throw;
//        //    }
//        //}

//        public static async Task<AdData> GetNjuskaloData2(string url, AdData data)
//        {
//            try
//            {
//                var list = new List<AdData>();

//                string SBR_CDP = $"wss://brd-customer-hl_615a7fb5-zone-scraping_browser2:xlwc4xjelvsu@brd.superproxy.io:9222";

//                Console.WriteLine($"Connecting with AUTH: brd-customer-hl_615a7fb5-zone-scraping_browser2:xlwc4xjelvsu...");
//                using var pw = await Playwright.CreateAsync();
//                await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
//                Console.WriteLine("Connected! Navigating...");

//                var page = await browser.NewPageAsync();
//                //var headers = new Dictionary<string, string>
//                //{
//                //    { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36" },
//                //    { "Accept-Language", "en-US,en;q=0.9" }
//                //};
//                Console.WriteLine($"Scraping page {page}...");
//                var html = await page.ContentAsync();
//                var htmlDoc = new HtmlDocument();
//                htmlDoc.LoadHtml(html);
//                //await page.SetExtraHTTPHeadersAsync(headers);
//                await page.GotoAsync(url, new()
//                {
//                    Timeout = 10000,
//                });

//                Console.WriteLine("Navigated! Scraping page content...");

//                html = await page.ContentAsync();
//                Console.WriteLine("HTML content is scrapped");

//                // Parse with HtmlAgilityPack
//                htmlDoc.LoadHtml(html);
//                Console.WriteLine("HTML content is loaded");

//                // Extract data (e.g., all links)
//                string today = DateTime.Now.ToString("yyyy-MM-dd");

//                //var titleNode = ad.SelectSingleNode(".//h3/a");
//                //var priceNode = ad.SelectSingleNode(".//strong[contains(@class, 'price price--hrk')]");
//                //var descNode = ad.SelectSingleNode(".//div[contains(@class, 'entity-description')]");
//                //var urlNode = ad.SelectSingleNode(".//h3/a/@href");
//                //var dateNode = ad.SelectSingleNode(".//time");

//                //string title = titleNode?.InnerText.Trim() ?? "N/A";
//                //string price = priceNode?.InnerText.Trim() ?? "N/A";
//                //string description = descNode?.InnerText.Trim() ?? "N/A";
//                //string link = urlNode?.GetAttributeValue("href", "N/A");
//                //string datePosted = dateNode?.GetAttributeValue("datetime", "N/A") ?? dateNode?.InnerText.Trim() ?? "N/A";

//                // Extract Ad Views: Find dt with "Oglas prikazan" and get the following dd
//                var viewsNode = htmlDoc.DocumentNode.SelectSingleNode("//dt[contains(text(), 'Oglas prikazan')]/following-sibling::dd[1]");
//                string viewsText = viewsNode?.InnerText.Trim() ?? "Not Found";
//                viewsText = viewsText.Replace(" puta", "");
//                var viewCounts = Convert.ToInt32(viewsText);

//                // Extract Phone Number (if available)
//                var phoneNode = htmlDoc.DocumentNode.SelectSingleNode("//p[contains(@class, 'UserPhoneNumber-phoneText')]");
//                string phoneNumber = phoneNode?.InnerText.Trim() ?? "Not Found";

//                // Extract Location
//                var locationNode = htmlDoc.DocumentNode.SelectSingleNode("//span[contains(@class, 'ClassifiedDetailBasicDetails-textWrapContainer')]");
//                string locationText = locationNode?.InnerText.Trim() ?? "Not Found";

//                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(?!outlook\.com$|gmail\.com$|yahoo\.com$)[a-zA-Z]{2,}$";

//                data.Views = viewCounts;
//                data.Location = locationText;
//                data.PhoneNumber = phoneNumber;

//                return data;
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        static async System.Threading.Tasks.Task Main(string[] args)
//        {
//            kuceZagreb = await GetNjuskaloData(prodajaKucaZagreb);
//            stanoviZagreb = await GetNjuskaloData(prodajaStanovaZagreb);
//            zemljistaZagreb = await GetNjuskaloData(prodajaZemljistaZagreb);
//            poslovniProstoriZagreb = await GetNjuskaloData(prodajaPoslovnihProstoraZagreb);

//            najamKucaZagrebLista = await GetNjuskaloData(najamKucaZagreb);
//            najamStanovaZagrebLista = await GetNjuskaloData(najamStanovaZagreb);
//            najamZemljistaZagrebLista = await GetNjuskaloData(najamZemljistaZagreb);
//            najamPoslovnihProstoraZagrebLista = await GetNjuskaloData(najamPoslovnihProstoraZagreb);

//            kuceKarlovac = await GetNjuskaloData(prodajaKucaKarlovac);
//            stanoviKarlovac = await GetNjuskaloData(prodajaStanovaKarlovac);
//            zemljistaKarlovac = await GetNjuskaloData(prodajaZemljistaKarlovac);
//            poslovniProstoriKarlovac = await GetNjuskaloData(prodajaPoslovnihProstoraKarlovac);

//            najamKucaKarlovacLista = await GetNjuskaloData(najamKucaKarlovac);
//            najamStanovaKarlovacLista = await GetNjuskaloData(najamStanovaKarlovac);
//            najamZemljistaKarlovacLista = await GetNjuskaloData(najamZemljistaKarlovac);
//            najamPoslovnihProstoraKarlovacLista = await GetNjuskaloData(najamPoslovnihProstoraKarlovac);

//            kuceNoviVinodolski = await GetNjuskaloData(prodajaKucaNoviVinodolski);
//            stanoviNoviVinodolski = await GetNjuskaloData(prodajaStanovaNoviVinodolski);
//            zemljistaNoviVinodolski = await GetNjuskaloData(prodajaZemljistaNoviVinodolski);
//            poslovniProstoriNoviVinodolski = await GetNjuskaloData(prodajaPoslovnihProstoraNoviVinodolski);

//            najamKucaNoviVinodolskiLista = await GetNjuskaloData(najamKucaNoviVinodolski);
//            najamStanovaNoviVinodolskiLista = await GetNjuskaloData(najamStanovaNoviVinodolski);
//            najamZemljistaNoviVinodolskiLista = await GetNjuskaloData(najamZemljistaNoviVinodolski);
//            najamPoslovnihProstoraNoviVinodolskiLista = await GetNjuskaloData(najamPoslovnihProstoraNoviVinodolski);

//            kuceCrikvenica = await GetNjuskaloData(prodajaKucaCrikvenica);
//            stanoviCrikvenica = await GetNjuskaloData(prodajaStanovaCrikvenica);
//            zemljistaCrikvenica = await GetNjuskaloData(prodajaZemljistaCrikvenica);
//            poslovniProstoriCrikvenica = await GetNjuskaloData(prodajaPoslovnihProstoraCrikvenica);

//            najamKucaCrikvenicaLista = await GetNjuskaloData(najamKucaCrikvenica);
//            najamStanovaCrikvenicaLista = await GetNjuskaloData(najamStanovaCrikvenica);
//            najamZemljistaCrikvenicaLista = await GetNjuskaloData(najamZemljistaCrikvenica);
//            najamPoslovnihProstoraCrikvenicaLista = await GetNjuskaloData(najamPoslovnihProstoraCrikvenica);

//            string[] Scopes = { "https://mail.google.com/" };



//            #region Kuće Zagreb
//            var emailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var emailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var contentBuilder = new StringBuilder();

//            foreach (var item in kuceZagreb)
//            {
//                var current = emailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                contentBuilder.Append(current);
//            }

//            var kuceZagrebTemplate = emailTemplate;
//            kuceZagrebTemplate = kuceZagrebTemplate.Replace(@"{category}", "Novi oglasi za kuće u Zagrebu");
//            kuceZagrebTemplate = kuceZagrebTemplate.Replace(@"{content}", contentBuilder.ToString());

//            var kuceZagrebMimeMessage = new MimeMessage();

//            kuceZagrebMimeMessage.To.Add(new MailboxAddress($"Luka", "pex.luka1@gmail.com"));
//            kuceZagrebMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            kuceZagrebMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            kuceZagrebMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            kuceZagrebMimeMessage.Subject = "Njuškalo - svježi oglasi kuća u Zagrebu";

//            kuceZagrebMimeMessage.Body = new TextPart("html")
//            {
//                Text = kuceZagrebTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(kuceZagrebMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Najam Kuće Zagreb
//            var emailTemplateNajamKucaZagreb = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var emailOglasItemNajamKucaZagreb = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var contentBuilderNajamKucaZagreb = new StringBuilder();

//            foreach (var item in najamKucaZagrebLista)
//            {
//                var current = emailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                contentBuilder.Append(current);
//            }

//            var najamKuceZagrebTemplate = emailTemplateNajamKucaZagreb;
//            najamKuceZagrebTemplate = najamKuceZagrebTemplate.Replace(@"{category}", "Novi oglasi za najam kuća u Zagrebu");
//            najamKuceZagrebTemplate = najamKuceZagrebTemplate.Replace(@"{content}", contentBuilder.ToString());

//            var najamKuceZagrebMimeMessage = new MimeMessage();

//            najamKuceZagrebMimeMessage.To.Add(new MailboxAddress($"Luka", "pex.luka1@gmail.com"));
//            najamKuceZagrebMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamKuceZagrebMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamKuceZagrebMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamKuceZagrebMimeMessage.Subject = "Njuškalo - svježi oglasi najma kuća u Zagrebu";

//            najamKuceZagrebMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamKuceZagrebTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamKuceZagrebMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Stanovi Zagreb
//            var stanoviZagrebEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var stanoviZagrebEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var stanoviZagrebContentBuilder = new StringBuilder();

//            foreach (var item in stanoviZagreb)
//            {
//                var current = stanoviZagrebEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                stanoviZagrebContentBuilder.Append(current);
//            }

//            var stanoviZagrebTemplate = stanoviZagrebEmailTemplate;
//            stanoviZagrebTemplate = stanoviZagrebTemplate.Replace(@"{category}", "Novi oglasi za stanove u Zagrebu");
//            stanoviZagrebTemplate = stanoviZagrebTemplate.Replace(@"{content}", stanoviZagrebContentBuilder.ToString());

//            var stanoviZagrebMimeMessage = new MimeMessage();

//            stanoviZagrebMimeMessage.To.Add(new MailboxAddress($"Luka", "pex.luka1@gmail.com"));
//            //stanoviZagrebMimeMessage.To.Add(new MailboxAddress($"zoml", "info@zoml.si"));
//            stanoviZagrebMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            stanoviZagrebMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            stanoviZagrebMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            stanoviZagrebMimeMessage.Subject = "Njuškalo - svježi oglasi stanova u Zagrebu";

//            stanoviZagrebMimeMessage.Body = new TextPart("html")
//            {
//                Text = stanoviZagrebTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(stanoviZagrebMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Najam Stanovi Zagreb
//            var najamStanoviZagrebEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var najamStanoviZagrebEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var najamStanoviZagrebContentBuilder = new StringBuilder();

//            foreach (var item in najamStanovaZagrebLista)
//            {
//                var current = najamStanoviZagrebEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                najamStanoviZagrebContentBuilder.Append(current);
//            }

//            var najamStanoviZagrebTemplate = najamStanoviZagrebEmailTemplate;
//            najamStanoviZagrebTemplate = najamStanoviZagrebTemplate.Replace(@"{category}", "Novi oglasi za najam stanova u Zagrebu");
//            najamStanoviZagrebTemplate = najamStanoviZagrebTemplate.Replace(@"{content}", najamStanoviZagrebContentBuilder.ToString());

//            var najamStanoviZagrebMimeMessage = new MimeMessage();

//            najamStanoviZagrebMimeMessage.To.Add(new MailboxAddress($"Luka", "pex.luka1@gmail.com"));
//            najamStanoviZagrebMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamStanoviZagrebMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamStanoviZagrebMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamStanoviZagrebMimeMessage.Subject = "Njuškalo - svježi oglasi najma stanova u Zagrebu";

//            najamStanoviZagrebMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamStanoviZagrebEmailTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamStanoviZagrebMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Zemljišta Zagreb
//            var zemljistaZagrebEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var zemljistaZagrebEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var zemljistaZagrebContentBuilder = new StringBuilder();

//            foreach (var item in zemljistaZagreb)
//            {
//                var current = zemljistaZagrebEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                zemljistaZagrebContentBuilder.Append(current);
//            }

//            var zemljistaZagrebTemplate = zemljistaZagrebEmailTemplate;
//            zemljistaZagrebTemplate = zemljistaZagrebTemplate.Replace(@"{category}", "Novi oglasi za zemljišta u Zagrebu");
//            zemljistaZagrebTemplate = zemljistaZagrebTemplate.Replace(@"{content}", zemljistaZagrebContentBuilder.ToString());

//            var zemljistaZagrebMimeMessage = new MimeMessage();

//            zemljistaZagrebMimeMessage.To.Add(new MailboxAddress($"Luka", "pex.luka1@gmail.com"));
//            zemljistaZagrebMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            zemljistaZagrebMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            zemljistaZagrebMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            zemljistaZagrebMimeMessage.Subject = "Njuškalo - svježi oglasi zemljišta u Zagrebu";

//            zemljistaZagrebMimeMessage.Body = new TextPart("html")
//            {
//                Text = zemljistaZagrebTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(zemljistaZagrebMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Najam Zemljišta Zagreb
//            var najamZemljistaZagrebEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var najamZemljistaZagrebEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var najamZemljistaZagrebContentBuilder = new StringBuilder();

//            foreach (var item in najamZemljistaZagrebLista)
//            {
//                var current = najamZemljistaZagrebEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                najamZemljistaZagrebContentBuilder.Append(current);
//            }

//            var najamZemljistaZagrebTemplate = najamZemljistaZagrebEmailTemplate;
//            najamZemljistaZagrebTemplate = najamZemljistaZagrebTemplate.Replace(@"{category}", "Novi oglasi za najam zemljišta u Zagrebu");
//            najamZemljistaZagrebTemplate = najamZemljistaZagrebTemplate.Replace(@"{content}", najamZemljistaZagrebContentBuilder.ToString());

//            var najamZemljistaZagrebMimeMessage = new MimeMessage();

//            najamZemljistaZagrebMimeMessage.To.Add(new MailboxAddress($"Luka", "pex.luka1@gmail.com"));
//            najamZemljistaZagrebMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamZemljistaZagrebMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamZemljistaZagrebMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamZemljistaZagrebMimeMessage.Subject = "Njuškalo - svježi oglasi najma zemljišta u Zagrebu";

//            najamZemljistaZagrebMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamZemljistaZagrebTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamZemljistaZagrebMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Poslovni prostori Zagreb
//            var najamPoslovniProstoriZagrebEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var najamPoslovniProstoriZagrebEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var najamPoslovniProstoriZagrebContentBuilder = new StringBuilder();

//            foreach (var item in najamPoslovnihProstoraZagrebLista)
//            {
//                var current = najamPoslovniProstoriZagrebEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                najamPoslovniProstoriZagrebContentBuilder.Append(current);
//            }

//            var najamPoslovniProstoriZagrebTemplate = najamPoslovniProstoriZagrebEmailTemplate;
//            najamPoslovniProstoriZagrebTemplate = najamPoslovniProstoriZagrebTemplate.Replace(@"{category}", "Novi oglasi za najam poslovnih prostora u Zagrebu");
//            najamPoslovniProstoriZagrebTemplate = najamPoslovniProstoriZagrebTemplate.Replace(@"{content}", najamPoslovniProstoriZagrebContentBuilder.ToString());

//            var najamPoslovniProstoriZagrebMimeMessage = new MimeMessage();

//            najamPoslovniProstoriZagrebMimeMessage.To.Add(new MailboxAddress($"Luka", "pex.luka1@gmail.com"));
//            najamPoslovniProstoriZagrebMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamPoslovniProstoriZagrebMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamPoslovniProstoriZagrebMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamPoslovniProstoriZagrebMimeMessage.Subject = "Njuškalo - svježi oglasi najma poslovnih prostora u Zagrebu";

//            najamPoslovniProstoriZagrebMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamPoslovniProstoriZagrebTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamPoslovniProstoriZagrebMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Kuće Karlovac
//            var kuceKarlovacEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var kuceKarlovacEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var kuceKarlovacContentBuilder = new StringBuilder();

//            foreach (var item in kuceKarlovac)
//            {
//                var current = kuceKarlovacEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                kuceKarlovacContentBuilder.Append(current);
//            }

//            var kuceKarlovacTemplate = kuceKarlovacEmailTemplate;
//            kuceKarlovacTemplate = kuceKarlovacTemplate.Replace(@"{category}", "Novi oglasi za kuće u Karlovcu");
//            kuceKarlovacTemplate = kuceKarlovacTemplate.Replace(@"{content}", kuceKarlovacContentBuilder.ToString());

//            var kuceKarlovacMimeMessage = new MimeMessage();

//            kuceKarlovacMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            kuceKarlovacMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            kuceKarlovacMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            kuceKarlovacMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            kuceKarlovacMimeMessage.Subject = "Njuškalo - svježi oglasi kuća u Karlovcu";

//            kuceKarlovacMimeMessage.Body = new TextPart("html")
//            {
//                Text = kuceKarlovacTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(kuceKarlovacMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Najam Kuće Karlovac
//            var najamKuceKarlovacEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var najamKuceKarlovacEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var najamKuceKarlovacContentBuilder = new StringBuilder();

//            foreach (var item in najamKucaKarlovacLista)
//            {
//                var current = najamKuceKarlovacEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                najamKuceKarlovacContentBuilder.Append(current);
//            }

//            var najamKuceKarlovacTemplate = kuceKarlovacEmailTemplate;
//            najamKuceKarlovacTemplate = najamKuceKarlovacTemplate.Replace(@"{category}", "Novi oglasi za najam kuća u Karlovcu");
//            najamKuceKarlovacTemplate = najamKuceKarlovacTemplate.Replace(@"{content}", kuceKarlovacContentBuilder.ToString());

//            var najamKuceKarlovacMimeMessage = new MimeMessage();

//            najamKuceKarlovacMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            najamKuceKarlovacMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamKuceKarlovacMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamKuceKarlovacMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamKuceKarlovacMimeMessage.Subject = "Njuškalo - svježi oglasi najma kuća u Karlovcu";

//            najamKuceKarlovacMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamKuceKarlovacTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamKuceKarlovacMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Stanovi Karlovac
//            var stanoviKarlovacEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var stanoviKarlovacEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var stanoviKarlovacContentBuilder = new StringBuilder();

//            foreach (var item in stanoviKarlovac)
//            {
//                var current = stanoviKarlovacEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                stanoviKarlovacContentBuilder.Append(current);
//            }

//            var stanoviKarlovacTemplate = stanoviKarlovacEmailTemplate;
//            stanoviKarlovacTemplate = stanoviKarlovacTemplate.Replace(@"{category}", "Novi oglasi za stanove u Karlovcu");
//            stanoviKarlovacTemplate = stanoviKarlovacTemplate.Replace(@"{content}", stanoviKarlovacContentBuilder.ToString());

//            var stanoviKarlovacMimeMessage = new MimeMessage();

//            stanoviKarlovacMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            stanoviKarlovacMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            stanoviKarlovacMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            stanoviKarlovacMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            stanoviKarlovacMimeMessage.Subject = "Njuškalo - svježi oglasi stanova u Karlovcu";

//            stanoviKarlovacMimeMessage.Body = new TextPart("html")
//            {
//                Text = stanoviKarlovacTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(stanoviKarlovacMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Zemljišta Karlovac
//            var najamZemljistaKarlovacEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var najamZemljistaKarlovacEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var najamZemljistaKarlovacContentBuilder = new StringBuilder();

//            foreach (var item in najamZemljistaKarlovacLista)
//            {
//                var current = najamZemljistaKarlovacEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                najamZemljistaKarlovacContentBuilder.Append(current);
//            }

//            var najamZemljistaKarlovacTemplate = najamZemljistaKarlovacEmailTemplate;
//            najamZemljistaKarlovacTemplate = najamZemljistaKarlovacTemplate.Replace(@"{category}", "Novi oglasi za najam zemljišta u Karlovcu");
//            najamZemljistaKarlovacTemplate = najamZemljistaKarlovacTemplate.Replace(@"{content}", najamZemljistaKarlovacContentBuilder.ToString());

//            var najamZemljistaKarlovacMimeMessage = new MimeMessage();

//            najamZemljistaKarlovacMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            najamZemljistaKarlovacMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamZemljistaKarlovacMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamZemljistaKarlovacMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamZemljistaKarlovacMimeMessage.Subject = "Njuškalo - svježi oglasi najma zemljišta u Karlovcu";

//            najamZemljistaKarlovacMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamZemljistaKarlovacTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamZemljistaKarlovacMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Poslovni prostori Karlovac
//            var poslovniProstoriKarlovacEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var poslovniProstoriKarlovacEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var poslovniProstoriKarlovacContentBuilder = new StringBuilder();

//            foreach (var item in poslovniProstoriKarlovac)
//            {
//                var current = poslovniProstoriKarlovacEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                poslovniProstoriKarlovacContentBuilder.Append(current);
//            }

//            var poslovniProstoriKarlovacTemplate = poslovniProstoriKarlovacEmailTemplate;
//            poslovniProstoriKarlovacTemplate = poslovniProstoriKarlovacTemplate.Replace(@"{category}", "Novi oglasi za poslovne prostore u Karlovcu");
//            poslovniProstoriKarlovacTemplate = poslovniProstoriKarlovacTemplate.Replace(@"{content}", poslovniProstoriKarlovacContentBuilder.ToString());

//            var poslovniProstoriKarlovacMimeMessage = new MimeMessage();

//            poslovniProstoriKarlovacMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            poslovniProstoriKarlovacMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            poslovniProstoriKarlovacMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            poslovniProstoriKarlovacMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            poslovniProstoriKarlovacMimeMessage.Subject = "Njuškalo - svježi oglasi poslovnih prostora u Karlovcu";

//            poslovniProstoriKarlovacMimeMessage.Body = new TextPart("html")
//            {
//                Text = poslovniProstoriKarlovacTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(poslovniProstoriKarlovacMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Najam Poslovni prostori Karlovac
//            var najamPoslovniProstoriKarlovacEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var najamPoslovniProstoriKarlovacEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var najamPoslovniProstoriKarlovacContentBuilder = new StringBuilder();

//            foreach (var item in najamPoslovnihProstoraKarlovacLista)
//            {
//                var current = najamPoslovniProstoriKarlovacEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                najamPoslovniProstoriKarlovacContentBuilder.Append(current);
//            }

//            var najamPoslovniProstoriKarlovacTemplate = najamPoslovniProstoriKarlovacEmailTemplate;
//            najamPoslovniProstoriKarlovacTemplate = najamPoslovniProstoriKarlovacTemplate.Replace(@"{category}", "Novi oglasi za najam poslovnih prostore u Karlovcu");
//            najamPoslovniProstoriKarlovacTemplate = najamPoslovniProstoriKarlovacTemplate.Replace(@"{content}", poslovniProstoriKarlovacContentBuilder.ToString());

//            var najamPoslovniProstoriKarlovacMimeMessage = new MimeMessage();

//            najamPoslovniProstoriKarlovacMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            najamPoslovniProstoriKarlovacMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamPoslovniProstoriKarlovacMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamPoslovniProstoriKarlovacMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamPoslovniProstoriKarlovacMimeMessage.Subject = "Njuškalo - svježi oglasi najma poslovnih prostora u Karlovcu";

//            najamPoslovniProstoriKarlovacMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamPoslovniProstoriKarlovacTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamPoslovniProstoriKarlovacMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Kuće Novi Vinodolski
//            var kuceNoviVinodolskiEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var kuceNoviVinodolskiEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var kuceNoviVinodolskiContentBuilder = new StringBuilder();

//            foreach (var item in kuceNoviVinodolski)
//            {
//                var current = kuceNoviVinodolskiEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                kuceNoviVinodolskiContentBuilder.Append(current);
//            }

//            var kuceNoviVinodolskiTemplate = kuceNoviVinodolskiEmailTemplate;
//            kuceNoviVinodolskiTemplate = kuceNoviVinodolskiTemplate.Replace(@"{category}", "Novi oglasi za kuće u Novom Vinodolskom");
//            kuceNoviVinodolskiTemplate = kuceNoviVinodolskiTemplate.Replace(@"{content}", kuceNoviVinodolskiContentBuilder.ToString());

//            var kuceNoviVinodolskiMimeMessage = new MimeMessage();

//            kuceNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            kuceNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            kuceNoviVinodolskiMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            kuceNoviVinodolskiMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            kuceNoviVinodolskiMimeMessage.Subject = "Njuškalo - svježi oglasi kuća u Novom Vinodolskom";

//            kuceNoviVinodolskiMimeMessage.Body = new TextPart("html")
//            {
//                Text = kuceNoviVinodolskiTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(kuceNoviVinodolskiMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Najam Kuće Novi Vinodolski
//            var najamKuceNoviVinodolskiEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var najamKuceNoviVinodolskiEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var najamKuceNoviVinodolskiContentBuilder = new StringBuilder();

//            foreach (var item in najamKucaNoviVinodolskiLista)
//            {
//                var current = najamKuceNoviVinodolskiEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                najamKuceNoviVinodolskiContentBuilder.Append(current);
//            }

//            var najamKuceNoviVinodolskiTemplate = najamKuceNoviVinodolskiEmailTemplate;
//            najamKuceNoviVinodolskiTemplate = najamKuceNoviVinodolskiTemplate.Replace(@"{category}", "Novi oglasi za najam kuća u Novom Vinodolskom");
//            najamKuceNoviVinodolskiTemplate = najamKuceNoviVinodolskiTemplate.Replace(@"{content}", kuceNoviVinodolskiContentBuilder.ToString());

//            var najamKuceNoviVinodolskiMimeMessage = new MimeMessage();

//            najamKuceNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            najamKuceNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamKuceNoviVinodolskiMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamKuceNoviVinodolskiMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamKuceNoviVinodolskiMimeMessage.Subject = "Njuškalo - svježi oglasi najma kuća u Novom Vinodolskom";

//            najamKuceNoviVinodolskiMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamKuceNoviVinodolskiTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamKuceNoviVinodolskiMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Stanovi Novi Vinodolski
//            var stanoviNoviVinodolskiEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var stanoviNoviVinodolskiEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var stanoviNoviVinodolskiContentBuilder = new StringBuilder();

//            foreach (var item in stanoviNoviVinodolski)
//            {
//                var current = stanoviNoviVinodolskiEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                stanoviNoviVinodolskiContentBuilder.Append(current);
//            }

//            var stanoviNoviVinodolskiTemplate = stanoviNoviVinodolskiEmailTemplate;
//            stanoviNoviVinodolskiTemplate = stanoviNoviVinodolskiTemplate.Replace(@"{category}", "Novi oglasi za stanove u Novom Vinodolskom");
//            stanoviNoviVinodolskiTemplate = stanoviNoviVinodolskiTemplate.Replace(@"{content}", stanoviNoviVinodolskiContentBuilder.ToString());

//            var stanoviNoviVinodolskiMimeMessage = new MimeMessage();

//            stanoviNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            stanoviNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            stanoviNoviVinodolskiMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            stanoviNoviVinodolskiMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            stanoviNoviVinodolskiMimeMessage.Subject = "Njuškalo - svježi oglasi stanova u Novom Vinodolskom";

//            stanoviNoviVinodolskiMimeMessage.Body = new TextPart("html")
//            {
//                Text = stanoviNoviVinodolskiTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(stanoviNoviVinodolskiMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Najam Stanovi Novi Vinodolski
//            var najamStanoviNoviVinodolskiEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var najamStanoviNoviVinodolskiEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var najamStanoviNoviVinodolskiContentBuilder = new StringBuilder();

//            foreach (var item in najamStanovaNoviVinodolskiLista)
//            {
//                var current = najamStanoviNoviVinodolskiEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                najamStanoviNoviVinodolskiContentBuilder.Append(current);
//            }

//            var najamStanoviNoviVinodolskiTemplate = najamStanoviNoviVinodolskiEmailTemplate;
//            najamStanoviNoviVinodolskiTemplate = najamStanoviNoviVinodolskiTemplate.Replace(@"{category}", "Novi oglasi za najam stanova u Novom Vinodolskom");
//            najamStanoviNoviVinodolskiTemplate = najamStanoviNoviVinodolskiTemplate.Replace(@"{content}", najamStanoviNoviVinodolskiContentBuilder.ToString());

//            var najamStanoviNoviVinodolskiMimeMessage = new MimeMessage();

//            najamStanoviNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            najamStanoviNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamStanoviNoviVinodolskiMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamStanoviNoviVinodolskiMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamStanoviNoviVinodolskiMimeMessage.Subject = "Njuškalo - svježi oglasi najma stanova u Novom Vinodolskom";

//            najamStanoviNoviVinodolskiMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamStanoviNoviVinodolskiTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamStanoviNoviVinodolskiMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Zemljišta Novi Vinodolski
//            var zemljistaNoviVinodolskiEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var zemljistaNoviVinodolskiEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var zemljistaNoviVinodolskiContentBuilder = new StringBuilder();

//            foreach (var item in zemljistaNoviVinodolski)
//            {
//                var current = zemljistaNoviVinodolskiEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                zemljistaNoviVinodolskiContentBuilder.Append(current);
//            }

//            var zemljistaNoviVinodolskiTemplate = zemljistaNoviVinodolskiEmailTemplate;
//            zemljistaNoviVinodolskiTemplate = zemljistaNoviVinodolskiTemplate.Replace(@"{category}", "Novi oglasi za zemljišta u Novom Vinodolskom");
//            zemljistaNoviVinodolskiTemplate = zemljistaNoviVinodolskiTemplate.Replace(@"{content}", zemljistaNoviVinodolskiContentBuilder.ToString());

//            var zemljistaNoviVinodolskiMimeMessage = new MimeMessage();

//            zemljistaNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            zemljistaNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            zemljistaNoviVinodolskiMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            zemljistaNoviVinodolskiMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            zemljistaNoviVinodolskiMimeMessage.Subject = "Njuškalo - svježi oglasi zemljišta u Novom Vinodolskom";

//            zemljistaNoviVinodolskiMimeMessage.Body = new TextPart("html")
//            {
//                Text = zemljistaNoviVinodolskiTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(zemljistaNoviVinodolskiMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Najam Zemljišta Novi Vinodolski
//            var najamZemljistaNoviVinodolskiEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var najamZemljistaNoviVinodolskiEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var najamZemljistaNoviVinodolskiContentBuilder = new StringBuilder();

//            foreach (var item in najamZemljistaNoviVinodolskiLista)
//            {
//                var current = najamZemljistaNoviVinodolskiEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                najamZemljistaNoviVinodolskiContentBuilder.Append(current);
//            }

//            var najamZemljistaNoviVinodolskiTemplate = najamZemljistaNoviVinodolskiEmailTemplate;
//            najamZemljistaNoviVinodolskiTemplate = najamZemljistaNoviVinodolskiTemplate.Replace(@"{category}", "Novi oglasi za najam zemljišta u Novom Vinodolskom");
//            najamZemljistaNoviVinodolskiTemplate = najamZemljistaNoviVinodolskiTemplate.Replace(@"{content}", najamZemljistaNoviVinodolskiContentBuilder.ToString());

//            var najamZemljistaNoviVinodolskiMimeMessage = new MimeMessage();

//            najamZemljistaNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            najamZemljistaNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamZemljistaNoviVinodolskiMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamZemljistaNoviVinodolskiMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamZemljistaNoviVinodolskiMimeMessage.Subject = "Njuškalo - svježi oglasi najma zemljišta u Novom Vinodolskom";

//            najamZemljistaNoviVinodolskiMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamZemljistaNoviVinodolskiTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamZemljistaNoviVinodolskiMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Poslovni prostori Novi Vinodolski
//            var poslovniProstoriNoviVinodolskiEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var poslovniProstoriNoviVinodolskiEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var poslovniProstoriNoviVinodolskiContentBuilder = new StringBuilder();

//            foreach (var item in poslovniProstoriNoviVinodolski)
//            {
//                var current = poslovniProstoriNoviVinodolskiEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                poslovniProstoriNoviVinodolskiContentBuilder.Append(current);
//            }

//            var poslovniProstoriNoviVinodolskiTemplate = poslovniProstoriNoviVinodolskiEmailTemplate;
//            poslovniProstoriNoviVinodolskiTemplate = poslovniProstoriNoviVinodolskiTemplate.Replace(@"{category}", "Novi oglasi za poslovne prostore u Novom Vinodolskom");
//            poslovniProstoriNoviVinodolskiTemplate = poslovniProstoriNoviVinodolskiTemplate.Replace(@"{content}", poslovniProstoriNoviVinodolskiContentBuilder.ToString());

//            var poslovniProstoriNoviVinodolskiMimeMessage = new MimeMessage();

//            poslovniProstoriNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            poslovniProstoriNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            poslovniProstoriNoviVinodolskiMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            poslovniProstoriNoviVinodolskiMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            poslovniProstoriNoviVinodolskiMimeMessage.Subject = "Njuškalo - svježi oglasi poslovnih prostora u Novom Vinodolskom";

//            poslovniProstoriNoviVinodolskiMimeMessage.Body = new TextPart("html")
//            {
//                Text = poslovniProstoriNoviVinodolskiTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(poslovniProstoriNoviVinodolskiMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Najam Poslovni prostori Novi Vinodolski
//            var najamPoslovniProstoriNoviVinodolskiEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var najamPoslovniProstoriNoviVinodolskiEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var najamPoslovniProstoriNoviVinodolskiContentBuilder = new StringBuilder();

//            foreach (var item in najamPoslovnihProstoraNoviVinodolskiLista)
//            {
//                var current = najamPoslovniProstoriNoviVinodolskiEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                najamPoslovniProstoriNoviVinodolskiContentBuilder.Append(current);
//            }

//            var najamPoslovniProstoriNoviVinodolskiTemplate = najamPoslovniProstoriNoviVinodolskiEmailTemplate;
//            najamPoslovniProstoriNoviVinodolskiTemplate = najamPoslovniProstoriNoviVinodolskiTemplate.Replace(@"{category}", "Novi oglasi za najam poslovnih prostora u Novom Vinodolskom");
//            najamPoslovniProstoriNoviVinodolskiTemplate = najamPoslovniProstoriNoviVinodolskiTemplate.Replace(@"{content}", najamPoslovniProstoriNoviVinodolskiContentBuilder.ToString());

//            var najamPoslovniProstoriNoviVinodolskiMimeMessage = new MimeMessage();

//            najamPoslovniProstoriNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            najamPoslovniProstoriNoviVinodolskiMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamPoslovniProstoriNoviVinodolskiMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamPoslovniProstoriNoviVinodolskiMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamPoslovniProstoriNoviVinodolskiMimeMessage.Subject = "Njuškalo - svježi oglasi najma poslovnih prostora u Novom Vinodolskom";

//            najamPoslovniProstoriNoviVinodolskiMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamPoslovniProstoriNoviVinodolskiTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamPoslovniProstoriNoviVinodolskiMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Kuće Crikvenica
//            var kuceCrikvenicaEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var kuceCrikvenicaEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var kuceCrikvenicaContentBuilder = new StringBuilder();

//            foreach (var item in kuceCrikvenica)
//            {
//                var current = kuceCrikvenicaEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                kuceCrikvenicaContentBuilder.Append(current);
//            }

//            var kuceCrikvenicaTemplate = kuceCrikvenicaEmailTemplate;
//            kuceCrikvenicaTemplate = kuceCrikvenicaTemplate.Replace(@"{category}", "Novi oglasi za kuće u Crikvenici");
//            kuceCrikvenicaTemplate = kuceCrikvenicaTemplate.Replace(@"{content}", kuceCrikvenicaContentBuilder.ToString());

//            var kuceCrikvenicaMimeMessage = new MimeMessage();

//            kuceCrikvenicaMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            kuceCrikvenicaMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            kuceCrikvenicaMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            kuceCrikvenicaMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            kuceCrikvenicaMimeMessage.Subject = "Njuškalo - svježi oglasi kuća u Crikvenici";

//            kuceCrikvenicaMimeMessage.Body = new TextPart("html")
//            {
//                Text = kuceCrikvenicaTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(kuceCrikvenicaMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Najam Kuće Crikvenica
//            var najamKuceCrikvenicaEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var najamKuceCrikvenicaEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var najamKuceCrikvenicaContentBuilder = new StringBuilder();

//            foreach (var item in najamKucaCrikvenicaLista)
//            {
//                var current = najamKuceCrikvenicaEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                najamKuceCrikvenicaContentBuilder.Append(current);
//            }

//            var najamKuceCrikvenicaTemplate = najamKuceCrikvenicaEmailTemplate;
//            najamKuceCrikvenicaTemplate = najamKuceCrikvenicaTemplate.Replace(@"{category}", "Novi oglasi za najam kuća u Crikvenici");
//            najamKuceCrikvenicaTemplate = najamKuceCrikvenicaTemplate.Replace(@"{content}", najamKuceCrikvenicaContentBuilder.ToString());

//            var najamKuceCrikvenicaMimeMessage = new MimeMessage();

//            najamKuceCrikvenicaMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            najamKuceCrikvenicaMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamKuceCrikvenicaMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamKuceCrikvenicaMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamKuceCrikvenicaMimeMessage.Subject = "Njuškalo - svježi oglasi najma kuća u Crikvenici";

//            najamKuceCrikvenicaMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamKuceCrikvenicaTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamKuceCrikvenicaMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Stanovi Crikvenica
//            var stanoviCrikvenicaEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var stanoviCrikvenicaEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var stanoviCrikvenicaContentBuilder = new StringBuilder();

//            foreach (var item in stanoviCrikvenica)
//            {
//                var current = stanoviCrikvenicaEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                stanoviCrikvenicaContentBuilder.Append(current);
//            }

//            var stanoviCrikvenicaTemplate = stanoviCrikvenicaEmailTemplate;
//            stanoviCrikvenicaTemplate = stanoviCrikvenicaTemplate.Replace(@"{category}", "Novi oglasi za stanove u Crikvenici");
//            stanoviCrikvenicaTemplate = stanoviCrikvenicaTemplate.Replace(@"{content}", stanoviCrikvenicaContentBuilder.ToString());

//            var stanoviCrikvenicaMimeMessage = new MimeMessage();

//            stanoviCrikvenicaMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            stanoviCrikvenicaMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            stanoviCrikvenicaMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            stanoviCrikvenicaMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            stanoviCrikvenicaMimeMessage.Subject = "Njuškalo - svježi oglasi stanova u Crikvenici";

//            stanoviCrikvenicaMimeMessage.Body = new TextPart("html")
//            {
//                Text = stanoviCrikvenicaTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(stanoviCrikvenicaMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Najam Stanova Crikvenica
//            var najamStanoviCrikvenicaEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var najamStanoviCrikvenicaEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var najamStanoviCrikvenicaContentBuilder = new StringBuilder();

//            foreach (var item in najamStanovaCrikvenicaLista)
//            {
//                var current = najamStanoviCrikvenicaEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                najamStanoviCrikvenicaContentBuilder.Append(current);
//            }

//            var najamStanoviCrikvenicaTemplate = najamStanoviCrikvenicaEmailTemplate;
//            najamStanoviCrikvenicaTemplate = najamStanoviCrikvenicaTemplate.Replace(@"{category}", "Novi oglasi za najam stanova u Crikvenici");
//            najamStanoviCrikvenicaTemplate = najamStanoviCrikvenicaTemplate.Replace(@"{content}", najamStanoviCrikvenicaContentBuilder.ToString());

//            var najamStanoviCrikvenicaMimeMessage = new MimeMessage();

//            najamStanoviCrikvenicaMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            najamStanoviCrikvenicaMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamStanoviCrikvenicaMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamStanoviCrikvenicaMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamStanoviCrikvenicaMimeMessage.Subject = "Njuškalo - svježi oglasi najma stanova u Crikvenici";

//            najamStanoviCrikvenicaMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamStanoviCrikvenicaTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamStanoviCrikvenicaMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Zemljišta Crikvenica
//            var zemljistaCrikvenicaEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var zemljistaCrikvenicaEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var zemljistaCrikvenicaContentBuilder = new StringBuilder();

//            foreach (var item in zemljistaCrikvenica)
//            {
//                var current = zemljistaCrikvenicaEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                zemljistaCrikvenicaContentBuilder.Append(current);
//            }

//            var zemljistaCrikvenicaTemplate = zemljistaCrikvenicaEmailTemplate;
//            zemljistaCrikvenicaTemplate = zemljistaCrikvenicaTemplate.Replace(@"{category}", "Novi oglasi za zemljišta u Crikvenici");
//            zemljistaCrikvenicaTemplate = zemljistaCrikvenicaTemplate.Replace(@"{content}", zemljistaCrikvenicaTemplate.ToString());

//            var zemljistaCrikvenicaMimeMessage = new MimeMessage();

//            zemljistaCrikvenicaMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            zemljistaCrikvenicaMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            zemljistaCrikvenicaMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            zemljistaCrikvenicaMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            zemljistaCrikvenicaMimeMessage.Subject = "Njuškalo - svježi oglasi zemljišta u Crikvenici";

//            zemljistaCrikvenicaMimeMessage.Body = new TextPart("html")
//            {
//                Text = zemljistaCrikvenicaTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(zemljistaCrikvenicaMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Najam Zemljišta Crikvenica
//            var najamZemljistaCrikvenicaEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var najamZemljistaCrikvenicaEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var najamZemljistaCrikvenicaContentBuilder = new StringBuilder();

//            foreach (var item in najamZemljistaCrikvenicaLista)
//            {
//                var current = najamZemljistaCrikvenicaEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                zemljistaCrikvenicaContentBuilder.Append(current);
//            }

//            var najamZemljistaCrikvenicaTemplate = najamZemljistaCrikvenicaEmailTemplate;
//            najamZemljistaCrikvenicaTemplate = najamZemljistaCrikvenicaTemplate.Replace(@"{category}", "Novi oglasi za najam zemljišta u Crikvenici");
//            najamZemljistaCrikvenicaTemplate = najamZemljistaCrikvenicaTemplate.Replace(@"{content}", najamZemljistaCrikvenicaTemplate.ToString());

//            var najamZemljistaCrikvenicaMimeMessage = new MimeMessage();

//            najamZemljistaCrikvenicaMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            najamZemljistaCrikvenicaMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamZemljistaCrikvenicaMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamZemljistaCrikvenicaMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamZemljistaCrikvenicaMimeMessage.Subject = "Njuškalo - svježi oglasi najma zemljišta u Crikvenici";

//            najamZemljistaCrikvenicaMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamZemljistaCrikvenicaTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamZemljistaCrikvenicaMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Poslovni prostori Crikvenica
//            var poslovniProstoriCrikvenicaEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var poslovniProstoriCrikvenicaEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var poslovniProstoriCrikvenicaContentBuilder = new StringBuilder();

//            foreach (var item in poslovniProstoriCrikvenica)
//            {
//                var current = poslovniProstoriCrikvenicaEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                poslovniProstoriCrikvenicaContentBuilder.Append(current);
//            }

//            var poslovniProstoriCrikvenicaTemplate = poslovniProstoriCrikvenicaEmailTemplate;
//            poslovniProstoriCrikvenicaTemplate = poslovniProstoriCrikvenicaTemplate.Replace(@"{category}", "Novi oglasi za poslovne prostore u Crikvenici");
//            poslovniProstoriCrikvenicaTemplate = poslovniProstoriCrikvenicaTemplate.Replace(@"{content}", poslovniProstoriCrikvenicaContentBuilder.ToString());

//            var poslovniProstoriCrikvenicaMimeMessage = new MimeMessage();

//            poslovniProstoriCrikvenicaMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            poslovniProstoriCrikvenicaMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            poslovniProstoriCrikvenicaMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            poslovniProstoriCrikvenicaMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            poslovniProstoriCrikvenicaMimeMessage.Subject = "Njuškalo - svježi oglasi poslovnih prostora u Crikvenici";

//            poslovniProstoriCrikvenicaMimeMessage.Body = new TextPart("html")
//            {
//                Text = poslovniProstoriCrikvenicaTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(poslovniProstoriCrikvenicaMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            #region Najam Poslovni prostori Crikvenica
//            var najamPoslovniProstoriCrikvenicaEmailTemplate = await File.ReadAllTextAsync("./Email/emailtemplate.html");
//            var najamPoslovniProstoriCrikvenicaEmailOglasItem = await File.ReadAllTextAsync("./Email/emailItem.html");

//            var najamPoslovniProstoriCrikvenicaContentBuilder = new StringBuilder();

//            foreach (var item in najamPoslovnihProstoraCrikvenicaLista)
//            {
//                var current = najamPoslovniProstoriCrikvenicaEmailOglasItem;

//                current = current.Replace(@"{title}", item.Title);
//                current = current.Replace(@"{description}", item.Description);
//                current = current.Replace(@"{link}", item.URL);
//                current = current.Replace(@"{price}", item.Price);

//                najamPoslovniProstoriCrikvenicaContentBuilder.Append(current);
//            }

//            var najamPoslovniProstoriCrikvenicaTemplate = najamPoslovniProstoriCrikvenicaEmailTemplate;
//            najamPoslovniProstoriCrikvenicaTemplate = najamPoslovniProstoriCrikvenicaTemplate.Replace(@"{category}", "Novi oglasi za najam poslovnih prostora u Crikvenici");
//            najamPoslovniProstoriCrikvenicaTemplate = najamPoslovniProstoriCrikvenicaTemplate.Replace(@"{content}", najamPoslovniProstoriCrikvenicaContentBuilder.ToString());

//            var najamPoslovniProstoriCrikvenicaMimeMessage = new MimeMessage();

//            najamPoslovniProstoriCrikvenicaMimeMessage.To.Add(new MailboxAddress($"T. Gregurec", "tgregurec2@gmail.com"));
//            najamPoslovniProstoriCrikvenicaMimeMessage.To.Add(new MailboxAddress($"Antonio Halužan", "ryukote@outlook.com"));
//            najamPoslovniProstoriCrikvenicaMimeMessage.From.Add(new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com"));
//            najamPoslovniProstoriCrikvenicaMimeMessage.Sender = new MailboxAddress($"Antonio Halužan", "svjezioglasi@gmail.com");
//            najamPoslovniProstoriCrikvenicaMimeMessage.Subject = "Njuškalo - svježi oglasi najma poslovnih prostora u Crikvenici";

//            najamPoslovniProstoriCrikvenicaMimeMessage.Body = new TextPart("html")
//            {
//                Text = najamPoslovniProstoriCrikvenicaTemplate.ToString()
//            };

//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);

//                client.Authenticate("svjezioglasi@gmail.com", "ldbqecgbfyilhonj");

//                client.Send(najamPoslovniProstoriCrikvenicaMimeMessage);
//                client.Disconnect(true);
//            }
//            #endregion

//            Console.ReadKey();
//        }
//    }
//}
