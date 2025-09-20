namespace LinkHub.Api.Dtos
{
    /// <summary>
    /// Representa uma categoria para que possa ser transferida via API. Com segurança, sem expor informações sensíveis.
    public class CategoryDto
    {
        /// <summary>
        /// Nome da categoria.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Cor em formato hexadecimal (ex: #ff5733) para identificacao visual da categoria.
        /// </summary>
        public string Color { get; set; }
    }
}
