namespace Fitness_Nutrition_AI.Models
{
    public class FitnessProfile
    {
        public int ID { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public int Age { get; set;}
        public Gender Gender { get; set; }
        public ActivityLevel ActivityLevel { get; set; }
        public FitnessGoal Goal { get; set; }

        public LastTimeInShape LastTTimeInShape { get; set; }
        public double CalculatedBmr { get; set; }
        public double CalculatedTdee { get; set; }
    }
}
