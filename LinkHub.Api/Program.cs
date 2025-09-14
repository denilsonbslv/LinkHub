// Importa os namespaces necessários que usaremos.
using LinkHub.Api.Infrastructure.Data;
using LinkHub.Api.Services;
using LinkHub.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURAÇÃO DOS SERVIÇOS ---

// Pega a string de conexão que salvamos no Gerenciador de Segredos.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra o ApplicationDbContext e configura o Entity Framework para usar o SQL Server.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
    {
        // Habilita a lógica de retentativa (retry)
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5, // Tenta até 5 vezes
            maxRetryDelay: TimeSpan.FromSeconds(30), // Espera no máximo 30s entre as tentativas
            errorNumbersToAdd: null); // Usa os erros transientes padrão do SQL Azure
    }));

// Habilita o uso de Controllers. Linha essencial para nossa arquitetura.
builder.Services.AddControllers();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization();

// Adiciona os serviços do Swagger para documentação da API.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    // Adiciona uma seção "Authorize" na UI do Swagger
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "LinkHub API", Version = "v1" });

    // 1. Define o esquema de segurança que a API usa (Bearer Authentication)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, insira 'Bearer' seguido de um espaço e o token JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.Http, // Usamos Http pois o Bearer é um esquema HTTP
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // 2. Aplica o requisito de segurança globalmente a todos os endpoints
    // Isso fará com que o Swagger envie o token em todas as requisições após o login.
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// --- 2. CONSTRUÇÃO DA APLICAÇÃO ---
var app = builder.Build();

// --- 3. CONFIGURAÇÃO DO PIPELINE HTTP ---

// Habilita a interface do Swagger apenas em ambiente de desenvolvimento.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redireciona requisições HTTP para HTTPS.
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Mapeia as rotas definidas nos nossos arquivos de Controller.
app.MapControllers();

// Inicia a aplicação.
app.Run();