using System.Text.Json.Serialization;
using FlightManagementSystem.Entities;

namespace FlightManagementSystem.Models
{
    public class CreateFlightDto
    {
        public int NumerLotu { get; set; }
        public string? DataWylotuString { get; set; }  // 16.04.2024 14:30
        public string MiejsceWylotu { get; set; }
        public string MiejscePrzylotu { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PlaneType TypSamolotu { get; set; }
    }
}
