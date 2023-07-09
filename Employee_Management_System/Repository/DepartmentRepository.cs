using EmployeeManagement.Data;
using EmployeeManagement.Model;
using EmployeeManagement.Repository.IRepository;

namespace EmployeeManagement.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
	{
		private readonly ApplicationDbContext _db;
		public DepartmentRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}
		public async Task<Department> UpdateAsync(Department entity)
		{

			_db.department.Update(entity);
			await _db.SaveChangesAsync();
			return entity;

		}

	}
}
