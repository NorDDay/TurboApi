using TurboClient.Models;

namespace TurboApi.Services
{
	public interface IUserContext
	{
		User CurrentUser { get; }
	}
}
