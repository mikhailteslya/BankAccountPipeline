using BankAccountPipeline.Pipeline.Handler;

namespace BankAccountPipeline.Pipeline.Helpers
{
    static class PipelineHelpers
    {
        public static RulePipeline InitPipeline() {
            var pipeline = new RulePipeline();
            pipeline.AddHandler<InformationCorrectnessRule>("Support");
            pipeline.AddHandler<MatchBusinessPrerequisitesRule>("OfficeManager");
            pipeline.AddHandler<SupervisorSignRule>("DepartmentHead");
            pipeline.AddHandler<MatchIssueCardPrerequisitesRule>("ClientManager");

            return pipeline;
        }
    }
}
