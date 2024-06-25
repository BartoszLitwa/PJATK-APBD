using MediatR;
using RCS.API.Data.Models;
using RCS.API.Softwares.Models.Requests;
using RCS.API.Softwares.Models.Responses;
using RCS.API.Softwares.Repository;

namespace RCS.API.Softwares.Commands;

public record CreateSoftwareCommand(CreateSoftwareRequest Request) : IRequest<CreateSoftwareResponse>;

public class CreateSoftwareHandler(ISoftwareRepository softwareRepository)
    : IRequestHandler<CreateSoftwareCommand, CreateSoftwareResponse>
{
    public async Task<CreateSoftwareResponse> Handle(CreateSoftwareCommand request, CancellationToken cancellationToken)
    {
        var software = new Software
        {
            Name = request.Request.Name,
            Description = request.Request.Description,
            CurrentVersion = request.Request.CurrentVersion,
            Category = request.Request.Category,
            Price = request.Request.Price
        };

        var softwareId = await softwareRepository.CreateSoftwareAsync(software);
        return new CreateSoftwareResponse(softwareId, software.Name);
    }
}
