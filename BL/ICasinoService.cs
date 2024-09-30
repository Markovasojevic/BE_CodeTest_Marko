using BE_CodeTest.Models;
using System.Threading.Tasks;

namespace BE_CodeTest.BL
{
	public interface ICasinoService
	{
		Task<TransactionResponse> Wager(int playerId, string externalGameId, string externalTransactionId, long externalRoundId, Money amount);
		Task<TransactionResponse> Win(int playerId, string externalGameId, string externalTransactionId, long externalRoundId, Money amount);
	}
}
