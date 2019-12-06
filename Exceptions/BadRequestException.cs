using System.Net;

namespace TurboApi.Exceptions
{
	public class BadRequestException : BaseException
	{
		public BadRequestException() : base(HttpStatusCode.BadRequest)
		{
		}
	}
}
