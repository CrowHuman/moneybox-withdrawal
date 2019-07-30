using System;
using Moneybox.App.Domain.Services;

namespace Moneybox.App
{
    public class Account
    {
        private const decimal payInLimit = 4000m;
        private const decimal insufficientFunds = 0m;
        private const decimal lowFundsAmount = 500m;

        public Guid Id { get; set; }

        public User User { get; set; }

        public decimal Balance { get; set; }

        public decimal Withdrawn { get; set; }

        public decimal PaidIn { get; set; }

        public bool ValidateBalance(decimal balance)
        {
            if (balance < insufficientFunds)
                throw new InvalidOperationException("Insufficient funds to make request");

            if (balance < lowFundsAmount)
                return false;

            return true;
        }

        public bool ValidatePayLimit(decimal paidInAmount)
        {
            if (paidInAmount > payInLimit)
                throw new InvalidOperationException("Account pay in limit reached");

            if (payInLimit - paidInAmount < lowFundsAmount)
                return false;

            return true;
        }
    }
}
