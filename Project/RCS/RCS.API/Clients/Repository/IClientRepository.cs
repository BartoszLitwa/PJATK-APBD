using RCS.API.Data.Models;

namespace RCS.API.Clients.Repository;

public interface IClientRepository
{
    Task<int> AddClientAsync(Client client);
    Task<Client> GetClientAsync(int id);
    Task UpdateClientAsync(Client client);
    Task SoftDeleteClientAsync(int id);
}