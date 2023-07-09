using EmployeeManagement.Model;
using EmployeeManagement.Model.DTO;
using EmployeeManagement.Model.DTO.User;

namespace EmployeeManagement.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        bool IsUniqueUser(string userName);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<User> Register(UserCreateDTO userCreateDTO);
        Task<User> UpdateAsync(User entity);

    }
}
