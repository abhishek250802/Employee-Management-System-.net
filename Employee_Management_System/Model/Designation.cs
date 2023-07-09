using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Model
{
	public class Designation
	{
		[Key]
		[DatabaseGenerated (DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public string DesignationCode { get; set; }
		public string DesignationName { get; set;}
	}
}
