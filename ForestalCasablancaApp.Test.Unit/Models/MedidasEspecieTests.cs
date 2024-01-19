namespace ForestalCasablancaApp.Tests.Unit.Models
{
    public class MedidasEspecieTests
    {
        private readonly MedidasEspecie _sut;

        public MedidasEspecieTests() { _sut = new MedidasEspecie(); }

        [Fact]
        public void DiametroIngresado_ShouldRaisePropertyChanged_WhenChanged()
        {
            // Arrange
            var propertyChanged = false;
            _sut.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_sut.DiametroIngresado))
                    propertyChanged = true;
            };

            // Act
            _sut.DiametroIngresado = "1";

            // Assert
            Assert.True(propertyChanged);
        }

        [Fact]
        public void CantidadIngresada_ShouldRaisePropertyChanged_WhenChanged()
        {
            // Arrange
            var propertyChanged = false;
            _sut.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_sut.CantidadIngresada))
                    propertyChanged = true;
            };

            // Act
            _sut.CantidadIngresada = "1";

            // Assert
            Assert.True(propertyChanged);
        }

        [Fact]
        public void NuevaEspecie_ShouldRaisePropertyChanged_WhenChanged()
        {
            // Arrange
            var propertyChanged = false;
            _sut.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_sut.NuevaEspecie))
                    propertyChanged = true;
            };

            // Act
            _sut.NuevaEspecie = "1";

            // Assert
            Assert.True(propertyChanged);
        }

        [Fact]
        public void SelectedEspecie_ShouldRaisePropertyChanged_WhenChanged()
        {
            // Arrange
            var propertyChanged = false;
            _sut.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_sut.SelectedEspecie))
                    propertyChanged = true;
            };

            // Act
            _sut.SelectedEspecie = "1";

            // Assert
            Assert.True(propertyChanged);
        }

        [Fact]
        public void UpdateEspecie_ShouldModifyEspecieToSelectedEspecie_WhenSelectedEspecieIsNotOtrasEspecies()
        {
            // Act
            _sut.SelectedEspecie = "1";

            // Assert
            _sut.Especie.Should().Be(_sut.SelectedEspecie);
        }

        [Fact]
        public void UpdateEspecie_ShouldModifyEspecieToNuevaEspecie_WhenSelectedEspecieIsOtrasEspecies()
        {
            // Act
            _sut.SelectedEspecie = "Otras Especies";

            // Assert
            _sut.Especie.Should().Be(_sut.NuevaEspecie);
        }

        [Fact]
        public void UpdateLargo_ShouldModifyLargoEspecieToSelectedLargo_WhenSelectedLargoIsNotOtros()
        {
            // Act
            _sut.SelectedLargo = "1";

            // Assert
            _sut.LargoEspecie.Should().Be(_sut.SelectedLargo);
        }

        [Fact]
        public void UpdateLargo_ShouldModifyLargoEspecieToNuevoLargo_WhenSelectedLargoIsOtros()
        {
            // Act
            _sut.SelectedLargo = "Otros";

            // Assert
            _sut.LargoEspecie.Should().Be(_sut.NuevoLargo);
        }

        [Fact]
        public void NuevoLargo_ShouldCorrectlyReplaceCharacters_WhenChanged()
        {
            // Arrange
            string expected = "1.1";
            string givenValue = "1,1";

            // Act
            _sut.NuevoLargo = givenValue;

            // Assert
            _sut.NuevoLargo.Should().Be(expected);
        }

    }
}
