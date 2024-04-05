using BosquesNalcahue.Dtos;
using System.Text.Json;
using System.Text;
using System.Diagnostics;
using System.Net;

namespace BosquesNalcahue.Services
{
    public class RestService : IRestService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public RestService(HttpClient client)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            _client = client;
        }

        public async Task<HttpStatusCode> PostAsync(BaseReport report)
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

                HttpResponseMessage response = await _client.PostAsync($"{_client.BaseAddress}/reports", content);
                
                return response.StatusCode;
            }
            catch(TaskCanceledException ex)
            {
                Debug.WriteLine($"Error posting report: {ex.Message}");
                return HttpStatusCode.RequestTimeout;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error posting report: {ex.Message}");
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
