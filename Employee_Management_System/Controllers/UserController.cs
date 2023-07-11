using AutoMapper;
using Azure;
using EmployeeManagement.Model;
using EmployeeManagement.Model.DTO;
using EmployeeManagement.Model.DTO.User;
using EmployeeManagement.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace EmployeeManagement.Controllers
{
  //  [Route("api/User")]
	    [Route("api/[Controller]")]

	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserRepository _dbUser;
		private readonly IMapper _mapper;
		protected ApiResponse _response;
		public UserController(IUserRepository dbUser, IMapper mapper)
		{
			_dbUser = dbUser;
			_mapper = mapper;
			_response = new();

		}
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
		{
			var loginResponse = await _dbUser.Login(model);
			if (loginResponse.user == null || string.IsNullOrEmpty(loginResponse.Token))
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("Username or Password is incorrect");
				return BadRequest(_response);
			}
			_response.StatusCode = HttpStatusCode.OK;
			_response.IsSuccess = true;
			_response.Result = loginResponse;
			return Ok(_response);


		}



		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] UserCreateDTO model)
		{
			bool ifUserNameUnique = _dbUser.IsUniqueUser(model.Name);
			if (!ifUserNameUnique)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("Username already Exist");
				return BadRequest(_response);

			}
			var user = await _dbUser.Register(model);
			if (user == null)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("Error While Registering");
				return BadRequest(_response);
			}
			_response.StatusCode = HttpStatusCode.OK;
			_response.IsSuccess = true;
			return Ok(_response);

		}
	

        [HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse>> GetUsers()
		{
			try
			{
				IEnumerable<User> users = await _dbUser.GetAllAsync();
				_response.Result = _mapper.Map<List<UserDTO>>(users);
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
		
		[HttpGet("{id:int}", Name = "GetUser")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse>> GetUser(int id)
		{
			try
			{
				if (id == 0)
				{
				
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);

				}
				
				var user = await _dbUser.GetAsync(u => u.Id == id);
				if (user == null)
				{
					
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);


				}
				
				_response.Result = _mapper.Map<UserDTO>(user);
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

		

		[HttpPut("{id:int}", Name = "UpdateUsers")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse>> UpdateUser(int id, [FromBody] UserUpdateDTO userUpdateDTO)
		{
			try
			{
				if (userUpdateDTO == null || id != userUpdateDTO.Id)
				{
					return BadRequest();
				}

				User model = _mapper.Map<User>(userUpdateDTO);



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

		[HttpDelete("{id:int}", Name = "DeleteUser")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse>> DeleteUsers(int id)
		{
			try
			{
				if (id == 0)
				{
					return BadRequest();
				}
				var user = await _dbUser.GetAsync(u => u.Id == id);
				if (user == null)
				{
					return NotFound();
				}
				
				await _dbUser.RemoveAsync(user);
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
