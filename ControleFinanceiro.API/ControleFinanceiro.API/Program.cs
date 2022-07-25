using ControleFinanceiro.BLL.Models;
using ControleFinanceiro.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var ConnectionString = builder.Configuration.GetConnectionString("ConexaoDb");
// Add services to the container.

builder.Services.AddDbContext<Contexto>(op => op.UseSqlServer(ConnectionString));

builder.Services.AddIdentity<Usuario, Funcao>().AddEntityFrameworkStores<Contexto>();

builder.Services.AddCors();

builder.Services.AddSpaStaticFiles(diretorio =>
{
    diretorio.RootPath = "ControleFinanceiro-UI";
});

builder.Services.AddControllers()
    .AddJsonOptions(opcoes =>
{
    opcoes.JsonSerializerOptions.IgnoreNullValues = true;

})
    .AddNewtonsoftJson(opcoes =>
    {
        opcoes.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opcoes => opcoes.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.UseSpaStaticFiles();

app.UseSpa(spa =>
{
    spa.Options.SourcePath = Path.Combine(Directory.GetCurrentDirectory(), "ControleFinanceiro-UI");
    if (app.Environment.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer($"http://localhost:4200/");
    }
});

app.MapControllers();

app.Run();
