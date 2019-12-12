using System.Threading.Tasks;
using TurboClient.Models;

namespace TurboClient
{
	public interface ITurboClient
	{
		Task<string> Login(User user);
	}
}
