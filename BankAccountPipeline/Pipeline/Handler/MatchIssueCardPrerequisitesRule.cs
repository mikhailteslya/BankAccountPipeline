using System;

namespace BankAccountPipeline.Pipeline.Handler
{
    class MatchIssueCardPrerequisitesRule : RuleHandler
    {
        public MatchIssueCardPrerequisitesRule(string reviewerRole) : base(reviewerRole)
        {
        }
        public override void Handle<T>(ref T request)
        {
            if (!request.IsSigned)
                return;
            request.CardIssued = true;
            request.CardIssueDate = DateTime.Now;
        }
    }
}
