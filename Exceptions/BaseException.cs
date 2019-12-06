using System;
using System.Net;

namespace TurboApi.Exceptions
{
	public class BaseException : Exception
	{
		public BaseException(HttpStatusCode httpStatusCode)
		{
			StatusCode = httpStatusCode;
		}

		public HttpStatusCode StatusCode;
	}
}
