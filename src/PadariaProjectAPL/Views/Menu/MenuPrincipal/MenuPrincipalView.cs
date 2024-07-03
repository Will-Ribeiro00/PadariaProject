using PadariaProjectAPL.Services.Authentication;
using PadariaProjectAPL.Utils;
using PadariaProjectAPL.Views.Login;
using PadariaProjectAPL.Views.Menu.MenuCaixa;
using PadariaProjectAPL.Views.Menu.MenuEstoque;
using PadariaProjectAPL.Views.Menu.MenuGerencial;
using PadariaProjectAPL.Views.Usuario;

namespace PadariaProjectAPL.Views.Menu.MenuBasico
{
    public class MenuPrincipalView
    {
        private readonly LoginView _login;
        private readonly UsuarioConectadoView _usuario;
        private readonly Autenticacao _autenticacao;
        private readonly MenuCaixaView _mCaixa;
        private readonly MenuEstoqueView _mEstoque;
        private readonly MenuGerencialView _mGerencial;
        private readonly Util _util;

        public MenuPrincipalView(LoginView login, Autenticacao autenticacao, MenuCaixaView mCaixa, MenuEstoqueView mEstoque, UsuarioConectadoView usuario, MenuGerencialView mGerencial, Util util)
        {
            _login = login;
            _usuario = usuario;
            _autenticacao = autenticacao;
            _mCaixa = mCaixa;
            _mEstoque = mEstoque;
            _mGerencial = mGerencial;
            _util = util;
        }

        public async Task ExibirMenu()
        {
            bool login = false;
            do
            {
                var usuarioConectado = await _login.Login();
                if (usuarioConectado != null)
                {
                    login = true;
                    bool menu = true;
                    do
                    {
                        Console.Clear();
                        _util.Cabecalho(usuarioConectado);
                        Console.WriteLine("\n*********MENU PRINCIPAL*********" +
                                    "\n1 - Menu Caixa" +
                                    "\n2 - Menu Estoque" +
                                    "\n3 - Menu Gerencial" +
                                    "\n4 - Meus Dados" +
                                    "\n5 - Alterar Operador" +
                                    "\n6 - Sair");
                        Console.Write("Informe a opção desejada: ");

                        int opc;
                        while (!int.TryParse(Console.ReadLine(), out opc) || opc < 1 || opc > 6)
                        {
                            Console.WriteLine("Valor Inválido");
                            Console.Write("Informe a opção desejada: ");
                        }
                        switch (opc)
                        {
                            case 1:
                                if (_autenticacao.ValidarPermissaoCaixa(usuarioConectado))
                                {
                                    await _mCaixa.ExibirMenu(usuarioConectado);
                                }
                                else Console.ReadKey();
                                break;
                            case 2:
                                if (_autenticacao.ValidarPermissaoEstoque(usuarioConectado))
                                {
                                    await _mEstoque.ExibirMenu(usuarioConectado);
                                }
                                else Console.ReadKey();
                                break;
                            case 3:
                                if (_autenticacao.ValidarPermissaoGerente(usuarioConectado))
                                {
                                    await _mGerencial.ExibirMenu(usuarioConectado);
                                }
                                else Console.ReadKey();
                                break;
                            case 4:
                                await _usuario.ExibirUsuario(usuarioConectado);
                                break;
                            case 5:
                                menu = false;
                                login = false;
                                break;
                            case 6:
                                Console.WriteLine("\nSISTEMA ENCERRADO !\n\n");
                                Environment.Exit(0);
                                break;
                        }
                    } while (menu);
                }
            } while (!login);
        }
    }
}
