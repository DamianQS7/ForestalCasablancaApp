using ForestalCasablancaApp.Models;
using System.Collections.ObjectModel;

namespace BosquesNalcahue.Models
{
    public class MedidasEspecie
    {
        public string SectionTitle { get; set; }
        public string Especie { get; set; }
        public string LargoEspecie { get; set; }
        public double? DiametroIngresado { get; set; }
        public int? CantidadIngresada { get; set; }
        public int CantidadTotalSum { get; set; }
        public double TotalSumFinal { get; set; }
        public ObservableCollection<MedidaTrozoAserrable> ListaMedidas { get; set; } = new();

        public List<string> ListaEspecies { get; set; } = new()
        {
            "Oregón",
            "Eucalipto",
            "Roble Estaca",
            "Roble Desecho",
            "Hualle",
            "Raulí",
            "Coihue",
            "Pellín",
            "Laurel",
            "Olivillo",
            "Tepa",
            "Ulmo",
            "Lingue",
            "Avellano",
            "Aromo",
            "Encino"
        };

        public List<string> ListaLargos { get; set; } = new()
        {
            "1.20",
            "1.60",
            "2.10",
            "2.44",
            "2.74",
            "3.20",
            "3.66",
            "4.00",
            "5.00",
            "6.00",
            "7.00"
        };
    }
}
