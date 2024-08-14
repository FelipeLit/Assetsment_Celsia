namespace assetsment_Celsia.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Acronym { get; set; }
        public DateTime TransactionDate { get; set; }
        public float Amount { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int ClientId { get; set; }
        public int PlatformId { get; set; }
    }
}