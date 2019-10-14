using BankAccountPipeline.Pipeline.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankAccountPipeline.Pipeline.Helpers
{
    static class UseCaseHelpers
    {
        public static IList<UserCase> GenerateRequests(int totalCount, int rejectedPercent = 15) {
            var rejectedNumber = (totalCount / 100) * rejectedPercent;
            if (rejectedPercent < 1)
                rejectedNumber = 1;
            var userCases = new List<UserCase>();
            //for items inside one of three generated parameters wouldn't pass validation
            for (var i = 0; i < rejectedNumber; i++) {
                var rand = new Random().Next(0, 30);
                var @case = new UserCase
                {
                    PersonalCode = rand >= 10 && rand < 20 ?
                        $"{rand}{i}{rand * 2}{(i * 2).ToString().Substring(1)}{(char)rand}{i}{(i * 2).ToString().Substring(1)}{rand * 3}" :
                        $"{rand}{i}{rand * 2}{(i * 2).ToString().Substring(1)}-{i}{(i * 2).ToString().Substring(1)}{rand * 3}",
                    FirstName = rand >= 0 && rand < 5 ? "" : RandomString(rand * 2),
                    LastName = rand >= 5 && rand < 10 ? "" : RandomString(rand),
                    ExpectedMonthlyIncome = rand >= 20 && rand <= 30 ? rand * rand : rand * rand * (rand / 10)
                };
                userCases.Add(@case);
            }
            var approvedNumber = totalCount - rejectedNumber;
            //here for all three generated params we have correct values
            for (var i = 0; i < approvedNumber; i++)
            {
                var rand = new Random().Next(13, 20);
                var @case = new UserCase
                {
                    PersonalCode = $"{rand}{i}{rand * 2}{(i * 2).ToString().Substring(1)}-{i}{(i * 2).ToString().Substring(1)}{rand * 3}",
                    FirstName = RandomString(rand * 2),
                    LastName = RandomString(rand),
                    ExpectedMonthlyIncome = rand * rand * rand
                };
                userCases.Add(@case);
            }
            return userCases;
        }

        private static string RandomString(int length) {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
            return string.Join("", Enumerable.Repeat(alphabet, length).Select(c => c[new Random().Next(0, alphabet.Length)]));
        }
    }
}
