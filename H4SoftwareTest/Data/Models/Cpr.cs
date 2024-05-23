using System.Text.Json.Serialization;

namespace H4SoftwareTest.Data.Models
{
    public class Cpr
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string CprNumber { get; set; }

        [JsonIgnore]
        List<Tasks> tasks { get; set; }
    }
}
