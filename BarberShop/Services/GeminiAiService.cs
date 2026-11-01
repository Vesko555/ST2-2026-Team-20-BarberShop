using System.Text;
using System.Text.Json;

namespace BarberShop.Services
{
    public class GeminiAiService
    {
        private readonly string _apiKey = "GEMINI_API_KEY";

        public async Task<string> GetHaircutRecommendationAsync(string serviceType)
        {
            if (string.IsNullOrEmpty(_apiKey) || _apiKey == "GEMINI_API_KEY")
            {
                return "AI препоръка: За перфектен стил с тази услуга препоръчваме консултация на място с нашия бръснар!";
            }

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";

            // Използваме Factory Pattern за генериране на текста на заявката
            var prompt = AiPromptFactory.CreateRecommendationPrompt(serviceType);

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

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            try
            {
                var response = await client.PostAsync(url, jsonContent);
                if (!response.IsSuccessStatusCode) return "Грешка при връзка с AI асистента.";

                var responseString = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseString);

                var textResult = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return textResult?.Trim() ?? "Няма отговор от AI.";
            }
            catch
            {
                return "В момента AI асистентът е недостъпен, но нашите бръснари са на разположение!";
            }
        }
    }
}
