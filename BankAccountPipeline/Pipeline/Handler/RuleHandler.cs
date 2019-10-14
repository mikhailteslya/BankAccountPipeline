using BankAccountPipeline.Pipeline.Request;
using System;

namespace BankAccountPipeline.Pipeline.Handler
{
    abstract class RuleHandler
    {
        protected RuleHandler(string reviewerRole)
        {
            ReviewerRole = reviewerRole;
            _handlerId = Guid.NewGuid();
        }
        public abstract void Handle<T>(ref T request) where T : UserCase;
        public string ReviewerRole { get; private set; }
        private Guid _handlerId;
        public Guid HandlerId {
            get {
                return _handlerId;
            }
        }
    }
}
