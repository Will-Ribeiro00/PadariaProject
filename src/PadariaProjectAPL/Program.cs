using Microsoft.Extensions.DependencyInjection;
using PadariaProjectAPL.Services.ServerProvider;
using PadariaProjectAPL.UseCases;

var serverProvider = Services.ConfigureServices();
var menu = serverProvider.GetService<MenuPrincipalUseCase>();


await menu.Execute();