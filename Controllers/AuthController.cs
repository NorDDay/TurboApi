using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurboApi.Exceptions;
using TurboApi.Extensions;
using TurboApi.Models;
using TurboApi.Services;

namespace TurboApi.Controllers
{
	[Route("auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly DbLocalContext db;

		public AuthController(
			DbLocalContext context)
		{
			db = context;
		}
		
		[HttpPost("login")]
		public async Task<string> Login([FromBody] User user)
		{
			if (user == null)
			{
				throw new BadRequestException();
			}

			var currentUser = await db.Users.FirstOrDefaultAsync(p => p.Login == user.Login && p.Password == user.Password.CreateMD5()).ConfigureAwait(false);
			if (currentUser == null)
			{
				throw new NotAuthException();
			}
			return CreateAuthSid(currentUser);
		}

		[HttpPost("singup")]
		public async Task<string> Singup([FromBody] User user)
		{
			var existUser = await db.Users.FirstOrDefaultAsync(p => p.Login == user.Login).ConfigureAwait(false);

			if (existUser != null)
			{
				throw new BadRequestException();
			}

			user.Password = user.Password.CreateMD5();
			var currentUser = db.Users.Add(user);
			await db.SaveChangesAsync().ConfigureAwait(false);

			return CreateAuthSid(currentUser.Entity);
		}

		private static string CreateAuthSid(User currentUser)
		{
			return $"{currentUser.Id}:" + $"{currentUser.Login}:{currentUser.Password}".CreateMD5();
		}
	}
}
