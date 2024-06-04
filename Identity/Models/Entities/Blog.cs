namespace Identity.Models.Entities
{
    public class Blog
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Users users { get; set; }
        public string UserId { get; set; }
    }
}
