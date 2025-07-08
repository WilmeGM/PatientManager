using PatientManager.Core.Application.Interfaces.Repositories;
using PatientManager.Core.Domain.Entities;
using PatientManager.Infrastructure.Persistence.Contexts;

namespace PatientManager.Infrastructure.Persistence.Repositories
{
    public class LaboratoryTestRepository : GenericRepository<LaboratoryTest>, ILaboratoryTestRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LaboratoryTestRepository(ApplicationDbContext dbContext) : base(dbContext)
        => _dbContext = dbContext;
    }
}
