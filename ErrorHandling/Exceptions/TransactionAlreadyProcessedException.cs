using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BE_CodeTest.ErrorHandling.Exceptions
{
	[ExcludeFromCodeCoverage]
	public class TransactionAlreadyProcessedException : Exception
    {
		private static string CreateExceptionMessage(string transactionId, int player, decimal balance)
		{
			return JsonConvert.SerializeObject(
				new
				{
					TransactionId = transactionId,
					Balance = balance,
					ErrorCode = "TransactionAlreadyProcessed",
					ErrorMessage = $"Transaction {transactionId} already processed."
				});
		}
		public TransactionAlreadyProcessedException(string transactionId, int player, decimal balance) : base(CreateExceptionMessage(transactionId, player, balance)) { }
	}
}