using CSharpFunctionalExtensions;
using MediatR;

namespace Animals.API.Animals.Queries;

public record GetAllAnimalsQuery() : IRequest<Result<IEnumerable<PlatformReadDto>, ValidationFailed>>;

public class GetAllAnimalsHandler()
    : IRequestHandler<GetAllAnimalsQuery, Result<IEnumerable<PlatformReadDto>, ValidationFailed>>
{
    public async Task<Result<IEnumerable<PlatformReadDto>, ValidationFailed>> Handle(GetAllAnimalsQuery request, CancellationToken cancellationToken)
    {
        var platforms = await dbContext.Platforms
            .ToListAsync(cancellationToken: cancellationToken);

        var platformsDtos= mapper.Map<List<PlatformReadDto>>(platforms);

        return Result.Success<IEnumerable<PlatformReadDto>, ValidationFailed>(platformsDtos);
    }
}