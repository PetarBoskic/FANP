using Microsoft.AspNetCore.Mvc;
using Fitness_Nutrition_AI.Models;
using Fitness_Nutrition_AI.Services;
namespace Fitness_Nutrition_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FitnessController : ControllerBase
    {
        private readonly CalculatorServices _calculatorService;

        public FitnessController(CalculatorServices calculatorServices)
        {
            _calculatorService = calculatorServices;
        }

        [HttpPost("calculate-tdee")]
        public IActionResult Calculate([FromBody] FitnessProfile profile)
        {
            profile.CalculatedBmr = _calculatorService.CalculateBmr(profile.Weight, profile.Height, profile.Age, profile.Gender);
            profile.CalculatedTdee = _calculatorService.CalculateTdee(profile.ActivityLevel, profile.CalculatedBmr);
            return Ok(profile);
        }
    }
}
