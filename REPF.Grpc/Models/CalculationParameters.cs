using Microsoft.ML.Data;
using System;
using System.Diagnostics.CodeAnalysis;

namespace REPF.Grpc.Models
{
    public class CalculationParameters
    {
        [LoadColumn(0)]
        public float Quadrature { get; set; }
        [LoadColumn(1)]
        public string HeatingType { get; set; } = null!;
        [LoadColumn(2)]
        public float Elevator { get; set; }
        [LoadColumn(3)]
        public float Price { get; set; }
        [LoadColumn(4)]
        public DateTime CreatedAt { get; set; }
        [LoadColumn(5)]
        public float RoomCount { get; set; }
        [LoadColumn(6)]
        public float RedactedFloor { get; set; }
        [LoadColumn(7)]
        public bool IsLastFloor { get; set; }

        [LoadColumn(8)]
        public string? FurnishedStatus { get; set; }
        [LoadColumn(9)]
        public string? RegisteredStatus { get; set; }

        [LoadColumn(10)]
        public string Location { get; set; } = null!;
    }
}
