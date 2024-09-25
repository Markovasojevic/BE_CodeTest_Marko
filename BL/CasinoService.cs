using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE_CodeTest.BL
{
    public class CasinoService
    {
        private WalletService _walletService;

        private Dictionary<string, long> _processedTransactions = new Dictionary<string, long>();
        private Dictionary<long, (Money TotalWager, Money TotalWin, bool Ended)> _rounds = new Dictionary<long, (Money TotalBet, Money TotalWin, bool ended)>();

        private long _lastTransactionId = 0;

        public CasinoService(WalletService walletService)
            => _walletService = walletService;

        public async Task<long> Wager(int playerId, string externalGameId, string externalTransactionId, long externalRoundId, Money amount)
        {
            if (_processedTransactions.ContainsKey(externalTransactionId))
            {
                throw new TransactionAlreadyProcessedException(_processedTransactions[externalTransactionId]);
            }

            if (!_rounds.ContainsKey(externalRoundId))
            {
                _rounds.Add(externalRoundId, (amount, new Money(amount.Currency), false));
            }
            else
            {
                var round = _rounds[externalRoundId];
                if (round.Ended)
                {
                    throw new RoundEndedException();
                }

                round.TotalWager += amount;
            }

            var transactionId = ++_lastTransactionId;

            if (await _walletService.GetBalance(playerId, amount.Currency) < amount)
            {
                throw new InsufficientFundsException();
            }

            await _walletService.Debit(playerId, amount);

            return transactionId;
        }

        public async Task<long> Win(int playerId, string externalGameId, string externalTransactionId, long externalRoundId, Money amount)
        {
            if (_processedTransactions.ContainsKey(externalTransactionId))
            {
                throw new TransactionAlreadyProcessedException(_processedTransactions[externalTransactionId]);
            }

            if (!_rounds.TryGetValue(externalRoundId, out var round))
            {
                throw new MissingRoundException();
            }

            if (round.Ended)
            {
                throw new RoundEndedException();
            }

            round.TotalWin += amount;

            var transactionId = ++_lastTransactionId;

            await _walletService.Credit(playerId, amount);

            return transactionId;
        }

        public Task EndRound(long externalRoundId)
        {
            if (!_rounds.TryGetValue(externalRoundId, out var round))
            {
                throw new MissingRoundException();
            }

            round.Ended = true;

            return Task.FromResult(0);
        }
    }

    public class TransactionAlreadyProcessedException : Exception
    {
        public long TransactionId { get; }

        public TransactionAlreadyProcessedException(long transactionId)
            => TransactionId = transactionId;
    }

    public class MissingRoundException : Exception { }

    public class RoundEndedException : Exception { }

    public class InsufficientFundsException : Exception { }
}