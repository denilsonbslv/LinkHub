namespace LinkHub.Core.Entities
{
    /// <summary>
    /// Representa uma categoria para organizar os links, que pertence a um usuário.
    /// </summary>
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Color { get; set; } = "#cccccc";

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Link> Links { get; set; } = new List<Link>();
    }
}
