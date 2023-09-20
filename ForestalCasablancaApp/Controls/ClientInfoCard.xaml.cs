namespace ForestalCasablancaApp.Controls;

public partial class ClientInfoCard : ContentView
{
    public static readonly BindableProperty NombreClienteProperty =
        BindableProperty.Create(nameof(NombreCliente), typeof(string), typeof(ClientInfoCard), default, BindingMode.TwoWay);

    public static readonly BindableProperty RutClienteProperty =
        BindableProperty.Create(nameof(RutCliente), typeof(string), typeof(ClientInfoCard), default, BindingMode.TwoWay);


    public string NombreCliente
    {
        get => (string)GetValue(NombreClienteProperty);
        set => SetValue(NombreClienteProperty, value);
    }

    public string RutCliente
    {
        get => (string)GetValue(RutClienteProperty);
        set => SetValue(RutClienteProperty, value);
    }

    public ClientInfoCard()
    {
        InitializeComponent();
        NombreEntry.SetBinding(Entry.TextProperty, new Binding(nameof(NombreCliente), source: this));
        RutEntry.SetBinding(Entry.TextProperty, new Binding(nameof(RutCliente), source: this));
    }
}