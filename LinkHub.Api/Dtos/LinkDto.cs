namespace LinkHub.Api.Dtos
{
    /// <summary>
    /// DTO para retornar os dados detalhados de um link.
    /// </summary>
    public class LinkDto
    {
        public int Id { get; set; }
        public required string OriginalUrl { get; set; }
        public required string ShortCode { get; set; }
        public string? Title { get; set; }
        public int Clicks { get; set; }
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// A URL completa do link encurtado, pronta para ser usada.
        /// </summary>
        public string ShortUrl { get; set; } = string.Empty;

        /// <summary>
        /// A categoria associada a este link, se houver.
        /// </summary>
        public CategoryDto? Category { get; set; }
    }
}
