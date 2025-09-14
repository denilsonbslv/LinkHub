using System.ComponentModel.DataAnnotations;

namespace LinkHub.Api.Dtos;

/// <summary>
/// DTO (Data Transfer Object) para receber os dados de registro de um novo usuário.
/// Usamos DataAnnotations como [Required] para validações automáticas.
/// </summary>
public class RegisterUserDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
    public required string Password { get; set; }
}