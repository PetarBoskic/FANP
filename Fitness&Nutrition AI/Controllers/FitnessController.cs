using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IAIPlanGeneratorService _aiservice;
     
        public FitnessController(CalculatorServices calculatorServices, AppDbContext context,IAIPlanGeneratorService aiservice)
        {
            _calculatorService = calculatorServices;
            _context = context;
            _aiservice = aiservice;
        }

        [HttpPost("calculate-tdee")]
        public async Task<IActionResult> Calculate([FromBody] CreateFitnessProfileDto dto)
        {
            var profile = new FitnessProfile
            {
                Weight = dto.Weight,
                Height = dto.Height,
                Age = dto.age,
                ActivityLevel = dto.activityLevel,
                Goal = dto.goal,
                trainingType = dto.trainingType,
                LastTTimeInShape = dto.lastTimeInShape
            };

            profile.CalculatedBmr = _calculatorService.CalculateBmr(profile.Weight, profile.Height, profile.Age, profile.Gender);
            profile.CalculatedTdee = _calculatorService.CalculateTdee(profile.ActivityLevel, profile.CalculatedBmr,profile.Goal);

            _context.FitnessProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return Ok(profile);
        }
        [HttpGet("profiles")]
        public async Task<IActionResult> GetAllProfiles()
        {
            var profiles = await _context.FitnessProfiles.ToListAsync();
            return Ok(profiles);

        }
        [HttpGet("profiles/{id}")]
        public async Task<IActionResult> GetProfilesByID(int id)
        {
            var profiles =await  _context.FitnessProfiles.FindAsync(id);
            return Ok(profiles);

        }
        [HttpPost("generate-plan/{profileId}")]
        public async Task<ActionResult> GenerateAIPlan(int profileId)
        {
            var profile = await _context.FitnessProfiles.FindAsync(profileId);
            if(profile==null)
            {
                return NotFound($"Profile with ID:{profileId}  doesn't exist");
            }
            try
            {
                var aiPlan = await _aiservice.GeneratePlanAsync(profile);
                return Ok(aiPlan);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error while generating AI Plan:{ex}");
            }
        }
    }
}