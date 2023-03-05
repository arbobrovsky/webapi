namespace webapi.Data.Models
{
    public class CurrencyChange
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } 
        public string FirstCurrency { get; set; } = string.Empty;
        public double FirstTotal { get; set; }
        public string SecoundCurrency { get; set; } = string.Empty;
        public double SecoundTotal { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}
