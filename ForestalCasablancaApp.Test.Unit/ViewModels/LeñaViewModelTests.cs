using ForestalCasablancaApp.Services;
using ForestalCasablancaApp.ViewModels;

namespace ForestalCasablancaApp.Tests.Unit.ViewModels
{
    public class LeñaViewModelTests
    {
        private readonly LeñaViewModel _sut;
        private readonly ICalculatorService _calculatorService = Substitute.For<ICalculatorService>();
        private readonly IPdfGeneratorService _pdfGeneratorService = Substitute.For<IPdfGeneratorService>();

        public LeñaViewModelTests()
        {
            _sut = new LeñaViewModel(_calculatorService, _pdfGeneratorService);
        }

        [Fact]
        public void ValidateInput_ShouldReturnFalse_WhenNoValuesAreGiven()
        {
            // Act
            var result = _sut.ValidateInput();

            // Assert
            result.Should().BeFalse();
        }

        //[Fact]
        //public void ValidateInput_ShouldReturnFalse_WhenThereAreMissingValuesForCalculation()
        //{

        //}

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

        
    }
}
