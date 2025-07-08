using Microsoft.EntityFrameworkCore;
using PatientManager.Core.Application.Interfaces.Repositories;
using PatientManager.Core.Domain.Entities;
using PatientManager.Infrastructure.Persistence.Contexts;

namespace PatientManager.Infrastructure.Persistence.Repositories
{
    public class LaboratoryResultRepository : GenericRepository<LaboratoryResult>, ILaboratoryResultRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LaboratoryResultRepository(ApplicationDbContext dbContext) : base(dbContext)
        => _dbContext = dbContext;

        public override async Task<LaboratoryResult> GetByIdAsync(int id)
        {
            return await _dbContext.LaboratoryResults
                .Include(lr => lr.LaboratoryTest)
                .Include(lr => lr.Appointment)
                .Include(lr => lr.Patient)
                .FirstOrDefaultAsync(lr => lr.Id == id);
        }
    }
}
