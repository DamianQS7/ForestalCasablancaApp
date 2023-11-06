namespace ForestalCasablancaApp.Controls;

public partial class NumericEntryCell : ContentView
{
    #region Bindable Properties

    public static readonly BindableProperty UserInputProperty =
        BindableProperty.Create(nameof(UserInput), typeof(string), typeof(NumericEntryCell), string.Empty, BindingMode.TwoWay);

    public static readonly BindableProperty ReadOnlyProperty =
        BindableProperty.Create(nameof(ReadOnly), typeof(bool), typeof(NumericEntryCell), false, BindingMode.TwoWay);

    public static readonly BindableProperty IdentifierProperty =
        BindableProperty.Create(nameof(Identifier), typeof(string), typeof(NumericEntryCell), string.Empty, BindingMode.TwoWay);

    public string UserInput
    {
        get => (string)GetValue(UserInputProperty);
        set => SetValue(UserInputProperty, value);
    }

    public bool ReadOnly
    {
        get => (bool)GetValue(ReadOnlyProperty);
        set => SetValue(ReadOnlyProperty, value);
    }

    public string Identifier
    {
        get => (string)GetValue(IdentifierProperty);
        set => SetValue(IdentifierProperty, value);
    }

    #endregion

    public NumericEntryCell()
    {
        InitializeComponent();
        
        UserInputEntry.SetBinding(Entry.TextProperty, new Binding(nameof(UserInput), source: this));
        UserInputEntry.SetBinding(Entry.IsReadOnlyProperty, new Binding(nameof(ReadOnly), source: this));
    }
}