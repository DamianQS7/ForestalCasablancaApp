
using BosquesNalcahue.Services;
using ForestalCasablancaApp.Controls;
using ForestalCasablancaApp.Helpers;
using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.Services;
using ForestalCasablancaApp.ViewModels;
using NSubstitute.ReturnsExtensions;
using System.Collections.ObjectModel;

namespace ForestalCasablancaApp.Tests.Unit.ViewModels
{
    public class TrozoAserrableViewModelTests
    {
        private readonly TrozoAserrableViewModel _sut;
        private readonly ICalculatorService _calculatorService = Substitute.For<ICalculatorService>();
        private readonly IPdfGeneratorService _pdfGeneratorService = Substitute.For<IPdfGeneratorService>();
        private readonly IInfoService _infoService = Substitute.For<IInfoService>();

        public TrozoAserrableViewModelTests()
        {
            _sut = new TrozoAserrableViewModel(_calculatorService, _pdfGeneratorService, _infoService);
        }

        [Theory(Skip = "Move to UI Test")]
        [InlineData("1", 1, 0, 0)]
        [InlineData("2", 0, 1, 0)]
        [InlineData("3", 0, 0, 1)]
        public void AddItemToList_ShouldAddItemToAppropriateList_WhenValuesAreGiven(string id, int expectedList1,
            int expectedList2, int expectedList3)
        {
            // Arrange
            var cell = new NumericEntryCell { Identifier = id };

            // Largo for the three species is given
            _sut.LargoEspecieUno = "1";
            _sut.LargoEspecieDos = "1";
            _sut.LargoEspecieTres = "1";
            // Diámetro for the three species is given and it is even
            _sut.DiametroIngresado = 3;
            _sut.DiametroIngresado2 = 5;
            _sut.DiametroIngresado3 = 7;
            // Cantidad for the three species is given
            _sut.CantidadIngresada = 1;
            _sut.CantidadIngresada2 = 1;
            _sut.CantidadIngresada3 = 1;

            // Act
            _sut.AddItemToList(cell);

            // Assert
            _sut.MedidasEspecieUno.Should().HaveCount(expectedList1);
            _sut.MedidasEspecieDos.Should().HaveCount(expectedList2);
            _sut.MedidasEspecieTres.Should().HaveCount(expectedList3);
        }

        [Fact]
        public void AddMedidaTrozoAsserableToList_ShouldAddItemToAppropriateList_WhenValuesAreGiven()
        {
            // Arrange
            _calculatorService.CalculateTrozoAserrableVolume(Arg.Any<string>(), Arg.Any<string>()).Returns(1);
            string anyDiametro = "2.0";
            string anyLargo = "2.0";
            string anyCantidad = "1";

            // Act
            _sut.AddMedidaTrozoAserrableToList(1, _sut.Especie1.ListaMedidas, anyDiametro, anyCantidad, anyLargo);

            // Assert
            _sut.Especie1.ListaMedidas.Should().HaveCount(1);
            _sut.Especie3.ListaMedidas.Should().HaveCount(0);
            _sut.Especie5.ListaMedidas.Should().HaveCount(0);
        }

