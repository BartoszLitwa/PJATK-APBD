using MediatR;
using RCS.API.Auth.Models.Requests;
using RCS.API.Auth.Models.Responses;
using RCS.API.Auth.Repository;
using RCS.API.Common;

namespace RCS.API.Auth.Commands;

public record LoginCommand(LoginRequest Request) : IRequest<LoginResponse>;

public class LoginHandler(IEmployeeRepository repository, IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var employee = await repository.GetEmployeeAsync(request.Request.Login, request.Request.Password, cancellationToken);
        if (employee is null) throw new Exception("Invalid login or password");

        var token = jwtTokenGenerator.GenerateToken(employee.Login, employee.Password);

        return new LoginResponse(token);
    }
}
