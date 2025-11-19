//using HtmlAgilityPack;
//using System;
//using System.Linq;
//using static System.Net.Mime.MediaTypeNames;
//using System.Text.RegularExpressions;
//using System.Xml.Linq;
//using TestTracker;
//using Microsoft.Playwright;
//using System.Threading.Tasks;
//using System.Reflection.PortableExecutable;
//using NjuskaloTracker;

//class Program
//{
//    private static List<string> authList = new List<string>
//    {
//        "brd-customer-hl_615a7fb5-zone-scraping_browser1:x609rxea4tb3"
//        //"brd-customer-hl_bf875d04-zone-scraping_browser1:ffnbmakvbvl9", //tinkrenic@gmail.com
//        //"brd-customer-hl_afaa2a27-zone-scraping_browser1:h4m9wzgxiakq" //milictomislav506@gmail.com

//    };

//    static HashSet<string> visitedSellers = new HashSet<string>();

//    public static async Task<List<AdData>> GetNjuskaloData(string baseUrl, int page = 1)
//    {
//        List<AdData> scrapedAds = new List<AdData>(); // Global list to store results

//        if (authList.Count == 0)
//        {
//            Console.WriteLine("No AUTH values left to use.");
//            return scrapedAds; // Return already scraped data
//        }

//        string auth = authList.First();
//        string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";

//        try
//        {
//            Console.WriteLine($"Connecting with AUTH: {auth}...");
//            using var pw = await Playwright.CreateAsync();
//            await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
//            Console.WriteLine("Connected! Navigating...");

//            var pageInstance = await browser.NewPageAsync();
//            bool hasRecentAds = true;
//            bool checkedNextPage = false;
//            DateTime now = DateTime.Now;

//            while (true)
//            {
//                string url = $"{baseUrl}";
//                await pageInstance.GotoAsync(url, new() { Timeout = 120000 });

//                Console.WriteLine($"Scraping page {page}...");
//                var html = await pageInstance.ContentAsync();
//                //await pageInstance.CloseAsync();
//                var htmlDoc = new HtmlDocument();
//                htmlDoc.LoadHtml(html);

//                var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");

//                pageInstance.Request += async (sender, request) =>
//                {
//                    //if (request.Url.Contains("tvoj-url.com")) // Zamijeni s ciljnim URL-om
//                    //{
//                    Console.WriteLine($"Request to: {request.Url}");
//                    var headers = await request.AllHeadersAsync();

//                    if (headers.ContainsKey("authorization"))
//                    {
//                        Console.WriteLine($"Authorization header: {headers["authorization"]}");
//                    }
//                    //}
//                };

//                //if (ads != null)
//                //{
//                //    foreach (var ad in ads)
//                //    {
//                //        // Skip promoted ads
//                //        if (ad.SelectSingleNode(".//div[contains(@class, 'adCard__promoted')]") != null)
//                //            continue;

//                //        // Extract title
//                //        var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
//                //        string title = titleNode?.InnerText.Trim() ?? "N/A";

//                //        // Extract price
//                //        var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
//                //        string price = priceNode?.InnerText.Trim() ?? "N/A";

//                //        // Extract date of publishing
//                //        var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
//                //        string date = dateNode?.InnerText.Trim() ?? "N/A";

//                //        // Extract location
//                //        var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
//                //        string location = locationNode?.InnerText.Trim() ?? "N/A";

//                //        // Extract link
//                //        var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
//                //        string? link = linkNode?.GetAttributeValue("href", "N/A");

//                //        Console.WriteLine($"Title: {title}");
//                //        Console.WriteLine($"Price: {price}");
//                //        Console.WriteLine($"Date: {date}");
//                //        Console.WriteLine($"Location: {location}");
//                //        Console.WriteLine($"Link: {link}");
//                //        Console.WriteLine(new string('-', 40));

//                //        await GetNjuskaloData2($"https://www.index.hr{link}", new AdData());
//                //    }
//                //}
//                //else
//                //{
//                //    Console.WriteLine("No ads found.");
//                //}

