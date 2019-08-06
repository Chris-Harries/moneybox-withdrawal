using System;
using System.Collections.Generic;
using System.Text;

namespace Moneybox.App.DataAccess
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Dictionary<Guid, Account> accountList = new Dictionary<Guid, Account>();

        public Account GetAccountById(Guid accountId)
        {
            if(!accountList.ContainsKey(accountId))
            {
                throw new InvalidOperationException("Account not found");
            }

            return accountList[accountId];
        }

        public void Update(Account account)
        {
            if(accountList.ContainsKey(account.Id))
            {
                accountList[account.Id] = account;
            }
            else
            {
                accountList.Add(account.Id, account);
            }
        }
    }
}
