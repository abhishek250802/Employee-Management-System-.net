using EmployeeManagement.Model.DTO.User;

namespace EmployeeManagement.Model.DTO
{
    public class LoginResponseDTO
	{
		public UserDTO user { get; set; }
		public string Token { get; set; }
	}
}
