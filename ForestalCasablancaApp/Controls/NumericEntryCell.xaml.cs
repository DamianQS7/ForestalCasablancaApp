namespace ForestalCasablancaApp.Controls;

public partial class NumericEntryCell : ContentView
{
    #region Bindable Properties

    public static readonly BindableProperty UserInputProperty =
        BindableProperty.Create(nameof(UserInput), typeof(string), typeof(NumericEntryCell), string.Empty, BindingMode.TwoWay);

    public string UserInput
    {
        get => (string)GetValue(UserInputProperty);
        set => SetValue(UserInputProperty, value);
    }

    #endregion

    public NumericEntryCell()
    {
        InitializeComponent();

        UserInputEntry.SetBinding(Entry.TextProperty, new Binding(nameof(UserInput), source: this));
    }
}