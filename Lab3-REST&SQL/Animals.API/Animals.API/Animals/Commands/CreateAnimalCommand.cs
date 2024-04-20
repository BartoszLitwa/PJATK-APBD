using System.ComponentModel.DataAnnotations;
using MediatR;
using CSharpFunctionalExtensions;
using Animals.API.Common.Exceptions;

namespace Animals.API.Animals.Commands;

public record CreateAnimalRequestDto(
    [MaxLength(200)] string Name, 
    [MaxLength(200)] string? Description, 
    [MaxLength(200)] string Category, 
    [MaxLength(200)] string Area);

public record CreateAnimalCommand(string Name, string? Description, string Category, string Area) 
    : IRequest<Result<CreateAnimalRequestDto, ValidationFailed>>;

public class CreateAnimalCommandHandler
    : IRequestHandler<CreateAnimalCommand, Result<CreateAnimalCommand, ValidationFailed>>
{
    public async Task<Result<CreateAnimalCommand, ValidationFailed>> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
    {
        await using var con = new SqlConnection();
        await con.OpenAsync(cancellationToken);

        var sqlCmd = "INSERT INTO [dbo].[Animals] (Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area)";
        await using var cmd = new SqlCommand(sqlCmd, con);
        cmd.Parameters.Add(new SqlParameter("@Name", request.Name));
        cmd.Parameters.Add(new SqlParameter("@Description", request.Description));
        cmd.Parameters.Add(new SqlParameter("@Category", request.Category));
        cmd.Parameters.Add(new SqlParameter("@Area", request.Area));

        await cmd.ExecuteNonQueryAsync(cancellationToken);

        return Result.Success<CommandReadDto, ValidationFailed>(new CommandReadDto());
    }
}