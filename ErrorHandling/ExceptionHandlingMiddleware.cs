using BE_CodeTest.ErrorHandling.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace BE_CodeTest.ErrorHandling
{
	[ExcludeFromCodeCoverage]
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var code = HttpStatusCode.InternalServerError; // 500 if unexpected
			var result = string.Empty;

			switch (exception)
			{
				case InsufficientFundsException e:
					code = HttpStatusCode.BadRequest;
					result = e.Message;
					break;
				case RoundEndedException e:
					code = HttpStatusCode.BadRequest;
					result = e.Message;
					break;
				case TransactionAlreadyProcessedException e:
					code = HttpStatusCode.BadRequest;
					result = e.Message;
					break;
				case InvalidCurrencyException e:
					code = HttpStatusCode.BadRequest;
					result = e.Message;
					break;
				default:
					result = JsonConvert.SerializeObject(new { ErrorCode = "UnknownError", ErrorMessage = exception.Message });
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)code;
			return context.Response.WriteAsync(result);
		}
	}
}
