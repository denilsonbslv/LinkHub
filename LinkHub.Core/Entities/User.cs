namespace LinkHub.Core.Entities
{
    /// <summary>
    /// Representa um usuário no sistema. É a entidade raiz.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }

        public ICollection<Link> Links { get; set; } = new List<Link>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
