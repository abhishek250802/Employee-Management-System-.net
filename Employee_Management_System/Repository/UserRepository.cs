using AutoMapper;
using EmployeeManagement.Data;
using EmployeeManagement.Model;
using EmployeeManagement.Model.DTO;
using EmployeeManagement.Model.DTO.User;
using EmployeeManagement.Repository.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagement.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
	{
		private readonly ApplicationDbContext _db;
		private string SecretKey;
		private readonly IMapper _mapper;
		public UserRepository(ApplicationDbContext db, IConfiguration iconfiguration, IMapper mapper) : base(db)
		{
			_db = db;
			SecretKey = iconfiguration.GetValue<string>("ApiSettings:Secret");
			_mapper = mapper;
		}

		public bool IsUniqueUser(string userName)
		{
			var user = _db.user.FirstOrDefault(x => x.Name == userName);
			if (user == null)
			{
				return true;
			}
			return false;
		}

		public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
		{
			var user = _db.user.FirstOrDefault(u => u.Name.ToLower() == loginRequestDTO.userName.ToLower()
			&& u.Password == loginRequestDTO.Password);
			if (user == null)
			{
				return new LoginResponseDTO()
				{
					Token = "",
					user = null
				};
			}
			
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(SecretKey);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Name.ToString()),
					new Claim(ClaimTypes.Role, user.Role)
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
			{
				Token = tokenHandler.WriteToken(token),
				
				user = _mapper.Map<UserDTO>(user),

			};
			return loginResponseDTO;


		}

		public async Task<User> Register(UserCreateDTO userCreateDTO)
		{
			User user = new()
			{
				Name = userCreateDTO.Name,
				Password = userCreateDTO.Password,
				Email = userCreateDTO.Email,
				Role = userCreateDTO.Role
			};
			_db.user.Add(user);
			await _db.SaveChangesAsync();
			user.Password = "";
			return user;
		}



		public async Task<User> UpdateAsync(User entity)
		{
			
			_db.user.Update(entity);
			await _db.SaveChangesAsync();
			return entity;

		}
	}
}
