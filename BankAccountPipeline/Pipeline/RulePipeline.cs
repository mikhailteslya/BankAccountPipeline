using BankAccountPipeline.Pipeline.Handler;
using BankAccountPipeline.Pipeline.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BankAccountPipeline.Pipeline
{
    internal class RulePipeline
    {
        private List<RuleHandler> _handlers;
        private List<RuleHandler> Handlers {
            get {
                return _handlers ?? (_handlers = new List<RuleHandler>());
            }
        }

        private bool _reversed;

        private int _index;

        public void AddHandler<T>(params object[] ruleConstructor) where T : RuleHandler {
            var handlerInstance = (T)Activator.CreateInstance(typeof(T), ruleConstructor);
            Handlers.Add(handlerInstance);
        }

        public Action<T> CompilePipeline<T>() where T : UserCase
        {
            Reverse();
            Action<T> empty = (context) => { };
            var cpl = Enumerable.Range(0, Handlers.Count).Select(i => GetInvoker<T>(i)).Aggregate(empty, (compiled, next) =>
            {
                return context =>
                {
                    next(context, compiled);
                };
            });
            return cpl;
        }

        public Action<T> CompilePipelineAsExpression<T>() where T : UserCase
        {
            BlockExpression body = null;
            ParameterExpression context = Expression.Parameter(typeof(T), "context");
            BlockExpression stack = Enumerable.Range(0, Handlers.Count).Select(i => Handlers[i]).Aggregate(body, (compiled, next) =>
            {
                var t = next.GetType();
                MethodCallExpression currentNode = Expression.Call(Expression.Constant(next), "Handle", new[] { typeof(T)}, context);
                if (compiled == null)
                    compiled = Expression.Block(currentNode);
                else
                {
                    var expressions = compiled.Expressions.ToList();
                    expressions.Add(currentNode);
                    compiled = Expression.Block(expressions);
                }
                return compiled;
            });
            LambdaExpression lambda = Expression.Lambda(stack, context);
            Delegate cpl = lambda.Compile();
            return (Action<T>)cpl;
        }

        public void Handle<T>(T request) where T : UserCase {
            UnReverse();
            InvokeNext(request, Handle);
        }

        private void Reverse()
        {
            if (!_reversed)
            {
                Handlers.Reverse();
                _reversed = true;
            }
        }

        private void UnReverse()
        {
            if (_reversed)
            {
                Handlers.Reverse();
                _reversed = false;
            }
        }

        private void InvokeNext<T>(T request, Action<T> next) where T : UserCase
        {
            var handler = NextHandler();
            if (handler == null)
                return;
            handler.Handle(ref request);
            next.Invoke(request);
        }

        private RuleHandler NextHandler() {
            if (_index == Handlers.Count)
            {
                _index = 0;
                return null;
            }

            var handler = Handlers[_index];
            _index++;
            return handler;
        }

        private Action<T, Action<T>> GetInvoker<T>(int index) where T : UserCase {
            return (context, next) => {
                Handlers[index].Handle(ref context);
                next.Invoke(context);
            };
        }
    }
}
