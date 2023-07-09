using EmployeeManagement.Model;

namespace EmployeeManagement.Repository.IRepository
{
    public interface IDesignationRepository : IRepository<Designation>
    {
        Task<Designation> UpdateAsync(Designation entity);
    }
}
