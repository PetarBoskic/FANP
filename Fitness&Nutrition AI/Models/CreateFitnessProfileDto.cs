
namespace Fitness_Nutrition_AI.Models
{
    public class CreateFitnessProfileDto
    {
        public double Weight { get; set; }
        public double Height { get; set; }
        public int age { get; set; }
        public Gender gender { get; set; }
        public FitnessGoal goal { get; set; }
        public ActivityLevel activityLevel { get; set; }
        public TrainingType trainingType { get; set; }
        public LastTimeInShape lastTimeInShape { get; set; }

    }
}
