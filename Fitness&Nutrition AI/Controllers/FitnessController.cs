using Microsoft.AspNetCore.Mvc;
using Fitness_Nutrition_AI.Models;
using Fitness_Nutrition_AI.Services;
using Fitness_Nutrition_AI.Data;

namespace Fitness_Nutrition_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FitnessController : ControllerBase
    {
        private readonly CalculatorServices _calculatorService;
        private readonly AppDbContext _context;

        public FitnessController(CalculatorServices calculatorServices, AppDbContext context)
        {
            _calculatorService = calculatorServices;
            _context = context;
        }

        [HttpPost("calculate-tdee")]
        public async Task<IActionResult> Calculate([FromBody] FitnessProfile profile)
        {
            profile.CalculatedBmr = _calculatorService.CalculateBmr(profile.Weight, profile.Height, profile.Age, profile.Gender);
            profile.CalculatedTdee = _calculatorService.CalculateTdee(profile.ActivityLevel, profile.CalculatedBmr);
            
            _context.FitnessProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return Ok(profile);
        }
    }
}