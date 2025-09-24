using System.ComponentModel.DataAnnotations;

namespace LinkHub.Api.Dtos
{
    /// <summary>
    /// DTO para receber os dados necessários para criar um novo link encurtado.
    /// </summary>
    public class CreateLinkDto
    {
        /// <summary>
        /// A URL original que será encurtada.
        /// </summary>
        [Required(ErrorMessage = "A URL original é obrigatória.")]
        [Url(ErrorMessage = "A URL original deve ser uma URL válida.")]
        public required string OriginalUrl { get; set; }

        /// <summary>
        /// Um titulo opcional para fácil identificação do link.
        /// </summary>
        [StringLength(100, ErrorMessage = "O título não pode exceder 100 caracteres.")]
        public string? Title { get; set; }

        /// <summary>
        /// o ID da categoria opcional da categoria a qual o link pertence.
        /// </summary>
        public int? CategoryId { get; set; }
    }
}
