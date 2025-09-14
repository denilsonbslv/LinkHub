using LinkHub.Core.Entities;

namespace LinkHub.Api.Services.Interfaces;

/// <summary>
/// Define o contrato para um serviço que gera tokens JWT.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Gera um token JWT para um usuário específico.
    /// </summary>
    /// <param name="user">O usuário para o qual o token será gerado.</param>
    /// <returns>Uma string representando o token JWT.</returns>
    string GenerateToken(User user);
}