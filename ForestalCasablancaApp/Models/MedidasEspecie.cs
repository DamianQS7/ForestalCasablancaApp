using ForestalCasablancaApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BosquesNalcahue.Models
{
    public class MedidasEspecie : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _diametroIngresado;
        public string DiametroIngresado
		{
            get => _diametroIngresado;
            set
            {
                if (_diametroIngresado == value)
                    return;

                _diametroIngresado = value;
                OnPropertyChanged(); 
            }
        }

        private string _cantidadIngresada;
        public string CantidadIngresada
        {
            get => _cantidadIngresada;
            set
            {
                if (_cantidadIngresada == value)
                    return;

                _cantidadIngresada = value;
                OnPropertyChanged();
            }
        }

        private string _nuevaEspecie;
        public string NuevaEspecie
        {
            get => _nuevaEspecie;
            set
            {
                if (_nuevaEspecie == value)
                    return;

                _nuevaEspecie = value;
                UpdateEspecie();
                OnPropertyChanged();
            }
        }

        private string _selectedEspecie;
        public string SelectedEspecie
        {
            get => _selectedEspecie;
            set
            {
                if (_selectedEspecie == value)
                    return;

                _selectedEspecie = value;
                UpdateEspecie();
                OnPropertyChanged();
            }
        }

        private string _selectedLargo;
        public string SelectedLargo
        {
            get => _selectedLargo;
            set
            {
                if (_selectedLargo == value)
                    return;

                _selectedLargo = value;
                UpdateLargo();
                OnPropertyChanged();
            }
        }

        private string _nuevoLargo;
        public string NuevoLargo
        {
            get => _nuevoLargo;
            set
            {
                if (_nuevoLargo == value)
                    return;

                _nuevoLargo = value.Trim().Replace(",", ".");
                UpdateLargo();
                OnPropertyChanged();
            }
        }

        public string UnidadOrigen { get; set; }
        public string Especie { get; set; }
        public string LargoEspecie { get; set; }
        public int CantidadTotalSum { get; set; }
        public double TotalSumFinal { get; set; }
        public ObservableCollection<MedidaTrozoAserrable> ListaMedidas { get; set; } = new();

        public void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void UpdateEspecie() => Especie = SelectedEspecie == "Otras Especies" ? NuevaEspecie : SelectedEspecie;
        public void UpdateLargo() => LargoEspecie = SelectedLargo == "Otros" ? NuevoLargo : SelectedLargo;
    }
}
