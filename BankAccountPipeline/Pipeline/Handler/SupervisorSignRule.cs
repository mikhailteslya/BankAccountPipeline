using System;
using BankAccountPipeline.Pipeline.Request;

namespace BankAccountPipeline.Pipeline.Handler
{
    class SupervisorSignRule : RuleHandler
    {
        public SupervisorSignRule(string reviewerRole) : base(reviewerRole)
        {
        }


        public override void Handle<T>(ref T request)
        {
            if (!request.FirstCheckupApproved || !request.SecondCheckupApproved)
                return;
            request.IsSigned = true;
            request.SignDate = DateTime.Now;
        }
    }
}
