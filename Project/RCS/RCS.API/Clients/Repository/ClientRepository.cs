using RCS.API.Data;
using RCS.API.Data.Models;

namespace RCS.API.Clients.Repository;

public class ClientRepository(RcsDbContext context) : IClientRepository
{
    private readonly RcsDbContext _context = context;
    
    public async Task<int> AddClientAsync(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client.Id;
    }

    public async Task<Client> GetClientAsync(int id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task UpdateClientAsync(Client client)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteClientAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client is IndividualClient individualClient)
        {
            individualClient.FirstName = "DELETED";
            individualClient.LastName = "DELETED";
            individualClient.Address = "DELETED";
            individualClient.Email = "DELETED";
            individualClient.PhoneNumber = "DELETED";
            individualClient.PESEL = "DELETED";
            individualClient.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        else if (client is CompanyClient)
        {
            throw new Exception("Company client cannot be deleted");
        }
    }
}