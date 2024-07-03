using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.DataAcess;
using PadariaProjectAPL.Utils;

namespace PadariaProjectAPL.Views.Menu.MenuEstoque
{
    public class MenuEstoqueView
    {
        private readonly EstoqueRepository _eRepository;
        private readonly CategoriaRepository _cRepository;
        private readonly ProdutoRepository _pRepository;
        private readonly Util _util;

        public MenuEstoqueView(EstoqueRepository eRepository, CategoriaRepository cRepository, ProdutoRepository pRepository, Util util)
        {
            _eRepository = eRepository;
            _cRepository = cRepository;
            _pRepository = pRepository;
            _util = util;
        }

        public async Task ExibirMenu(Funcionarios funcionario)
        {
            do
            {
                Console.Clear();
                _util.Cabecalho(funcionario);
                Console.WriteLine("\n************ESTOQUE*************");
                Console.WriteLine("1 - Receber Mercadoria" +
                              "\n2 - Criar Novo Produto" +
                              "\n3 - Pesquisar Produto" +
                              "\n4 - Atualizar Produto" +
                              "\n5 - Deletar Produto" +
                              "\n6 - Criar Nova Categoria" +
                              "\n7 - Pesquisar Categoria" +
                              "\n8 - Atualizar Categoria" +
                              "\n9 - Deletar Categoria" +
                              "\n10 - Realizar Inventário" +
                              "\n11 - Voltar");
                Console.Write("Informe a opção desejada: ");
                int opc;
                while (!int.TryParse(Console.ReadLine(), out opc) || opc < 1 || opc > 11)
                {
                    Console.WriteLine("Valor Inválido");
                    Console.Write("Informe a opção desejada: ");
                }
                switch (opc)
                {
                    case 1:
                        Console.Clear();
                        await _eRepository.ReceberProduto(funcionario);
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        await _pRepository.CriarProduto();
                        Console.ReadKey();
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("1 - Por código" +
                                          "\n2 - Por categoria" +
                                          "\n3 - Todos" +
                                          "\n4 - Voltar");
                        Console.Write("Informe a opção desejada: ");
                        int opcProduto;
                        while (!int.TryParse(Console.ReadLine(), out opcProduto) || opcProduto < 1 || opcProduto > 4)
                        {
                            Console.WriteLine("Valor Inválido!");
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
                    case 4:
                        Console.Clear();
                        await _pRepository.EditarProduto();
                        Console.ReadKey();
                        break;
                    case 5:
                        Console.Clear();
                        await _pRepository.DeletarProduto();
                        Console.ReadKey();
                        break;
                    case 6:
                        Console.Clear();
                        await _cRepository.CriarCategoria();
                        Console.ReadKey();
                        break;
                    case 7:
                        Console.Clear();
                        await _cRepository.ExibirCategorias();
                        Console.ReadKey();
                        break;
                    case 8:
                        Console.Clear();
                        await _cRepository.AtualizarCategoria();
                        Console.ReadKey();
                        break;
                    case 9:
                        Console.Clear();
                        await _cRepository.DeletarCategoria();
                        Console.ReadKey();
                        break;
                    case 10:
                        Console.Clear();
                        await _eRepository.Inventario(funcionario);
                        Console.ReadKey();
                        break;
                    case 11:
                        return;
                }
            } while (true);


        }
    }
}
