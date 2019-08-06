using System;

namespace Moneybox.App
{
    public class Account
    {
        public const decimal PayInLimit = 4000m;

        public Guid Id { get; set; }

        public User User { get; set; }

        public decimal Balance { get; set; }

        public decimal Withdrawn { get; set; }

        public decimal PaidIn { get; set; }

        public bool Withdraw(decimal amount)
        {
            if(Balance - amount < 0)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            Balance -= amount;
            Withdrawn -= amount;

            return Balance > 500m;
        }

        public bool PayIn(decimal amount)
        {

            if(PaidIn + amount > PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            Balance += amount;
            PaidIn += amount;

            return PayInLimit - PaidIn > 500m;
        }


    }
}
