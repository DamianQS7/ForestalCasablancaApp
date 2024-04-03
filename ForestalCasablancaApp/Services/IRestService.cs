using BosquesNalcahue.Dtos;

namespace BosquesNalcahue.Services
{
    public interface IRestService
    {
        Task PostAsync(BaseReport report);
    }
}
