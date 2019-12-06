using System.Net;

namespace TurboApi.Exceptions
{
	public class NotAuthException : BaseException
	{
		public NotAuthException() : base(HttpStatusCode.Unauthorized)
		{
		}
	}
}
