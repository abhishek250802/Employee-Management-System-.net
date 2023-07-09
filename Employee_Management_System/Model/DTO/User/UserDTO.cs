using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Model.DTO.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