//                //var ads = await pageInstance.Locator("div.ant-col.ant-col-xs-24").AllAsync();
//                Console.WriteLine($"Total ads found: {ads.Count}");

//                foreach (var ad in ads)
//                {
//                    //var title = ad.SelectNodes(".//div[contains(@class, 'adCard__promoted')]");
//                    ////.TextContentAsync() ?? "N/A";
//                    //var price = ad.SelectNodes(".adPrice__price");
//                    //    //.TextContentAsync() ?? "N/A";
//                    //var date = ad.SelectNodes(".AdSummary__info___ span");
//                    ////.TextContentAsync() ?? "N/A";
//                    //var location = ad.SelectNodes(".adLocation__location");
//                    ////.TextContentAsync() ?? "N/A";
//                    //var link =  ad.SelectNodes(".AdLink__link");
//                    ////.GetAttributeAsync("href") ?? "N/A";
//                    ///
//                    var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
//                    string title = titleNode?.InnerText.Trim() ?? "N/A";

//                    // Dohvati cijenu
//                    var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
//                    string price = priceNode?.InnerText.Trim() ?? "N/A";

//                    // Dohvati datum objave
//                    var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
//                    string date = dateNode?.InnerText.Trim() ?? "N/A";

//                    // Dohvati lokaciju
//                    var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
//                    string location = locationNode?.InnerText.Trim() ?? "N/A";

//                    // Dohvati link oglasa
//                    var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
//                    string? link = linkNode?.GetAttributeValue("href", "N/A");

//                    Console.WriteLine($"Title: {title}");
//                    Console.WriteLine($"Price: {price}");
//                    Console.WriteLine($"Date: {date}");
//                    Console.WriteLine($"Location: {location}");
//                    Console.WriteLine($"Link: {link}");
//                    Console.WriteLine("--------------------------------");


//                    await GetNjuskaloData2($"https://www.index.hr{link}", new AdData());
//                }

//                //if (!hasRecentAds)
//                //{
//                //    if (checkedNextPage) break;
//                //    checkedNextPage = true;
//                //    page++;
//                //}
//                //else
//                //{
//                //    checkedNextPage = false;
//                //    page++; // Move to next page
//                //}
//            }

//            //await browser.CloseAsync();

//            //await pageInstance.CloseAsync();

//            //return scrapedAds;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error with AUTH {auth}: {ex.Message}");
//            return await GetNjuskaloData(baseUrl, page); // Retry with next AUTH, keeping the page number
//        }
//    }

//    private static async Task<bool> IsValidAd(string url, IPage pageInstance)
//    {
//        await pageInstance.GotoAsync(url, new() { Timeout = 120000 });
//        var html = await pageInstance.ContentAsync();
//        var htmlDoc = new HtmlDocument();
//        htmlDoc.LoadHtml(html);

//        string pattern = @"(info|nekret|agen)";
//        string websitePattern = @"(https?://|www\\.)";

//        var ownerDetails = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'ClassifiedDetailsOwnerDetails')]")?.InnerText ?? "";
//        var descriptionDetails = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'ClassifiedDetailDescription-text')]")?.InnerText ?? "";

//        if (Regex.IsMatch(ownerDetails, pattern, RegexOptions.IgnoreCase) || Regex.IsMatch(ownerDetails, websitePattern, RegexOptions.IgnoreCase))
//            return false;
//        if (Regex.IsMatch(descriptionDetails, pattern, RegexOptions.IgnoreCase) || Regex.IsMatch(descriptionDetails, websitePattern, RegexOptions.IgnoreCase))
//            return false;

//        return true;
//    }

//    public static async Task<AdData> GetNjuskaloData2(string url, AdData data)
//    {
//        try
//        {
//            var b = url;
//            //var list = new List<AdData>();

//            string SBR_CDP = $"wss://brd-customer-hl_615a7fb5-zone-scraping_browser2:xlwc4xjelvsu@brd.superproxy.io:9222";

//            Console.WriteLine($"Connecting with AUTH2: brd-customer-hl_615a7fb5-zone-scraping_browser2:xlwc4xjelvsu...");
//            using var pw = await Playwright.CreateAsync();
//            var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
//            //Console.WriteLine("Connected! Navigating...");

