using MediatR;
using RCS.API.Contracts.Models.Requests;
using RCS.API.Contracts.Models.Responses;
using RCS.API.Contracts.Repository;
using RCS.API.Data.Models;

namespace RCS.API.Contracts.Commands;

public record IssuePaymentCommand(IssuePaymentRequest Request) : IRequest<IssuePaymentResponse>;

public class IssuePaymentHandler(IContractRepository contractRepository)
    : IRequestHandler<IssuePaymentCommand, IssuePaymentResponse>
{
    public async Task<IssuePaymentResponse> Handle(IssuePaymentCommand request, CancellationToken cancellationToken)
    {
        var contract = await contractRepository.GetContractAsync(request.Request.ContractId);
        if (contract == null) return new IssuePaymentResponse(false, "Contract not found");

        if (DateTime.Now > contract.EndDate)
        {
            return new IssuePaymentResponse(false, "Payment deadline has passed");
        }

        var totalPaid = contract.Payments.Sum(p => p.Amount);
        var remainingAmount = contract.Price - totalPaid;

        if (request.Request.Amount > remainingAmount)
        {
            return new IssuePaymentResponse(false, "Payment amount exceeds remaining balance");
        }

        var payment = new Payment
        {
            ContractId = request.Request.ContractId,
            Amount = request.Request.Amount,
            PaymentDate = DateTime.Now
        };

        await contractRepository.AddPaymentAsync(payment);

        if (totalPaid + request.Request.Amount >= contract.Price)
        {
            contract.IsSigned = true;
            await contractRepository.UpdateContractAsync(contract);
        }

        return new IssuePaymentResponse(true, "Payment successful");
    }
}
