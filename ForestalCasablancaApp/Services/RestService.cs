using BosquesNalcahue.Dtos;
using System.Text.Json;
using System.Text;

namespace BosquesNalcahue.Services
{
    public class RestService : IRestService
    {
        private readonly HttpClient _client;
        private readonly string _baseUri;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public RestService(HttpClient client)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            _client = client;
            _baseUri = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:7117"
                                                                     : "http://localhost:7117";
            _url = $"{_baseUri}/api";
        }

        public async Task PostAsync(BaseReport report)
        { 
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                throw new Exception("No hay conexión a internet.");
            }

            try
            {
                string json = JsonSerializer.Serialize(report, _jsonSerializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync($"{_url}/reports", content);

                if (response.IsSuccessStatusCode)
                    Console.WriteLine("Reporte generado con éxito.");
                else
                    Console.WriteLine("Hubo un error al procesar el reporte. Por favor vuelva a intentar.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error posting report: {ex.Message}");
            }

            return;
        }
    }
}
