namespace BarberShop.Services
{
    public static class AiPromptFactory
    {
        public static string CreateRecommendationPrompt(string serviceType)
        {
            return $"Дай кратък, професионален и стилен съвет или препоръка на български език за клиент, който ще си прави следната услуга в бръснарница: '{serviceType}'. Съветът трябва да е до 2 изречения.";
        }
    }
}
