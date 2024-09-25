using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE_CodeTest.BL
{
    public class WalletService
    {
        private Dictionary<int, Money> _wallets = new Dictionary<int, Money>()
        {
            { 1, new Money("SEK", 100m) },
            { 2, new Money("EUR", 250m) }
        };

        public WalletService()
        {
        }

        public Task<bool> HasWallet(int playerId, string currency)
            => Task.FromResult(_wallets.ContainsKey(playerId) && _wallets[playerId].Currency == currency);

        public Task<Money> GetBalance(int playerId, string currency)
            => Task.FromResult(_wallets[playerId]);

        public Task Credit(int playerId, Money amount)
        {
            _wallets[playerId] += amount;
            return Task.FromResult(0);
        }

        public Task Debit(int playerId, Money amount)
        {
            if (_wallets[playerId] < amount)
                throw new InsufficientFundsException();

            _wallets[playerId] -= amount;

            return Task.FromResult(0);
        }
    }
}
