namespace LinkHub.Core.Entities
{
    /// <summary>
    /// A entidade principal: o link encurtado. Depende de User e, opcionalmente, de Category.
    /// </summary>
    public class Link
    {
        public int Id { get; set; }
        public required string OriginalUrl { get; set; }
        public required string ShortCode { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Clicks { get; set; } = 0;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
