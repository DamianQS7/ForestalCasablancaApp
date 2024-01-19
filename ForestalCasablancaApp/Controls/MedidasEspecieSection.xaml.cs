using ForestalCasablancaApp.Controls;
using ForestalCasablancaApp.Models;
using System.Collections.ObjectModel;

namespace BosquesNalcahue.Controls;

public partial class MedidasEspecieSection : ContentView
{
    public static readonly BindableProperty ComponentTitleProperty =
        BindableProperty.Create(nameof(ComponentTitle), typeof(string), typeof(MedidasEspecieSection), default, BindingMode.TwoWay);

    public static readonly BindableProperty EspeciePickerItemSourceProperty =
        BindableProperty.Create(nameof(EspeciePickerItemSource), typeof(List<string>), typeof(MedidasEspecieSection), default, BindingMode.TwoWay);

    public static readonly BindableProperty SelectedEspecieProperty =
        BindableProperty.Create(nameof(SelectedEspecie), typeof(string), typeof(MedidasEspecieSection), default, BindingMode.TwoWay);

    public static readonly BindableProperty LargoPickerItemSourceProperty =
        BindableProperty.Create(nameof(LargoPickerItemSource), typeof(List<string>), typeof(MedidasEspecieSection), default, BindingMode.TwoWay);

    public static readonly BindableProperty SelectedLargoProperty =
        BindableProperty.Create(nameof(SelectedLargo), typeof(string), typeof(MedidasEspecieSection), default, BindingMode.TwoWay);

    public static readonly BindableProperty DiametroCellIdProperty =
        BindableProperty.Create(nameof(DiametroCellId), typeof(string), typeof(MedidasEspecieSection), default, BindingMode.TwoWay);

    public static readonly BindableProperty DiametroInputProperty =
        BindableProperty.Create(nameof(DiametroInput), typeof(string), typeof(MedidasEspecieSection), default, BindingMode.TwoWay);

    public static readonly BindableProperty CantidadInputProperty =
        BindableProperty.Create(nameof(CantidadInput), typeof(string), typeof(MedidasEspecieSection), default, BindingMode.TwoWay);

    public static readonly BindableProperty ListaMedidasSourceProperty = BindableProperty.Create(nameof(ListaMedidasSource), 
        typeof(ObservableCollection<MedidaTrozoAserrable>), typeof(MedidasEspecieSection), default, BindingMode.TwoWay);

    public static readonly BindableProperty ListaMedidasCountProperty =
        BindableProperty.Create(nameof(ListaMedidasCount), typeof(int), typeof(MedidasEspecieSection), default, BindingMode.TwoWay);

    public static readonly BindableProperty UnidadOrigenProperty =
        BindableProperty.Create(nameof(UnidadOrigen), typeof(string), typeof(MedidasEspecieSection), default, BindingMode.TwoWay);

    public static readonly BindableProperty NuevaEspecieProperty =
        BindableProperty.Create(nameof(NuevaEspecie), typeof(string), typeof(MedidasEspecieSection), default, BindingMode.TwoWay);

    public static readonly BindableProperty NuevoLargoProperty =
        BindableProperty.Create(nameof(NuevoLargo), typeof(string), typeof(MedidasEspecieSection), default, BindingMode.TwoWay);

    public string ComponentTitle
    {
        get => (string)GetValue(ComponentTitleProperty);
        set => SetValue(ComponentTitleProperty, value);
    }

    public List<string> EspeciePickerItemSource
    {
        get => (List<string>)GetValue(EspeciePickerItemSourceProperty);
        set => SetValue(EspeciePickerItemSourceProperty, value);
    }

    public string SelectedEspecie
    {
        get => (string)GetValue(SelectedEspecieProperty);
        set => SetValue(SelectedEspecieProperty, value);
    }

    public List<string> LargoPickerItemSource
    {
        get => (List<string>)GetValue(LargoPickerItemSourceProperty);
        set => SetValue(LargoPickerItemSourceProperty, value);
    }

    public string SelectedLargo
    {
        get => (string)GetValue(SelectedLargoProperty);
        set => SetValue(SelectedLargoProperty, value);
    }

    public string NuevoLargo
    {
        get => (string)GetValue(NuevoLargoProperty);
        set => SetValue(NuevoLargoProperty, value);
    }

    public string DiametroCellId
    {
        get => (string)GetValue(DiametroCellIdProperty);
        set => SetValue(DiametroCellIdProperty, value);
    }

    public string DiametroInput
    {
        get => (string)GetValue(DiametroInputProperty);
        set => SetValue(DiametroInputProperty, value);
    }

    public string CantidadInput
    {
        get => (string)GetValue(CantidadInputProperty);
        set => SetValue(CantidadInputProperty, value);
    }
    
    public ObservableCollection<MedidaTrozoAserrable> ListaMedidasSource
    {
        get => (ObservableCollection<MedidaTrozoAserrable>)GetValue(ListaMedidasSourceProperty);
        set => SetValue(ListaMedidasSourceProperty, value);
    }

    public int ListaMedidasCount
    {
        get => (int)GetValue(ListaMedidasCountProperty);
        set => SetValue(ListaMedidasCountProperty, value);
    }

    public string UnidadOrigen
    {
        get => (string)GetValue(UnidadOrigenProperty);
        set => SetValue(UnidadOrigenProperty, value);
    }

    public string NuevaEspecie
    {
        get => (string)GetValue(NuevaEspecieProperty);
        set => SetValue(NuevaEspecieProperty, value);
    }

    public MedidasEspecieSection()
	{
		InitializeComponent();

        DiametroCell.SetBinding(NumericEntryCell.UserInputProperty, new Binding(nameof(DiametroInput), source: this));
        CantidadCell.SetBinding(NumericEntryCell.UserInputProperty, new Binding(nameof(CantidadInput), source: this));

    }
}