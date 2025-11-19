using HtmlAgilityPack;
using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using PuppeteerSharp;
using System;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using ZalagaonicaTracker.Helpers;

namespace ZalagaonicaTracker.Scrapers
{
    public static class TrzalackaGlazbala
    {
        private static List<string> authList = new List<string>
        {
            "brd-customer-hl_615a7fb5-zone-scraping_browser1:x609rxea4tb3"
        };

        private static string? GetSMSNumberForLocation(string location)
        {
            var locationMap = new Dictionary<string, string>
            {
                { "Trešnjevka - Sjever", "0993006000" },
                { "Trešnjevka - Jug", "0925008000" },
                { "Gornja Dubrava", "0977700077" },
                { "Donja Dubrava", "0977700077" },
                { "Maksimir", "0977700077" },
                { "Stenjevec", "0993006000" },
                { "Novi Zagreb - Istok", "0925008000" },
                { "Podsused - Vrapče", "0993006000" },
                { "Novi Zagreb - Zapad", "0925008000" },
                { "Donji Grad", "0925000550" },
                { "Brezovica", "0925008000" },
                { "Trnje", "0925000550" },
                { "Podsljeme", "0977700077" },
                { "Črnomerec", "0993006000" },
                { "Gornji Grad - Medveščak", "0925000550" },
                { "Krap", "0993006000" },
                { "Sisa", "0925008000" },
                { "Bjelov", "0977700077" },
                { "Međim", "0977700077" },
                { "Varaž", "0977700077" },
                { "Karlov", "0925009000" },
                { "Rije", "0997000400" },
                { "Istr", "0997000400" },
                { "Istar", "0997000400" },
                { "Zadar", "0994000700" },
                { "Ličk", "0994000700" },
                { "Lika", "0994000700" },
                { "Dubrov", "098776677" },
                { "Split", "098776677" },
                { "Dalm", "098776677" }
            };

            return locationMap.FirstOrDefault(kvp => location.Contains(kvp.Key)).Value;
        }

