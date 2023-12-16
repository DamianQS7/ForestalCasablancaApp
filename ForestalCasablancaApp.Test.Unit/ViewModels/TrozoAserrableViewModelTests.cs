
using ForestalCasablancaApp.Controls;
using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.Services;
using ForestalCasablancaApp.ViewModels;
using System.Collections.ObjectModel;

namespace ForestalCasablancaApp.Tests.Unit.ViewModels
{
    public class TrozoAserrableViewModelTests
    {
        private readonly TrozoAserrableViewModel _sut;
        private readonly ICalculatorService _calculatorService = Substitute.For<ICalculatorService>();
        private readonly IPdfGeneratorService _pdfGeneratorService = Substitute.For<IPdfGeneratorService>();

        public TrozoAserrableViewModelTests()
        {
            _sut = new TrozoAserrableViewModel(_calculatorService, _pdfGeneratorService);
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
            _calculatorService.CalculateTrozoAserrableVolume(Arg.Any<double>(), Arg.Any<int>(), Arg.Any<string>()).Returns(1);

            // Act
            _sut.AddMedidaTrozoAserrableToList(2, _sut.MedidasEspecieDos, Arg.Any<double>(), Arg.Any<int>(), Arg.Is("1.5"));

            // Assert
            _sut.MedidasEspecieUno.Should().HaveCount(0);
            _sut.MedidasEspecieDos.Should().HaveCount(1);
            _sut.MedidasEspecieTres.Should().HaveCount(0);
        }

        [Fact]
        public void ClearPage_ShouldClearAllValues_WhenInvoked()
        {
            // Arrange
            _sut.MedidasEspecieUno.Add(new MedidaTrozoAserrable());
            _sut.MedidasEspecieDos.Add(new MedidaTrozoAserrable());
            _sut.MedidasEspecieTres.Add(new MedidaTrozoAserrable());
            _sut.Cliente.Nombre = "Kobe Bryant";
            _sut.DatosCamion.Chofer = "John Snow";
            _sut.EspecieUno = "Especie 1";
            _sut.EspecieDos = "Especie 2";
            _sut.EspecieTres = "Especie 3";
            _sut.LargoEspecieUno = "1";
            _sut.LargoEspecieDos = "1";
            _sut.LargoEspecieTres = "1";
            _sut.DiametroIngresado = 2;
            _sut.DiametroIngresado2 = 2;
            _sut.DiametroIngresado3 = 2;
            _sut.CantidadIngresada = 1;
            _sut.CantidadIngresada2 = 1;
            _sut.CantidadIngresada3 = 1;

            // Act
            _sut.ClearPage();

            // Assert
            _sut.MedidasEspecieUno.Should().BeEmpty();
            _sut.MedidasEspecieDos.Should().BeEmpty();
            _sut.MedidasEspecieTres.Should().BeEmpty();
            _sut.Cliente.Nombre.Should().BeNullOrEmpty();
            _sut.DatosCamion.Chofer.Should().BeNullOrEmpty();
            _sut.EspecieUno.Should().BeNullOrEmpty();
            _sut.EspecieDos.Should().BeNullOrEmpty();
            _sut.EspecieTres.Should().BeNullOrEmpty();
            _sut.LargoEspecieUno.Should().BeNullOrEmpty();
            _sut.LargoEspecieDos.Should().BeNullOrEmpty();
            _sut.LargoEspecieTres.Should().BeNullOrEmpty();
            _sut.DiametroIngresado.Should().BeNull();
            _sut.DiametroIngresado2.Should().BeNull();
            _sut.DiametroIngresado3.Should().BeNull();
            _sut.CantidadIngresada.Should().BeNull();
            _sut.CantidadIngresada2.Should().BeNull();
            _sut.CantidadIngresada3.Should().BeNull();
        }

        [Fact(Skip = "Needs AlertService")]
        public async Task DisplaySummaryAsync_ShouldRaiseAnAlert_WhenAllListsAreEmpty()
        {
            // Arrange

            // Act
            await _sut.DisplaySummaryAsync();

            // Assert
            // Alert service received call
        }

