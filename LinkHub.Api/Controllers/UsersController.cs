using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using LinkHub.Api.Dtos;
using LinkHub.Api.Infrastructure.Data;
using LinkHub.Api.Services;
using LinkHub.Api.Services.Interfaces;
using LinkHub.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public UsersController(ApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
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

    /// <summary>
    /// Endpoint para autenticar um usuário e retornar um token JWT.
    /// </summary>
    [HttpPost("login")] // Rota: POST api/users/login
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        // 1. Encontrar o usuário pelo e-mail
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        // 2. Verificar se o usuário existe E se a senha está correta.
        // O BCrypt.Verify compara a senha enviada (texto plano) com o hash salvo no banco.
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            // Usamos uma mensagem genérica por segurança, para não informar se foi o e-mail ou a senha que errou.
            return Unauthorized("Credenciais inválidas.");
        }

        // 3. Se as credenciais estiverem corretas, gerar o token JWT
        var token = _tokenService.GenerateToken(user);

        // 4. Retornar o token para o cliente
        return Ok(new { Token = token });
    }

    /// <summary>
    /// Endpoint protegido que retorna os dados do usuário logado.
    /// </summary>
    [HttpGet("me")] // Rota: GET api/users/me
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized();
        }

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == int.Parse(userIdString));

        if (user is null)
        {
            return NotFound("Usuário não encontrado.");
        }

        return Ok(new { user.Id, user.Name, user.Email, user.CreatedAt, user.LastUpdatedAt });
    }
}