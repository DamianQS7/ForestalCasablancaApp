namespace ForestalCasablancaApp.Controls;

public partial class CardView : ContentView
{

    #region Bindable Properties

    public static readonly BindableProperty CardTitleProperty = BindableProperty.Create(nameof(CardTitle), typeof(string), typeof(CardView), string.Empty, BindingMode.TwoWay);
    public static readonly BindableProperty CardImageProperty = BindableProperty.Create(nameof(CardImage), typeof(ImageSource), typeof(CardView), null);

    public string CardTitle
    {
        get => (string)GetValue(CardTitleProperty);
        set => SetValue(CardTitleProperty, value);
    }

    public ImageSource CardImage
    {
        get => (ImageSource)GetValue(CardImageProperty);
        set => SetValue(CardImageProperty, value);
    }

    #endregion

    public CardView()
	{
		InitializeComponent();
	}
}