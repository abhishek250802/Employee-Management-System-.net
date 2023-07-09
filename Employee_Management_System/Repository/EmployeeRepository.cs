using EmployeeManagement.Data;
using EmployeeManagement.Model;
using EmployeeManagement.Repository.IRepository;

namespace EmployeeManagement.Repository
{
    public class EmployeeRepository : Repository<Employees>, IEmployeeRepository
	{
		private readonly ApplicationDbContext _db;
		public EmployeeRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}



		public async Task<Employees> UpdateAsync(Employees entity)
		{
			
			_db.employee.Update(entity);
			await _db.SaveChangesAsync();
			return entity;

		}
	}
}
