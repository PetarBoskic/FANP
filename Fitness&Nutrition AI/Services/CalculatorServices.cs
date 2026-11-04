using Fitness_Nutrition_AI.Models;

namespace Fitness_Nutrition_AI.Services
{
    public class CalculatorServices
    {
        public double CalculateBmr(double weight, double height, double age, Gender g)
        {
            double bmr = 0;
            if (g == Gender.Male)
            {
                bmr = (10 * weight) + (6.25 * height) - (5 * age) + 5;
            }
            else
            {
                bmr = (10 * weight) + (6.25 * height) - (5 * age) - 161;
            }
            return bmr;
        }
        public double CalculateTdee(ActivityLevel a, double bmr,FitnessGoal goal)
        {
            double multi = a switch
            {
                ActivityLevel.NotInTraining => 1.2,
                ActivityLevel.LightTraining => 1.375,
                ActivityLevel.ModerateTraining => 1.55,
                ActivityLevel.Active => 1.725,
                ActivityLevel.VeryActive => 1.9,
                _ => 1.2
            };
            double tdee = bmr * multi;
            return goal switch
            {
                FitnessGoal.Cut => tdee - 500,
                FitnessGoal.MainTain => tdee,
                FitnessGoal.Bulk => tdee + 300,
                _ => tdee


            };
        }
        
    }
}
