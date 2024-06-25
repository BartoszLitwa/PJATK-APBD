using RCS.API.Data;
using RCS.API.Data.Models;

namespace RCS.API.Clients.Repository;

public class ClientRepository(RcsDbContext context) : IClientRepository
{
    private readonly RcsDbContext _context = context;
    
    public async Task<Client> AddClientAsync(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client;
    }

    public async Task<Client?> GetClientAsync(int id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task<Client> UpdateClientAsync(Client client)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
        return client;
    }

    public async Task DeleteClientAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client is IndividualClient individualClient)
        {
            individualClient.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        else if (client is CompanyClient)
        {
            throw new Exception("Company client cannot be deleted");
        }
    }
}