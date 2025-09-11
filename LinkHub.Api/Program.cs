// Importa os namespaces necessários que usaremos.
using LinkHub.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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
    options.UseSqlServer(connectionString));

// Habilita o uso de Controllers. Linha essencial para nossa arquitetura.
builder.Services.AddControllers();

// Adiciona os serviços do Swagger para documentação da API.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

// Habilita a autorização (usaremos mais tarde).
app.UseAuthorization();

// Mapeia as rotas definidas nos nossos arquivos de Controller.
app.MapControllers();

// Inicia a aplicação.
app.Run();