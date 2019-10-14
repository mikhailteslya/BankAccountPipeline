using System;

namespace BankAccountPipeline.Pipeline.Request
{
    class UserCase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsCitizen { get; set; }
        public string PersonalCode { get; set; }
        public decimal ExpectedMonthlyIncome { get; set; }

        public string Comment { get; set; }
        public bool FirstCheckupApproved { get; set; }
        public DateTime? FirstCheckupApprovalDate { get; set; }
        public bool SecondCheckupApproved { get; set; }
        public DateTime? SecondCheckupApprovalDate { get; set; }
        public bool AccountCreated { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool CardIssued { get; set; }
        public DateTime? CardIssueDate { get; set; }
        public bool IsSigned { get; set; }
        public DateTime? SignDate { get; set; }
    }
}
