using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TurboApi.Exceptions;
using TurboApi.Extensions;
using TurboApi.Models;

namespace TurboApi.Services
{
	public class UserContext : IUserContext
	{
		public User CurrentUser => currentUser;

		private readonly HttpRequest httpContext;
		private readonly DbLocalContext db;
		private User currentUser;

		public UserContext(HttpRequest httpContext, DbLocalContext db)
		{
			this.httpContext = httpContext;
			this.db = db;
			ValidateAuth().GetAwaiter().GetResult();
		}

		private async Task ValidateAuth()
		{
			var result = httpContext.Cookies.TryGetValue("auth.sid", out var value);
			if (!result)
			{
				throw new NotAuthException();
			}

			var splittedAuthSid = value.Split(":");
			if (splittedAuthSid.Length != 2)
			{
				throw new NotAuthException();
			}
			var currerntUser = await db.Users.FindAsync(int.Parse(splittedAuthSid[0])).ConfigureAwait(false);
			if ($"{currerntUser.Login}:{currerntUser.Password}".CreateMD5() != splittedAuthSid[1])
			{
				throw new NotAuthException();
			}

			currentUser = currerntUser;
		}
	}
}
