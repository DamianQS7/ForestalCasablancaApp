namespace ForestalCasablancaApp.Controls;

public partial class NumericEntryCell : ContentView
{
    #region Bindable Properties

    public static readonly BindableProperty UserInputProperty = 
        BindableProperty.Create(nameof(UserInput), typeof(string), typeof(NumericEntryCell), string.Empty, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (NumericEntryCell)bindable;

            control.Input.Text = newValue as string;
        });
    
    public string UserInput
    {
        get => (string)GetValue(UserInputProperty);
        set => SetValue(UserInputProperty, value);
    }

    #endregion


    public NumericEntryCell()
	{
		InitializeComponent();

	}
}