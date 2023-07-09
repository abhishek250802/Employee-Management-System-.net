using EmployeeManagement.Model;

namespace EmployeeManagement.Repository.IRepository
{
    public interface IEmployeeRepository : IRepository<Employees>
    {
        Task<Employees> UpdateAsync(Employees entity);
    }
}
