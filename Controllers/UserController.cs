using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TurboApi.Models;
using TurboApi.Services;

namespace TurboApi.Controllers
{
	[Route("user")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserContext userContext;

		public UserController(
			IUserContext userContext)
		{
			this.userContext = userContext;
		}

		[HttpGet("")]
		public async Task<User> User()
		{
			return userContext.CurrentUser;
		}
	}
}
