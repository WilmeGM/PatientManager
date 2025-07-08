using PatientManager.Core.Application.Interfaces.Repositories;
using PatientManager.Core.Domain.Entities;
using PatientManager.Infrastructure.Persistence.Contexts;

namespace PatientManager.Infrastructure.Persistence.Repositories
{
    public class ConsultoryRepository : GenericRepository<Consultory>, IConsultoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ConsultoryRepository(ApplicationDbContext dbContext) : base(dbContext) => _dbContext = dbContext;
    }
}
