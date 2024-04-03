using BosquesNalcahue.Dtos;
using BosquesNalcahue.Mapping;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace ForestalCasablancaApp.Tests.Unit.Mapping
{
    public class ModelToDtoMapperTests
    {
        private readonly LeñaViewModel _leñaViewModel;
        private readonly MetroRumaViewModel _metroViewModel;
        private readonly TrozoAserrableViewModel _trozoViewModel;
        private readonly ICalculatorService _calculatorService = Substitute.For<ICalculatorService>();
        private readonly IPdfGeneratorService _pdfGeneratorService = Substitute.For<IPdfGeneratorService>();
        private readonly IInfoService _infoService = Substitute.For<IInfoService>();
        private readonly IRestService _restService = Substitute.For<IRestService>();

        public ModelToDtoMapperTests()
        {
            _leñaViewModel = new LeñaViewModel(_calculatorService, _pdfGeneratorService, _infoService, _restService);

            _metroViewModel = new MetroRumaViewModel(_calculatorService, _pdfGeneratorService, _infoService, _restService);

            _trozoViewModel = new TrozoAserrableViewModel(_calculatorService, _pdfGeneratorService, _infoService, _restService);
        }

        [Fact(Skip = "")]
        public void MapToSingleProductReport_LeñaViewModel_MapsCorrectlyAllFields()
        {
            // Arrange
            _leñaViewModel.Folio = "12345";
            _leñaViewModel.ReportDate = DateTime.Now;
            _leñaViewModel.Cliente = new Cliente { Nombre = "Test Client", RUT = "12345678-9" };
            _leñaViewModel.DatosCamion = new DatosCamion 
            { 
                EmpresaTransportista = "Test Company", 
                Chofer = "Test Driver", 
                RutChofer = "98765432-1", 
                Patente = "AB-1234" 
            };
            _leñaViewModel.Despacho = new DespachoModel 
            { 
                Especie = "Test Species", 
                UnidadOrigen = "Test Origin", 
                AlturaMedia = 1.5, 
                LargoCamion = "2.5", 
                Bancos = "3", 
                AlturaMediaPalomera = 1.5, 
                AnchoPalomera = "2.5", 
                TotalMetros = 4.5 
            };

            // Act
            var report = ModelToDtoMapper.MapToSingleProductReport(_leñaViewModel);

            // Assert
            report.Should().BeOfType<SingleProductReport>();
            report.ReportType.Should().Be("SingleProductReport");
            report.ProductType.Should().Be("Leña");
            report.Folio.Should().Be("12345");
            report.Date.Should().Be(_leñaViewModel.ReportDate);
            report.ClientName.Should().Be("Test Client");
            report.ClientId.Should().Be("12345678-9");
            report.TruckCompany.Should().Be("Test Company");
            report.TruckDriver.Should().Be("Test Driver");
            report.TruckDriverId.Should().Be("98765432-1");
            report.TruckPlate.Should().Be("AB-1234");
            report.ProductName.Should().Be("Test Species");
            report.Origin.Should().Be("Test Origin");
            report.TruckHeight.Should().Be(1.5);
            report.TruckLength.Should().Be(2.5);
            report.Banks.Should().Be(3);
            report.PalomeraHeight.Should().Be(1.5);
            report.PalomeraWidth.Should().Be(2.5);
            report.FinalQuantity.Should().Be(4.5);
        }

        [Fact(Skip ="")]
        public void MapToSingleProductReport_MetroRumaViewModel_MapsCorrectlyAllFields()
        {
            // Arrange
            _metroViewModel.Folio = "12345";
            _metroViewModel.ReportDate = DateTime.Now;
            _metroViewModel.Cliente = new Cliente { Nombre = "Test Client", RUT = "12345678-9" };
            _metroViewModel.DatosCamion = new DatosCamion
            {
                EmpresaTransportista = "Test Company",
                Chofer = "Test Driver",
                RutChofer = "98765432-1",
                Patente = "AB-1234"
            };
            _metroViewModel.Despacho = new DespachoModel
            {
                Especie = "Test Species",
                UnidadOrigen = "Test Origin",
                AlturaMedia = 1.5,
                LargoCamion = "2.5",
                Bancos = "3",
                AlturaMediaPalomera = 1.5,
                AnchoPalomera = "2.5",
                TotalMetros = 4.5
            };

            // Act
            var report = ModelToDtoMapper.MapToSingleProductReport(_metroViewModel);

            // Assert
            report.Should().BeOfType<SingleProductReport>();
            report.ReportType.Should().Be("SingleProductReport");
            report.ProductType.Should().Be("Metro Ruma");
            report.Folio.Should().Be("12345");
            report.Date.Should().Be(_metroViewModel.ReportDate);
            report.ClientName.Should().Be("Test Client");
            report.ClientId.Should().Be("12345678-9");
            report.TruckCompany.Should().Be("Test Company");
            report.TruckDriver.Should().Be("Test Driver");
            report.TruckDriverId.Should().Be("98765432-1");
            report.TruckPlate.Should().Be("AB-1234");
            report.ProductName.Should().Be("Test Species");
            report.Origin.Should().Be("Test Origin");
            report.TruckHeight.Should().Be(1.5);
            report.TruckLength.Should().Be(2.5);
            report.Banks.Should().Be(3);
            report.PalomeraHeight.Should().Be(1.5);
            report.PalomeraWidth.Should().Be(2.5);
            report.FinalQuantity.Should().Be(4.5);
        }
    }
}
