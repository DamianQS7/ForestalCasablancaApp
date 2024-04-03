#nullable enable
namespace BosquesNalcahue.Dtos
{
    public class SingleProductReport : BaseReport
    {
        public string? Origin { get; set; }
        public double TruckHeight { get; set; }
        public double TruckLength { get; set; }
        public int Banks { get; set; }
        public double PalomeraHeight { get; set; }
        public double PalomeraWidth { get; set; }
        public double FinalQuantity { get; set; }
    }
}
