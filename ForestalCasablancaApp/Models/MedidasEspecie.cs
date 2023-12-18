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

        public string Especie { get; set; }
        public string LargoEspecie { get; set; }
        public int CantidadTotalSum { get; set; }
        public double TotalSumFinal { get; set; }
        public ObservableCollection<MedidaTrozoAserrable> ListaMedidas { get; set; } = new();

        public void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
