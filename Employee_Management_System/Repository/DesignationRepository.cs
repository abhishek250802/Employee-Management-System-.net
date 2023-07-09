using EmployeeManagement.Data;
using EmployeeManagement.Model;
using EmployeeManagement.Repository.IRepository;

namespace EmployeeManagement.Repository
{
    public class DesignationRepository : Repository<Designation>, IDesignationRepository
	{
		private readonly ApplicationDbContext _db;
		public DesignationRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}
		public async Task<Designation> UpdateAsync(Designation entity)
		{
			
			_db.designation.Update(entity);
			await _db.SaveChangesAsync();
			return entity;

		}
	}
}
