using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Model.DTO.Employee
{
    public class EmployeeCreateDTO
    {

        [Required]
        public int DesignationId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Qualification { get; set; }

    }
}
