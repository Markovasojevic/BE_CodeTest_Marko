using System.Diagnostics.CodeAnalysis;

namespace BE_CodeTest.Models
{
	[ExcludeFromCodeCoverage]
	public record BetRequest(
		 int PlayerId,
		 string Game,
		 string TransactionId,
		 string Currency,
		 decimal Amount,
		 long RoundId
	);
}