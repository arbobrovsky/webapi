using webapi.Data.Models;

namespace webapi.Models.DTO
{
    public class CurrencyChangeDTO
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstCurrency { get; set; } = string.Empty;
        public double FirstTotal { get; set; }
        public string SecoundCurrency { get; set; } = string.Empty;
        public double SecoundTotal { get; set; }
    }
}
