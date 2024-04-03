#nullable enable
namespace BosquesNalcahue.Dtos
{
    public class BaseReport
    {
        public string? ReportType { get; set; }
        public string? ProductType { get; set; }
        public string? OperatorName { get; set; }
        public string? Folio { get; set; }
        public DateTime Date { get; set; }
        public string? ClientName { get; set; }
        public string? ClientId { get; set; }
        public string? TruckCompany { get; set; }
        public string? TruckDriver { get; set; }
        public string? TruckDriverId { get; set; }
        public string? TruckPlate { get; set; }
        public IEnumerable<string>? Species { get; set; }
        public string? ProductName { get; set; }
    }
}
