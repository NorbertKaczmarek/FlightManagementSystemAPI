﻿using FlightManagementSystem.Entities;
using System.Text.Json.Serialization;

namespace FlightManagementSystem.Models
{
    public class FlightEditDto
    {
        public int NumerLotu { get; set; }
        public DateTimeOffset DataWylotu { get; set; }
        public string MiejsceWylotu { get; set; }
        public string MiejscePrzylotu { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PlaneType TypSamolotu { get; set; }
    }
}
