using FlightManagementSystem.Entities;
using System.Text.Json.Serialization;

namespace FlightManagementSystem.Models
{
    public class EditFlightDto
    {
        public int NumerLotu { get; set; }
        public DateTime DataWylotu { get; set; }
        public string MiejsceWylotu { get; set; }
        public string MiejscePrzylotu { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PlaneType TypSamolotu { get; set; }
    }
}
