using BosquesNalcahue.Dtos;
using System.Text.Json;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;

namespace BosquesNalcahue.Services
{
    public class RestService : IRestService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly string _apiKey = "MarrichiweuPupeiPuLamngen";

        public RestService()
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            HttpClientHandler handler = GetInsecureHandler();
            _client = new HttpClient(handler)
            {
                BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7117/api" : "http://localhost:5207/api"),
                Timeout = TimeSpan.FromSeconds(60),
                DefaultRequestHeaders = { { "api-key", _apiKey } }
            };
        }

        public async Task<HttpResponseMessage> PostAsync(BaseReport report, string route)
        { 
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                throw new Exception("No hay conexión a internet.");
            }

            try
            {
                string json = report is SingleProductReport ?
                              JsonSerializer.Serialize(report as SingleProductReport, _jsonSerializerOptions):
                              JsonSerializer.Serialize(report as MultiProductReport, _jsonSerializerOptions);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var endpoint = $"{_client.BaseAddress}/reports/{route}";
                HttpResponseMessage response = await _client.PostAsync(endpoint, content);
                return response;
            }
            catch(TaskCanceledException ex)
            {
                Debug.WriteLine($"Error posting report due to a timeout: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error posting report: {ex.Message}, inner exception {ex.InnerException}");
                return null;
            }
        }

        private HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }
    }
}