//            ////using var playwright = await Playwright.CreateAsync();
//            ////await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
//            var page = await browser.NewPageAsync();
//            await page.GotoAsync(url);

//            string content = await page.ContentAsync();

//            //// Load content into HtmlAgilityPack
//            var htmlDoc = new HtmlDocument();
//            htmlDoc.LoadHtml(content);

//            //var ads = htmlDoc.DocumentNode.SelectNodes("//article");
//            //var username = htmlDoc.DocumentNode.SelectNodes("//div[class*='SellerInfo__username']");
//            var username = htmlDoc.DocumentNode.SelectSingleNode(".//div[contains(@class, 'SellerInfo__username')]").InnerHtml;
//            var a1 = username;
//            //var username = await htmlDoc.Locator("div[class*='SellerInfo__username']").TextContentAsync();
//            //username = username?.Trim();

//            //if (string.IsNullOrEmpty(username))
//            //{
//            //    Console.WriteLine("Username not found.");
//            //}

//            Console.WriteLine($"Found seller: {username}");

//            //// Check if seller is already visited
//            if (visitedSellers.Contains(username))
//            {
//                Console.WriteLine($"Skipping already visited seller: {username}");
//            }
//            else
//            {
//                visitedSellers.Add(username);
//                Console.WriteLine($"New seller found, visiting their listings: {username}");

//                // Click on "Pogledaj sve oglase" link
//                var allAdsLink = htmlDoc.DocumentNode.SelectSingleNode(".//a[contains(@class, 'sellerLink')]");

//                string pattern = @"href\s*=\s*""([^""]+)""";
//                Match match = Regex.Match(allAdsLink.OuterHtml, pattern);

//                if (match.Success)
//                {
//                    Console.WriteLine("Extracted href: " + match.Groups[1].Value);
//                    var userDetails = match.Groups[1].Value;

//                    //string SBR_CDP = $"wss://brd-customer-hl_615a7fb5-zone-scraping_browser2:xlwc4xjelvsu@brd.superproxy.io:9222";

//                    Console.WriteLine($"Connecting with AUTH2: brd-customer-hl_615a7fb5-zone-scraping_browser2:xlwc4xjelvsu...");
//                    using var pw2 = await Playwright.CreateAsync();
//                    var browser2 = await pw2.Chromium.ConnectOverCDPAsync(SBR_CDP);

//                    var page2 = await browser2.NewPageAsync();
//                    await page2.GotoAsync($"https://www.index.hr{userDetails}");

//                    string content2 = await page2.ContentAsync();

//                    var htmlDoc2 = new HtmlDocument();
//                    htmlDoc2.LoadHtml(content2);

//                    var phones = htmlDoc2.DocumentNode.SelectNodes("//span[contains(@class, 'SellerSectionWeb__infoValue')]");

//                    if (phones != null)
//                    {
//                        foreach (var phone in phones)
//                        {
//                            string href = phone.InnerText;

//                            // Check if the number starts with "09" after "tel:"
//                            if (href.Contains("09"))
//                            {
//                                string pattern2 = @"^0"; // Regex koji traži 0 na početku stringa
//                                string replacement = "+385";

//                                string result = Regex.Replace(href, pattern2, replacement);
//                                Console.WriteLine($"Found number: {href}");
//                            }
//                        }
//                    }
//                    else
//                    {
//                        Console.WriteLine("No matching links found.");
//                    }

//                }
//                else
//                {
//                    Console.WriteLine("No match found.");
//                }

//                await FindPhoneNumber();
//                //var allAdsLink = await page.GetAttributeAsync("a[class*='SellerInfo__sellerLink']", "href");
//                //if (!string.IsNullOrEmpty(allAdsLink))
//                //{
//                //    await page.GotoAsync($"https://www.index.hr{allAdsLink}");
//                //    //await page.GotoAsync(allAdsLink);
//                //    Console.WriteLine($"Navigated to: {allAdsLink}");
//                //}
//                //else
//                //{
//                //    Console.WriteLine("Seller's listing page not found.");
//                //}
//            }

