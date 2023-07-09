using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Model.DTO.Department;
using EmployeeManagement.Model.DTO.Designation;

namespace EmployeeManagement.Model.DTO.Employee
{
    public class EmployeeDTO
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public int DesignationId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public DesignationDTO designation { get; set; }
        public DepartmentDTO department { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Qualification { get; set; }

    }
}
