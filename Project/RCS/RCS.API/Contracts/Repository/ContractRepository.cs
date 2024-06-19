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

        var currentRevenue = await contractsQuery
            .Where(c => c.IsSigned)
            .SumAsync(c => c.Payments.Sum(p => p.Amount));

        return currentRevenue;
    }

    public async Task<decimal> CalculatePredictedRevenueAsync(string? productName = null)
    {
        var contractsQuery = context.Contracts.AsQueryable();

        if (!string.IsNullOrEmpty(productName))
        {
            contractsQuery = contractsQuery.Where(c => c.Software.Name == productName);
        }

        var currentRevenue = await contractsQuery
            .Where(c => c.IsSigned)
            .SumAsync(c => c.Price);

        var pendingRevenue = await contractsQuery
            .Where(c => !c.IsSigned)
            .SumAsync(c => c.Price);

        return currentRevenue + pendingRevenue;
    }

    public async Task<decimal> GetExchangeRateAsync(string currency)
    {
        // Assume we have a service that provides the exchange rate
        // For example purposes, return a fixed rate
        return currency switch
        {
            "USD" => 0.25m,
            "EUR" => 0.22m,
            _ => 1m // Default to PLN
        };
    }
    
    public async Task<Software> GetSoftwareAsync(int softwareId)
    {
        return await context.Software.FindAsync(softwareId);
    }
}