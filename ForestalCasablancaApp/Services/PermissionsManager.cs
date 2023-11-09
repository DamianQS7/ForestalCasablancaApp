using static Microsoft.Maui.ApplicationModel.Permissions;
#if ANDROID
using Android;
using Android.Content;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
#endif

namespace ForestalCasablancaApp.Services
{
    public class PermissionsManager : BasePlatformPermission, IPermissionsManager
    {
        public async Task GetPermissionsAsync()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android && DeviceInfo.Version.Major >= 11)
            {
                //await Shell.Current.DisplayAlert("Permiso requerido", "Es necesario permitir el acceso de la app a todos los archivos", "Ok");
#if ANDROID
                await RequestManageAllFilesPermissionAsync();
#endif
            }
            else
            {
                var status = await CheckStatusAsync<PermissionsManager>();

                if (status != PermissionStatus.Granted)
                {
                    status = await RequestAsync<PermissionsManager>();
                }

                if (status != PermissionStatus.Granted)
                {
                    await Shell.Current.DisplayAlert("Permiso requerido", "Es necesario otorgar permisos a la app", "Ok");
                }
            }
        }

#if ANDROID
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
               new List<(string androidPermission, bool isRuntime)>
               {
                 ("android.permission.WRITE_EXTERNAL_STORAGE", true)
               }.ToArray();


        public async Task RequestManageAllFilesPermissionAsync()
        {
            var activity = Platform.CurrentActivity ?? throw new NullReferenceException("Current Activity is Null.");

            var intent = new Intent(Android.Provider.Settings.ActionManageAllFilesAccessPermission);

            activity.StartActivityForResult(intent, 1);
        }
#endif
    }
}
