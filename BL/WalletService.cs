using BE_CodeTest.ErrorHandling.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE_CodeTest.BL
{
	public class WalletService : IWalletService
	{
		private static readonly Dictionary<int, Money> _wallets = new()
		{
			{ 1, new Money("SEK", 100m) },
			{ 2, new Money("EUR", 250m) }
		};

		public WalletService() { }

		public Task<bool> HasWallet(int playerId, string currency)
			=> Task.FromResult(_wallets.ContainsKey(playerId) && _wallets[playerId].Currency == currency);

		public Task<Money> GetBalance(int playerId, string currency)
			=> Task.FromResult(_wallets[playerId]);

		public Task<Money> Credit(int playerId, Money amount)
		{
			var playerBalance = _wallets[playerId] += amount;
			return Task.FromResult(playerBalance);
		}

		public Task<Money> Debit(int playerId, Money balance)
		{
			if (_wallets[playerId] < balance) 
			throw new InsufficientFundsException(playerId, balance.Amount);

			var playerBalance = _wallets[playerId] -= balance;

			return Task.FromResult(playerBalance);
		}
	}
}
