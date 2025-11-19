using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Application.Fiskalizacija.Models
{
    public class FiskalniRacunRequest
    {
        [JsonPropertyName("Oib")]
        public string Oib { get; set; }

        [JsonPropertyName("DatumVrijeme")]
        public DateTime DatumVrijeme { get; set; }

        [JsonPropertyName("OibOperatera")]
        public string OibOperatera { get; set; }

        [JsonPropertyName("IdPrimatelja")]
        public string IdPrimatelja { get; set; }

        [JsonPropertyName("NacinPlacanja")]
        public int NacinPlacanja { get; set; }

        [JsonPropertyName("Ukupno")]
        public decimal Ukupno { get; set; }

        [JsonPropertyName("Stavke")]
        public List<StavkaRacuna> Stavke { get; set; } = new List<StavkaRacuna>();

        [JsonPropertyName("UkupniPorez")]
        public decimal UkupniPorez { get; set; }

        [JsonPropertyName("UkupniPopust")]
        public decimal UkupniPopust { get; set; }

        [JsonPropertyName("Napomena")]
        public string? Napomena { get; set; }
    }

    public class StavkaRacuna
    {
        [JsonPropertyName("Sifra")]
        public string Sifra { get; set; }

        [JsonPropertyName("Naziv")]
        public string Naziv { get; set; }

        [JsonPropertyName("Kolicina")]
        public decimal Kolicina { get; set; }

        [JsonPropertyName("JedinicaMjere")]
        public string JedinicaMjere { get; set; }

        [JsonPropertyName("Cijena")]
        public decimal Cijena { get; set; }

        [JsonPropertyName("Popust")]
        public decimal Popust { get; set; }

        [JsonPropertyName("Porez")]
        public decimal Porez { get; set; }

        [JsonPropertyName("Iznos")]
        public decimal Iznos { get; set; }
    }

    public class FiskalniRacunResponse
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [JsonPropertyName("BrojRacuna")]
        public string BrojRacuna { get; set; }

        [JsonPropertyName("QRcode")]
        public string QRcode { get; set; }

        [JsonPropertyName("Poruka")]
        public string Poruka { get; set; }
    }

    public class FiskalniStatusResponse
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [JsonPropertyName("BrojRacuna")]
        public string BrojRacuna { get; set; }

        [JsonPropertyName("DatumVrijeme")]
        public DateTime DatumVrijeme { get; set; }

        [JsonPropertyName("Poruka")]
        public string Poruka { get; set; }
    }
}
