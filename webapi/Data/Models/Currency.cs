using Microsoft.EntityFrameworkCore.Metadata;

namespace webapi.Data.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public int Cur_ID { get; set; }
        public DateTime Date { get; set; }
        public string Cur_Abbreviation { get; set; }
        public int Cur_Scale { get; set; }
        public string Cur_Name { get; set; }
        public double Cur_OfficialRate { get; set;}
        public DateTime UpdateDataInDb { get; set; } = DateTime.Now;

    }
}
