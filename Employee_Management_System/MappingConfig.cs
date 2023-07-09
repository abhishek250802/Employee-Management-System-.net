using AutoMapper;
using EmployeeManagement.Model;
using EmployeeManagement.Model.DTO.Department;
using EmployeeManagement.Model.DTO.Designation;
using EmployeeManagement.Model.DTO.Employee;
using EmployeeManagement.Model.DTO.User;

namespace EmployeeManagement
{
    public class MappingConfig :Profile
	{
		public MappingConfig()
		{
			CreateMap<User, UserDTO>().ReverseMap();
			CreateMap<User, UserCreateDTO>().ReverseMap();
			CreateMap<User, UserUpdateDTO>().ReverseMap();
			CreateMap<Designation, DesignationDTO>().ReverseMap();
			CreateMap<Designation, DesignationCreateDTO>().ReverseMap();
			CreateMap<Designation, DesignationUpdateDTO>().ReverseMap();
			CreateMap<Department, DepartmentDTO>().ReverseMap();
			CreateMap<Department, DepartmentCreateDTO>().ReverseMap();
			CreateMap<Department, DepartmentUpdateDTO>().ReverseMap();
			CreateMap<Employees, EmployeeDTO>().ReverseMap();
			CreateMap<Employees, EmployeeCreateDTO>().ReverseMap();
			CreateMap<Employees, EmployeeUpdateDTO>().ReverseMap();

		}	
	}
}
