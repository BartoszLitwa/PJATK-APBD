using MediatR;
using RCS.API.Softwares.Models.Responses;
using RCS.API.Softwares.Repository;

namespace RCS.API.Softwares.Queries;

public record GetAllSoftwareQuery : IRequest<IEnumerable<SoftwareResponse>>;

public class GetAllSoftwareHandler(ISoftwareRepository softwareRepository)
    : IRequestHandler<GetAllSoftwareQuery, IEnumerable<SoftwareResponse>>
{
    private readonly ISoftwareRepository _softwareRepository = softwareRepository;

    public async Task<IEnumerable<SoftwareResponse>> Handle(GetAllSoftwareQuery request, CancellationToken cancellationToken)
    {
        var softwareList = await _softwareRepository.GetAllSoftwareAsync();
        return softwareList.Select(s => new SoftwareResponse(s.Id, s.Name, s.Description, s.CurrentVersion, s.Category, s.Price));
    }
}