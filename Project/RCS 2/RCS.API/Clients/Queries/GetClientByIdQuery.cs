using MediatR;
using Microsoft.EntityFrameworkCore;
using RCS.API.Data;
using RCS.API.Data.Models;

namespace RCS.API.Clients.Queries;

public record GetClientByIdQuery(int ClientId) : IRequest<Client>;

public class GetClientByIdHandler(RcsDbContext context) : IRequestHandler<GetClientByIdQuery, Client>
{
    public async Task<Client> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await context.Clients.FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);
        if (client == null) 
            throw new Exception("Client not found");
        return client;
    }
}