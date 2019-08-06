using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moneybox.App;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using System;

namespace MoneyBoxTests
{
    [TestClass]
    public class MoneyBoxWithdrawMoneyTests
    {
        private IAccountRepository accountRepository;
        private INotificationService accountNotificationService;
        private WithdrawMoney withdrawMoney;
        private Account fromAccount;

        [TestInitialize]
        public void TestInit()
        {
            fromAccount = new Account
            {
                Id = System.Guid.NewGuid(),
                User = new User { Name = "Chris", Id = System.Guid.NewGuid() },
                Balance = 2000m,
                Withdrawn = 0m,
                PaidIn = 2000m
            };
            accountRepository = new AccountRepository();
            accountRepository.Update(fromAccount);

            accountNotificationService = new AccountNotificationService();

            withdrawMoney = new WithdrawMoney(accountRepository, accountNotificationService);

        }

        [TestMethod]
        public void Execute_SmallAmount_TransferSuccessful()
        {
            withdrawMoney.Execute(fromAccount.Id, 500m);

            Assert.IsTrue(accountRepository.GetAccountById(fromAccount.Id).Balance == 1500m);
            Assert.IsTrue(accountRepository.GetAccountById(fromAccount.Id).Withdrawn == -500m);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
            "Insufficient funds to make transfer")]
        public void Execute_FromAccountCantAfford_TransferUnsuccessful()
        {
            withdrawMoney.Execute(fromAccount.Id, 5000m);

            Assert.IsTrue(accountRepository.GetAccountById(fromAccount.Id).Balance == 2000m);
            Assert.IsTrue(accountRepository.GetAccountById(fromAccount.Id).Withdrawn == 0m);

        }
    }
}
