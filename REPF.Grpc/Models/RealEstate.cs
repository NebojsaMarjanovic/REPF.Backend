using Microsoft.ML.Data;
using System;

namespace REPF.Grpc.Models
{
    public class RealEstate
    {
        [LoadColumn(0)]
        public int m2 { get; set; }
        [LoadColumn(1)]
        public string heatingType { get; set; } //district-centralno; central-etazno
        [LoadColumn(2)]
        public int elevator { get; set; }
        [LoadColumn(3)]
        public double price { get; set; }
        [LoadColumn(4)]
        public DateTime createdAt { get; set; }
        [LoadColumn(5)]
        public double roomCount { get; set; }
        [LoadColumn(6)]
        public int redactedFloor { get; set; }
        [LoadColumn(7)]
        public string placeTitle { get; set; }
    }
}
