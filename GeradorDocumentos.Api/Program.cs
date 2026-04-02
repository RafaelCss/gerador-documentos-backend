using GeradorDocumento.Ioc.Infra.MediatorExtensions;
using GeradorDocumento.Ioc.Dominio;
using GeradorDocumentos.Dominio.Contrato;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddConfiguraDominio();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMediatorHandlers(
    GeradorDocumentos.Dominio.Metadado.GetAssembly() ,
    GeradorDocumentos.Infra.Metadado.GetAssembly()
);
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
