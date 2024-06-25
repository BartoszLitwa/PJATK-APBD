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
    
    public async Task<decimal> CalculateCurrentRevenueAsync(string? productName = null)
    {
        var contractsQuery = context.Contracts.AsQueryable();

        if (!string.IsNullOrEmpty(productName))
        {
            contractsQuery = contractsQuery.Where(c => c.Software.Name == productName);
        }

        var signedContracts = await contractsQuery
            .Where(c => c.IsSigned)
            .Include(c => c.Payments)
            .ToListAsync();

        var currentRevenue = signedContracts
            .SelectMany(c => c.Payments)
            .Sum(p => p.Amount);

        return currentRevenue;
    }

    public async Task<decimal> CalculatePredictedRevenueAsync(string? productName = null)
    {
        var contractsQuery = context.Contracts.AsQueryable();

        if (!string.IsNullOrEmpty(productName))
        {
            contractsQuery = contractsQuery.Where(c => c.Software.Name == productName);
        }

        var signedContracts = await contractsQuery
            .Where(c => c.IsSigned)
            .ToListAsync();

        var unsignedContracts = await contractsQuery
            .Where(c => !c.IsSigned)
            .ToListAsync();

        var currentRevenue = signedContracts.Sum(c => c.Price);
        var pendingRevenue = unsignedContracts.Sum(c => c.Price);

        return currentRevenue + pendingRevenue;
    }
    
    public async Task<Software> GetSoftwareAsync(int softwareId)
    {
        return await context.Software.FindAsync(softwareId);
    }
    
    public async Task<bool> HasActiveContractAsync(int clientId, int softwareId)
    {
        return await context.Contracts.AnyAsync(c => c.ClientId == clientId && c.SoftwareId == softwareId && !c.IsSigned);
    }
    
    public async Task<bool> HasPreviousContractAsync(int clientId)
    {
        return await context.Contracts.AnyAsync(c => c.ClientId == clientId && c.IsSigned);
    }
}