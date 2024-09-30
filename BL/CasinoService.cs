using BE_CodeTest.ErrorHandling.Exceptions;
using BE_CodeTest.Models;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BE_CodeTest.BL
{
	public class CasinoService : ICasinoService
	{
		private readonly IWalletService _walletService;

		private static ConcurrentDictionary<string, TransactionResponse> Transactions = new();
		private static ConcurrentDictionary<long, bool> Rounds = new();

		private long _lastTransactionId = 0;

		public CasinoService(IWalletService walletService) => _walletService = walletService;

		public async Task<TransactionResponse> Wager(int playerId, string externalGameId, string externalTransactionId, long externalRoundId, Money amount)
		{
			var transactionId = ++_lastTransactionId;

			var playerBalances = await _walletService.GetBalance(playerId, amount.Currency);

			ValidateTransactionsAndRound(playerId, externalTransactionId, externalRoundId, transactionId, playerBalances);

			if (playerBalances < amount)
			{
				throw new InsufficientFundsException(transactionId.ToString(), playerId, playerBalances.Amount);
			}

			playerBalances = await _walletService.Debit(playerId, amount);

			var response = GetResponse(transactionId.ToString(), playerBalances.Amount);

			Transactions[externalTransactionId] = response;

			return response;
		}

		public async Task<TransactionResponse> Win(int playerId, string externalGameId, string externalTransactionId, long externalRoundId, Money amount)
		{
			var transactionId = ++_lastTransactionId;

			var playerBalances = await _walletService.GetBalance(playerId, amount.Currency);

			ValidateTransactionsAndRound(playerId, externalTransactionId, externalRoundId, transactionId, playerBalances);

			playerBalances = await _walletService.Credit(playerId, amount);
			Rounds[externalRoundId] = true; // Mark the round as closed

			var response = GetResponse(transactionId.ToString(), playerBalances.Amount);

			Transactions[externalTransactionId] = response;
			return response;
		}

		private static void ValidateTransactionsAndRound(int playerId, string externalTransactionId, long externalRoundId, long transactionId, Money playerBalances)
		{
			if (Transactions.ContainsKey(externalTransactionId))
			{
				throw new TransactionAlreadyProcessedException(transactionId.ToString(), playerId, playerBalances.Amount);
			}

			if (!Rounds.ContainsKey(externalRoundId))
			{
				Rounds.AddOrUpdate(externalRoundId, false, (key, value) => false);
			}
			else
			{
				var round = Rounds[externalRoundId];
				if (round)
				{
					throw new RoundEndedException(transactionId.ToString(), externalRoundId, playerBalances.Amount);
				}
			}
		}

		private static TransactionResponse GetResponse(string transactionId, decimal balance)
		{
			return new TransactionResponse
			{
				TransactionId = transactionId,
				Balance = balance,
				ErrorCode = "NoError",
				ErrorMessage = string.Empty
			};
		}
	}
}