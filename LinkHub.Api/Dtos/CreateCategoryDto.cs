using System.ComponentModel.DataAnnotations;

/// <summary>
/// DTO para receber os dados necessários para a criação de uma nova categoria.
/// </summary>
namespace LinkHub.Api.Dtos
{
    public class CreateCategoryDto
    {
        /// <summary>
        /// Nome da categoria.
        /// </summary>
        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 50 caracteres.")]
        public required string Name { get; set; }

        /// <summary>
        /// Cor em formato hexadecimal (ex: #ff5733) para identificacao visual da categoria.
        /// </summary>
        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "A cor deve estar em formato hexadecimal válido (ex: #FF5733).")]
        public string Color { get; set; } = "#cccccc";
    }
}
