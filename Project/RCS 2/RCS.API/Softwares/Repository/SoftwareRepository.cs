using Microsoft.EntityFrameworkCore;
using RCS.API.Data;
using RCS.API.Data.Models;

namespace RCS.API.Softwares.Repository;

public class SoftwareRepository(RcsDbContext _context) : ISoftwareRepository
{
    public async Task<int> CreateSoftwareAsync(Software software)
    {
        _context.Software.Add(software);
        await _context.SaveChangesAsync();
        return software.Id;
    }
    
    public async Task<IEnumerable<Software>> GetAllSoftwareAsync()
    {
        return await _context.Software.ToListAsync();
    }

    public Task<Software?> GetSoftwareAsync(int requestSoftwareId)
    {
        return _context.Software.FirstOrDefaultAsync(s => s.Id == requestSoftwareId);
    }
}