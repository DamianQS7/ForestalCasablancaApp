
namespace ForestalCasablancaApp.Models
{
    public class DespachoModel
    {
        public double? AltoPalomera { get; set; }
        public double? AltoPalomera2 { get; set; }
        public double? AnchoPalomera { get; set; }
        public double? LargoCamion { get; set; }
        public int? Bancos { get; set; }
        public double? AlturaMedia { get; set; }
        public double? TotalMetros { get; set; }
        public string Especie { get; set; }
        public List<double?> Alturas { get; set; } = new List<double?> 
        { 
            null, null, null, null, 
            null, null, null, null,
            null, null, null, null
        };
    }
}
