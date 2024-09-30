using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BE_CodeTest.ErrorHandling.Exceptions
{
	[ExcludeFromCodeCoverage]
	public class MissingRoundException : Exception
	{
		private static string CreateExceptionMessage(string transactionId, int roundId, decimal balance)
		{
			return JsonConvert.SerializeObject(
				new
				{
					TransactionId = transactionId,
					Balance = balance,
					ErrorCode = "RoundDoesNotExist",
					ErrorMessage = $"The Round {transactionId} Doesn't exist."
				});
		}
		public MissingRoundException(string transactionId, int roundId, decimal balance) : base(CreateExceptionMessage(transactionId, roundId, balance)) { }
	}
}