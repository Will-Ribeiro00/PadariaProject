using PadariaProjectAPL.Views.Menu.MenuBasico;

namespace PadariaProjectAPL.UseCases
{
    public class MenuPrincipalUseCase
    {
        private readonly MenuPrincipalView _view;
        public MenuPrincipalUseCase(MenuPrincipalView view) => _view = view;

        public async Task Execute() => await _view.ExibirMenu();
    }
}