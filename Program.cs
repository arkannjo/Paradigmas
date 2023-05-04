// Importar namespaces necessários
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {
        // Criar a instância do aplicativo Web
        var builder = WebApplication.CreateBuilder(args);

        // Adicionar serviços ao contêiner
        builder.Services.AddRazorPages();
        builder.Services.AddControllersWithViews();

        // Construir o aplicativo
        var app = builder.Build();

        // Definir a rota de exceções
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            IApplicationBuilder applicationBuilder = app.UseDeveloperExceptionPage();
        }

        // Permitir o uso de arquivos estáticos
        app.UseStaticFiles();

        // Habilitar roteamento
        app.UseRouting();

        // Adicionar autorização
        app.UseAuthorization();

        // Mapear páginas Razor
        app.MapRazorPages();

        // Registrar rotas de controle na camada superior
        app.MapControllerRoute(name: "default",
                               pattern: "{controller=Home}/{action=Index}/{id?}");

        // Iniciar o aplicativo
        app.Run();
    }
}