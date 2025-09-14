using System.ComponentModel.DataAnnotations;

namespace LinkHub.Api.Dtos
{
    /// <summary>
    /// DTO (Data Transfer Object) para receber Email e Senha.
    /// Usamos DataAnnotations como [Required] para validações automáticas.
    /// </summary>
    public class LoginDto
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public required string Password { get; set; }
    }
}
