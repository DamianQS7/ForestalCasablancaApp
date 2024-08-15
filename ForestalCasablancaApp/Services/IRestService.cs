using BosquesNalcahue.Dtos;
using System.Net;

namespace BosquesNalcahue.Services
{
    public interface IRestService
    {
        Task<HttpResponseMessage> PostAsync(BaseReport report, string route);
    }
}
