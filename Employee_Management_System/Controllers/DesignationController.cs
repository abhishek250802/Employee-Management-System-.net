using AutoMapper;
using EmployeeManagement.Model;
using EmployeeManagement.Model.DTO.Designation;
using EmployeeManagement.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace EmployeeManagement.Controllers
{
    [Route("api/Designation")]
	[ApiController]
	public class DesignationController : Controller
	{
		private readonly IDesignationRepository _dbUser;
		private readonly IMapper _mapper;
		protected ApiResponse _response;
		public DesignationController(IDesignationRepository dbUser, IMapper mapper)
		{
			_dbUser = dbUser;
			_mapper = mapper;
			_response = new();

		}
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse>> GetDesignations()
		{
			try
			{
				IEnumerable<Designation> designations = await _dbUser.GetAllAsync();
				_response.Result = _mapper.Map<List<DesignationDTO>>(designations);
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

		[HttpGet("{id:int}", Name = "GetDesignation")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetDesignation(int id)
		{
			try
			{
				if (id == 0)
				{

					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);

				}

				var designation = await _dbUser.GetAsync(u => u.Id == id);
				if (designation == null)
				{

					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);


				}

				_response.Result = _mapper.Map<DesignationDTO>(designation);
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
		public async Task<ActionResult<ApiResponse>> CreateDesignations([FromBody] DesignationCreateDTO designationCreateDTO)
		{
			try
			{
				if (await _dbUser.GetAsync(u => u.DesignationName.ToLower() == designationCreateDTO.DesignationName.ToLower()) != null)
				{
					ModelState.AddModelError("ErrorMessages", "Designation already exists!");
					return BadRequest(ModelState);
				}
				if (designationCreateDTO == null)
				{
					return BadRequest();
				}

				Designation designation = _mapper.Map<Designation>(designationCreateDTO);
				await _dbUser.CreateAsync(designation);
				_response.Result = _mapper.Map<DesignationDTO>(designation);
				_response.StatusCode = HttpStatusCode.OK;
				return CreatedAtRoute("GetDesignation", new { id = designation.Id }, _response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string> { ex.ToString() };
			}
			return _response;
		}

		[HttpPut("{id:int}", Name = "UpdateDesignation")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse>> UpdateDesignation(int id, [FromBody] DesignationUpdateDTO designationUpdateDTO)
		{
			try
			{
				if (designationUpdateDTO == null || id != designationUpdateDTO.Id)
				{
					return BadRequest();
				}

				Designation model = _mapper.Map<Designation>(designationUpdateDTO);



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

		[HttpDelete("{id:int}", Name = "DeleteDesignation")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse>> DeleteDesignation(int id)
		{
			try
			{
				if (id == 0)
				{
					return BadRequest();
				}
				var designation = await _dbUser.GetAsync(u => u.Id == id);
				if (designation == null)
				{
					return NotFound();
				}

				await _dbUser.RemoveAsync(designation);
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
