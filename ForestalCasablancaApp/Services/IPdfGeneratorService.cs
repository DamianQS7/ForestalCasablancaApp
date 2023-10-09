using ForestalCasablancaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.Services
{
    public interface IPdfGeneratorService
    {
        void GenerateTrozoAserrablePDF(TrozoAserrableViewModel model);
        void GenerateLeñaPDF(LeñaViewModel model);
        void GenerateMetroRumaPDF(MetroRumaViewModel model);
    }
}
