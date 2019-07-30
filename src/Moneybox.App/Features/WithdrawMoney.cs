using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class WithdrawMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public WithdrawMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, decimal amount)
        {
            var user = this.accountRepository.GetAccountById(fromAccountId);

            var userBalance = user.Balance - amount;

            if (!user.ValidateBalance(userBalance))
            {
                this.notificationService.NotifyFundsLow(user.User.Email);
            }

            user.Balance -= amount;
            user.Withdrawn -= amount;

            this.accountRepository.Update(user);
        }
    }
}
