using ForestalCasablancaApp.Helpers;

namespace BosquesNalcahue.Services
{
    public interface IInfoService
    {
        Task ShowToast(string message);
        Task ShowAlert(InfoMessage infoMessage);
        Task ShowAlert(string message);
    }
}
