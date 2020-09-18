using FizzBuzzApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace FizzBuzzApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FizzController : ControllerBase
    {
        private readonly IFizzBuzzService _fizzBuzzService;

        public FizzController(IFizzBuzzService fizzBuzzService)
        {
            _fizzBuzzService = fizzBuzzService;
        }


        [HttpGet("Buzz")]
        public IActionResult Buzz()
        {
            return  Ok(_fizzBuzzService.Fizz());
        }
        
    }
}