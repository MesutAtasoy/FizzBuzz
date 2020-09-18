using System;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace FizzBuzzApplication.Proxies
{
    public class FizzBuzzProxy<TDecorated> : DispatchProxy
    {
        private TDecorated _decorated;
        private ILogger<TDecorated> _logger;
        
        
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            _logger.LogInformation("Hey, I should say Fizz before calling the method");
            var result = targetMethod.Invoke(_decorated, args);
            _logger.LogInformation("Hey, I should say again Fizz after calling the method");
            return result;
        }
        
        public static TDecorated Create(TDecorated decorated, ILogger<TDecorated> logger)
        {
            object proxy = Create<TDecorated, FizzBuzzProxy<TDecorated>>();
            ((FizzBuzzProxy<TDecorated>)proxy).SetParameters(decorated, logger);
            return (TDecorated)proxy;
        }

        private void SetParameters(TDecorated decorated,ILogger<TDecorated> logger)
        {
            _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}