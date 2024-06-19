using RCS.API.Data.Models;

namespace RCS.API.Contracts.Repository;

public interface IContractRepository
{
    Task<int> CreateContractAsync(Contract contract);
    Task<Contract> GetContractAsync(int contractId);
    Task UpdateContractAsync(Contract contract);
    Task AddPaymentAsync(Payment payment);
    
    Task<decimal> CalculateCurrentRevenueAsync(string? productName = null);
    Task<decimal> CalculatePredictedRevenueAsync(string? productName = null);
    Task<decimal> GetExchangeRateAsync(string currency);
    
    Task<Software> GetSoftwareAsync(int softwareId);
}