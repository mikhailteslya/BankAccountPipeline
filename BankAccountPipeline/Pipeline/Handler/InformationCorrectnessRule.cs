using System;
using BankAccountPipeline.Pipeline.Request;

namespace BankAccountPipeline.Pipeline.Handler
{
    class InformationCorrectnessRule : RuleHandler
    {
        public InformationCorrectnessRule(string reviewerRole) : base(reviewerRole)
        {

        }

        private bool CheckName(UserCase request) {
            return !string.IsNullOrWhiteSpace(request.FirstName) && !string.IsNullOrWhiteSpace(request.LastName);
        }

        private bool CheckPersonalCode(UserCase request) {
            return !string.IsNullOrWhiteSpace(request.PersonalCode) 
                && request.PersonalCode.Contains("-") 
                && request.PersonalCode.Split('-').Length == 2;
        }
        //here we just validate it is correct number; is it enough and other prerequisites are not to be decided on that node
        private bool CheckIncome(UserCase request) {
            return request.ExpectedMonthlyIncome >= 0;
        }

        public override void Handle<T>(ref T request)
        {
            var nameValid = CheckName(request);
            var personalCodeValid = CheckPersonalCode(request);
            var incomeValid = CheckIncome(request);
            if (!nameValid || !personalCodeValid || !incomeValid)
                return;
            else
            {
                request.FirstCheckupApproved = true;
                request.FirstCheckupApprovalDate = DateTime.Now;
            }
        }
    }
}
