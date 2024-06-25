using Microsoft.EntityFrameworkCore;
using RCS.API.Data;
using RCS.API.Data.Models;

namespace RCS.API.Auth.Repository;

public class EmployeeRepository(RcsDbContext context) : IEmployeeRepository
{
    public async Task<int> CreateEmployeeAsync(Employee employee)
    {
        context.Employees.Add(employee);
        await context.SaveChangesAsync();
        return employee.Id;
    }

    public Task<Employee?> GetEmployeeAsync(string login, string password, CancellationToken cancellationToken = default)
    {
        return context.Employees.FirstOrDefaultAsync(e => e.Login == login && e.Password == password, cancellationToken);
    }

    public Task<Employee?> GetEmployeeByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return context.Employees.FirstOrDefaultAsync(e => e.Login == login, cancellationToken);
    }
}