        private static string GetEncodedTrzalackaGlazbalaIndexZagrebUrl(int page)
        {
            try
            {
                string baseUrl = "https://www.index.hr/oglasi/pretraga?searchQuery=";
                string query = $@"
                    {{
                        ""text"": ""gitara"",
                        ""category"": ""glazbeni-instrumenti"",
                        ""module"": ""stvari"",
                        ""priceFrom"": 50,
                        ""includeCountyIds"": [""056b6c84-e6f1-433f-8bdc-9b8dbb86d6fb""],
                        ""sortOption"": 4,
                        ""page"": {page}
                    }}";

                string encodedQuery = HttpUtility.UrlEncode(query);

                return baseUrl + encodedQuery;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string GetEncodedTrzalackaGlazbalaIndexKarlovacUrl(int page)
        {
            try
            {
                string baseUrl = "https://www.index.hr/oglasi/pretraga?searchQuery=";
                string query = $@"
                    {{
                        ""text"": ""gitara"",
                        ""category"": ""glazbeni-instrumenti"",
                        ""module"": ""stvari"",
                        ""priceFrom"": 50,
                        ""includeCountyIds"": [""d4574535-6275-4872-8920-22383b7466f6""],
                        ""sortOption"": 4,
                        ""page"": {page}
                    }}";

                string encodedQuery = HttpUtility.UrlEncode(query);

                return baseUrl + encodedQuery;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string GetEncodedTrzalackaGlazbalaIndexRijekaIstraUrl(int page)
        {
            try
            {
                string baseUrl = "https://www.index.hr/oglasi/pretraga?searchQuery=";
                string query = $@"
                    {{
                        ""text"": ""gitara"",
                        ""category"": ""glazbeni-instrumenti"",
                        ""module"": ""stvari"",
                        ""priceFrom"": 50,
                        ""includeCountyIds"": [""95c57539-6835-4b89-8ddd-a5de2100686e"", ""5116f71e-2bd4-4af5-a9a5-f4baa0af21d0""],
                        ""sortOption"": 4,
                        ""page"": {page}
                    }}";

                string encodedQuery = HttpUtility.UrlEncode(query);

                return baseUrl + encodedQuery;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string GetEncodedTrzalackaGlazbalaIndexZadarLikaUrl(int page)
        {
            try
            {
                string baseUrl = "https://www.index.hr/oglasi/pretraga?searchQuery=";
                string query = $@"
                    {{
                        ""text"": ""gitara"",
                        ""category"": ""glazbeni-instrumenti"",
                        ""module"": ""stvari"",
                        ""priceFrom"": 50,
                        ""includeCountyIds"": [""488cb05a-6ead-4858-896b-3aa5721fe41a"", ""b7fe5c4b-0bd4-4274-ac5c-c1be018b5789""],
                        ""sortOption"": 4,
                        ""page"": {page}
                    }}";

                string encodedQuery = HttpUtility.UrlEncode(query);

                return baseUrl + encodedQuery;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string GetEncodedTrzalackaGlazbalaIndexDubrovnikSplitUrl(int page)
        {
            try
            {
                string baseUrl = "https://www.index.hr/oglasi/pretraga?searchQuery=";
                string query = $@"
                    {{
                        ""text"": ""gitara"",
                        ""category"": ""glazbeni-instrumenti"",
                        ""module"": ""stvari"",
                        ""priceFrom"": 50,
                        ""includeCountyIds"": [""886a40e7-96c7-432c-9fe2-723687a5ab4d"", ""191f5ede-cf31-4c40-b353-257a9061462e""],
                        ""sortOption"": 4,
                        ""page"": {page}
                    }}";

                string encodedQuery = HttpUtility.UrlEncode(query);

                return baseUrl + encodedQuery;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string GetEncodedTrzalackaGlazbalaIndexKrapinskoZagorskaUrl(int page)
        {
            try
            {
                string baseUrl = "https://www.index.hr/oglasi/pretraga?searchQuery=";
                string query = $@"
                    {{
                        ""text"": ""gitara"",
                        ""category"": ""glazbeni-instrumenti"",
                        ""module"": ""stvari"",
                        ""priceFrom"": 50,
                        ""includeCountyIds"": [""be820870-44d8-4aec-a117-220dd82396c0""],
                        ""sortOption"": 4,
                        ""page"": {page}
                    }}";

                string encodedQuery = HttpUtility.UrlEncode(query);

                return baseUrl + encodedQuery;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string GetEncodedTrzalackaGlazbalaIndexSisackoMoslavackaUrl(int page)
        {
            try
            {
                string baseUrl = "https://www.index.hr/oglasi/pretraga?searchQuery=";
                string query = $@"
                    {{
                        ""text"": ""gitara"",
                        ""category"": ""glazbeni-instrumenti"",
                        ""module"": ""stvari"",
                        ""priceFrom"": 50,
                        ""includeCountyIds"": [""b9eb633d-4dd2-4a00-8b62-75c8e657b593""],
                        ""sortOption"": 4,
                        ""page"": {page}
                    }}";

                string encodedQuery = HttpUtility.UrlEncode(query);

                return baseUrl + encodedQuery;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string GetEncodedTrzalackaGlazbalaIndexBjelovarskaMedimurskaVarazdinskaUrl(int page)
        {
            try
            {
                string baseUrl = "https://www.index.hr/oglasi/pretraga?searchQuery=";
                string query = $@"
                    {{
                        ""text"": ""gitara"",
                        ""category"": ""glazbeni-instrumenti"",
                        ""module"": ""stvari"",
                        ""priceFrom"": 50,
                        ""includeCountyIds"": [""6d9687c7-a324-49c1-8f4d-6da61d4357dc"", ""68b8cc76-341a-4414-a1a4-f6ef6b4a32b7"", ""c467424b-456c-4b52-ab98-91403c171311""],
                        ""sortOption"": 4,
                        ""page"": {page}
                    }}";

                string encodedQuery = HttpUtility.UrlEncode(query);

                return baseUrl + encodedQuery;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string GetTrzalackaGlazbalaZagrebUrl(int page)
        {
            return $"https://www.njuskalo.hr/trzalacki-instrumenti?geo%5BlocationIds%5D=1153&price%5Bmin%5D=50&page={page}";
        }

        private static string GetTrzalackaGlazbalaKarlovacUrl(int page)
        {
            return $"https://www.njuskalo.hr/trzalacki-instrumenti?geo%5BlocationIds%5D=1155&price%5Bmin%5D=50&page={page}";
        }

        private static string GetTrzalackaGlazbalaRijekaIstraUrl(int page)
        {
            return $"https://www.njuskalo.hr/trzalacki-instrumenti?geo%5BlocationIds%5D=1162%2C1154&price%5Bmin%5D=50&page={page}";
        }

        private static string GetTrzalackaGlazbalaZadarLikaUrl(int page)
        {
            return $"https://www.njuskalo.hr/trzalacki-instrumenti?geo%5BlocationIds%5D=1169%2C1158&price%5Bmin%5D=50&page={page}";
        }

        private static string GetTrzalackaGlazbalaDubrovnikSplitUrl(int page)
        {
            return $"https://www.njuskalo.hr/trzalacki-instrumenti?geo%5BlocationIds%5D=1152%2C1164&price%5Bmin%5D=50&page={page}";
        }

        private static string GetTrzalackaGlazbalaKrapinskoZagorskaUrl(int page)
        {
            return $"https://www.njuskalo.hr/trzalacki-instrumenti?geo%5BlocationIds%5D=1157&price%5Bmin%5D=50&page={page}";
        }

        private static string GetTrzalackaGlazbalaSisackoMoslavackaUrl(int page)
        {
            return $"https://www.njuskalo.hr/trzalacki-instrumenti?geo%5BlocationIds%5D=1163&price%5Bmin%5D=50&page={page}";
        }

        private static string GetTrzalackaGlazbalaBjelovarskaMedimurskaVarazdinskaUrl(int page)
        {
            return $"https://www.njuskalo.hr/trzalacki-instrumenti?geo%5BlocationIds%5D=1150%2C1159%2C1166&price%5Bmin%5D=50&page={page}";
        }

        private static async Task<AdData> GetAdInfo(AdData adData)
        {
            try
            {
                string auth = authList.First();
                string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";

                try
                {
                    //Console.WriteLine($"Connecting with AUTH: {auth}...");
                    //using var pw = await Playwright.CreateAsync();
                    //await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
                    Console.WriteLine($"Connected! Navigating and scrapping Trzalačka glazbala ad detail on Index {adData.Location}");
                    var client = new WebClient();
                    client.Proxy = new WebProxy("brd.superproxy.io:33335");
                    client.Proxy.Credentials = new NetworkCredential("brd-customer-hl_22f6b788-zone-web_unlocker1", "6eg9mmymhpgt");
                    client.Headers[HttpRequestHeader.Authorization] = "47fc9378b1f073d867f9cf4c2cd98bdeddebc5697b47599c1624854b286c5ea1";

                    bool hasRecentAds = true;
                    bool checkedNextPage = false;
                    DateTime now = DateTime.Now;

                    //await pageInstance.GotoAsync(, new() { Timeout = 30000 });

                    Console.WriteLine($"Scraping specific ad data...");

                    var html = client.DownloadString($"https://www.index.hr{adData.URL}");
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    var username = htmlDoc.DocumentNode.SelectSingleNode(".//div[contains(@class, 'SellerInfo__username')]").InnerHtml;
                    var a1 = username;

                    Console.WriteLine($"Found seller: {username}");

                    // Click on "Pogledaj sve oglase" link
                    var allAdsLink = htmlDoc.DocumentNode.SelectSingleNode(".//a[contains(@class, 'sellerLink')]");

                    string pattern = @"href\s*=\s*""([^""]+)""";
                    Match match = Regex.Match(allAdsLink.OuterHtml, pattern);

                    if (match.Success)
                    {
                        Console.WriteLine("Extracted href: " + match.Groups[1].Value);
                        var userDetails = match.Groups[1].Value;

                        Console.WriteLine($"Connecting with AUTH2: brd-customer-hl_615a7fb5-zone-scraping_browser2:xlwc4xjelvsu...");
                        using var pw2 = await Playwright.CreateAsync();
                        var browser2 = await pw2.Chromium.ConnectOverCDPAsync(SBR_CDP);

                        var page2 = await browser2.NewPageAsync();
                        Console.WriteLine($"Going to seller details");
                        await page2.GotoAsync($"https://www.index.hr{userDetails}");

                        string content2 = await page2.ContentAsync();

                        var htmlDoc2 = new HtmlDocument();
                        htmlDoc2.LoadHtml(content2);

                        var phones = htmlDoc2.DocumentNode.SelectNodes("//span[contains(@class, 'SellerSectionWeb__infoValue')]");

                        if (phones != null)
                        {
                            Console.WriteLine($"Getting phone number of {username}");
                            foreach (var phone in phones)
                            {
                                string href = phone.InnerText;

                                if (href.Contains("09") && !string.IsNullOrEmpty(href))
                                {
                                    string pattern2 = @"^0"; // Regex koji traži 0 na početku stringa
                                    string replacement = "+385";

                                    string result = Regex.Replace(href, pattern2, replacement);
                                    Console.WriteLine($"Found number: {href} of {username}");
                                    adData.PhoneNumber = result;
                                    Console.WriteLine($"Saved phone number of {username}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("No matching links found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No match found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error with AUTH {auth}: {ex.Message}");
                }

                return adData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static async Task<List<AdData>> GetNjuskaloData(string url, int page = 1)
        {
            List<AdData> scrapedAds = new List<AdData>(); // Global list to store results

            if (authList.Count == 0)
            {
                Console.WriteLine("No AUTH values left to use.");
                return scrapedAds; // Return already scraped data
            }

            string auth = authList.First();
            string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";

            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var client = new WebClient();
                client.Proxy = new WebProxy("brd.superproxy.io:33335");
                client.Proxy.Credentials = new NetworkCredential("brd-customer-hl_22f6b788-zone-web_unlocker1", "6eg9mmymhpgt");
                client.Headers[HttpRequestHeader.Authorization] = "47fc9378b1f073d867f9cf4c2cd98bdeddebc5697b47599c1624854b286c5ea1";

                bool hasRecentAds = true;
                bool checkedNextPage = false;
                DateTime now = DateTime.Now;

                while (true)
                {
                    Console.WriteLine($"Scraping page {page}...");
                    var html = client.DownloadString(url);
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

                        var dateNode = ad.SelectSingleNode("//time[@class='date date--full']/@datetime");

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

                        var adData = new AdData();
                        adData.Title = title;
                        adData.Price = price.Replace("&nbsp;", "");
                        adData.Description = description;
                        adData.URL = "https://www.njuskalo.hr" + link;
                        adData.AdPostedOn = datePosted;

                        adData = await GetNjuskaloAdInfo(adData.URL, adData);

                        if (adData.Views < 500)
                        {
                            scrapedAds.Add(adData);
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

                //await browser.CloseAsync();

                //await pageInstance.CloseAsync();

                return scrapedAds;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with AUTH {auth}: {ex.Message}");
                return await GetNjuskaloData(url, page); // Retry with next AUTH, keeping the page number
            }
        }

        private static async Task<AdData> GetNjuskaloAdInfo(string url, AdData data)
        {
            try
            {
                var list = new List<AdData>();

                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var client = new WebClient();
                client.Proxy = new WebProxy("brd.superproxy.io:33335");
                client.Proxy.Credentials = new NetworkCredential("brd-customer-hl_22f6b788-zone-web_unlocker1", "6eg9mmymhpgt");
                client.Headers[HttpRequestHeader.Authorization] = "47fc9378b1f073d867f9cf4c2cd98bdeddebc5697b47599c1624854b286c5ea1";

                var html = client.DownloadString(url);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                Console.WriteLine("Navigated! Scraping page content...");

                //html = await page.ContentAsync();
                Console.WriteLine("HTML content is scrapped");

                // Parse with HtmlAgilityPack
                htmlDoc.LoadHtml(html);
                Console.WriteLine("HTML content is loaded");

                var sellerUsernameNode = htmlDoc.DocumentNode.SelectSingleNode("//a[starts-with(@href, 'https://www.njuskalo.hr/korisnik/')]");
                //string sellerUsernameText = sellerUsernameNode?.InnerText.Trim() ?? "Not Found";

                string hrefValue = sellerUsernameNode.GetAttributeValue("href", "");
                var sellerUsername = hrefValue.Split('/').LastOrDefault();

                var sellerInfoHtml = client.DownloadString($"https://www.njuskalo.hr/korisnik/{sellerUsername}");

                var sellerInfoHtmlDoc = new HtmlDocument();
                sellerInfoHtmlDoc.LoadHtml(sellerInfoHtml);

                var node = sellerInfoHtmlDoc.DocumentNode.SelectSingleNode("//script[contains(text(), 'UserProfileDetailsContactAction')]/text()");

                if (node != null)
                {
                    string scriptContent = node.InnerText;
                    //Match match = Regex.Match(scriptContent, @"userId\s*:\s*(\d+)");
                    string extracted = scriptContent.Substring(scriptContent.IndexOf("userId") + 7); // Pomaknemo se iza "userId"
                    Match match = Regex.Match(extracted, @"\d+"); // Prvi broj nakon userId
                    if (match.Success)
                    {
                        Console.WriteLine("User ID: " + match.Groups[1].Value);

                        using (WebClient userClient = new WebClient())
                        {
                            string jsonContent = File.ReadAllText("Config.json");

                            // Parse JSON and get the value of "NjuskaloToken"
                            using JsonDocument doc = JsonDocument.Parse(jsonContent);
                            string token = doc.RootElement.GetProperty("NjuskaloToken").GetString();
                            // Postavljanje Content-Type headera
                            userClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                            userClient.Headers[HttpRequestHeader.Authorization] = $"Bearer {token}";
                            // Slanje POST zahtjeva i primanje odgovora
                            string response = userClient.DownloadString($"https://www.njuskalo.hr/ccapi/v4/phone-numbers/user/{match.Groups[0].Value}");

                            JObject json = JObject.Parse(response);

                            // Ensure "data" exists and is an object or array
                            JToken dataToken = json["data"];

                            if (dataToken is JArray dataArray)  // If "data" is an array, take the first element
                            {
                                dataToken = dataArray.FirstOrDefault();
                            }

                            // Extract the rawNumber if it exists
                            string rawNumber = dataToken?["attributes"]?["numbers"]?[0]?["rawNumber"]?.ToString();

                            if (!string.IsNullOrEmpty(rawNumber))
                            {
                                Console.WriteLine("Broj telefona: " + rawNumber);
                                data.PhoneNumber = rawNumber;
                            }
                            else
                            {
                                Console.WriteLine("Broj telefona nije pronađen.");
                            }
                            // Dekodiranje odgovora
                            //string responseString = Encoding.UTF8.GetString(response);
                            //Console.WriteLine("Response: " + responseString);
                        }
                    }
                    else
                    {
                        Console.WriteLine("User ID nije pronađen.");
                    }
                }
                else
                {
                    Console.WriteLine("Script tag nije pronađen.");
                }

                string today = DateTime.Now.ToString("yyyy-MM-dd");
                var viewsNode = htmlDoc.DocumentNode.SelectSingleNode("//dt[contains(text(), 'Oglas prikazan')]/following-sibling::dd[1]");
                string viewsText = viewsNode?.InnerText.Trim() ?? "Not Found";
                viewsText = viewsText.Replace(" puta", "");
                var viewCounts = Convert.ToInt32(viewsText);

                // Extract Phone Number (if available)
                var phoneNode = htmlDoc.DocumentNode.SelectSingleNode("//p[contains(@class, 'UserPhoneNumber-phoneText')]");
                string phoneNumber = phoneNode?.InnerText.Trim() ?? "Not Found";

                // Extract Location
                var locationNode = htmlDoc.DocumentNode.SelectSingleNode("//span[contains(@class, 'ClassifiedDetailBasicDetails-textWrapContainer')]");
                string locationText = locationNode?.InnerText.Trim() ?? "Not Found";

                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(?!outlook\.com$|gmail\.com$|yahoo\.com$)[a-zA-Z]{2,}$";

                data.Views = viewCounts;
                data.Location = locationText;
                //data.PhoneNumber = phoneNumber;

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<List<AdData>> TrzalackaGlazbalaIndexZagreb(List<AdData> scrapedAds, HashSet<string> sentNumbers, int page = 1)
        {
            try
            {
                Console.WriteLine("Connected! Navigating and scrapping Trzalačka glazbala ads on Index in Zagreb");

                bool hasAds = true;

                while (hasAds) // Keep looping until no ads are found
                {
                    try
                    {
                        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                        {
                            Headless = true,
                            ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                        });
                        await using var browserPage = await browser.NewPageAsync();

                        var response = await browserPage.GoToAsync(GetEncodedTrzalackaGlazbalaIndexZagrebUrl(page), new NavigationOptions
                        {
                            WaitUntil = new[] { WaitUntilNavigation.Networkidle2 } // Ensures dynamic content is loaded
                        });

                        var html = await browserPage.GetContentAsync();
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");
                        Console.WriteLine($"Page {page}: Total ads found: {ads?.Count}");

                        if (ads == null || ads.Count == 0)
                        {
                            hasAds = false;
                            break;
                        }

                        foreach (var ad in ads)
                        {
                            var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
                            string title = titleNode?.InnerText.Trim() ?? "N/A";

                            var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
                            string price = priceNode?.InnerText.Trim() ?? "N/A";

                            var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
                            string date = dateNode?.InnerText.Trim() ?? "N/A";

                            var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
                            string location = locationNode?.InnerText.Trim() ?? "N/A";

                            var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
                            string? link = linkNode?.GetAttributeValue("href", "N/A");

                            var adData = new AdData()
                            {
                                Title = title,
                                Price = price,
                                AdPostedOn = Convert.ToDateTime(date),
                                Location = location,
                                URL = link
                            };

                            await GetAdInfo(scrapedAds, adData);

                            // Send SMS only if the phone number is new
                            if (!sentNumbers.Contains(adData.PhoneNumber))
                            {
                                string smsNumber = GetSMSNumberForLocation(adData.Location);
                                if (smsNumber != null)
                                {
                                    await SMSClient.SendSMS(adData.PhoneNumber, smsNumber);
                                    sentNumbers.Add(adData.PhoneNumber); // Mark number as sent
                                }
                            }

                            scrapedAds.Add(adData);
                        }

                        page++; // Move to the next page
                        await Task.Delay(200); // Small delay to prevent rate limiting
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error on page {page}: {ex.Message}"); hasAds = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
            }

            return scrapedAds;

            //try
            //{
            //    List<AdData> scrapedAds = new List<AdData>();

            //    if (authList.Count == 0)
            //    {
            //        Console.WriteLine("No AUTH values left to use.");
            //        return scrapedAds;
            //    }

            //    string auth = authList.First();
            //    string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";

            //    try
            //    {
            //        Console.WriteLine($"Connecting with AUTH: {auth}...");
            //        //using var pw = await Playwright.CreateAsync();
            //        //await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
            //        Console.WriteLine("Connected! Navigating and scrapping Trzalačka glazbala ads on Index in Zagreb");

            //        //var pageInstance = await browser.NewPageAsync();
            //        bool hasRecentAds = true;
            //        bool checkedNextPage = false;
            //        DateTime now = DateTime.Now;

            //        while (true)
            //        {
            //            //await pageInstance.GotoAsync(GetEncodedTrzalackaGlazbalaIndexZagrebUrl(page), new() { Timeout = 30000 });

            //            Console.WriteLine($"Scraping page...");
            //            //await new BrowserFetcher().DownloadAsync(BrowserTag.Stable);
            //            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            //            {
            //                Headless = true,
            //                ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
            //                Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
            //            });
            //            await using var browserPage = await browser.NewPageAsync();

            //            var response = await browserPage.GoToAsync(GetEncodedTrzalackaGlazbalaIndexBjelovarskaMedimurskaVarazdinskaUrl(page), new NavigationOptions
            //            {
            //                Timeout = 30000,
            //                WaitUntil = new[] { WaitUntilNavigation.Networkidle0 } // Ensures dynamic content is loaded
            //            });

            //            var result = await browserPage.WaitForSelectorAsync("div.ant-col.ant-col-xs-24");

            //            var isVisible = await result.IsVisibleAsync();

            //            //await pageInstance.GotoAsync(, new() { Timeout = 30000 });

            //            Console.WriteLine($"Scraping specific ad data...");

            //            var html = await browserPage.GetContentAsync();
            //            //var html = await pageInstance.ContentAsync();
            //            var htmlDoc = new HtmlDocument();
            //            htmlDoc.LoadHtml(html);

            //            var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");
            //            Console.WriteLine($"Total ads found: {ads.Count}");

            //            foreach (var ad in ads)
            //            {
            //                Console.WriteLine($"Processing ad #{ads.IndexOf(ad) + 1}");

            //                var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
            //                string title = titleNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati cijenu
            //                var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
            //                string price = priceNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati datum objave
            //                var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
            //                string date = dateNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati lokaciju
            //                var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
            //                string location = locationNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati link oglasa
            //                var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
            //                string? link = linkNode?.GetAttributeValue("href", "N/A");

            //                //scrapedAds
            //                Console.WriteLine($"Title: {title}");
            //                Console.WriteLine($"Price: {price}");
            //                Console.WriteLine($"Date: {date}");
            //                Console.WriteLine($"Location: {location}");
            //                Console.WriteLine($"Link: {link}");
            //                Console.WriteLine("--------------------------------");

            //                var adData = new AdData()
            //                {
            //                    Title = title,
            //                    Price = price,
            //                    AdPostedOn = Convert.ToDateTime(date),
            //                    Location = location,
            //                    URL = link
            //                };
            //                await GetAdInfo(adData);

            //                if (adData.Location.Contains("Trešnjevka - Sjever"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0993006000");
            //                }

            //                else if (adData.Location.Contains("Trešnjevka - Jug"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0925008000");
            //                }

            //                else if (adData.Location.Contains("Gornja Dubrava"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0977700077");
            //                }

            //                else if (adData.Location.Contains("Donja Dubrava"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0977700077");
            //                }

            //                else if (adData.Location.Contains("Maksimir"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0977700077");
            //                }

            //                else if (adData.Location.Contains("Stenjevec"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0993006000");
            //                }

            //                else if (adData.Location.Contains("Novi Zagreb - Istok"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0925008000");
            //                }

            //                else if (adData.Location.Contains("Podsused - Vrapče"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0993006000");
            //                }

            //                else if (adData.Location.Contains("Novi Zagreb - Zapad"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0925008000");
            //                }

            //                else if (adData.Location.Contains("Donji Grad"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0925000550");
            //                }

            //                else if (adData.Location.Contains("Brezovica"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0925008000");
            //                }

            //                else if (adData.Location.Contains("Trnje"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0925000550");
            //                }

            //                else if (adData.Location.Contains("Podsljeme"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0977700077");
            //                }

            //                else if (adData.Location.Contains("Brezovica"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0925008000");
            //                }

            //                else if (adData.Location.Contains("Črnomerec"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0993006000");
            //                }

            //                else if (adData.Location.Contains("Gornji Grad - Medveščak"))
            //                {
            //                    //await SMSClient.SendSMS(adData.PhoneNumber, "0925000550");
            //                }

            //                scrapedAds.Add(adData);
            //            }

            //            if (!hasRecentAds)
            //            {
            //                if (checkedNextPage) break;
            //                checkedNextPage = true;
            //                page++;
            //            }
            //            else
            //            {
            //                checkedNextPage = false;
            //                page++;
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error with AUTH {auth}: {ex.Message}");
            //    }

            //    return scrapedAds;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        public static async Task<List<AdData>> TrzalackaGlazbalaIndexKarlovac(List<AdData> scrapedAds, HashSet<string> sentNumbers, int page = 1)
        {
            try
            {
                Console.WriteLine("Connected! Navigating and scrapping Trzalačka glazbala ads on Index in Karlovac");

                bool hasAds = true;

                while (hasAds) // Keep looping until no ads are found
                {
                    try
                    {
                        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                        {
                            Headless = true,
                            ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                        });
                        await using var browserPage = await browser.NewPageAsync();

                        var response = await browserPage.GoToAsync(GetEncodedTrzalackaGlazbalaIndexKarlovacUrl(page), new NavigationOptions
                        {
                            WaitUntil = new[] { WaitUntilNavigation.Networkidle2 } // Ensures dynamic content is loaded
                        });

                        var html = await browserPage.GetContentAsync();
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");
                        Console.WriteLine($"Page {page}: Total ads found: {ads?.Count}");

                        if (ads == null || ads.Count == 0)
                        {
                            hasAds = false;
                            break;
                        }

                        foreach (var ad in ads)
                        {
                            var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
                            string title = titleNode?.InnerText.Trim() ?? "N/A";

                            var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
                            string price = priceNode?.InnerText.Trim() ?? "N/A";

                            var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
                            string date = dateNode?.InnerText.Trim() ?? "N/A";

                            var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
                            string location = locationNode?.InnerText.Trim() ?? "N/A";

                            var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
                            string? link = linkNode?.GetAttributeValue("href", "N/A");

                            var adData = new AdData()
                            {
                                Title = title,
                                Price = price,
                                AdPostedOn = Convert.ToDateTime(date),
                                Location = location,
                                URL = link
                            };

                            await GetAdInfo(scrapedAds, adData);

                            // Send SMS only if the phone number is new
                            if (!sentNumbers.Contains(adData.PhoneNumber))
                            {
                                string smsNumber = GetSMSNumberForLocation(adData.Location);
                                if (smsNumber != null)
                                {
                                    await SMSClient.SendSMS(adData.PhoneNumber, smsNumber);
                                    sentNumbers.Add(adData.PhoneNumber); // Mark number as sent
                                }
                            }

                            scrapedAds.Add(adData);
                        }

                        page++; // Move to the next page
                        await Task.Delay(200); // Small delay to prevent rate limiting
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error on page {page}: {ex.Message}"); hasAds = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
            }

            return scrapedAds;

            //try
            //{
            //    List<AdData> scrapedAds = new List<AdData>();

            //    if (authList.Count == 0)
            //    {
            //        Console.WriteLine("No AUTH values left to use.");
            //        return scrapedAds;
            //    }

            //    string auth = authList.First();
            //    string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";

            //    try
            //    {
            //        Console.WriteLine($"Connecting with AUTH: {auth}...");
            //        using var pw = await Playwright.CreateAsync();
            //        await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
            //        Console.WriteLine("Connected! Navigating and scrapping Trzalačka glazbala ads on Index in Karlovac");

            //        var pageInstance = await browser.NewPageAsync();
            //        bool hasRecentAds = true;
            //        bool checkedNextPage = false;
            //        DateTime now = DateTime.Now;

            //        while (true)
            //        {
            //            await pageInstance.GotoAsync(GetEncodedTrzalackaGlazbalaIndexKarlovacUrl(page), new() { Timeout = 30000 });

            //            Console.WriteLine($"Scraping page...");
            //            var html = await pageInstance.ContentAsync();
            //            var htmlDoc = new HtmlDocument();
            //            htmlDoc.LoadHtml(html);

            //            var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");
            //            Console.WriteLine($"Total ads found: {ads.Count}");

            //            foreach (var ad in ads)
            //            {
            //                Console.WriteLine($"Processing ad #{ads.IndexOf(ad) + 1}");

            //                var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
            //                string title = titleNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati cijenu
            //                var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
            //                string price = priceNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati datum objave
            //                var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
            //                string date = dateNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati lokaciju
            //                var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
            //                string location = locationNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati link oglasa
            //                var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
            //                string? link = linkNode?.GetAttributeValue("href", "N/A");

            //                //scrapedAds
            //                Console.WriteLine($"Title: {title}");
            //                Console.WriteLine($"Price: {price}");
            //                Console.WriteLine($"Date: {date}");
            //                Console.WriteLine($"Location: {location}");
            //                Console.WriteLine($"Link: {link}");
            //                Console.WriteLine("--------------------------------");

            //                var adData = new AdData()
            //                {
            //                    Title = title,
            //                    Price = price,
            //                    AdPostedOn = Convert.ToDateTime(date),
            //                    Location = location,
            //                    URL = link
            //                };
            //                await GetAdInfo(adData);

            //                //await SMSClient.SendSMS(adData.PhoneNumber, "0925009000");

            //                scrapedAds.Add(adData);
            //            }

            //            if (!hasRecentAds)
            //            {
            //                if (checkedNextPage) break;
            //                checkedNextPage = true;
            //                page++;
            //            }
            //            else
            //            {
            //                checkedNextPage = false;
            //                page++;
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error with AUTH {auth}: {ex.Message}");
            //    }

            //    return scrapedAds;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        public static async Task<List<AdData>> TrzalackaGlazbalaIndexRijekaIstra(List<AdData> scrapedAds, HashSet<string> sentNumbers, int page = 1)
        {
            try
            {
                Console.WriteLine("Connected! Navigating and scrapping Trzalačka glazbala ads on Index in Rijeka and Istra");

                bool hasAds = true;

                while (hasAds) // Keep looping until no ads are found
                {
                    try
                    {
                        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                        {
                            Headless = true,
                            ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                        });
                        await using var browserPage = await browser.NewPageAsync();

                        var response = await browserPage.GoToAsync(GetEncodedTrzalackaGlazbalaIndexRijekaIstraUrl(page), new NavigationOptions
                        {
                            WaitUntil = new[] { WaitUntilNavigation.Networkidle2 } // Ensures dynamic content is loaded
                        });

                        var html = await browserPage.GetContentAsync();
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");
                        Console.WriteLine($"Page {page}: Total ads found: {ads?.Count}");

                        if (ads == null || ads.Count == 0)
                        {
                            hasAds = false;
                            break;
                        }

                        foreach (var ad in ads)
                        {
                            var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
                            string title = titleNode?.InnerText.Trim() ?? "N/A";

                            var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
                            string price = priceNode?.InnerText.Trim() ?? "N/A";

                            var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
                            string date = dateNode?.InnerText.Trim() ?? "N/A";

                            var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
                            string location = locationNode?.InnerText.Trim() ?? "N/A";

                            var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
                            string? link = linkNode?.GetAttributeValue("href", "N/A");

                            var adData = new AdData()
                            {
                                Title = title,
                                Price = price,
                                AdPostedOn = Convert.ToDateTime(date),
                                Location = location,
                                URL = link
                            };

                            await GetAdInfo(scrapedAds, adData);

                            // Send SMS only if the phone number is new
                            if (!sentNumbers.Contains(adData.PhoneNumber))
                            {
                                string smsNumber = GetSMSNumberForLocation(adData.Location);
                                if (smsNumber != null)
                                {
                                    await SMSClient.SendSMS(adData.PhoneNumber, smsNumber);
                                    sentNumbers.Add(adData.PhoneNumber); // Mark number as sent
                                }
                            }

                            scrapedAds.Add(adData);
                        }

                        page++; // Move to the next page
                        await Task.Delay(200); // Small delay to prevent rate limiting
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error on page {page}: {ex.Message}"); hasAds = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
            }

            return scrapedAds;

            //try
            //{
            //    List<AdData> scrapedAds = new List<AdData>();

            //    if (authList.Count == 0)
            //    {
            //        Console.WriteLine("No AUTH values left to use.");
            //        return scrapedAds;
            //    }

            //    string auth = authList.First();
            //    string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";

            //    try
            //    {
            //        Console.WriteLine($"Connecting with AUTH: {auth}...");
            //        using var pw = await Playwright.CreateAsync();
            //        await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
            //        Console.WriteLine("Connected! Navigating and scrapping Trzalačka glazbala ads on Index in Rijeka and Istra");

            //        var pageInstance = await browser.NewPageAsync();
            //        bool hasRecentAds = true;
            //        bool checkedNextPage = false;
            //        DateTime now = DateTime.Now;

            //        while (true)
            //        {
            //            await pageInstance.GotoAsync(GetEncodedTrzalackaGlazbalaIndexRijekaIstraUrl(page), new() { Timeout = 30000 });

            //            Console.WriteLine($"Scraping page...");
            //            var html = await pageInstance.ContentAsync();
            //            var htmlDoc = new HtmlDocument();
            //            htmlDoc.LoadHtml(html);

            //            var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");
            //            Console.WriteLine($"Total ads found: {ads.Count}");

            //            foreach (var ad in ads)
            //            {
            //                Console.WriteLine($"Processing ad #{ads.IndexOf(ad) + 1}");

            //                var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
            //                string title = titleNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati cijenu
            //                var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
            //                string price = priceNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati datum objave
            //                var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
            //                string date = dateNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati lokaciju
            //                var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
            //                string location = locationNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati link oglasa
            //                var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
            //                string? link = linkNode?.GetAttributeValue("href", "N/A");

            //                //scrapedAds
            //                Console.WriteLine($"Title: {title}");
            //                Console.WriteLine($"Price: {price}");
            //                Console.WriteLine($"Date: {date}");
            //                Console.WriteLine($"Location: {location}");
            //                Console.WriteLine($"Link: {link}");
            //                Console.WriteLine("--------------------------------");

            //                var adData = new AdData()
            //                {
            //                    Title = title,
            //                    Price = price,
            //                    AdPostedOn = Convert.ToDateTime(date),
            //                    Location = location,
            //                    URL = link
            //                };
            //                await GetAdInfo(adData);

            //                //await SMSClient.SendSMS(adData.PhoneNumber, "0997000400");

            //                scrapedAds.Add(adData);
            //            }

            //            if (!hasRecentAds)
            //            {
            //                if (checkedNextPage) break;
            //                checkedNextPage = true;
            //                page++;
            //            }
            //            else
            //            {
            //                checkedNextPage = false;
            //                page++;
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error with AUTH {auth}: {ex.Message}");
            //    }

            //    return scrapedAds;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        public static async Task<List<AdData>> TrzalackaGlazbalaIndexZadarLika(List<AdData> scrapedAds, HashSet<string> sentNumbers, int page = 1)
        {
            try
            {
                Console.WriteLine("Connected! Navigating and scrapping Trzalačka glazbala ads on Index in Zadar and Lika");

                bool hasAds = true;

                while (hasAds) // Keep looping until no ads are found
                {
                    try
                    {
                        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                        {
                            Headless = true,
                            ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                        });
                        await using var browserPage = await browser.NewPageAsync();

                        var response = await browserPage.GoToAsync(GetEncodedTrzalackaGlazbalaIndexZadarLikaUrl(page), new NavigationOptions
                        {
                            WaitUntil = new[] { WaitUntilNavigation.Networkidle2 } // Ensures dynamic content is loaded
                        });

                        var html = await browserPage.GetContentAsync();
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");
                        Console.WriteLine($"Page {page}: Total ads found: {ads?.Count}");

                        if (ads == null || ads.Count == 0)
                        {
                            hasAds = false;
                            break;
                        }

                        foreach (var ad in ads)
                        {
                            var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
                            string title = titleNode?.InnerText.Trim() ?? "N/A";

                            var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
                            string price = priceNode?.InnerText.Trim() ?? "N/A";

                            var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
                            string date = dateNode?.InnerText.Trim() ?? "N/A";

                            var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
                            string location = locationNode?.InnerText.Trim() ?? "N/A";

                            var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
                            string? link = linkNode?.GetAttributeValue("href", "N/A");

                            var adData = new AdData()
                            {
                                Title = title,
                                Price = price,
                                AdPostedOn = Convert.ToDateTime(date),
                                Location = location,
                                URL = link
                            };

                            await GetAdInfo(scrapedAds, adData);

                            // Send SMS only if the phone number is new
                            if (!sentNumbers.Contains(adData.PhoneNumber))
                            {
                                string smsNumber = GetSMSNumberForLocation(adData.Location);
                                if (smsNumber != null)
                                {
                                    await SMSClient.SendSMS(adData.PhoneNumber, smsNumber);
                                    sentNumbers.Add(adData.PhoneNumber); // Mark number as sent
                                }
                            }

                            scrapedAds.Add(adData);
                        }

                        page++; // Move to the next page
                        await Task.Delay(200); // Small delay to prevent rate limiting
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error on page {page}: {ex.Message}"); hasAds = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
            }

            return scrapedAds;

            //try
            //{
            //    List<AdData> scrapedAds = new List<AdData>();

            //    if (authList.Count == 0)
            //    {
            //        Console.WriteLine("No AUTH values left to use.");
            //        return scrapedAds;
            //    }

            //    string auth = authList.First();
            //    string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";

            //    try
            //    {
            //        Console.WriteLine($"Connecting with AUTH: {auth}...");
            //        using var pw = await Playwright.CreateAsync();
            //        await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
            //        Console.WriteLine("Connected! Navigating and scrapping Trzalačka glazbala ads on Index in Zadar and Lika");

            //        var pageInstance = await browser.NewPageAsync();
            //        bool hasRecentAds = true;
            //        bool checkedNextPage = false;
            //        DateTime now = DateTime.Now;

            //        while (true)
            //        {
            //            await pageInstance.GotoAsync(GetEncodedTrzalackaGlazbalaIndexZadarLikaUrl(page), new() { Timeout = 30000 });

            //            Console.WriteLine($"Scraping page...");
            //            var html = await pageInstance.ContentAsync();
            //            var htmlDoc = new HtmlDocument();
            //            htmlDoc.LoadHtml(html);

            //            var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");
            //            Console.WriteLine($"Total ads found: {ads.Count}");

            //            foreach (var ad in ads)
            //            {
            //                Console.WriteLine($"Processing ad #{ads.IndexOf(ad) + 1}");

            //                var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
            //                string title = titleNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati cijenu
            //                var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
            //                string price = priceNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati datum objave
            //                var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
            //                string date = dateNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati lokaciju
            //                var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
            //                string location = locationNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati link oglasa
            //                var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
            //                string? link = linkNode?.GetAttributeValue("href", "N/A");

            //                //scrapedAds
            //                Console.WriteLine($"Title: {title}");
            //                Console.WriteLine($"Price: {price}");
            //                Console.WriteLine($"Date: {date}");
            //                Console.WriteLine($"Location: {location}");
            //                Console.WriteLine($"Link: {link}");
            //                Console.WriteLine("--------------------------------");

            //                var adData = new AdData()
            //                {
            //                    Title = title,
            //                    Price = price,
            //                    AdPostedOn = Convert.ToDateTime(date),
            //                    Location = location,
            //                    URL = link
            //                };
            //                await GetAdInfo(adData);

            //                //await SMSClient.SendSMS(adData.PhoneNumber, "0994000700");

            //                scrapedAds.Add(adData);
            //            }

            //            if (!hasRecentAds)
            //            {
            //                if (checkedNextPage) break;
            //                checkedNextPage = true;
            //                page++;
            //            }
            //            else
            //            {
            //                checkedNextPage = false;
            //                page++;
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error with AUTH {auth}: {ex.Message}");
            //    }

            //    return scrapedAds;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        public static async Task<List<AdData>> TrzalackaGlazbalaIndexDubrovnikSplit(List<AdData> scrapedAds, HashSet<string> sentNumbers, int page = 1)
        {
            try
            {
                Console.WriteLine("Connected! Navigating and scrapping Trzalačka glazbala ads on Index in Dubrovnik and Split");

                bool hasAds = true;

                while (hasAds) // Keep looping until no ads are found
                {
                    try
                    {
                        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                        {
                            Headless = true,
                            ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                        });
                        await using var browserPage = await browser.NewPageAsync();

                        var response = await browserPage.GoToAsync(GetEncodedTrzalackaGlazbalaIndexDubrovnikSplitUrl(page), new NavigationOptions
                        {
                            WaitUntil = new[] { WaitUntilNavigation.Networkidle2 } // Ensures dynamic content is loaded
                        });

                        var html = await browserPage.GetContentAsync();
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");
                        Console.WriteLine($"Page {page}: Total ads found: {ads?.Count}");

                        if (ads == null || ads.Count == 0)
                        {
                            hasAds = false;
                            break;
                        }

                        foreach (var ad in ads)
                        {
                            var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
                            string title = titleNode?.InnerText.Trim() ?? "N/A";

                            var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
                            string price = priceNode?.InnerText.Trim() ?? "N/A";

                            var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
                            string date = dateNode?.InnerText.Trim() ?? "N/A";

                            var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
                            string location = locationNode?.InnerText.Trim() ?? "N/A";

                            var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
                            string? link = linkNode?.GetAttributeValue("href", "N/A");

                            var adData = new AdData()
                            {
                                Title = title,
                                Price = price,
                                AdPostedOn = Convert.ToDateTime(date),
                                Location = location,
                                URL = link
                            };

                            await GetAdInfo(scrapedAds, adData);

                            // Send SMS only if the phone number is new
                            if (!sentNumbers.Contains(adData.PhoneNumber))
                            {
                                string smsNumber = GetSMSNumberForLocation(adData.Location);
                                if (smsNumber != null)
                                {
                                    await SMSClient.SendSMS(adData.PhoneNumber, smsNumber);
                                    sentNumbers.Add(adData.PhoneNumber); // Mark number as sent
                                }
                            }

                            scrapedAds.Add(adData);
                        }

                        page++; // Move to the next page
                        await Task.Delay(200); // Small delay to prevent rate limiting
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error on page {page}: {ex.Message}"); hasAds = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
            }

            return scrapedAds;
        }

        public static async Task<List<AdData>> TrzalackaGlazbalaIndexKrapinskoZagorska(List<AdData> scrapedAds, HashSet<string> sentNumbers, int page = 1)
        {
            try
            {
                Console.WriteLine("Connected! Navigating and scrapping Trzalačka glazbala ads on Index in Krapinsko Zagorska");

                bool hasAds = true;

                while (hasAds) // Keep looping until no ads are found
                {
                    try
                    {
                        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                        {
                            Headless = true,
                            ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                        });
                        await using var browserPage = await browser.NewPageAsync();

                        var response = await browserPage.GoToAsync(GetEncodedTrzalackaGlazbalaIndexKrapinskoZagorskaUrl(page), new NavigationOptions
                        {
                            WaitUntil = new[] { WaitUntilNavigation.Networkidle2 } // Ensures dynamic content is loaded
                        });

                        var html = await browserPage.GetContentAsync();
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");
                        Console.WriteLine($"Page {page}: Total ads found: {ads?.Count}");

                        if (ads == null || ads.Count == 0)
                        {
                            hasAds = false;
                            break;
                        }

                        foreach (var ad in ads)
                        {
                            var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
                            string title = titleNode?.InnerText.Trim() ?? "N/A";

                            var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
                            string price = priceNode?.InnerText.Trim() ?? "N/A";

                            var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
                            string date = dateNode?.InnerText.Trim() ?? "N/A";

                            var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
                            string location = locationNode?.InnerText.Trim() ?? "N/A";

                            var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
                            string? link = linkNode?.GetAttributeValue("href", "N/A");

                            var adData = new AdData()
                            {
                                Title = title,
                                Price = price,
                                AdPostedOn = Convert.ToDateTime(date),
                                Location = location,
                                URL = link
                            };

                            await GetAdInfo(scrapedAds, adData);

                            // Send SMS only if the phone number is new
                            if (!sentNumbers.Contains(adData.PhoneNumber))
                            {
                                string smsNumber = GetSMSNumberForLocation(adData.Location);
                                if (smsNumber != null)
                                {
                                    await SMSClient.SendSMS(adData.PhoneNumber, smsNumber);
                                    sentNumbers.Add(adData.PhoneNumber); // Mark number as sent
                                }
                            }

                            scrapedAds.Add(adData);
                        }

                        page++; // Move to the next page
                        await Task.Delay(200); // Small delay to prevent rate limiting
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error on page {page}: {ex.Message}"); hasAds = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
            }

            return scrapedAds;

            //try
            //{
            //    List<AdData> scrapedAds = new List<AdData>();

            //    if (authList.Count == 0)
            //    {
            //        Console.WriteLine("No AUTH values left to use.");
            //        return scrapedAds;
            //    }

            //    string auth = authList.First();
            //    string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";

            //    try
            //    {
            //        Console.WriteLine($"Connecting with AUTH: {auth}...");
            //        using var pw = await Playwright.CreateAsync();
            //        await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
            //        Console.WriteLine("Connected! Navigating and scrapping Trzalačka glazbala ads on Index in Krapina and Zagorje");

            //        var pageInstance = await browser.NewPageAsync();
            //        bool hasRecentAds = true;
            //        bool checkedNextPage = false;
            //        DateTime now = DateTime.Now;

            //        while (true)
            //        {
            //            await pageInstance.GotoAsync(GetEncodedTrzalackaGlazbalaIndexKrapinskoZagorskaUrl(page), new() { Timeout = 30000 });

            //            Console.WriteLine($"Scraping page...");
            //            var html = await pageInstance.ContentAsync();
            //            var htmlDoc = new HtmlDocument();
            //            htmlDoc.LoadHtml(html);

            //            var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");
            //            Console.WriteLine($"Total ads found: {ads.Count}");

            //            foreach (var ad in ads)
            //            {
            //                Console.WriteLine($"Processing ad #{ads.IndexOf(ad) + 1}");

            //                var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
            //                string title = titleNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati cijenu
            //                var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
            //                string price = priceNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati datum objave
            //                var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
            //                string date = dateNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati lokaciju
            //                var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
            //                string location = locationNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati link oglasa
            //                var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
            //                string? link = linkNode?.GetAttributeValue("href", "N/A");

            //                //scrapedAds
            //                Console.WriteLine($"Title: {title}");
            //                Console.WriteLine($"Price: {price}");
            //                Console.WriteLine($"Date: {date}");
            //                Console.WriteLine($"Location: {location}");
            //                Console.WriteLine($"Link: {link}");
            //                Console.WriteLine("--------------------------------");

            //                var adData = new AdData()
            //                {
            //                    Title = title,
            //                    Price = price,
            //                    AdPostedOn = Convert.ToDateTime(date),
            //                    Location = location,
            //                    URL = link
            //                };
            //                await GetAdInfo(adData);

            //                //await SMSClient.SendSMS(adData.PhoneNumber, "0993006000");

            //                scrapedAds.Add(adData);
            //            }

            //            if (!hasRecentAds)
            //            {
            //                if (checkedNextPage) break;
            //                checkedNextPage = true;
            //                page++;
            //            }
            //            else
            //            {
            //                checkedNextPage = false;
            //                page++;
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error with AUTH {auth}: {ex.Message}");
            //    }

            //    return scrapedAds;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        public static async Task<List<AdData>> TrzalackaGlazbalaIndexSisackoMoslavacka(List<AdData> scrapedAds, HashSet<string> sentNumbers, int page = 1)
        {
            try
            {
                Console.WriteLine("Connected! Navigating and scrapping Trzalačka glazbala ads on Index in Sisačko Moslavačka");

                bool hasAds = true;

                while (hasAds) // Keep looping until no ads are found
                {
                    try
                    {
                        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                        {
                            Headless = true,
                            ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                        });
                        await using var browserPage = await browser.NewPageAsync();

                        var response = await browserPage.GoToAsync(GetEncodedTrzalackaGlazbalaIndexSisackoMoslavackaUrl(page), new NavigationOptions
                        {
                            WaitUntil = new[] { WaitUntilNavigation.Networkidle2 } // Ensures dynamic content is loaded
                        });

                        var html = await browserPage.GetContentAsync();
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");
                        Console.WriteLine($"Page {page}: Total ads found: {ads?.Count}");

                        if (ads == null || ads.Count == 0)
                        {
                            hasAds = false;
                            break;
                        }

                        foreach (var ad in ads)
                        {
                            var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
                            string title = titleNode?.InnerText.Trim() ?? "N/A";

                            var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
                            string price = priceNode?.InnerText.Trim() ?? "N/A";

                            var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
                            string date = dateNode?.InnerText.Trim() ?? "N/A";

                            var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
                            string location = locationNode?.InnerText.Trim() ?? "N/A";

                            var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
                            string? link = linkNode?.GetAttributeValue("href", "N/A");

                            var adData = new AdData()
                            {
                                Title = title,
                                Price = price,
                                AdPostedOn = Convert.ToDateTime(date),
                                Location = location,
                                URL = link
                            };

                            await GetAdInfo(scrapedAds, adData);

                            // Send SMS only if the phone number is new
                            if (!sentNumbers.Contains(adData.PhoneNumber))
                            {
                                string smsNumber = GetSMSNumberForLocation(adData.Location);
                                if (smsNumber != null)
                                {
                                    await SMSClient.SendSMS(adData.PhoneNumber, smsNumber);
                                    sentNumbers.Add(adData.PhoneNumber); // Mark number as sent
                                }
                            }

                            scrapedAds.Add(adData);
                        }

                        page++; // Move to the next page
                        await Task.Delay(200); // Small delay to prevent rate limiting
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error on page {page}: {ex.Message}"); hasAds = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
            }

            return scrapedAds;

            //try
            //{
            //    List<AdData> scrapedAds = new List<AdData>();

            //    if (authList.Count == 0)
            //    {
            //        Console.WriteLine("No AUTH values left to use.");
            //        return scrapedAds;
            //    }

            //    string auth = authList.First();
            //    string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";

            //    try
            //    {
            //        Console.WriteLine($"Connecting with AUTH: {auth}...");
            //        using var pw = await Playwright.CreateAsync();
            //        await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
            //        Console.WriteLine("Connected! Navigating and scrapping Trzalačka glazbala ads on Index in Sisak and Moslavina");

            //        var pageInstance = await browser.NewPageAsync();
            //        bool hasRecentAds = true;
            //        bool checkedNextPage = false;
            //        DateTime now = DateTime.Now;

            //        while (true)
            //        {
            //            await pageInstance.GotoAsync(GetEncodedTrzalackaGlazbalaIndexSisackoMoslavackaUrl(page), new() { Timeout = 30000 });

            //            Console.WriteLine($"Scraping page...");
            //            var html = await pageInstance.ContentAsync();
            //            var htmlDoc = new HtmlDocument();
            //            htmlDoc.LoadHtml(html);

            //            var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");
            //            Console.WriteLine($"Total ads found: {ads.Count}");

            //            foreach (var ad in ads)
            //            {
            //                Console.WriteLine($"Processing ad #{ads.IndexOf(ad) + 1}");

            //                var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
            //                string title = titleNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati cijenu
            //                var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
            //                string price = priceNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati datum objave
            //                var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
            //                string date = dateNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati lokaciju
            //                var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
            //                string location = locationNode?.InnerText.Trim() ?? "N/A";

            //                // Dohvati link oglasa
            //                var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
            //                string? link = linkNode?.GetAttributeValue("href", "N/A");

            //                //scrapedAds
            //                Console.WriteLine($"Title: {title}");
            //                Console.WriteLine($"Price: {price}");
            //                Console.WriteLine($"Date: {date}");
            //                Console.WriteLine($"Location: {location}");
            //                Console.WriteLine($"Link: {link}");
            //                Console.WriteLine("--------------------------------");

            //                var adData = new AdData()
            //                {
            //                    Title = title,
            //                    Price = price,
            //                    AdPostedOn = Convert.ToDateTime(date),
            //                    Location = location,
            //                    URL = link
            //                };
            //                await GetAdInfo(adData);

            //                //await SMSClient.SendSMS(adData.PhoneNumber, "0925008000");

            //                scrapedAds.Add(adData);
            //            }

            //            if (!hasRecentAds)
            //            {
            //                if (checkedNextPage) break;
            //                checkedNextPage = true;
            //                page++;
            //            }
            //            else
            //            {
            //                checkedNextPage = false;
            //                page++;
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error with AUTH {auth}: {ex.Message}");
            //    }

            //    return scrapedAds;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        private static async Task<AdData> GetAdInfo(List<AdData> scrapedAds, AdData adData)
        {
            try
            {
                string auth = authList.First();
                string SBR_CDP = $"wss://{auth}@brd.superproxy.io:9222";

                try
                {
                    //Console.WriteLine($"Connecting with AUTH: {auth}...");
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                    //var client = new WebClient();
                    //client.Proxy = new WebProxy("brd.superproxy.io:33335");
                    //client.Proxy.Credentials = new NetworkCredential("brd-customer-hl_615a7fb5-zone-web_unlocker1", "cm8cbanne8a5");
                    //client.Headers[HttpRequestHeader.Authorization] = "ae77c49246aad19e9122956f1d144bcf256aebb9a7808f25139b7f8c75314895";


                    //using var pw = await Playwright.CreateAsync();
                    //await using var browser = await pw.Chromium.ConnectOverCDPAsync(SBR_CDP);
                    Console.WriteLine($"Connected! Navigating and scrapping Fotoaparati ad detail on Index {adData.Location}");

                    //var pageInstance = await browser.NewPageAsync();
                    DateTime now = DateTime.Now;

                    //await pageInstance.GotoAsync($"https://www.index.hr{adData.URL}", new() { Timeout = 30000 });

                    //Console.WriteLine($"Scraping specific ad data...");
                    //var html = await pageInstance.ContentAsync();

                    await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                    {
                        Headless = true,
                        ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                        //Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
                        DumpIO = true
                    });
                    await using var browserPage = await browser.NewPageAsync();

                    var response = await browserPage.GoToAsync($"https://www.index.hr{adData.URL}", new NavigationOptions
                    {
                        WaitUntil = new[] { WaitUntilNavigation.Networkidle2 } // Ensures dynamic content is loaded
                    });

                    var visibleSelector = await browserPage.WaitForSelectorAsync("div.ant-col.ant-col-xs-24");

                    var isVisible = await visibleSelector.IsVisibleAsync();

                    //await pageInstance.GotoAsync(, new() { Timeout = 30000 });

                    Console.WriteLine($"Scraping specific ad data...");

                    var html = await browserPage.GetContentAsync();

                    //var html = client.DownloadString();
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    var username = htmlDoc.DocumentNode.SelectSingleNode(".//div[contains(@class, 'SellerInfo__username')]")?.InnerHtml ?? "";
                    var a1 = username;

                    Console.WriteLine($"Found seller: {username}");

                    // Click on "Pogledaj sve oglase" link
                    var allAdsLink = htmlDoc.DocumentNode.SelectSingleNode(".//a[contains(@class, 'sellerLink')]");

                    if (allAdsLink != null)
                    {
                        string pattern = @"href\s*=\s*""([^""]+)""";
                        Match match = Regex.Match(allAdsLink.OuterHtml, pattern);

                        if (match.Success)
                        {
                            Console.WriteLine("Extracted href: " + match.Groups[1].Value);
                            var userDetails = match.Groups[1].Value;

                            Console.WriteLine($"Connecting with AUTH2: brd-customer-hl_615a7fb5-zone-scraping_browser2:xlwc4xjelvsu...");
                            //using var pw2 = await Playwright.CreateAsync();
                            //var browser2 = await pw2.Chromium.ConnectOverCDPAsync(SBR_CDP);

                            //var page2 = await browser2.NewPageAsync();
                            //Console.WriteLine($"Going to seller details");
                            //await page2.GotoAsync($"https://www.index.hr{userDetails}");

                            //string content2 = await page2.ContentAsync();

                            //;

                            var htmlDoc2 = new HtmlDocument();

                            await using var browser2 = await Puppeteer.LaunchAsync(new LaunchOptions
                            {
                                Headless = true,
                                ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                                //Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
                                DumpIO = true
                            });
                            await using var browserPage2 = await browser2.NewPageAsync();

                            var response2 = await browserPage2.GoToAsync($"https://www.index.hr{userDetails}", new NavigationOptions
                            {
                                WaitUntil = new[] { WaitUntilNavigation.Networkidle2 } // Ensures dynamic content is loaded
                            });

                            var visibleSelector2 = await browserPage2.WaitForSelectorAsync("span[class*='SellerSectionWeb__infoValue']");

                            //, new WaitForSelectorOptions
                            //  {
                            //      Visible = true // Ensures it's visible before continuing
                            //  }

                            //var a = await browserPage2.WaitForFunctionAsync("document.querySelector(\"img[class^='PhoneButton__phoneIcon']\") !== null", new WaitForFunctionOptions
                            //{
                            //    Timeout = 5000
                            //});

                            //await a.JsonValueAsync();

                            //var isVisible2 = await visibleSelector2.IsVisibleAsync();

                            //await pageInstance.GotoAsync(, new() { Timeout = 30000 });

                            Console.WriteLine($"Scraping specific ad data...");

                            var html2 = await browserPage2.GetContentAsync();

                            htmlDoc2.LoadHtml(html2);

                            var phones = htmlDoc2.DocumentNode.SelectNodes("//span[contains(@class, 'SellerSectionWeb__infoValue')]");

                            if (phones != null)
                            {
                                Console.WriteLine($"Getting phone number of {username}");
                                foreach (var phone in phones)
                                {
                                    string href = phone.InnerText;

                                    if (!string.IsNullOrEmpty(href) && (href == "+385959066565"))
                                    {
                                        return adData;
                                    }

                                    if (href.Contains("09") && !string.IsNullOrEmpty(href))
                                    {
                                        string pattern2 = @"^0"; // Regex koji traži 0 na početku stringa
                                        string replacement = "+385";

                                        string result = Regex.Replace(href, pattern2, replacement);
                                        result = result.Replace("/", "");
                                        result = result.Replace(" ", "");
                                        Console.WriteLine($"Found number: {username} of {username}");

                                        if (Appender.CheckAndAppend(result))
                                        {
                                            adData.PhoneNumber = result;
                                        }

                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("No matching links found.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No match found.");
                        }
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error with AUTH {auth}: {ex.Message}");
                }

                return adData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<List<AdData>> TrzalackaGlazbalaIndexBjelovarskaMedimurskaVarazdinska(List<AdData> scrapedAds, HashSet<string> sentNumbers, int page = 1)
        {
            try
            {
                Console.WriteLine("Connected! Navigating and scrapping Trzalačka glazbala ads on Index in Bjelovarsko Međimurska and Varaždinska");

                bool hasAds = true;

                while (hasAds) // Keep looping until no ads are found
                {
                    try
                    {
                        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                        {
                            Headless = true,
                            ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                            
                        });
                        await using var browserPage = await browser.NewPageAsync();

                        var response = await browserPage.GoToAsync(GetEncodedTrzalackaGlazbalaIndexBjelovarskaMedimurskaVarazdinskaUrl(page), new NavigationOptions
                        {
                            WaitUntil = new[] { WaitUntilNavigation.Networkidle2 }, // Ensures dynamic content is loaded,
                            Timeout = 60000
                        });

                        var html = await browserPage.GetContentAsync();
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        var ads = htmlDoc.DocumentNode.SelectNodes("//div[@class='ant-col ant-col-xs-24']");
                        Console.WriteLine($"Page {page}: Total ads found: {ads?.Count}");

                        if (ads == null || ads.Count == 0)
                        {
                            hasAds = false;

                            if (scrapedAds == null)
                            {
                                scrapedAds = new List<AdData>(); // Initialize if null
                            }

                            return scrapedAds;
                        }

                        foreach (var ad in ads)
                        {
                            var titleNode = ad.SelectSingleNode(".//span[contains(@class, 'AdSummary__title')]");
                            string title = titleNode?.InnerText.Trim() ?? "N/A";

                            var priceNode = ad.SelectSingleNode(".//span[contains(@class, 'adPrice__price')]");
                            string price = priceNode?.InnerText.Trim() ?? "N/A";

                            var dateNode = ad.SelectSingleNode(".//div[contains(@class, 'AdSummary__info___')]/span");
                            string date = dateNode?.InnerText.Trim() ?? "N/A";

                            var locationNode = ad.SelectSingleNode(".//div[contains(@class, 'adLocation__location') and not(*)]");
                            string location = locationNode?.InnerText.Trim() ?? "N/A";

                            var linkNode = ad.SelectSingleNode(".//a[contains(@class, 'AdLink__link')]");
                            string? link = linkNode?.GetAttributeValue("href", "N/A");

                            var adData = new AdData()
                            {
                                Title = title,
                                Price = price,
                                AdPostedOn = Convert.ToDateTime(date),
                                Location = location,
                                URL = link
                            };

                            await GetAdInfo(scrapedAds, adData);

                            // Send SMS only if the phone number is new
                            if (!sentNumbers.Contains(adData.PhoneNumber))
                            {
                                string smsNumber = GetSMSNumberForLocation(adData.Location);
                                if (smsNumber != null)
                                {
                                    await SMSClient.SendSMS(adData.PhoneNumber, smsNumber);
                                    sentNumbers.Add(adData.PhoneNumber); // Mark number as sent
                                }
                            }

                            scrapedAds.Add(adData);
                        }

                        page++; // Move to the next page
                        await Task.Delay(200); // Small delay to prevent rate limiting
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error on page {page}: {ex.Message}"); hasAds = false; hasAds = false;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
            }

            return scrapedAds;
        }

        public static async Task<List<AdData>> GetNjuskaloTrzalackaGlazbalaZagreb()
        {
            return await GetNjuskaloData(GetTrzalackaGlazbalaZagrebUrl(1), 1);
        }

        public static async Task<List<AdData>> GetNjuskaloTrzalackaGlazbalaKarlovac()
        {
            return await GetNjuskaloData(GetTrzalackaGlazbalaKarlovacUrl(1), 1);
        }

        public static async Task<List<AdData>> GetNjuskaloTrzalackaGlazbalaRijekaIstra()
        {
            return await GetNjuskaloData(GetTrzalackaGlazbalaRijekaIstraUrl(1), 1);
        }

        public static async Task<List<AdData>> GetNjuskaloTrzalackaGlazbalaZadarLika()
        {
            return await GetNjuskaloData(GetTrzalackaGlazbalaZadarLikaUrl(1), 1);
        }

        public static async Task<List<AdData>> GetNjuskaloTrzalackaGlazbalaDubrovnikSplit()
        {
            return await GetNjuskaloData(GetTrzalackaGlazbalaDubrovnikSplitUrl(1), 1);
        }

        public static async Task<List<AdData>> GetNjuskaloTrzalackaGlazbalaKrapinskoZagorska()
        {
            return await GetNjuskaloData(GetTrzalackaGlazbalaKrapinskoZagorskaUrl(1), 1);
        }

        public static async Task<List<AdData>> GetNjuskaloTrzalackaGlazbalaSisackoMoslavacka()
        {
            return await GetNjuskaloData(GetTrzalackaGlazbalaSisackoMoslavackaUrl(1), 1);
        }

        public static async Task<List<AdData>> GetNjuskaloTrzalackaGlazbalaBjelovarskaMedimurskaVarazdinska()
        {
            return await GetNjuskaloData(GetTrzalackaGlazbalaBjelovarskaMedimurskaVarazdinskaUrl(1), 1);
        }
    }
}
