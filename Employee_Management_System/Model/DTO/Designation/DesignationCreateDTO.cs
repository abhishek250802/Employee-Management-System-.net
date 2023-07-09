using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Model.DTO.Designation
{
    public class DesignationCreateDTO
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string DesignationCode { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Only Alphabets  allowed.")]
        public string DesignationName { get; set; }
    }
}
