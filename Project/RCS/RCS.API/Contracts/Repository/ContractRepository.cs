using Microsoft.EntityFrameworkCore;
using RCS.API.Data;
using RCS.API.Data.Models;

namespace RCS.API.Contracts.Repository;

public class ContractRepository(RcsDbContext context) : IContractRepository
{
    public async Task<int> CreateContractAsync(Contract contract)
    {
        context.Contracts.Add(contract);
        await context.SaveChangesAsync();
        return contract.Id;
    }

    public async Task<Contract> GetContractAsync(int contractId)
    {
        return await context.Contracts.Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.Id == contractId);
    }

    public async Task UpdateContractAsync(Contract contract)
    {
        context.Contracts.Update(contract);
        await context.SaveChangesAsync();
    }

    public async Task AddPaymentAsync(Payment payment)
    {
        context.Payments.Add(payment);
        await context.SaveChangesAsync();
    }
}