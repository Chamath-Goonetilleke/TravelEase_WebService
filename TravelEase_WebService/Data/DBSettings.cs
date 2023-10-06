namespace TavelEase_WebService.Data
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string BackOfficeUserCollectionName { get; set; } = string.Empty;
        public string TravelAgentCollectionName { get; set; } = string.Empty;
        public string TravellerCollectionName { get; set; } = string.Empty;
    }
}