using RCS.API.Data.Models;

namespace RCS.API.Softwares.Repository;

public interface ISoftwareRepository
{
    Task<int> CreateSoftwareAsync(Software software);
    Task<IEnumerable<Software>> GetAllSoftwareAsync();
    Task<Software?> GetSoftwareAsync(int requestSoftwareId);
}
