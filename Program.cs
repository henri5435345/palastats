using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Ajoutez les services nécessaires ici
builder.Services.AddControllers();

var app = builder.Build();

// Configurez le pipeline de requêtes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles(); // Permet d'utiliser les fichiers statiques

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        context.Response.Redirect("/index.html");
    });
    endpoints.MapControllers(); // Permet d'accéder aux contrôleurs
});

app.Run();
