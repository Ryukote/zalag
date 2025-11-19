using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ZalagaonicaTracker.Scrapers
{
    public static class InvesticijskoZlatoSrebro
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

        private static string GetInvesticijskoZlatoSrebroZagrebUrl(int page)
        {
            return $"https://www.njuskalo.hr/rucni-alati?geo%5BlocationIds%5D=1153&page={page}";
        }

        private static string GetInvesticijskoZlatoSrebroKarlovacUrl(int page)
        {
            return $"https://www.njuskalo.hr/rucni-alati?geo%5BlocationIds%5D=1155&page={page}";
        }

        private static string GetInvesticijskoZlatoSrebroRijekaIstraUrl(int page)
        {
            return $"https://www.njuskalo.hr/rucni-alati?geo%5BlocationIds%5D=1162%2C1154&page={page}";
        }

        private static string GetInvesticijskoZlatoSrebroZadarLikaUrl(int page)
        {
            return $"https://www.njuskalo.hr/rucni-alati?geo%5BlocationIds%5D=1169%2C1158&page={page}";
        }

        private static string GetInvesticijskoZlatoSrebroDubrovnikSplitUrl(int page)
        {
            return $"https://www.njuskalo.hr/rucni-alati?geo%5BlocationIds%5D=1152%2C1164&page={page}";
        }

        private static string GetInvesticijskoZlatoSrebroKrapinskoZagorskaUrl(int page)
        {
            return $"https://www.njuskalo.hr/rucni-alati?geo%5BlocationIds%5D=1157&page={page}";
        }

        private static string GetInvesticijskoZlatoSrebroSisackoMoslavackaUrl(int page)
        {
            return $"https://www.njuskalo.hr/rucni-alati?geo%5BlocationIds%5D=1163&page={page}";
        }

        private static string GetInvesticijskoZlatoSrebroBjelovarskaMedimurskaVarazdinskaUrl(int page)
        {
            return $"https://www.njuskalo.hr/rucni-alati?geo%5BlocationIds%5D=1150%2C1159%2C1166&page={page}";
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

        public static async Task<List<AdData>> GetNjuskaloInvesticijskoZlatoSrebroZagreb()
        {
            return await GetNjuskaloData(GetInvesticijskoZlatoSrebroZagrebUrl(1), 1);
        }

        public static async Task<List<AdData>> GetNjuskaloInvesticijskoZlatoSrebroKarlovac()
        {
            return await GetNjuskaloData(GetInvesticijskoZlatoSrebroKarlovacUrl(1), 1);
        }

        public static async Task<List<AdData>> GetNjuskaloInvesticijskoZlatoSrebroRijekaIstra()
        {
            return await GetNjuskaloData(GetInvesticijskoZlatoSrebroRijekaIstraUrl(1), 1);
        }

        public static async Task<List<AdData>> GetNjuskaloInvesticijskoZlatoSrebroZadarLika()
        {
            return await GetNjuskaloData(GetInvesticijskoZlatoSrebroZadarLikaUrl(1), 1);
        }

        public static async Task<List<AdData>> GetNjuskaloInvesticijskoZlatoSrebroDubrovnikSplit()
        {
            return await GetNjuskaloData(GetInvesticijskoZlatoSrebroDubrovnikSplitUrl(1), 1);
        }

        public static async Task<List<AdData>> GetNjuskaloInvesticijskoZlatoSrebroKrapinskoZagorska()
        {
            return await GetNjuskaloData(GetInvesticijskoZlatoSrebroKrapinskoZagorskaUrl(1), 1);
        }

        public static async Task<List<AdData>> GetNjuskaloInvesticijskoZlatoSrebroSisackoMoslavacka()
        {
            return await GetNjuskaloData(GetInvesticijskoZlatoSrebroSisackoMoslavackaUrl(1), 1);
        }

        public static async Task<List<AdData>> GetNjuskaloInvesticijskoZlatoSrebroBjelovarskaMedimurskaVarazdinska()
        {
            return await GetNjuskaloData(GetInvesticijskoZlatoSrebroBjelovarskaMedimurskaVarazdinskaUrl(1), 1);
        }
    }
}
