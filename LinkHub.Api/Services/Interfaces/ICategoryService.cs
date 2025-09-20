using LinkHub.Api.Dtos;
using LinkHub.Core.Entities;

namespace LinkHub.Api.Services.Interfaces
{
    /// <summary>
    /// Define o contrato para o serviço de gerenciamento de categorias.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Cria uma nova categoria para um usuário específico.
        /// </summary>
        /// <param name="categoryDto">DTO com os dados para a criacao.</param>
        /// <param name="userId">ID do usuario dono da categoria.</param>
        /// <returns>A entidade Category recem-criada.</returns>
        Task<Category> CreateAsync(CreateCategoryDto categoryDto, int userId);

        /// <summary>
        /// Busca uma categoria pelo ID, garantindo que ela pertence ao usuário especificado.
        /// </summary>
        /// <param name="id">ID da categoria a ser buscada</param>
        /// <param name="userId">ID do usuário dono da categoria</param>
        /// <retuns>A entidade Category ou null se nao for encontrada ou nao pertencer ao usuario.</retuns>
        Task<Category?> GetByIdAsync(int id, int userId);
    }
}