//            //await browser.CloseAsync();

//            ////var page = await browser.NewPageAsync();
//            ////var headers = new Dictionary<string, string>
//            ////{
//            ////    { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36" },
//            ////    { "Accept-Language", "en-US,en;q=0.9" }
//            ////};
//            //Console.WriteLine($"Scraping page {page}...");
//            //var html = await page.ContentAsync();
//            ////var htmlDoc = new HtmlDocument();
//            //htmlDoc.LoadHtml(html);
//            ////await page.SetExtraHTTPHeadersAsync(headers);
//            //await page.GotoAsync(url, new()
//            //{
//            //    Timeout = 10000,
//            //});

//            //Console.WriteLine("Navigated! Scraping page content...");

//            //html = await page.ContentAsync();
//            //Console.WriteLine("HTML content is scrapped");

//            //// Parse with HtmlAgilityPack
//            //htmlDoc.LoadHtml(html);
//            //Console.WriteLine("HTML content is loaded");

//            //// Extract data (e.g., all links)
//            //string today = DateTime.Now.ToString("yyyy-MM-dd");

//            ////var titleNode = ad.SelectSingleNode(".//h3/a");
//            ////var priceNode = ad.SelectSingleNode(".//strong[contains(@class, 'price price--hrk')]");
//            ////var descNode = ad.SelectSingleNode(".//div[contains(@class, 'entity-description')]");
//            ////var urlNode = ad.SelectSingleNode(".//h3/a/@href");
//            ////var dateNode = ad.SelectSingleNode(".//time");

//            ////string title = titleNode?.InnerText.Trim() ?? "N/A";
//            ////string price = priceNode?.InnerText.Trim() ?? "N/A";
//            ////string description = descNode?.InnerText.Trim() ?? "N/A";
//            ////string link = urlNode?.GetAttributeValue("href", "N/A");
//            ////string datePosted = dateNode?.GetAttributeValue("datetime", "N/A") ?? dateNode?.InnerText.Trim() ?? "N/A";

//            //// Extract Ad Views: Find dt with "Oglas prikazan" and get the following dd
//            ////var viewsNode = htmlDoc.DocumentNode.SelectSingleNode("//dt[contains(text(), 'Oglas prikazan')]/following-sibling::dd[1]");
//            ////string viewsText = viewsNode?.InnerText.Trim() ?? "Not Found";
//            ////viewsText = viewsText.Replace(" puta", "");
//            ////var viewCounts = Convert.ToInt32(viewsText);

//            ////// Extract Phone Number (if available)
//            ////var phoneNode = htmlDoc.DocumentNode.SelectSingleNode("//p[contains(@class, 'UserPhoneNumber-phoneText')]");
//            ////string phoneNumber = phoneNode?.InnerText.Trim() ?? "Not Found";

//            //// Extract Location
//            ////var locationNode = htmlDoc.DocumentNode.SelectSingleNode("//span[contains(@class, 'ClassifiedDetailBasicDetails-textWrapContainer')]");
//            ////string locationText = locationNode?.InnerText.Trim() ?? "Not Found";

//            ////string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(?!outlook\.com$|gmail\.com$|yahoo\.com$)[a-zA-Z]{2,}$";

//            ////data.Views = viewCounts;
//            ////data.Location = locationText;
//            ////data.PhoneNumber = phoneNumber;
//            //await page.CloseAsync();
//            return data;
//        }
//        catch (Exception ex)
//        {
//            throw;
//        }
//    }

//    public static async Task FindPhoneNumber()
//    {
//        string auth = authList.First();
//        string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";
//        //using var playwright = await Playwright.CreateAsync();
//        //await using var browser = await playwright.Chromium.ConnectOverCDPAsync(SBR_CDP);

//        //var context = await browser.NewContextAsync();
//        //var page = await context.NewPageAsync();

//        using var playwright = await Playwright.CreateAsync();
//        var browser = await playwright.Chromium.LaunchAsync(new()
//        {
//            Headless = false,
//            Proxy = new Proxy()
//            {
//                Server = SBR_CDP
//            }
//        });
//        var context = await browser.NewContextAsync();

