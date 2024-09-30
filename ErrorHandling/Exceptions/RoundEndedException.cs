using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BE_CodeTest.ErrorHandling.Exceptions
{
	[ExcludeFromCodeCoverage]
	public class RoundEndedException : Exception 
	{
		private static string CreateExceptionMessage(string transactionId, long roundId, decimal balance)
		{
			return JsonConvert.SerializeObject(
				new
				{
					TransactionId = transactionId,
					Balance = balance,
					ErrorCode = "RoundDoesNotExist",
					ErrorMessage = $"The Round {roundId.ToString()} is closed."
				});
		}
		public RoundEndedException(string transactionId, long roundId, decimal balance) : base(CreateExceptionMessage(transactionId, roundId, balance)) { }
	}
}