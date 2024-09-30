using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BE_CodeTest.ErrorHandling.Exceptions
{
	[ExcludeFromCodeCoverage]
	public class InsufficientFundsException : Exception
	{
		private static string CreateExceptionMessage(string transactionId, int player, decimal balance)
		{
			return JsonConvert.SerializeObject(
				new
				{
					TransactionId = transactionId,
					Balance = balance,
					ErrorCode = "InsufficientFunds",
					ErrorMessage = $"Player {player} does not have enough funds."
				});
		}

		private static string CreateExceptionMessage(int player, decimal balance)
		{
			return JsonConvert.SerializeObject(
				new
				{
					Balance = balance,
					ErrorCode = "InsufficientFunds",
					ErrorMessage = $"Player {player} does not have enough funds."
				});
		}
		public InsufficientFundsException(string transactionId, int player, decimal balance) : base(CreateExceptionMessage(transactionId, player, balance)) { }
		public InsufficientFundsException(int player, decimal balance) : base(CreateExceptionMessage(player, balance)) { }
	}
}