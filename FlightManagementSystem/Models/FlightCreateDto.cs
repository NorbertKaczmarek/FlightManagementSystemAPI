using System.Text.Json.Serialization;
using FlightManagementSystem.Entities;

namespace FlightManagementSystem.Models
{
    public class FlightCreateDto
    {
        public int NumerLotu { get; set; }
        public DateTimeOffset DataWylotu { get; set; }
        public string MiejsceWylotu { get; set; }
        public string MiejscePrzylotu { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PlaneType TypSamolotu { get; set; }
    }
}
