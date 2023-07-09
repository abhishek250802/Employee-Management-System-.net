using AutoMapper;
using EmployeeManagement.Model;
using EmployeeManagement.Model.DTO.Employee;
using EmployeeManagement.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace EmployeeManagement.Controllers
{
    [Route("api/Employee")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly IEmployeeRepository _dbEmployee;
		private readonly IDepartmentRepository _dbDepartment;
		private readonly IDesignationRepository _dbDesignation;
		private readonly IMapper _mapper;
		protected ApiResponse _response;
		public EmployeeController(IEmployeeRepository dbEmployee, IMapper mapper, IDepartmentRepository dbDepartment, IDesignationRepository dbDesignation)
		{
			_dbEmployee = dbEmployee;
			_mapper = mapper;
			_response = new();
			_dbDepartment = dbDepartment;
			_dbDesignation = dbDesignation;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse>> GetEmployees()
		{

			try
			{
				IEnumerable<Employees> employee = await _dbEmployee.GetAllAsync(includeProperties: "Designation,Department");
				_response.Result = _mapper.Map<List<EmployeeDTO>>(employee);
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.ToString() };
			}
			return _response;
		}



		[HttpGet("{id:int}", Name = "GetEmployee")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]

		public async Task<ActionResult<ApiResponse>> GetEmployee(int id)
		{
			try
			{
				if (id == 0)
				{

					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);

				}

				var employee = await _dbEmployee.GetAsync(u => u.Id == id, includeProperties: "Designation,Department");
				if (employee == null)
				{

					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);


				}

				_response.Result = _mapper.Map<EmployeeDTO>(employee);
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.ToString() };
			}
			return _response;
		}

		

		
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<ApiResponse>> CreateEmployee([FromBody] EmployeeCreateDTO  employeeCreateDTO)
		{

			try
			{
				if (await _dbEmployee.GetAsync(u => u.Name.ToLower() == employeeCreateDTO.Name.ToLower()) != null)
				{
					ModelState.AddModelError("ErrorMessages", "Employee already exists!");
					return BadRequest(ModelState);
				}
				if (await _dbDepartment.GetAsync(u => u.Id == employeeCreateDTO.DepartmentId) == null)
				{
					ModelState.AddModelError("ErrorMessages", "DepartmentID is invalid");
					return BadRequest(ModelState);
				}
				if (await _dbDesignation.GetAsync(u => u.Id == employeeCreateDTO.DesignationId) == null)
				{
					ModelState.AddModelError("ErrorMessages", "DesignationID is invalid");
					return BadRequest(ModelState);
				}
				if (employeeCreateDTO == null)
				{
					return BadRequest();
				}

				Employees employee = _mapper.Map<Employees>(employeeCreateDTO);

				await _dbEmployee.CreateAsync(employee);

				_response.Result = _mapper.Map<EmployeeDTO>(employee);
				_response.StatusCode = HttpStatusCode.OK;
				return CreatedAtRoute("GetEmployee", new { id = employee.Id }, _response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.ToString() };
			}
			return _response;
		}

		
		[HttpDelete("{id:int}", Name = "DeleteEmployee")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse>> DeleteEmployee(int id)
		{
			try
			{
				if (id == 0)
				{
					return BadRequest();
				}

				var employee = await _dbEmployee.GetAsync(u => u.Id == id);
				if (employee == null)
				{
					return NotFound();
				}

				await _dbEmployee.RemoveAsync(employee);
				_response.StatusCode = HttpStatusCode.NoContent;
				_response.IsSuccess = true;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.ToString() };
			}
			return _response;

		}


		
		[HttpPut("{id:int}", Name = "UpdateEmployee")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse>> UpdateEmployee(int id, [FromBody] EmployeeUpdateDTO employeeUpdateDTO)
		{
			try
			{
				if (employeeUpdateDTO == null || id != employeeUpdateDTO.Id)
				{
					return BadRequest();
				}
				if (await _dbDepartment.GetAsync(u => u.Id == employeeUpdateDTO.DepartmentId) == null)
				{
					ModelState.AddModelError("ErrorMessages", "DeprtmentID is invalid");
					return BadRequest(ModelState);
				}
				if (await _dbDesignation.GetAsync(u => u.Id == employeeUpdateDTO.DesignationId) == null)
				{
					ModelState.AddModelError("ErrorMessages", "DesignationId is invalid");
					return BadRequest(ModelState);
				}

				Employees model = _mapper.Map<Employees>(employeeUpdateDTO);


				await _dbEmployee.UpdateAsync(model);
				_response.StatusCode = HttpStatusCode.NoContent;
				_response.IsSuccess = true;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.ToString() };
			}
			return _response;

		}

	}
}
