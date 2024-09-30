using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BE_CodeTest.ErrorHandling.Exceptions
{
	[ExcludeFromCodeCoverage]
	public class InvalidCurrencyException : Exception
	{
		private static string CreateExceptionMessage(string currency)
		{
			return JsonConvert.SerializeObject(
				new
				{
					ErrorCode = "InvalidCurrency",
					ErrorMessage = $"{currency} not supported"
				});
		}
		public InvalidCurrencyException(string currency) : base(CreateExceptionMessage(currency)) { }
	}
}