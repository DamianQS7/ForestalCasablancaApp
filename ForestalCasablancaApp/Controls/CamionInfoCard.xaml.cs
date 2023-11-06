namespace ForestalCasablancaApp.Controls;

public partial class CamionInfoCard : ContentView
{
    public static readonly BindableProperty EmpresaProperty =
        BindableProperty.Create(nameof(Empresa), typeof(string), typeof(CamionInfoCard), default, BindingMode.TwoWay);

    public static readonly BindableProperty NombreChoferProperty =
        BindableProperty.Create(nameof(NombreChofer), typeof(string), typeof(CamionInfoCard), default, BindingMode.TwoWay);

    public static readonly BindableProperty RutChoferProperty =
        BindableProperty.Create(nameof(RutChofer), typeof(string), typeof(CamionInfoCard), default, BindingMode.TwoWay);

    public static readonly BindableProperty PatenteCamionProperty =
        BindableProperty.Create(nameof(PatenteCamion), typeof(string), typeof(CamionInfoCard), default, BindingMode.TwoWay);

    public static readonly BindableProperty OrigenProperty =
        BindableProperty.Create(nameof(Origen), typeof(string), typeof(CamionInfoCard), default, BindingMode.TwoWay);

    public string Empresa
    {
        get => (string)GetValue(EmpresaProperty);
        set => SetValue(EmpresaProperty, value);
    }

    public string NombreChofer
    {
        get => (string)GetValue(NombreChoferProperty);
        set => SetValue(NombreChoferProperty, value);
    }

    public string RutChofer
    {
        get => (string)GetValue(RutChoferProperty);
        set => SetValue(RutChoferProperty, value);
    }

    public string PatenteCamion
    {
        get => (string)GetValue(PatenteCamionProperty);
        set => SetValue(PatenteCamionProperty, value);
    }

    public string Origen
    {
        get => (string)GetValue(OrigenProperty);
        set => SetValue(OrigenProperty, value);
    }

    public CamionInfoCard()
	{
		InitializeComponent();
        EmpresaEntry.SetBinding(Entry.TextProperty, new Binding(nameof(Empresa), source: this));
        ChoferEntry.SetBinding(Entry.TextProperty, new Binding(nameof(NombreChofer), source: this));
        RutChoferEntry.SetBinding(Entry.TextProperty, new Binding(nameof(RutChofer), source: this));
        PatenteEntry.SetBinding(Entry.TextProperty, new Binding(nameof(PatenteCamion), source: this));
        OrigenEntry.SetBinding(Entry.TextProperty, new Binding(nameof(Origen), source: this));
    }
}