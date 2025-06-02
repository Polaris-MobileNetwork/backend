namespace Domain.Entities
{
    public class NetworkMeasurement
    {
        public Guid Id { get; set; }
        public long TimeStamp { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string NetworkType { get; set; }
        public string? PLMNId { get; set; }
        public int? Lac { get; set; }
        public int? Tac { get; set; }
        public int? Rac { get; set; }
        public string? CellId { get; set; }
        public int? ARFCN { get; set; }
        public string? FrequencyBand { get; set; }
        public double? ActualFrequencyMhz { get; set; }
        public int SignalStrength { get; set; }
        public int? RSRP { get; set; }
        public int? RSRQ { get; set; }
        public int? RSCP { get; set; }
        public int? RXLEV { get; set; }
        public double? ECNO { get; set; }

    }
}
