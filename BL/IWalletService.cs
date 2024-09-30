using System.Threading.Tasks;

namespace BE_CodeTest.BL
{
	public interface IWalletService
	{
		Task<bool> HasWallet(int playerId, string currency);
		Task<Money> GetBalance(int playerId, string currency);
		Task<Money> Credit(int playerId, Money amount);
		Task<Money> Debit(int playerId, Money amount);
	}
}
