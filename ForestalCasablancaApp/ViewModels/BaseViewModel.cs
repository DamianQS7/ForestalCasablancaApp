using BosquesNalcahue.Services;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ForestalCasablancaApp.Helpers;
using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.Services;

namespace ForestalCasablancaApp.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    #region Dependencies

    internal readonly ICalculatorService _calculatorService;
    internal readonly IPdfGeneratorService _pdfGeneratorService;
    internal readonly IInfoService _infoService;
    internal readonly IRestService _restService;
    
    #endregion

    #region Properties

    [ObservableProperty] private string _title;
    [ObservableProperty] private bool _isValidInput;
    [ObservableProperty] private string _folio;
    [ObservableProperty] private Cliente _cliente;
    [ObservableProperty] private DatosCamion _datosCamion;
    [ObservableProperty] private DateTime _reportDate;
    [ObservableProperty] private DespachoModel _despacho;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;
    public bool IsNotBusy => !IsBusy;

    public string ReportType { get; set;}
    public string FileId { get; set; }

    public List<string> ListaEspecies { get; set; }

    public Popup Popup { get; set; }

    public string OperatorName => Preferences.Get("CurrentUser", "Usuario sin definir.");
    
    public static Page BasePage => Application.Current.MainPage;

    #endregion

    #region Methods
    public BaseViewModel(ICalculatorService calculatorService, IPdfGeneratorService pdfGeneratorService, IInfoService infoService)
    {
        _calculatorService = calculatorService;
        _pdfGeneratorService = pdfGeneratorService;
        _infoService = infoService;
        _restService = new RestService();
        _cliente = new();
        _datosCamion = new();
        _despacho = new();
        IsValidInput = false;
        IsBusy = false;
    }

    public void GenerateFileMetadata()
    {
        GenerateFolio();
        ReportDate = DateTime.Now;
        FileId = PdfGeneratorService.GenerateFileName(Cliente.Nombre);
    }
    private void GenerateFolio()
    {
        string initials = Preferences.Get("CurrentUserInitials", "NN");
        string date = DateTime.Now.ToString("dd'/'MM'/'yy'-'HH:mm");
        int currentNumber = Preferences.Get("CurrentFileNumber", 1);

        Folio = $"{currentNumber}{initials}{date}";
    }

    /// <summary>
    /// Validates the alturas (heights) in the Despacho model and updates the model's AlturaMedia accordingly.
    /// </summary>
    private void ValidateAlturasAndUpdateModelAccordingly()
    {
        if (_calculatorService.CheckIfAlturasAreValid(Despacho.Alturas))
            Despacho.AlturaMedia = _calculatorService.CalculateAlturaMedia(Despacho.Alturas);
        else
            Despacho.AlturaMedia = 0;
    }

    /// <summary>
    /// Validates the palomera in the Despacho model and updates the model's properties accordingly.
    /// </summary>
    private void ValidatePalomeraAndUpdateModelAccordingly()
    {
        if (_calculatorService.CheckPalomera(Despacho.AnchoPalomera, Despacho.AltoPalomera, Despacho.AltoPalomera2))
        {
            Despacho.AlturaMediaPalomera = _calculatorService.CalculateAlturaMediaPalomera(Despacho.AltoPalomera, Despacho.AltoPalomera2);
            Despacho.MedidaPalomera = _calculatorService.CalculateMedidaPalomera(Despacho.AlturaMediaPalomera, Despacho.AnchoPalomera);
            Despacho.IsPalomeraValid = true;
        }
        else
        {
            Despacho.MedidaPalomera = 0;
            Despacho.IsPalomeraValid = false;
        }
    }

    /// <summary>
    /// Validates the input data for a Despacho and updates the model accordingly.
    /// </summary>
    /// <returns>
    /// Returns true if the input data is valid; otherwise, shows an alert and returns false.
    /// </returns>
    public virtual bool ValidateInput()
    {
        ValidateAlturasAndUpdateModelAccordingly();
        ValidatePalomeraAndUpdateModelAccordingly();

        if (Despacho.AlturaMedia <= 0 || Despacho.Bancos is null || Despacho.LargoCamion is null)
        {
            _infoService.ShowAlert(ReportType == "Metro Ruma" ? InfoMessage.MissingMetroRumaData
                                                              : InfoMessage.MissingLeñaData);
            return false;
        }

        if (!Despacho.IsPalomeraValid)
        {
            _infoService.ShowAlert(InfoMessage.InvalidPalomera);
            return false;
        }

        return true;
    }

    #endregion

    #region Commands

    /// <summary>
    /// Asynchronously closes the currently displayed popup.
    /// </summary>
    [RelayCommand]
    public async Task ClosePopup()
    {
        await Popup.CloseAsync();
    }

    [RelayCommand]
    async static Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    #endregion
}
