using MediatR;
using RCS.API.Auth.Models.Requests;
using RCS.API.Auth.Models.Responses;
using RCS.API.Auth.Repository;
using RCS.API.Common;
using RCS.API.Data.Models;

namespace RCS.API.Auth.Commands;

public record RegisterEmployeeCommand(CreateEmployeeRequest Request) : IRequest<LoginResponse>;

public class CreateEmployeeHandler(IEmployeeRepository repository, IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<RegisterEmployeeCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(RegisterEmployeeCommand request, CancellationToken cancellationToken)
    {
        var existingEmployee = await repository.GetEmployeeByLoginAsync(request.Request.Login, cancellationToken);
        if (existingEmployee != null) throw new Exception("Employee with this login already exists");

        var employee = new Employee
        {
            Login = request.Request.Login,
            Password = request.Request.Password,
            IsAdmin = request.Request.IsAdmin
        };

        var employeeCreated = await repository.CreateEmployeeAsync(employee);
        if (employeeCreated == 0) throw new Exception("Failed to create employee");
        
        var token = jwtTokenGenerator.GenerateToken(employee.Login, employee.Password);
        return new LoginResponse(token);
    }
}
