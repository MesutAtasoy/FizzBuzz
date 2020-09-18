using FizzBuzzApplication.Attributes;

namespace FizzBuzzApplication.Services
{
    public interface IFizzBuzzService
    {
        [Cache(Key = "Key", TTL = 3600)]
        string Fizz();
    }

    public class FizzBuzzService : IFizzBuzzService
    {
        public string Fizz()
        {
            return "Buzz";
        }
    }
}