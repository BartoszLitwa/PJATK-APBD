using RCS.API.Data.Models;

namespace RCS.API.Contracts.Repository;

public interface IContractRepository
{
    Task<int> CreateContractAsync(Contract contract);
    Task<Contract> GetContractAsync(int contractId);
    Task UpdateContractAsync(Contract contract);
    Task AddPaymentAsync(Payment payment);
}