using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness_Nutrition_AI.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "trainingType",
                table: "FitnessProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "trainingType",
                table: "FitnessProfiles");
        }
    }
}
