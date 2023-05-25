namespace APIzza.Models
{
    public class Review
    {
        public int Review_Id { get; set; }
        public int Rating { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }

        public Review() { }
    }
}