//        // Create an API request context
//        var request = await context.APIRequest.PostAsync("https://www.njuskalo.hr/oauth2/token", new()
//        {
//            Headers = new Dictionary<string, string>
//            {
//                { "authority", "www.njuskalo.hr" },
//                { "Referer", "https://www.njuskalo.hr/korisnik/gb46" },
//                { "Content-Type", "application/x-www-form-urlencoded;charset=UTF-8" },
//                { "Accept-Encoding", "gzip, deflate, br, zstd" }
//            }
//        });

//        var text = await request.TextAsync();

//        // Define the form data (modify according to API requirements)
//        var formData = new Dictionary<string, string>
//        {
//            { "grant_type", "client_credentials" },  // Modify this based on API requirements
//            { "client_id", "your-client-id" },       // Replace with actual client_id
//            { "client_secret", "your-client-secret" } // Replace with actual client_secret
//        };

//        // Send the POST request
//        //var response = await request.PostAsync("https://www.njuskalo.hr/oauth2/token", new()
//        //{
//        //    Form = formData
//        //});

//        // Get response text
//        //string responseText = await response.TextAsync();

//        // Print response
//        //Console.WriteLine("Response: " + responseText);

//        // Close browser
//        await browser.CloseAsync();




//        // Otvori stranicu
//        //await page.GotoAsync("https://www.njuskalo.hr/korisnik/gb46");

//        //// Čekaj da se učita dugme koje sadrži klasu "UserPhoneNumber-"
//        //var button = await page.WaitForSelectorAsync("[class*=UserPhoneNumber-]");

//        //if (button != null)
//        //{
//        //    // Klikni dugme
//        //    await button.ScrollIntoViewIfNeededAsync();
//        //    await button.ClickAsync(new() { Force = true });

//        //    // Sačekaj 1 sekundu da se broj prikaže
//        //    await Task.Delay(2000);

//        //    Console.WriteLine("Kliknuto na dugme za prikaz broja.");

//        //    await page.WaitForFunctionAsync("document.querySelector('p[class*=UserPhoneNumber-listItemText]') !== null");

//        //    // Dohvati broj telefona
//        //    var newHtml = new HtmlDocument();

//        //    string content2 = await page.ContentAsync();

//        //    var htmlDoc2 = new HtmlDocument();
//        //    htmlDoc2.LoadHtml(content2);

//        //    //var phones = htmlDoc2.DocumentNode.SelectNodes("//p[contains(@class, 'UserPhoneNumber-listItemText')]");
//        //    var phonesElement = page.Locator("p[class*=UserPhoneNumber-listItemText]");

//        //    if (await phonesElement.CountAsync() > 0)
//        //    {
//        //        var a = "Da";
//        //    }
//        //    //var phoneNumberElement = await page.WaitForSelectorAsync("p[class*=UserPhoneNumber-listItemText]", new() { Timeout = 5000 });
//        //    //if (phoneNumberElement != null)
//        //    //{
//        //    //    //string phoneNumber = await phoneNumberElement.InnerTextAsync();
//        //    //    Console.WriteLine("Prikazani broj telefona: " + phoneNumber);
//        //    //}
//        //    //else
//        //    //{
//        //    //    Console.WriteLine("Broj telefona nije pronađen.");
//        //    //}
//        //}
//        //else
//        //{
//        //    Console.WriteLine("Dugme nije pronađeno.");
//        //}

//        // Zatvori browser
//        //await browser.CloseAsync();
//    }

//    static async Task Main()
//    {

//        var result = await GetNjuskaloData("https://www.index.hr/oglasi/nekretnine/prodaja-kuca/grad-zagreb/pretraga?searchQuery=%257B%2522category%2522%253A%2522prodaja-kuca%2522%252C%2522module%2522%253A%2522nekretnine%2522%252C%2522includeCountyIds%2522%253A%255B%2522056b6c84-e6f1-433f-8bdc-9b8dbb86d6fb%2522%255D%252C%2522page%2522%253A1%252C%2522sortOption%2522%253A4%257D", 1);
//        var a = result;
//    }
//}
