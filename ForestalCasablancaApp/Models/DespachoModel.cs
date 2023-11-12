
namespace ForestalCasablancaApp.Models
{
    public class DespachoModel
    {
        public string AltoPalomera { get; set; }
        public string AltoPalomera2 { get; set; }
        public string AnchoPalomera { get; set; }
        public string LargoCamion { get; set; }
        public string Bancos { get; set; }
        public double AlturaMedia { get; set; }
        public double TotalMetros { get; set; }
        public string Especie { get; set; }
        public double MedidaPalomera { get; set; }
        public bool IsPalomeraValid { get; set; } = true;
        public List<string> Alturas { get; set; } = new List<string> 
        { 
            null, null, null, null, 
            null, null, null, null,
            null, null, null, null
        };
    }
}
