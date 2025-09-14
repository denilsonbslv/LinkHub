using LinkHub.Api.Dtos;
using LinkHub.Api.Infrastructure.Data;
using LinkHub.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Endpoint para registrar um novo usuário no sistema.
    /// </summary>
    [HttpPost("register")] // Rota: POST api/users/register
    public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
    {
        // 1. Verificar se já existe um usuário com o mesmo e-mail
        var userExists = await _context.Users.AnyAsync(u => u.Email == registerUserDto.Email);
        if (userExists)
        {
            // Retorna um erro 400 (Bad Request) informando que o e-mail já está em uso.
            return BadRequest("Este e-mail já está em uso.");
        }

        // 2. Criptografar a senha (gerar o hash)
        // O método HashPassword faz todo o trabalho de gerar um hash seguro.
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password);

        // 3. Criar a nova entidade User
        var user = new User
        {
            Name = registerUserDto.Name,
            Email = registerUserDto.Email,
            PasswordHash = passwordHash
        };

        // 4. Adicionar o usuário ao DbContext e salvar no banco de dados
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // 5. Retornar uma resposta de sucesso
        // É uma boa prática retornar o objeto criado (sem dados sensíveis).
        // Aqui, retornamos um DTO simples, mas poderíamos criar um UserDto específico.
        return Ok(new { user.Id, user.Name, user.Email });
    }
}