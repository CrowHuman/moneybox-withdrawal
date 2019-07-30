using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class TransferMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public TransferMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);
            var to = this.accountRepository.GetAccountById(toAccountId);

            var fromBalance = from.Balance - amount;

            if (!from.ValidateBalance(fromBalance))
            {
                this.notificationService.NotifyFundsLow(from.User.Email);
            }

            var paidIn = to.PaidIn + amount;

            if (!to.ValidatePayLimit(paidIn))
            {
                this.notificationService.NotifyApproachingPayInLimit(to.User.Email);
            }

            from.Balance -= amount;
            from.Withdrawn -= amount;

            to.Balance += amount;
            to.PaidIn += amount;

            this.accountRepository.Update(from);
            this.accountRepository.Update(to);
        }
    }
}
