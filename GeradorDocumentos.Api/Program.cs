using GeradorDocumentos.Dominio.Comandos;
using GeradorDocumentos.Dominio.Contrato;
using GeradorDocumentos.Infra.Mediator;
using GeradorDocumentos.Infra.Servicos.FakeAI;
using GeradorDocumentos.Infra.Servicos.Pdf;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IMediator , Mediator>();
builder.Services.AddScoped<IPdfService , PdfService>();
builder.Services.AddScoped<IAIService , FakeAIService>();

builder.Services.AddScoped<IRequestHandler<GerarDocumentoComando , byte[]> , GenerateDocumentHandler>();
builder.Services.AddScoped<IRequestHandler<ExtractDataCommand , string> , ExtractDataHandler>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
