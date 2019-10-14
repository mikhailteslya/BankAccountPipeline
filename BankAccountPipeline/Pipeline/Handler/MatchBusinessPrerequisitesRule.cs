using System;

namespace BankAccountPipeline.Pipeline.Handler
{
    class MatchBusinessPrerequisitesRule : RuleHandler
    {
        public MatchBusinessPrerequisitesRule(string reviewerRole) : base(reviewerRole)
        {
        }

        public override void Handle<T>(ref T request)
        {
            request.SecondCheckupApproved = request.IsCitizen || request.ExpectedMonthlyIncome > 2000;
            if (!request.SecondCheckupApproved)
                return;
            request.SecondCheckupApprovalDate = DateTime.Now;
        }
    }
}
