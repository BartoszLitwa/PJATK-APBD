using RCS.API.Data.Models;

namespace RCS.API.Clients.Repository;

public interface IClientRepository
{
    Task<Client> AddClientAsync(Client client);
    Task<Client?> GetClientAsync(int id);
    Task<Client> UpdateClientAsync(Client client);
    Task DeleteClientAsync(int id);
}