using LinkHub.Api.Dtos;
using LinkHub.Core.Entities;

namespace LinkHub.Api.Services.Interfaces
{
    /// <summary>
    /// Define o contrato para o serviço que gerencia as operações relacionadas a links.
    /// </summary>
    public interface ILinkService
    {
        /// <summary>
        /// Cria um novo link encurtado para um usuário específico.
        /// </summary>
        /// <param name="linkDto">DTO com os dados para a criação.</param>
        /// <param name="userId">ID do usuário dono link.</param>
        /// returns>A entidade Link recém-criada.</returns>
        Task<Link> CreateAsync(CreateLinkDto linkDto, int userId);

    }
}
