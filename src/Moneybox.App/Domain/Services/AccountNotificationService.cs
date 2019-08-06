using System;
using System.Collections.Generic;
using System.Text;

namespace Moneybox.App.Domain.Services
{
    public class AccountNotificationService : INotificationService
    {
        public void NotifyApproachingPayInLimit(string emailAddress)
        {
            Console.WriteLine($"Address: {emailAddress}\nSubject: Approaching Pay In Limit\nBody: You are within £5:00 of your pay in limit.");
        }

        public void NotifyFundsLow(string emailAddress)
        {
            Console.WriteLine($"Address: {emailAddress}\nSubject: Your funds are low\nBody: You have less than £5:00 in your account.");
        }
    }
}
