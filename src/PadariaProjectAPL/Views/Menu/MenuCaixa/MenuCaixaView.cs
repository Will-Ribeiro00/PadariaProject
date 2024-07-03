using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.DataAcess;
using PadariaProjectAPL.Utils;

namespace PadariaProjectAPL.Views.Menu.MenuCaixa
{
    public class MenuCaixaView
    {
        private readonly ProdutoRepository _pRepository;
        private readonly ClienteRepository _cRepository;
        private readonly PedidoRepository _peRepository;
        private readonly Util _util;
        public MenuCaixaView(ProdutoRepository pRepository, ClienteRepository cRepository, PedidoRepository peRepository, Util util)
        {
            _cRepository = cRepository;
            _pRepository = pRepository;
            _peRepository = peRepository;
            _util = util;
        }

        public async Task ExibirMenu(Funcionarios funcionario)
        {
            do
            {
                Console.Clear();
                _util.Cabecalho(funcionario);
                Console.WriteLine("\n*************CAIXA**************");
                Console.WriteLine("1 - Nova Venda" +
                              "\n2 - Pesquisar Produto" +
                              "\n3 - Pesquisar Cliente" +
                              "\n4 - Cadastrar Cliente" +
                              "\n5 - Atualizar Cliente" +
                              "\n6 - Excluir Cliente" +
                              "\n7 - Voltar");
                Console.Write("Informe a opção desejada: ");
                int opc;
                while (!int.TryParse(Console.ReadLine(), out opc) || opc < 1 || opc > 7)
                {
                    Console.WriteLine("Valor Inválido");
                    Console.Write("Informe a opção desejada: ");
                }

                switch (opc)
                {
                    case 1:
                        Console.Clear();
                        await _peRepository.NovoPedido(funcionario);
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("1 - Por código" +
                                          "\n2 - Por categoria" +
                                          "\n3 - Todos" +
                                          "\n4 - Voltar");
                        Console.Write("Informe a opção desejada: ");
                        int opcProduto;
                        while (!int.TryParse(Console.ReadLine(), out opcProduto) || opcProduto < 1 || opcProduto > 4)
                        {
                            Console.WriteLine("Valor Inválido");
                            Console.Write("Informe a opção desejada: ");
                        }
                        switch (opcProduto)
                        {
                            case 1:
                                await _pRepository.ExibirProdutoPorCodigo();
                                Console.ReadKey();
                                break;
                            case 2:
                                await _pRepository.ExibirProdutosPorCategoria();
                                Console.ReadKey();
                                break;
                            case 3:
                                await _pRepository.ExibirTodosProdutos();
                                Console.ReadKey();
                                break;
                            case 4:
                                return;
                        }
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("1 - Por código" +
                                          "\n2 - Todos" +
                                          "\n3 - Voltar");
                        Console.Write("Informe a opção desejada: ");
                        int opcCliente;
                        while (!int.TryParse(Console.ReadLine(), out opcCliente) || opcCliente < 1 || opcCliente > 3)
                        {
                            Console.WriteLine("Valor Inválido");
                            Console.Write("Informe a opção desejada: ");
                        }
                        switch (opcCliente)
                        {
                            case 1:
                                await _cRepository.ExibirClientePorCodigo();
                                Console.ReadKey();
                                break;
                            case 2:
                                await _cRepository.ExibirTodosClientes();
                                Console.ReadKey();
                                break;
                            case 3:
                                return;
                        }
                        break;
                    case 4:
                        Console.Clear();
                        await _cRepository.CadastrarCliente(funcionario);
                        Console.ReadKey();
                        break;
                    case 5:
                        Console.Clear();
                        await _cRepository.AtualizarCliente();
                        Console.ReadKey();
                        break;
                    case 6:
                        Console.Clear();
                        await _cRepository.DeletarCliente();
                        Console.ReadKey();
                        break;
                    case 7:
                        return;
                }
            } while (true);

        }
    }
}
