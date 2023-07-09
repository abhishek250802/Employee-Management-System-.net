using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Model.DTO.User
{
    public class UserUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }

    }
}
