using RCS.API.Data.Models;

namespace RCS.API.Auth.Repository;

public interface IEmployeeRepository
{
    Task<int> CreateEmployeeAsync(Employee employee);
    Task<Employee?> GetEmployeeAsync(string login, string password, CancellationToken cancellationToken = default);
    Task<Employee?> GetEmployeeByLoginAsync(string login, CancellationToken cancellationToken = default);
}