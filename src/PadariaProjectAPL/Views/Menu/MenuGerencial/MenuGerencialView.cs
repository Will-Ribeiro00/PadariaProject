using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.DataAcess;
using PadariaProjectAPL.Utils;

namespace PadariaProjectAPL.Views.Menu.MenuGerencial
{
    public class MenuGerencialView
    {
        private readonly GerenteRepository _gRepository;
        private readonly FuncionarioRepository _fRepository;
        private readonly Util _util;

        public MenuGerencialView(GerenteRepository gRepository, FuncionarioRepository fRepository, Util util)
        {
            _gRepository = gRepository;
            _fRepository = fRepository;
            _util = util;
        }

        public async Task ExibirMenu(Funcionarios funcionario)
        {
            do
            {
                Console.Clear();
                _util.Cabecalho(funcionario);
                Console.WriteLine("\n***********GERENCIAL************");
                Console.WriteLine("1 - Cadastrar Funcionário" +
                                  "\n2 - Pesquisar Funcionário" +
                                  "\n3 - Editar Funcionário" +
                                  "\n4 - Resetar Senha Funcionário" +
                                  "\n5 - Deletar Funcionário" +
                                  "\n6 - Relatorio Faturamento" +
                                  "\n7 - Relatorio Estoque" +
                                  "\n8 - Relatorio Cliente" +
                                  "\n9 - Voltar");
                Console.Write("Informe a opção desejada: ");
                int opc;
                while (!int.TryParse(Console.ReadLine(), out opc) || opc < 1 || opc > 9)
                {
                    Console.WriteLine("Valor Inválido");
                    Console.Write("Informe a opção desejada: ");
                }
                switch (opc)
                {
                    case 1:
                        Console.Clear();
                        await _fRepository.CadastrarFuncionario();
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("1 - Por código" +
                                          "\n2 - Por cargo" +
                                          "\n3 - Todos" +
                                          "\n4 - Voltar");
                        Console.Write("Informe a opção desejada: ");
                        int opcFuncionario;
                        while (!int.TryParse(Console.ReadLine(), out opcFuncionario) || opcFuncionario < 1 || opcFuncionario > 4)
                        {
                            Console.WriteLine("Valor Inválido");
                            Console.Write("Informe a opção desejada: ");
                        }
                        switch (opcFuncionario)
                        {
                            case 1:
                                await _fRepository.ExibirFuncionarioPorCodigo();
                                Console.ReadKey();
                                break;
                            case 2:
                                await _fRepository.ExibirFuncionariosPorCargo();
                                Console.ReadKey();
                                break;
                            case 3:
                                await _fRepository.ExibirTodosFuncionarios();
                                Console.ReadKey();
                                break;
                            case 4:
                                return;

                        }
                        break;
                    case 3:
                        Console.Clear();
                        await _fRepository.AtualizarFuncionario();
                        break;
                    case 4:
                        Console.Clear();
                        await _gRepository.ResetarSenhaFuncionario();
                        break;
                    case 5:
                        Console.Clear();
                        await _fRepository.DeletarFuncionario();
                        Console.ReadKey();
                        break;
                    case 6:
                        Console.Clear();
                        Console.WriteLine("1 - Por período" +
                                          "\n2 - Anual" +
                                          "\n3 - Voltar");
                        Console.Write("Informe a opção desejada: ");
                        int opcFaturamento;
                        while (!int.TryParse(Console.ReadLine(), out opcFaturamento) || opcFaturamento < 1 || opcFaturamento > 3)
                        {
                            Console.WriteLine("Valor Inválido!");
                            Console.Write("Informe a opção desejada: ");
                        }
                        switch (opcFaturamento)
                        {
                            case 1:
                                await _gRepository.FaturamentoPorPeriodo();
                                Console.ReadKey();
                                break;
                            case 2:
                                await _gRepository.FaturamentoAnual();
                                Console.ReadKey();
                                break;
                            case 3:
                                continue;
                        }
                        break;
                    case 7:
                        Console.Clear();
                        Console.Clear();
                        Console.WriteLine("1 - Recebimentos" +
                                          "\n2 - Inventários" +
                                          "\n3 - Voltar");
                        Console.Write("Informe a opção desejada: ");
                        int opcEstoque;
                        while (!int.TryParse(Console.ReadLine(), out opcEstoque) || opcEstoque < 1 || opcEstoque > 3)
                        {
                            Console.WriteLine("Valor Inválido!");
                            Console.Write("Informe a opção desejada: ");
                        }
                        switch (opcEstoque)
                        {
                            case 1:
                                Console.Clear();
                                await _gRepository.RelatorioRecebimentoProdutoPorPeriodo();
                                Console.ReadKey();
                                break;
                            case 2:
                                Console.Clear();
                                await _gRepository.RelatorioInventarioPorPeriodo();
                                Console.ReadKey();
                                break;
                            case 3:
                                continue;
                        }
                        break;
                    case 8:
                        Console.Clear();
                        Console.Clear();
                        Console.WriteLine("1 - Cadastros" +
                                          "\n2 - Pedidos de um Cliente" +
                                          "\n3 - Voltar");
                        Console.Write("Informe a opção desejada: ");
                        int opcCliente;
                        while (!int.TryParse(Console.ReadLine(), out opcCliente) || opcCliente < 1 || opcCliente > 3)
                        {
                            Console.WriteLine("Valor Inválido!");
                            Console.Write("Informe a opção desejada: ");
                        }
                        switch (opcCliente)
                        {
                            case 1:

                                Console.Clear();
                                await _gRepository.RelatorioCadastroClientePorPeriodo();
                                Console.ReadKey();
                                break;
                            case 2:
                                Console.Clear();
                                await _gRepository.RelatiorioPedidosClientePorPeriodo();
                                Console.ReadKey();
                                break;
                            case 3:
                                continue;
                        }
                        break;
                    case 9:
                        return;
                }
            } while (true);
        }
    }
}
