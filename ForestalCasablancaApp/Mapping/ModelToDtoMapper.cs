using BosquesNalcahue.Dtos;
using BosquesNalcahue.Models;
using ForestalCasablancaApp.ViewModels;
using System.Globalization;

namespace BosquesNalcahue.Mapping
{
    public static class ModelToDtoMapper
    {
        public static SingleProductReport MapToSingleProductReport(LeñaViewModel model)
        {
            return new SingleProductReport
            {
                ReportType = "SingleProductReport",
                ProductType = "Leña",
                OperatorName = model.OperatorName,
                Folio = model.Folio,
                Date = model.ReportDate,
                ClientName = model.Cliente.Nombre,
                ClientId = model.Cliente.RUT,
                TruckCompany = model.DatosCamion.EmpresaTransportista,
                TruckDriver = model.DatosCamion.Chofer,
                TruckDriverId = model.DatosCamion.RutChofer,
                TruckPlate = model.DatosCamion.Patente,
                ProductName = model.Despacho.Especie,
                Origin = model.Despacho.UnidadOrigen,
                TruckHeight = model.Despacho.AlturaMedia,
                TruckLength = double.Parse(model.Despacho.LargoCamion, CultureInfo.InvariantCulture),
                Banks = int.Parse(model.Despacho.Bancos),
                PalomeraHeight = model.Despacho.AlturaMediaPalomera,
                PalomeraWidth = string.IsNullOrEmpty(model.Despacho.AnchoPalomera) 
                                ? 0 
                                : double.Parse(model.Despacho.AnchoPalomera, CultureInfo.InvariantCulture),
                FinalQuantity = model.Despacho.TotalMetros,
            };
        }

        public static SingleProductReport MapToSingleProductReport(MetroRumaViewModel model)
        {
            return new SingleProductReport
            {
                ReportType = "SingleProductReport",
                ProductType = "Metro Ruma",
                OperatorName = model.OperatorName,
                Folio = model.Folio,
                Date = model.ReportDate,
                ClientName = model.Cliente.Nombre,
                ClientId = model.Cliente.RUT,
                TruckCompany = model.DatosCamion.EmpresaTransportista,
                TruckDriver = model.DatosCamion.Chofer,
                TruckDriverId = model.DatosCamion.RutChofer,
                TruckPlate = model.DatosCamion.Patente,
                ProductName = model.Despacho.Especie,
                Origin = model.Despacho.UnidadOrigen,
                TruckHeight = model.Despacho.AlturaMedia,
                TruckLength = double.Parse(model.Despacho.LargoCamion, CultureInfo.InvariantCulture),
                Banks = int.Parse(model.Despacho.Bancos),
                PalomeraHeight = model.Despacho.AlturaMediaPalomera,
                PalomeraWidth = double.Parse(model.Despacho.AnchoPalomera, CultureInfo.InvariantCulture),
                FinalQuantity = model.Despacho.TotalMetros,
            };
        }

        public static MultiProductReport MapToMultiProductReport(TrozoAserrableViewModel model)
        {
            List<Product> products = new();
            List<string> species = new();

            if(model.Especie1.ListaMedidas.Count > 0)
            {
                products.Add(MapToProduct(model.Especie1));
                species.Add(model.Especie1.Especie);
            }
            
            if (model.Especie2.ListaMedidas.Count > 0)
            {
                products.Add(MapToProduct(model.Especie2));
                species.Add(model.Especie2.Especie);
            }
            
            if (model.Especie3.ListaMedidas.Count > 0)
            {
                products.Add(MapToProduct(model.Especie3));
                species.Add(model.Especie3.Especie);
            }

            if (model.Especie4.ListaMedidas.Count > 0)
            {
                products.Add(MapToProduct(model.Especie4));
                species.Add(model.Especie4.Especie);
            }

            if (model.Especie5.ListaMedidas.Count > 0)
            {
                products.Add(MapToProduct(model.Especie5));
                species.Add(model.Especie5.Especie);
            }

            if (model.Especie6.ListaMedidas.Count > 0)
            {
                products.Add(MapToProduct(model.Especie6));
                species.Add(model.Especie6.Especie);
            }

            return new MultiProductReport
            {
                ReportType = "MultiProductReport",
                ProductType = "Trozo Aserrable",
                OperatorName = model.OperatorName,
                Folio = model.Folio,
                Date = model.ReportDate,
                ClientName = model.Cliente.Nombre,
                ClientId = model.Cliente.RUT,
                TruckCompany = model.DatosCamion.EmpresaTransportista,
                TruckDriver = model.DatosCamion.Chofer,
                TruckDriverId = model.DatosCamion.RutChofer,
                TruckPlate = model.DatosCamion.Patente,
                FinalQuantity = model.CantidadFinalDespacho,
                FinalVolume = model.VolumenFinalDespacho,
                Products = products,
                Species = species
            };
        }

        public static Product MapToProduct(MedidasEspecie model)
        {
            return new Product
            {
                Origin = model.UnidadOrigen,
                Species = model.Especie,
                Length = double.Parse(model.LargoEspecie, CultureInfo.InvariantCulture),
                QuantitySum = model.CantidadTotalSum,
                VolumeSum = model.TotalSumFinal,
                Measurements = model.ListaMedidas.Select(m => new ProductMeasurement
                {
                    Diameter = (double)m.Diametro,
                    Quantity = (int)m.Cantidad,
                    Volume = m.Volumen,
                    Total = (double)m.Total
                }).ToList()
            };
        }

    }
}
