using AutoMapper;
using EmployeeManagement.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using EmployeeManagement.Model.DTO.Department;
using EmployeeManagement.Repository.IRepository;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Controllers
{
    [Route("api/Department")]
	[ApiController]
	public class DepartmentController : Controller
	{
		private readonly IDepartmentRepository _dbUser;
		private readonly IMapper _mapper;
		protected ApiResponse _response;
		public DepartmentController(IDepartmentRepository dbUser, IMapper mapper)
		{
			_dbUser = dbUser;
			_mapper = mapper;
			_response = new();

		}
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse>> GetDepartments()
		{
			try
			{
				IEnumerable<Department> department = await _dbUser.GetAllAsync();
				_response.Result = _mapper.Map<List<DepartmentDTO>>(department);
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);

			}
			catch (Exception e)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { e.ToString() };
			}
			return _response;

		}

		[HttpGet("{id:int}", Name = "GetDepartment")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse>> GetDepartment(int id)
		{
			try
			{
				if (id == 0)
				{

					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);

				}

				var department = await _dbUser.GetAsync(u => u.Id == id);
				if (department == null)
				{

					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);


				}

				_response.Result = _mapper.Map<DepartmentDTO>(department);
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
		public async Task<ActionResult<ApiResponse>> CreateDepartments([FromBody] DepartmentCreateDTO departmentCreateDTO)
		{
			try
			{
				if (await _dbUser.GetAsync(u => u.DepartmentName.ToLower() == departmentCreateDTO.DepartmentName.ToLower()) != null)
				{
					ModelState.AddModelError("ErrorMessages", "Department already exists!");
					return BadRequest(ModelState);
				}
				if (departmentCreateDTO == null)
				{
					return BadRequest();
				}

				Department department = _mapper.Map<Department>(departmentCreateDTO);
				await _dbUser.CreateAsync(department);
				_response.Result = _mapper.Map<DepartmentDTO>(department);
				_response.StatusCode = HttpStatusCode.OK;
				return CreatedAtRoute("GetDepartment", new { id = department.Id }, _response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.ToString() };
			}
			return _response;
		}

		[HttpPut("{id:int}", Name = "UpdateDepartment")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse>> UpdateDepartment(int id, [FromBody] DepartmentUpdateDTO departmentUpdateDTO)
		{
			try
			{
				if (departmentUpdateDTO == null || id != departmentUpdateDTO.Id)
				{
					return BadRequest();
				}

				Department model = _mapper.Map<Department>(departmentUpdateDTO);



				await _dbUser.UpdateAsync(model);
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

		[HttpDelete("{id:int}", Name = "DeleteDepartment")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse>> DeleteDepartment(int id)
		{
			try
			{
				if (id == 0)
				{
					return BadRequest();
				}
				var department = await _dbUser.GetAsync(u => u.Id == id);
				if (department == null)
				{
					return NotFound();
				}

				await _dbUser.RemoveAsync(department);
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
