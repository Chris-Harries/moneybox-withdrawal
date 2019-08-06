using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moneybox.App;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using System;

namespace MoneyBoxTests
{
    [TestClass]
    public class MoneyMoxTransferMoneyTests
    {
        private IAccountRepository accountRepository;
        private INotificationService accountNotificationService;
        private TransferMoney transferMoney;
        private Account toAccount;
        private Account fromAccount;

        [TestInitialize]
        public void TestInit()
        {
            fromAccount = new Account {
                Id = System.Guid.NewGuid(),
                User = new User { Name = "Chris", Id = System.Guid.NewGuid() },
                Balance = 2000m,
                Withdrawn = 0m,
                PaidIn = 2000m
            };
            toAccount = new Account
            {
                Id = System.Guid.NewGuid(),
                User = new User { Name = "Jen", Id = System.Guid.NewGuid() },
                Balance = 3000m,
                Withdrawn = 0m,
                PaidIn = 3000m
            };
            accountRepository = new AccountRepository();
            accountRepository.Update(fromAccount);
            accountRepository.Update(toAccount);

            accountNotificationService = new AccountNotificationService();

            transferMoney = new TransferMoney(accountRepository, accountNotificationService);

        }

        [TestMethod]
        public void Execute_SmallAmount_TransferSuccessful()
        {
            transferMoney.Execute(fromAccount.Id, toAccount.Id, 500m);

            Assert.IsTrue(accountRepository.GetAccountById(fromAccount.Id).Balance == 1500m);
            Assert.IsTrue(accountRepository.GetAccountById(fromAccount.Id).Withdrawn == -500m);

            Assert.IsTrue(accountRepository.GetAccountById(toAccount.Id).Balance == 3500m);
            Assert.IsTrue(accountRepository.GetAccountById(toAccount.Id).PaidIn == 3500m);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
            "Insufficient funds to make transfer")]
        public void Execute_FromAccountCantAfford_TransferUnsuccessful()
        {
            transferMoney.Execute(fromAccount.Id, toAccount.Id, 5000m);

            Assert.IsTrue(accountRepository.GetAccountById(fromAccount.Id).Balance == 2000m);
            Assert.IsTrue(accountRepository.GetAccountById(fromAccount.Id).Withdrawn == 0m);

            Assert.IsTrue(accountRepository.GetAccountById(toAccount.Id).Balance == 3000m);
            Assert.IsTrue(accountRepository.GetAccountById(toAccount.Id).PaidIn == 3000m);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
            "Account pay in limit reached")]
        public void Execute_ExceedToAccountPayInLimit_TransferUnsuccessful()
        {
            transferMoney.Execute(fromAccount.Id, toAccount.Id, 1500m);

            Assert.IsTrue(accountRepository.GetAccountById(fromAccount.Id).Balance == 2000m);
            Assert.IsTrue(accountRepository.GetAccountById(fromAccount.Id).Withdrawn == 0m);

            Assert.IsTrue(accountRepository.GetAccountById(toAccount.Id).Balance == 3000m);
            Assert.IsTrue(accountRepository.GetAccountById(toAccount.Id).PaidIn == 3000m);
        }
    }
}
