using CommunityToolkit.Maui.Views;
using ForestalCasablancaApp.Popups;
using ForestalCasablancaApp.Services;
using ForestalCasablancaApp.ViewModels;
using ForestalCasablancaApp;
using BosquesNalcahue.Services;
using ForestalCasablancaApp.Helpers;
using NSubstitute.ReturnsExtensions;

namespace ForestalCasablancaApp.Tests.Unit.ViewModels
{
    public class LeñaViewModelTests
    {
        private readonly LeñaViewModel _sut;
        private readonly ICalculatorService _calculatorService = Substitute.For<ICalculatorService>();
        private readonly IPdfGeneratorService _pdfGeneratorService = Substitute.For<IPdfGeneratorService>();
        private readonly IInfoService _infoService = Substitute.For<IInfoService>();

        public LeñaViewModelTests()
        {
            _sut = new LeñaViewModel(_calculatorService, _pdfGeneratorService, _infoService);
        }

        [Fact]
        public void ValidateInput_ShouldReturnFalse_WhenNoValuesAreGiven()
        {
            // Arrange
            _calculatorService.CheckIfAlturasAreValid(Arg.Any<List<string>>()).Returns(false);
            _calculatorService.CheckPalomera(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            // Act
            var result = _sut.ValidateInput();

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(0, "2", "2")] // Altura media is missing
        [InlineData(1.5, "2", null)] // Largo camion is missing
        [InlineData(1.5, null, "3")] // Bancos is missing
        public void ValidateInput_ShouldReturnFalse_WhenThereAreMissingValuesForCalculation(double alturaMedia,
            string? bancos, string? largoCamion)
        {
            // Arrange
            _calculatorService.CheckIfAlturasAreValid(Arg.Any<List<string>>()).Returns(true);
            _calculatorService.CalculateAlturaMedia(Arg.Any<List<string>>()).Returns(alturaMedia);
            _calculatorService.CheckPalomera(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);
            _sut.Despacho.Bancos = bancos;
            _sut.Despacho.LargoCamion = largoCamion;

            // Act
            var result = _sut.ValidateInput();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ValidateInput_ShouldReturnFalse_WhenValuesForCalculationAreGivenButPalomeraIsInvalid()
        {
            // Arrange
            _calculatorService.CheckIfAlturasAreValid(Arg.Any<List<string>>()).Returns(true);
            _calculatorService.CalculateAlturaMedia(Arg.Any<List<string>>()).Returns(2);
            _calculatorService.CheckPalomera(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(false);
            _sut.Despacho.Bancos = "2";
            _sut.Despacho.LargoCamion = "2.5";

            // Act
            var result = _sut.ValidateInput();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ValidateInput_ShouldDisplayMessage_WhenValuesForCalculationAreGivenButPalomeraIsInvalid()
        {
            // Arrange
            _infoService.ShowAlert(Arg.Any<InfoMessage>()).ReturnsNull();
            _calculatorService.CheckIfAlturasAreValid(Arg.Any<List<string>>()).Returns(true);
            _calculatorService.CalculateAlturaMedia(Arg.Any<List<string>>()).Returns(2);
            _calculatorService.CheckPalomera(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(false);
            _sut.Despacho.Bancos = "2";
            _sut.Despacho.LargoCamion = "2.5";

            // Act
            _sut.ValidateInput();

            // Assert
            _infoService.Received(1).ShowAlert(InfoMessage.InvalidPalomera);
            
        }

        [Fact]
        public void ValidateInput_ShouldDisplayMessage_WhenThereAreMissingValuesForCalculation()
        {
            // Arrange
            _infoService.ShowAlert(Arg.Any<InfoMessage>()).ReturnsNull();
            _calculatorService.CheckIfAlturasAreValid(Arg.Any<List<string>>()).Returns(false);
            _calculatorService.CheckPalomera(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);
            _sut.Despacho.Bancos = "2";
            _sut.Despacho.LargoCamion = "2.5";

            // Act
            _sut.ValidateInput();

            // Assert
            _infoService.Received(1).ShowAlert(InfoMessage.MissingLeñaData);

        }

        [Fact]
        public void ValidateInput_ShouldReturnTrue_WhenAllValuesAreGiven()
        {
            // Arrange
            _calculatorService.CheckIfAlturasAreValid(Arg.Any<List<string>>()).Returns(true);
            _calculatorService.CalculateAlturaMedia(Arg.Any<List<string>>()).Returns(2);
            _calculatorService.CheckPalomera(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);
            _sut.Despacho.Bancos = "2";
            _sut.Despacho.LargoCamion = "2.5";

            // Act
            var result = _sut.ValidateInput();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ValidateAlturasAndUpdateModelAccordingly_ShouldSetAlturaMediaToZero_WhenAlturasAreInvalid()
        {
            // Arrange
            _calculatorService.CheckIfAlturasAreValid(Arg.Any<List<string>>()).Returns(false);

            // Act
            _sut.ValidateAlturasAndUpdateModelAccordingly();

            // Assert
            _sut.Despacho.AlturaMedia.Should().Be(0);
        }

        [Fact]
        public void ValidateAlturasAndUpdateModelAccordingly_ShouldSetAlturaMediaToCalculatedValue_WhenAlturasAreValid()
        {
            // Arrange
            _calculatorService.CheckIfAlturasAreValid(Arg.Any<List<string>>()).Returns(true);
            _calculatorService.CalculateAlturaMedia(Arg.Any<List<string>>()).Returns(2);

            // Act
            _sut.ValidateAlturasAndUpdateModelAccordingly();

            // Assert
            _sut.Despacho.AlturaMedia.Should().Be(2);
        }

        [Fact]
        public void ValidatePalomeraAndUpdateModelAccordingly_ShouldSetIsPalomeraValidToFalse_WhenPalomeraIsInvalid()
        {
            // Arrange
            _calculatorService.CheckPalomera(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            // Act
            _sut.ValidatePalomeraAndUpdateModelAccordingly();

            // Assert
            _sut.Despacho.IsPalomeraValid.Should().BeFalse();
            _sut.Despacho.MedidaPalomera.Should().Be(0);
        }

        [Fact]
        public void ValidatePalomeraAndUpdateModelAccordingly_ShouldSetIsPalomeraValidToTrue_WhenPalomeraIsValid()
        {
            // Arrange
            _calculatorService.CheckPalomera(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);
            _calculatorService.CalculateAlturaMediaPalomera(Arg.Any<string>(), Arg.Any<string>()).Returns(2);
            _calculatorService.CalculateMedidaPalomera(Arg.Any<double>(), Arg.Any<string>()).Returns(2);

            // Act
            _sut.ValidatePalomeraAndUpdateModelAccordingly();

            // Assert
            _sut.Despacho.IsPalomeraValid.Should().BeTrue();
            _sut.Despacho.MedidaPalomera.Should().Be(2);
            _sut.Despacho.AlturaMediaPalomera.Should().Be(2);
        }

        [Fact(Skip = "Requires a Popup object -> Move to UI Testing")]
        public void DisplaySummaryAsyncCommand_ShouldUpdateProperties_WhenValidateInputReturnsTrue()
        {
            // Arrange
            _calculatorService.CalculateTotalMetros(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<double>(), Arg.Any<double>())
                .Returns(2);
            _calculatorService.CheckIfAlturasAreValid(Arg.Any<List<string>>()).Returns(true);
            _calculatorService.CalculateAlturaMedia(Arg.Any<List<string>>()).Returns(2);
            _calculatorService.CheckPalomera(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);
            _sut.Despacho.Bancos = "2";
            _sut.Despacho.LargoCamion = "2.5";
            _sut.Despacho.IsPalomeraValid = true;

            // Act
            _sut.DisplaySummaryAsync();

            // Assert
            _sut.IsValidInput.Should().BeTrue();
            _sut.Despacho.TotalMetros.Should().Be(2);
        }

        [Fact]
        public void DisplaySummaryAsync_ShouldNotUpdateProperties_WhenValidateInputReturnsFalse()
        {
            // Arrange
            _sut.Despacho.AlturaMedia = 0;
            _sut.Despacho.Bancos = "2";
            _sut.Despacho.LargoCamion = "2.5";
            _sut.Despacho.IsPalomeraValid = false;

            // Act
            _sut.DisplaySummaryAsync();

            // Assert
            _sut.IsValidInput.Should().BeFalse();
            _sut.Despacho.TotalMetros.Should().Be(0);
        }

        [Fact (Skip = "Requires a Popup object -> Move to UI Testing")]
        public void DisplaySummaryAsync_ShouldCallShowPopup_WhenValidateInputReturnsTrue()
        {
            // Arrange
            _sut.Despacho.AlturaMedia = 2;
            _sut.Despacho.Bancos = "2";
            _sut.Despacho.LargoCamion = "2.5";
            _sut.Despacho.IsPalomeraValid = true;

            // Act
            _sut.DisplaySummaryAsync();

            // Assert
            //_sut.BasePage.Received(1).ShowPopup(Arg.Any<ConfirmationPopup>());
        }

        [Fact]
        public void ClearPage_ShouldReturnNewObjectsWithinTheViewModel()
        {
            // Arrange
            _sut.Despacho.AlturaMedia = 2;
            _sut.Cliente.Nombre = "Kobe Bryant";
            _sut.DatosCamion.Chofer = "John Snow";
            _sut.IsValidInput = true;

            // Act
            _sut.ClearPage();

            // Assert
            _sut.Despacho.AlturaMedia.Should().Be(0);
            _sut.Cliente.Nombre.Should().BeNullOrEmpty();
            _sut.DatosCamion.Chofer.Should().BeNullOrEmpty();
            _sut.IsValidInput.Should().BeFalse();
        }


        [Fact(Skip = "Requires a Popup object -> Move to UI Testing")]
        public async Task ClosePopup_ShouldCallCloseAsyncInThePopup()
        {
            await _sut.ClosePopup();
        }
    }
}
