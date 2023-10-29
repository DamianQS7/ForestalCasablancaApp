using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.Models
{
    public class DespachoLeñaModel
    {
        public List<double?> Alturas { get; set; } = new List<double?> {null, null, null, null, null, null, null, null };
        public double? AltoPalomera { get; set; }
        public double? AnchoPalomera { get; set; }
        public double? LargoCamion { get; set; }
        public int? Bancos { get; set; }
        public double? AlturaMedia { get; set; }
        public double? TotalMetrosLeña { get; set; }
    }
}
