using Fitness_Nutrition_AI.Models;

namespace Fitness_Nutrition_AI.Services
 
{
    public interface IAIPlanGeneratorService
    {
        Task<AiPlanResponseDto> GeneratePlanAsync(FitnessProfile profile);
    }
}
