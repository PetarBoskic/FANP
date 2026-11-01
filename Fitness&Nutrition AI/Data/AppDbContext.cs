using Fitness_Nutrition_AI.Models;
 using Microsoft.EntityFrameworkCore;
namespace Fitness_Nutrition_AI.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<FitnessProfile> FitnessProfiles { get; set; }


    }
}
