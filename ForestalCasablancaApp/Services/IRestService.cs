using BosquesNalcahue.Dtos;
using System.Net;

namespace BosquesNalcahue.Services
{
    public interface IRestService
    {
        Task<HttpStatusCode> PostAsync(BaseReport report);
    }
}
