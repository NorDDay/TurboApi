using System;
using System.Net;
using Autofac.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace TurboApi.Exceptions
{
	public class ExceptionHandler : IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			var exception = context.Exception;
			if (exception is DependencyResolutionException)
			{
				exception = FindInnerException<BaseException>(exception);
			}
			var customException = exception as BaseException;
			var statusCode = HttpStatusCode.InternalServerError;

			if (null != customException)
			{
				statusCode = customException.StatusCode;
			}
			var response = context.HttpContext.Response;
			response.ContentType = "application/json";
			response.StatusCode = (int)statusCode;
			context.ExceptionHandled = true;
			response.WriteAsync(JsonConvert.SerializeObject(new BaseException(statusCode))).Wait();
		}

		private static TException FindInnerException<TException>(Exception e)
			where TException : Exception
		{
			do
			{
				if (e is TException exception)
					return exception;
				if (e == e?.InnerException)
					return null;
				e = e.InnerException;
			}
			while (e != null);

			return null;
		}
	}
}