        [Fact(Skip = "Needs fixing -> Create Alerts Service")]
        public async Task GeneratePDF_ShouldCallGenerateTrozoAserrablePDFMethod()
        {
            // Arrange
            _pdfGeneratorService.GenerateTrozoAserrablePDF(Arg.Any<TrozoAserrableViewModel>());

            // Act
            await _sut.GeneratePDF();

            // Assert
            _pdfGeneratorService.Received(1).
                GenerateTrozoAserrablePDF(Arg.Any<TrozoAserrableViewModel>());

        }

        [Theory]
        [InlineData(1, 0, 1, 1)]
        [InlineData(2, 1, 0, 1)]
        [InlineData(3, 1, 1, 0)]
        public void RemoveItemFromList_ShouldRemoveItemFromAppropriateList_WhenAnItemIsGiven(int id, int expectedList1,
            int expectedList2, int expectedList3)
        {
            // Arrange
            var medida = new MedidaTrozoAserrable { NumeroLista = id };

            _sut.MedidasEspecieUno.Add(medida);
            _sut.MedidasEspecieDos.Add(medida);
            _sut.MedidasEspecieTres.Add(medida);

            // Act
            _sut.RemoveItemFromList(medida);

            // Assert
            _sut.MedidasEspecieUno.Should().HaveCount(expectedList1);
            _sut.MedidasEspecieDos.Should().HaveCount(expectedList2);
            _sut.MedidasEspecieTres.Should().HaveCount(expectedList3);
        }

        [Fact]
        public void UpdateViewModelTotals_ShouldUpdateTotals_WhenListsAreNotEmpty()
        {
            // Arrange

            var list1 = new ObservableCollection<MedidaTrozoAserrable> { new() { Cantidad = 1, Total = 2 } };
            var list2 = new ObservableCollection<MedidaTrozoAserrable> { new() { Cantidad = 2, Total = 3 } };
            var list3 = new ObservableCollection<MedidaTrozoAserrable> { new() { Cantidad = 3, Total = 4 } };

            _sut.MedidasEspecieUno = list1;
            _sut.MedidasEspecieDos = list2;
            _sut.MedidasEspecieTres = list3;

            _calculatorService.CalculateFinalTotalSum(Arg.Any<ObservableCollection<MedidaTrozoAserrable>>()).Returns(10);
            _calculatorService.CalculateTotalSum(Arg.Any<ObservableCollection<MedidaTrozoAserrable>>()).Returns(5);

            // Act
            _sut.UpdateViewModelTotals();

            // Assert
            _sut.FinalTotalSumLista1.Should().Be(10);
            _sut.FinalTotalSumLista2.Should().Be(10);
            _sut.FinalTotalSumLista3.Should().Be(10);
            _sut.TotalSumLista1.Should().Be(5);
            _sut.TotalSumLista2.Should().Be(5);
            _sut.TotalSumLista3.Should().Be(5);
        }

        [Fact]
        public void UpdateViewModelTotals_ShouldNotUpdateTotals_WhenListsAreEmpty()
        {
            // Act
            _sut.UpdateViewModelTotals();

            // Assert
            _sut.FinalTotalSumLista1.Should().Be(0);
            _sut.FinalTotalSumLista2.Should().Be(0);
            _sut.FinalTotalSumLista3.Should().Be(0);
            _sut.TotalSumLista1.Should().Be(0);
            _sut.TotalSumLista2.Should().Be(0);
            _sut.TotalSumLista3.Should().Be(0);
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
            _sut.LargoEspecieUno = "1";
            _sut.LargoEspecieDos = "1";
            _sut.LargoEspecieTres = "1";
            // Diámetro for the three species is given and it is even
            _sut.DiametroIngresado = 2;
            _sut.DiametroIngresado2 = 2;
            _sut.DiametroIngresado3 = 2;
            // Cantidad for the three species is given
            _sut.CantidadIngresada = 1;
            _sut.CantidadIngresada2 = 1;
            _sut.CantidadIngresada3 = 1;

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
            var result = _sut.ValidateInput(especie);

            // Assert
            result.Should().BeFalse();
        }

        


        #region Data Providers



        #endregion
    }
}
