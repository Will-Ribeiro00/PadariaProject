using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PadariaProjectAPL.repositories.Context;
using PadariaProjectAPL.repositories.DataAcess;
using PadariaProjectAPL.Services.Authentication;
using PadariaProjectAPL.Services.Serialize;
using PadariaProjectAPL.UseCases;
using PadariaProjectAPL.Utils;
using PadariaProjectAPL.Views.Login;
using PadariaProjectAPL.Views.Menu.MenuBasico;
using PadariaProjectAPL.Views.Menu.MenuCaixa;
using PadariaProjectAPL.Views.Menu.MenuEstoque;
using PadariaProjectAPL.Views.Menu.MenuGerencial;
using PadariaProjectAPL.Views.Usuario;

namespace PadariaProjectAPL.Services.ServerProvider
{
    public class Services
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection().AddDbContext<PadariaDbContext>(options =>
            options.UseOracle("DATA SOURCE=localhost:1521/XEPDB1;TNS_ADMIN=C:\\Users\\willi\\Oracle\\network\\admin;PERSIST SECURITY INFO=True;USER ID=PADARIA;PASSWORD=010203"));

            //Repositorios
            services.AddScoped<CategoriaRepository>();
            services.AddScoped<PedidoRepository>();
            services.AddScoped<ProdutoRepository>();
            services.AddScoped<EstoqueRepository>();
            services.AddScoped<FuncionarioRepository>();
            services.AddScoped<ClienteRepository>();
            services.AddScoped<PagamentoRepository>();
            services.AddScoped<FidelidadeRepository>();
            services.AddScoped<GerenteRepository>();
            services.AddScoped<PedidoProdutoRepository>();

            //Services
            services.AddScoped<Serializacao>();

            //Utilidades
            services.AddScoped<Autenticacao>();
            services.AddScoped<Util>();

            //Views:
            services.AddScoped<LoginView>();
            services.AddScoped<UsuarioConectadoView>();
            services.AddScoped<MenuPrincipalView>();
            services.AddScoped<MenuCaixaView>();
            services.AddScoped<MenuEstoqueView>();
            services.AddScoped<MenuGerencialView>();

            //Caso de Uso para chamar todos os metodos
            services.AddScoped<MenuPrincipalUseCase>();

            return services.BuildServiceProvider();
        }
    }
}
