using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    // Glavni servis za fiskalizaciju e-računa
    public class FiskalizacijaService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FiskalizacijaService> _logger;
        private readonly string _baseUri;
        private readonly string _authToken;

        public FiskalizacijaService(HttpClient httpClient, ILogger<FiskalizacijaService> logger, string baseUri, string authToken)
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseUri = baseUri;
            _authToken = authToken;
        }

        // Slanje e-računa na fiskalizaciju
        public async Task<FiskalniRacunResponse> PosaljiRacunAsync(FiskalniRacunRequest racun)
        {
            try
            {
                var json = JsonSerializer.Serialize(racun, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUri}/fiskalni-racun");
                request.Headers.Add("Authorization", $"Bearer {_authToken}");
                request.Content = content;

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var racunResponse = JsonSerializer.Deserialize<FiskalniRacunResponse>(responseString);
                return racunResponse ?? throw new Exception("Nema odgovora od fiskalnog servisa.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri slanju fiskalnog računa.");
                throw;
            }
        }

        // Dohvat statusa e-računa
        public async Task<FiskalniStatusResponse> DohvatiStatusRacunaAsync(string brojRacuna)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUri}/fiskalni-racun/status/{brojRacuna}");
                request.Headers.Add("Authorization", $"Bearer {_authToken}");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var statusResponse = JsonSerializer.Deserialize<FiskalniStatusResponse>(responseString);
                return statusResponse ?? throw new Exception("Nema odgovora za status fiskalnog računa.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri dohvaćanju statusa fiskalnog računa.");
                throw;
            }
        }
    }

    // DTO za e-račun (malo izrezano zbog dužine, ali sa svim poljima po specifikaciji)
    public class FiskalniRacunRequest
    {
        [JsonPropertyName("Oib")]
        public string Oib { get; set; }

        [JsonPropertyName("DatumVrijeme")]
        public string DatumVrijeme { get; set; } // ISO 8601 format

        [JsonPropertyName("OibOperatera")]
        public string OibOperatera { get; set; }

        [JsonPropertyName("IdPrimatelja")]
        public string IdPrimatelja { get; set; }

        [JsonPropertyName("NacinPlacanja")]
        public int NacinPlacanja { get; set; }

        [JsonPropertyName("Ukupno")]
        public decimal Ukupno { get; set; }

        [JsonPropertyName("Stavke")]
        public List<StavkaRacuna> Stavke { get; set; }

        // Detaljna polja stavki, iznosi, porezi, popusti, porezne stope itd su u StavkaRacuna

        [JsonPropertyName("Napomena")]
        public string Napomena { get; set; }
    }

    public class StavkaRacuna
    {
        [JsonPropertyName("SifraArtikla")]
        public string SifraArtikla { get; set; }

        [JsonPropertyName("Naziv")]
        public string Naziv { get; set; }

        [JsonPropertyName("Kolicina")]
        public decimal Kolicina { get; set; }

        [JsonPropertyName("JedinicaMjere")]
        public string JedinicaMjere { get; set; }

        [JsonPropertyName("CijenaBezPoreza")]
        public decimal CijenaBezPoreza { get; set; }

        [JsonPropertyName("PostotakPoreza")]
        public decimal PostotakPoreza { get; set; }

        [JsonPropertyName("IznosPoreza")]
        public decimal IznosPoreza { get; set; }

        [JsonPropertyName("Iznos")]
        public decimal Iznos { get; set; }
    }

    // Odgovor od fiskalne službe
    public class FiskalniRacunResponse
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [JsonPropertyName("BrojRacuna")]
        public string BrojRacuna { get; set; }

        [JsonPropertyName("KodQr")]
        public string KodQr { get; set; }

        [JsonPropertyName("Poruka")]
        public string Poruka { get; set; }
    }

    // Status računa (npr. potvrda)
    public class FiskalniStatusResponse
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [JsonPropertyName("BrojRacuna")]
        public string BrojRacuna { get; set; }

        [JsonPropertyName("Podaci")]
        public string Podaci { get; set; }
    }
}