        [Fact]
        public void CheckIfMedidaTrozoAserrableIsValid_ShouldReturnFalse_WhenValuesAreNotGiven()
        {
            // Act
            var result = _sut.CheckIfMedidaTrozoAserrableIsValid(_sut.Especie1.LargoEspecie, _sut.Especie1.DiametroIngresado, _sut.Especie1.CantidadIngresada );

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CheckIfMedidaTrozoAserrableIsValid_ShouldReturnTrue_WhenValuesAreGiven()
        {
            // Arrange
            _sut.Especie1.LargoEspecie = "1.5";
            _sut.Especie1.DiametroIngresado = "22";
            _sut.Especie1.CantidadIngresada = "20";

            // Act
            var result = _sut.CheckIfMedidaTrozoAserrableIsValid(_sut.Especie1.LargoEspecie, _sut.Especie1.DiametroIngresado, _sut.Especie1.CantidadIngresada);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ClearPage_ShouldDisplayToast_WhenCalled()
        {
            // Arrange
            _infoService.ShowToast(Arg.Any<string>()).ReturnsNull();

            // Act
            _sut.ClearPage();

            // Assert
            _infoService.Received(1).ShowToast("Módulo reiniciado con éxito");
        }

        [Fact]
        public void ClearPage_ShouldClearAllValues_WhenInvoked()
        {
            // Arrange
            _sut.Especie1.ListaMedidas.Add(new MedidaTrozoAserrable());
            _sut.Especie3.ListaMedidas.Add(new MedidaTrozoAserrable());
            _sut.Especie5.ListaMedidas.Add(new MedidaTrozoAserrable());
            _sut.Cliente.Nombre = "Kobe Bryant";
            _sut.DatosCamion.Chofer = "John Snow";
            _sut.Especie1.Especie = "Especie 1";
            _sut.Especie3.Especie = "Especie 2";
            _sut.Especie5.Especie = "Especie 3";
            _sut.Especie1.LargoEspecie = "1";
            _sut.Especie3.LargoEspecie = "1";
            _sut.Especie5.LargoEspecie = "1";
            _sut.Especie1.DiametroIngresado = "2";
            _sut.Especie3.DiametroIngresado = "2";
            _sut.Especie5.DiametroIngresado = "2";
            _sut.Especie1.CantidadIngresada = "2";
            _sut.Especie3.CantidadIngresada = "2";
            _sut.Especie5.CantidadIngresada = "2";

            // Act
            _sut.ClearPage();

            // Assert
            _sut.Especie1.ListaMedidas.Should().BeEmpty();
            _sut.Especie3.ListaMedidas.Should().BeEmpty();
            _sut.Especie5.ListaMedidas.Should().BeEmpty();
            _sut.Cliente.Nombre.Should().BeNullOrEmpty();
            _sut.DatosCamion.Chofer.Should().BeNullOrEmpty();
            _sut.Especie1.Especie.Should().BeNullOrEmpty();
            _sut.Especie3.Especie.Should().BeNullOrEmpty();
            _sut.Especie5.Especie.Should().BeNullOrEmpty();
            _sut.Especie1.LargoEspecie.Should().BeNullOrEmpty();
            _sut.Especie3.LargoEspecie.Should().BeNullOrEmpty();
            _sut.Especie5.LargoEspecie.Should().BeNullOrEmpty();
            _sut.Especie1.DiametroIngresado.Should().BeNullOrEmpty();
            _sut.Especie3.DiametroIngresado.Should().BeNullOrEmpty();
            _sut.Especie5.DiametroIngresado.Should().BeNullOrEmpty();
            _sut.Especie1.CantidadIngresada.Should().BeNullOrEmpty();
            _sut.Especie3.CantidadIngresada.Should().BeNullOrEmpty();
            _sut.Especie5.CantidadIngresada.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task DisplaySummaryAsync_ShouldRaiseAnAlert_WhenAllListsAreEmpty()
        {
            // Arrange
            _infoService.ShowAlert(InfoMessage.MissingMedidaTrozoAserrable).Returns(Task.CompletedTask);

            // Act
            await _sut.DisplaySummaryAsync();

            // Assert
            await _infoService.Received(1).ShowAlert(InfoMessage.MissingMedidaTrozoAserrable);
        }

        [Fact]
        public void GetListaMedidasTotals_ShouldNotUpdateModelValues_WhenListIsEmpty()
        {
            // Act
            _sut.GetListaMedidasTotals(_sut.Especie1);

            // Assert
            _sut.CantidadFinalDespacho.Should().Be(0);
            _sut.VolumenFinalDespacho.Should().Be(0);
        }

        [Fact]
        public void GetListaMedidasTotals_ShouldUpdateModelValues_WhenListIsNotEmpty()
        {
            // Arrange
            _sut.Especie1.ListaMedidas.Add(new MedidaTrozoAserrable { Cantidad = 1, Total = 2 });
            _calculatorService.CalculateTotalSum(Arg.Any<ObservableCollection<MedidaTrozoAserrable>>()).Returns(2);
            _calculatorService.CalculateFinalTotalSum(Arg.Any<ObservableCollection<MedidaTrozoAserrable>>()).Returns(2);

            // Act
            _sut.GetListaMedidasTotals(_sut.Especie1);

            // Assert
            _sut.Especie1.CantidadTotalSum.Should().Be(2);
            _sut.Especie1.TotalSumFinal.Should().Be(2);
        }

        [Theory]
        [InlineData(2, 0, 1, 1)]
        [InlineData(4, 1, 0, 1)]
        [InlineData(6, 1, 1, 0)]
        public void RemoveItemFromList_ShouldRemoveItemFromAppropriateList_WhenAnItemIsGiven(int id, int expectedCount1,
            int expectedCount2, int expectedCount3)
        {
            // Arrange
            var medida = new MedidaTrozoAserrable { NumeroLista = id };

            _sut.Especie2.ListaMedidas.Add(medida);
            _sut.Especie4.ListaMedidas.Add(medida);
            _sut.Especie6.ListaMedidas.Add(medida);

            // Act
            _sut.RemoveItemFromList(medida);

            // Assert
            _sut.Especie2.ListaMedidas.Should().HaveCount(expectedCount1);
            _sut.Especie4.ListaMedidas.Should().HaveCount(expectedCount2);
            _sut.Especie6.ListaMedidas.Should().HaveCount(expectedCount3);
        }

        [Fact]
        public void UpdateViewModelTotals_ShouldUpdateTotals_WhenListsAreNotEmpty()
        {
            // Arrange

            var list1 = new ObservableCollection<MedidaTrozoAserrable> { new() { Cantidad = 5, Total = 2 } };
            var list3 = new ObservableCollection<MedidaTrozoAserrable> { new() { Cantidad = 5, Total = 3 } };
            var list5 = new ObservableCollection<MedidaTrozoAserrable> { new() { Cantidad = 5, Total = 4 } };

            _sut.Especie1.ListaMedidas = list1;
            _sut.Especie3.ListaMedidas = list3;
            _sut.Especie5.ListaMedidas = list5;

            _calculatorService.CalculateFinalTotalSum(Arg.Any<ObservableCollection<MedidaTrozoAserrable>>()).Returns(9);
            _calculatorService.CalculateTotalSum(Arg.Any<ObservableCollection<MedidaTrozoAserrable>>()).Returns(5);
            _calculatorService.GetCantidadFinalDespachoTrozos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(),
                               Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(15);
            _calculatorService.GetVolumenFinalDespachoTrozos(Arg.Any<double>(), Arg.Any<double>(), Arg.Any<double>(),
                                Arg.Any<double>(), Arg.Any<double>(), Arg.Any<double>()).Returns(27);

            // Act
            _sut.UpdateViewModelTotals();

            // Assert
            _sut.Especie1.CantidadTotalSum.Should().Be(5);
            _sut.Especie1.TotalSumFinal.Should().Be(9);

            _sut.Especie2.CantidadTotalSum.Should().Be(0);
            _sut.Especie2.TotalSumFinal.Should().Be(0);

            _sut.Especie3.CantidadTotalSum.Should().Be(5);
            _sut.Especie3.TotalSumFinal.Should().Be(9);

            _sut.Especie5.CantidadTotalSum.Should().Be(5);
            _sut.Especie5.TotalSumFinal.Should().Be(9);

            _sut.CantidadFinalDespacho.Should().Be(15);
            _sut.VolumenFinalDespacho.Should().Be(27);
        }

        [Fact]
        public void UpdateViewModelTotals_ShouldNotUpdateTotals_WhenListsAreEmpty()
        {
            // Act
            _sut.UpdateViewModelTotals();

            // Assert
            _sut.Especie1.CantidadTotalSum.Should().Be(0);
            _sut.Especie6.CantidadTotalSum.Should().Be(0);
            _sut.Especie2.TotalSumFinal.Should().Be(0);
            _sut.Especie4.TotalSumFinal.Should().Be(0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ValidateInput_ShouldDisplayMessage_WhenThereAreMissingValuesForCalculation(int especie)
        {
            // Arrange
            _infoService.ShowAlert(Arg.Any<InfoMessage>()).ReturnsNull();

            // Act
            _sut.ValidateInput(especie);

            // Assert
            _infoService.Received(1).ShowAlert(InfoMessage.MissingTrozoData);

        }

        [Fact]
        public void ValidateInput_ShouldDisplayMessage_WhenAllValuesAreGivenAndDiameterIsNotEven()
        {
            // Arrange
            _infoService.ShowAlert(Arg.Any<InfoMessage>()).ReturnsNull();
            
            _sut.Especie1.LargoEspecie = "1"; // Largo is given
            _sut.Especie1.DiametroIngresado = "3"; // Diámetro is not even
            _sut.Especie1.CantidadIngresada = "1"; // Cantidad is given

            // Act
            _sut.ValidateInput(1);

            // Assert
            _infoService.Received(1).ShowAlert(InfoMessage.InvalidDiameter);

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ValidateInput_ShouldReturnFalse_WhenNoValuesAreGiven(int especie)
        {
            // Act
            var result = _sut.ValidateInput(especie);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ValidateInput_ShouldReturnTrue_WhenAllValuesAreGivenAndDiameterIsEven(int especie)
        {
            // Arrange

            // Largo for the three species is given
            _sut.Especie1.LargoEspecie = "1";
            _sut.Especie2.LargoEspecie = "1";
            _sut.Especie3.LargoEspecie = "1";
            // Diámetro for the three species is given and it is even
            _sut.Especie1.DiametroIngresado = "2";
            _sut.Especie2.DiametroIngresado = "2";
            _sut.Especie3.DiametroIngresado = "2";
            // Cantidad for the three species is given
            _sut.Especie1.CantidadIngresada = "2";
            _sut.Especie2.CantidadIngresada = "2";
            _sut.Especie3.CantidadIngresada = "2";

            // Act
            var result = _sut.ValidateInput(especie);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ValidateInput_ShouldReturnFalse_WhenAllValuesAreGivenAndDiameterIsNotEven(int especie)
        {
            // Arrange

            // Largo for the three species is given
            _sut.Especie1.LargoEspecie = "1";
            _sut.Especie2.LargoEspecie = "1";
            _sut.Especie3.LargoEspecie = "1";
            // Diámetro for the three species is given and it is even
            _sut.Especie1.DiametroIngresado = "1";
            _sut.Especie2.DiametroIngresado = "3";
            _sut.Especie3.DiametroIngresado = "5";
            // Cantidad for the three species is given
            _sut.Especie1.CantidadIngresada = "2";
            _sut.Especie2.CantidadIngresada = "2";
            _sut.Especie3.CantidadIngresada = "2";

            // Act
            var result = _sut.ValidateInput(especie);

            // Assert
            result.Should().BeFalse();
        }

    }
}
