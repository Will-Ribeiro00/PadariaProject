using PadariaProjectAPL.Entities;
using PadariaProjectAPL.Services.Authentication;

namespace PadariaProjectAPL.Views.Login
{
    public class LoginView
    {
        private readonly Autenticacao _autenticacao;
        public LoginView(Autenticacao autenticacao) => _autenticacao = autenticacao;

        public async Task<Funcionarios> Login()
        {
            var usuarioConectado = await _autenticacao.Login();
            return usuarioConectado;
        }

    }
}
