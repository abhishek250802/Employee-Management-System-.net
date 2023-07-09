using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Model
{
	public class Employees
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
		[ForeignKey("Designation")]
		public int DesignationId { get; set; }
		public Designation Designation { get; set; }
		[ForeignKey("Department")]
		public int DepartmentId { get; set; }
		public Department Department { get; set; }
		[Required]
		public string Name { get; set; }

		public string Email { get; set; }
		public string Phone { get; set; }
		public string Qualification { get; set; }


	}
}
