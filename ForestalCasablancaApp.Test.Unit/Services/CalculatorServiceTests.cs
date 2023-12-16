using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.Services;
using System.Collections.ObjectModel;

namespace ForestalCasablancaApp.Tests.Unit.Services
{
    public class CalculatorServiceTests
    {
        private readonly CalculatorService _sut;

        public CalculatorServiceTests()
        {
            _sut = new CalculatorService();
        }

        [Fact]
        public void CheckIfAlturasAreValid_ShouldReturnFalse_WhenEmptyListIsGiven()
        {
            // Arrange
            List<string> alturas = new List<string>();

            // Act
            var result = _sut.CheckIfAlturasAreValid(alturas);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CheckIfAlturasAreValid_ShouldReturnFalse_WhenListContainingOnlyZerosIsGiven()
        {
            // Arrange
            List<string> alturas = new List<string> { "0", "0"};

            // Act
            var result = _sut.CheckIfAlturasAreValid(alturas);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(ListOfNumbersAsStrings))]
        public void CheckIfAlturasAreValid_ShouldReturnTrue_WhenListOfNumbersGreaterThanZeroIsGiven(List<string> list)
        {
            // Act
            var result = _sut.CheckIfAlturasAreValid(list);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CalculateAlturaMedia_ShouldReturnZero_WhenListOfContainingZerosOnlyIsGiven()
        {
            // Arrange
            double expected = 0;
            List<string> alturas = new List<string> { "0", "0" };

            // Act
            var result = _sut.CalculateAlturaMedia(alturas);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(ListOfNumbersAsStringsAndMeans))]
        public void CalculateAlturaMedia_ShouldReturnMean_WhenListOfNumbersAsStringsIsGiven(List<string> list, double expected)
        {
            // Act
            var result = _sut.CalculateAlturaMedia(list);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void CheckPalomera_ShouldReturnTrue_WhenEmptyNoValuesAreGiven()
        {
            // Arrange
            string ancho = "";
            string alto = "";
            string alto2 = "";

            // Act
            var result = _sut.CheckPalomera(ancho, alto, alto2);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CheckPalomera_ShouldReturnTrue_WhenOnlyZerosAreGiven()
        {
            // Arrange
            string ancho = "0";
            string alto = "0";
            string alto2 = "0";

            // Act
            var result = _sut.CheckPalomera(ancho, alto, alto2);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("1.5", "2.0", "2.0")] // All three values are greater than zero
        [InlineData("2.0", "1", "0")] // Only one altura (alto2) is missing
        [InlineData("1.8", "0", "1")] // Only one altura (alto) is missing
        public void CheckPalomera_ShouldReturnTrue_WhenValidDataIsGiven(string ancho, string alto, string alto2)
        {
            // Act
            var result = _sut.CheckPalomera(ancho, alto, alto2);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("0", "2.0", "2.0")] // Ancho is missing
        [InlineData("2.0", "0", "0")] // Alto and alto2 are missing
        public void CheckPalomera_ShouldReturnFalse_WhenInValidDataIsGiven(string ancho, string alto, string alto2)
        {
            // Act
            var result = _sut.CheckPalomera(ancho, alto, alto2);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("2.0", "2.0", 2.0, 1.65, 9.65)] // All values are given
        [InlineData("2.0", "1.0", 2.0, 0, 4)] // medidaPalomera is zero 
        public void CalculateTotalMetros_ShouldReturnValidTotal_WhenValidDataIsGiven(string bancos, string largo, 
            double alturaMedia, double medidaPalomera, double expected)
        {
            // Act
            var result = _sut.CalculateTotalMetros(bancos, largo, alturaMedia, medidaPalomera);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("2.0", "2.0", 2.0)] // Both alturas are given
        [InlineData("0", "1.0", 1.0)] // Only one altura is given (alto)
        [InlineData("2.0", "0", 2.0)] // Only one altura is given (alto2)
        public void CalculateAlturaMediaPalomera_ShouldReturnValidMean_WhenAtLeastOneValidValueIsGiven(string alto, string alto2, 
            double expected)
        {
            // Act
            var result = _sut.CalculateAlturaMediaPalomera(alto, alto2);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("0", "0")] // Both are zero
        [InlineData("Invalid", "Input")] // Only one altura is given (alto)
        public void CalculateAlturaMediaPalomera_ShouldReturnZero_WhenNoValidValuesAreGiven(string alto, string alto2)
        {
            // Arrange
            double expected = 0;

            // Act
            var result = _sut.CalculateAlturaMediaPalomera(alto, alto2);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(2, "0")] // Ancho is zero
        [InlineData(2.5, "Invalid Input")] // Ancho is invalid
        public void CalculateMedidaPalomera_ShouldReturnZero_WhenAnchoIsInvalid(double alturaMedia, string alto2)
        {
            // Arrange
            double expected = 0;

            // Act
            var result = _sut.CalculateMedidaPalomera(alturaMedia, alto2);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(2, "2", 4)]
        [InlineData(2.5, "2", 5)]
        public void CalculateMedidaPalomera_ShouldReturnValidCalculation_WhenValidInputIsGiven(double alturaMedia, string alto2,
            double expected)
        {
            // Act
            var result = _sut.CalculateMedidaPalomera(alturaMedia, alto2);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(NullableNumericValuesGreaterThanCutOff))]
        public void CalculateTrozoAserrableVolume_ShouldReturnValidCalculation_WhenLargoIsGreaterThanCutoffValue
            (double? diametro, int? cantidad, string largo, double expected)
        {
            // Act
            var result = _sut.CalculateTrozoAserrableVolume(diametro, cantidad, largo);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(NullableNumericValuesLessThanCutOff))]
        public void CalculateTrozoAserrableVolume_ShouldReturnValidCalculation_WhenLargoIsLessThanCutoffValue
            (double? diametro, int? cantidad, string largo, double expected)
        {
            // Act
            var result = _sut.CalculateTrozoAserrableVolume(diametro, cantidad, largo);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void CalculateTotalSum_ShouldReturnValidSum_WhenListIsNotEmpty()
        {
            // Arrange
            ObservableCollection<MedidaTrozoAserrable> lista = new ObservableCollection<MedidaTrozoAserrable>
            {
                new() { Diametro = 22, Cantidad = 64 },
                new() { Diametro = 24, Cantidad = 36 }
            };
            int expected = 100;

            // Act
            var result = _sut.CalculateTotalSum(lista);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void CalculateTotalSum_ShouldReturnZero_WhenListIsEmpty()
        {
            // Arrange
            ObservableCollection<MedidaTrozoAserrable> lista = new();
            int expected = 0;

            // Act
            var result = _sut.CalculateTotalSum(lista);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void CalculateFinalTotalSum_ShouldReturnValidSum_WhenListIsNotEmpty()
        {
            // Arrange
            ObservableCollection<MedidaTrozoAserrable> lista = new ObservableCollection<MedidaTrozoAserrable>
            {
                new() { Diametro = 22, Cantidad = 64, Total = 12 },
                new() { Diametro = 24, Cantidad = 36, Total = 18 }
            };
            int expected = 30;

            // Act
            var result = _sut.CalculateFinalTotalSum(lista);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void CalculateFinalTotalSum_ShouldReturnZero_WhenListIsEmpty()
        {
            // Arrange
            ObservableCollection<MedidaTrozoAserrable> lista = new();
            int expected = 0;

            // Act
            var result = _sut.CalculateFinalTotalSum(lista);

            // Assert
            result.Should().Be(expected);
        }

        #region Data Providers

        public static IEnumerable<object[]> NullableNumericValuesGreaterThanCutOff =>
        new List<object[]>
        {
            new object[] 
            { 
                Nullable.GetValueRefOrDefaultRef<double>(20),
                Nullable.GetValueRefOrDefaultRef<int>(15),
                "7.0",
                0.3281975
            },

            new object[]
            {
                Nullable.GetValueRefOrDefaultRef<double>(32),
                Nullable.GetValueRefOrDefaultRef<int>(20),
                "6.0",
                0.6642899999999999
            }
        };

        public static IEnumerable<object[]> NullableNumericValuesLessThanCutOff =>
        new List<object[]>
        {
            new object[]
            {
                Nullable.GetValueRefOrDefaultRef<double>(20),
                Nullable.GetValueRefOrDefaultRef<int>(15),
                "5.0",
                0.2
            },

            new object[]
            {
                Nullable.GetValueRefOrDefaultRef<double>(32),
                Nullable.GetValueRefOrDefaultRef<int>(20),
                "3.2",
                0.32768
            }
        };

        public static IEnumerable<object[]> ListOfNumbersAsStrings =>
        new List<object[]>
        {
            new object[] { new List<string> { "2.5" } },
            new object[] { new List<string> { "1.5", "2.0", "2.5" } }
        };

        public static IEnumerable<object[]> ListOfNumbersAsStringsAndMeans =>
        new List<object[]>
        {
            new object[] { new List<string> { "2.5" }, 2.5 },
            new object[] { new List<string> { "2.5", "3.5" }, 3 }
        };

        #endregion
    }
}
