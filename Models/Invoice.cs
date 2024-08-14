namespace assetsment_Celsia.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string Period { get; set; }
        public float BilledAmount { get; set; }
        public float PaidAmount { get; set; }
        public int ClientId { get; set; }
        public int TransactionId { get; set; }
    }
}