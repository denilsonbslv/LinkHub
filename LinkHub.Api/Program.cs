// Importa os namespaces necess�rios que usaremos.
using LinkHub.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURA��O DOS SERVI�OS ---

// Pega a string de conex�o que salvamos no Gerenciador de Segredos.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra o ApplicationDbContext e configura o Entity Framework para usar o SQL Server.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Habilita o uso de Controllers. Linha essencial para nossa arquitetura.
builder.Services.AddControllers();

// Adiciona os servi�os do Swagger para documenta��o da API.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// --- 2. CONSTRU��O DA APLICA��O ---
var app = builder.Build();

// --- 3. CONFIGURA��O DO PIPELINE HTTP ---

// Habilita a interface do Swagger apenas em ambiente de desenvolvimento.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redireciona requisi��es HTTP para HTTPS.
app.UseHttpsRedirection();

// Habilita a autoriza��o (usaremos mais tarde).
app.UseAuthorization();

// Mapeia as rotas definidas nos nossos arquivos de Controller.
app.MapControllers();

// Inicia a aplica��o.
app.Run();