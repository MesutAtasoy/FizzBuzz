using FizzBuzzApplication.Services;
using Microsoft.Extensions.Logging;

namespace FizzBuzzApplication.Decorators
{
    public class FizzBuzzDecorator : IFizzBuzzService
    {
        private readonly IFizzBuzzService _fizzBuzzService;
        private readonly ILogger<FizzBuzzDecorator> _logger;
        

        public FizzBuzzDecorator(IFizzBuzzService fizzBuzzService,
            ILogger<FizzBuzzDecorator> logger)
        {
            _fizzBuzzService = fizzBuzzService;
            _logger = logger;
        }

        public string Fizz()
        {
            _logger.LogInformation("Hey, I should say Fizz before calling the method");
            var result = _fizzBuzzService.Fizz();
            _logger.LogInformation("Hey, I should say again Fizz after calling the method");
            return result;
        }
    }
}