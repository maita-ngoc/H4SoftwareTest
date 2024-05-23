namespace H4SoftwareTest.Data.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public int UserID { get; set; }

        Cpr cpr { get; set; }
    }
}
