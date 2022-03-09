using Player.Sharp.Client;
using Player.Sharp.Data;
using Player.Sharp.Consumers;
using Player.Sharp.Core;

namespace Player.Sharp.Services
{
    public class TransactionService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(
            ILogger<TransactionService> logger,
            ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }

        public bool IsMyTransactionId(string transactionId)
        {
            return _transactionRepository.ExistsById(transactionId);
        }

        public void SaveTransactionId(string transactionId)
        {
            var transaction = new Transaction(transactionId);
            _transactionRepository.Save(transaction);
        }

        public void ClearTransactions()
        {
            _transactionRepository.Clear();
        }
    }
}
