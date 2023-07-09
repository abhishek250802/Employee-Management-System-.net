using EmployeeManagement.Model;

namespace EmployeeManagement.Repository.IRepository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Department> UpdateAsync(Department entity);
    }
}
