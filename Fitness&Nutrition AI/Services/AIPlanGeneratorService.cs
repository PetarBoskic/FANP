using System.Text;
using Fitness_Nutrition_AI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Fitness_Nutrition_AI.Services
{
    public class AIPlanGeneratorService : IAIPlanGeneratorService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apikey;
        public AIPlanGeneratorService(HttpClient http,IConfiguration configuration)
        {
            _httpClient = http;
            _apikey = configuration["GeminiApi:ApiKey"]
                ?? throw new InvalidOperationException("Gemini API key is not optimised in appsettings.json");

        }
        public async Task<AiPlanResponseDto> GeneratePlanAsync(FitnessProfile profile)
        {
            var prompt = $@"
You are an expert AI personal trainer and nutritionist. Generate a comprehensive, highly structured workout and nutrition plan in English for the following user:
- Height: {profile.Height} cm
- Weight: {profile.Weight} kg
- Age: {profile.Age}
- Gender: {profile.Gender}
- Activity Level: {profile.ActivityLevel}
- Goal: {profile.Goal}
- Calculated BMR: {profile.CalculatedBmr} kcal
- Target Daily Calories (TDEE + Goal): {profile.CalculatedTdee} kcal

Return the response EXCLUSIVELY as a valid JSON object with no additional text or markdown formatting outside of JSON. The JSON structure must be exactly like this:
{{
  ""workoutPlan"": ""Detailed weekly workout routine divided by days with exercises, sets, reps, and rest intervals."",
  ""nutritionPlan"": ""Detailed daily meal plan divided by meals (Breakfast, Lunch, Dinner, Snacks) with estimated calories and macros."",
  ""recommendations"": ""Actionable advice on hydration, sleep, recovery, progressive overload, and supplementation.""
}}";

            var requestBody = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new { text = prompt }
                }
            }
        }
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            // Lista modela koje ćemo probati (prvo flash, pa pro kao rezervni)
            var models = new[] { "gemini-3.6-flash", "gemini-3.1-pro" };
            HttpResponseMessage? response = null;

            foreach (var model in models)
            {
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={_apikey}";
                int maxRetries = 3;

                for (int i = 0; i < maxRetries; i++)
                {
                    response = await _httpClient.PostAsync(url, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        break;
                    }

                    // Ako je 503 ili 429, sačekaj 3 sekunde
                    if ((int)response.StatusCode == 503 || (int)response.StatusCode == 429)
                    {
                        await Task.Delay(3000);
                        continue;
                    }

                    break;
                }

                if (response != null && response.IsSuccessStatusCode)
                {
                    break;
                }
            }

            if (response == null || !response.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini API trenutno nije dostupan (Status: {response?.StatusCode}). Pokušajte ponovo za minut.");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);

            var rawText = jsonResponse["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

            if (string.IsNullOrEmpty(rawText))
            {
                throw new Exception("Prazan odgovor od AI servisa.");
            }

            var cleanJson = rawText.Replace("```json", "").Replace("```", "").Trim();

            var planDto = JsonConvert.DeserializeObject<AiPlanResponseDto>(cleanJson);

            return planDto ?? new AiPlanResponseDto
            {
                Recommendations = "Error deserializing AI response."
            };
        }
    }
}
