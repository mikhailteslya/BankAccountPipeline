using BankAccountPipeline.Pipeline;
using BankAccountPipeline.Pipeline.Handler;
using BankAccountPipeline.Pipeline.Helpers;
using BankAccountPipeline.Pipeline.Request;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAccountPipeline
{
    class Program
    {
        static void Main(string[] args)
        {
            var separator = string.Join("", Enumerable.Repeat('-', 10));
            Console.WriteLine("Initializing pipeline");
            
            var pipeline = PipelineHelpers.InitPipeline();

            ProcessViaClassicPipeline(pipeline);
            Console.WriteLine(separator);
            ProcessViaExpressionsPipeline(pipeline);
            Console.WriteLine(separator);
            ProcessViaInMemoryDelegate(pipeline);
        }

        private static void ProcessViaClassicPipeline(RulePipeline pipeline) {
            var userCases = UseCaseHelpers.GenerateRequests(10000);
            System.Diagnostics.Stopwatch w = new System.Diagnostics.Stopwatch();
            Console.WriteLine("Initializing delegate chain - non-compiled");
            w.Start();
            Action<UserCase> compiled = (context) => {
                pipeline.Handle(context);
            };

            Console.WriteLine($"Delegate chain initialized: {w.ElapsedMilliseconds}ms");

            w.Reset();
            Console.WriteLine("Delegate chain request start");
            w.Start();
            foreach (var userCase in userCases)
            {
                compiled(userCase);
            }

            Console.WriteLine($"Delegate chain request completed: {w.ElapsedMilliseconds}ms");
            Console.WriteLine($"Cases approved first stage: {userCases.Count(x => x.FirstCheckupApproved)}");
            Console.WriteLine($"Cases approved second stage: {userCases.Count(x => x.SecondCheckupApproved)}");
            Console.WriteLine($"Cases signed by supervisor: {userCases.Count(x => x.IsSigned)}");
            Console.WriteLine($"Finally cards issued: {userCases.Count(x => x.CardIssued)}");
        }

        private static void ProcessViaExpressionsPipeline(RulePipeline pipeline) {
            var userCases = UseCaseHelpers.GenerateRequests(10000);
            System.Diagnostics.Stopwatch w = new System.Diagnostics.Stopwatch();
            Console.WriteLine("Initializing delegate chain compiled via BlockExpression");

            w.Start();
            Action<UserCase> compiled = pipeline.CompilePipelineAsExpression<UserCase>();

            Console.WriteLine($"Delegate chain compilation with block expression took: {w.ElapsedMilliseconds}ms");

            w.Reset();
            Console.WriteLine("Compiled block expression start");
            w.Start();
            foreach (var userCase in userCases)
            {
                compiled(userCase);
            }

            Console.WriteLine($"Classic pipeline handler request completed: {w.ElapsedMilliseconds}ms; User cases processed total: {userCases.Count}");
            Console.WriteLine($"Cases approved first stage: {userCases.Count(x => x.FirstCheckupApproved)}");
            Console.WriteLine($"Cases approved second stage: {userCases.Count(x => x.SecondCheckupApproved)}");
            Console.WriteLine($"Cases signed by supervisor: {userCases.Count(x => x.IsSigned)}");
            Console.WriteLine($"Finally cards issued: {userCases.Count(x => x.CardIssued)}");
        }

        private static void ProcessViaInMemoryDelegate(RulePipeline pipeline) {
            var userCases = UseCaseHelpers.GenerateRequests(10000);
            System.Diagnostics.Stopwatch w = new System.Diagnostics.Stopwatch();
            Console.WriteLine("Initializing delegate chain compiled to single in-memory delegate");

            w.Start();
            Action<UserCase> compiled = pipeline.CompilePipeline<UserCase>();

            Console.WriteLine($"Delegate chain compilationto single in-memory delegate took: {w.ElapsedMilliseconds}ms");

            w.Reset();
            Console.WriteLine("Compiled in-memory delegate start");
            w.Start();
            foreach (var userCase in userCases)
            {
                compiled(userCase);
            }

            Console.WriteLine($"Classic pipeline handler request completed: {w.ElapsedMilliseconds}ms; User cases processed total: {userCases.Count}");
            Console.WriteLine($"Cases approved first stage: {userCases.Count(x => x.FirstCheckupApproved)}");
            Console.WriteLine($"Cases approved second stage: {userCases.Count(x => x.SecondCheckupApproved)}");
            Console.WriteLine($"Cases signed by supervisor: {userCases.Count(x => x.IsSigned)}");
            Console.WriteLine($"Finally cards issued: {userCases.Count(x => x.CardIssued)}");
        }
    }
}
