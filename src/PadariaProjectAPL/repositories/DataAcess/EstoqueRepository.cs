using Microsoft.EntityFrameworkCore;
using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.Context;
using PadariaProjectAPL.Utils;

namespace PadariaProjectAPL.repositories.DataAcess
{
    public class EstoqueRepository
    {
        private readonly PadariaDbContext _context;
        private readonly PedidoProdutoRepository _ppRepository;
        private readonly Util _util;
        public EstoqueRepository(PadariaDbContext context, Util util, PedidoProdutoRepository ppRepository)
        {
            _context = context;
            _util = util;
            _ppRepository = ppRepository;
        }

        public async Task ReceberProduto(Funcionarios funcionario)
        {
            double qtd;
            int cod = 0;
            var loop = true;
            do
            {
                Console.Clear();
                Console.Write("Informe o código do produto: ");
                while (!int.TryParse(Console.ReadLine(), out cod))
                {
                    Console.WriteLine("Valor Inválido.");
                    Console.Write("Informe o código do produto: ");
                }

                var produto = await _context.PRODUTO.Include(p => p.Estoque).FirstOrDefaultAsync(p => p.COD_PRODUTO == cod);

                if (produto == null)
                {
                    Console.WriteLine("Não foi localizado nenhum produto com o código informado!");
                    return;
                }
                Console.WriteLine($"{produto.DESCRICAO} - Quantidade Atual: {produto.Estoque.QUANTIDADE} {produto.UnidadeMedida.UNIDADE_MEDIDA}");
                Console.Write("\nInforme a quantidade: ");
                while (!double.TryParse(Console.ReadLine(), out qtd))
                {
                    Console.WriteLine("Valor Inválido.");
                    Console.Write("Informe a quantidade: ");
                }

                var recebimento = new EstoqueMovimentos()
                {
                    ESTOQUE_FK = produto.Estoque.COD_ESTOQUE,
                    FUNCIONARIO_FK = funcionario.COD_FUNCIONARIO,
                    QUANTIDADE = qtd,
                    TIPO_MOVIMENTO_FK = 1,
                    DATA_ESTOQUE = DateTime.Now
                };
                produto.Estoque.QUANTIDADE += recebimento.QUANTIDADE;


                await _context.ESTOQUE_MOVIMENTO.AddAsync(recebimento);
                await _context.SaveChangesAsync();

                Console.WriteLine("Produto recebido com sucesso!");
                Console.WriteLine($"\n#{produto.COD_PRODUTO} - {produto.DESCRICAO} - Quantidade atual: {produto.Estoque.QUANTIDADE} {produto.UnidadeMedida.UNIDADE_MEDIDA}\n");


                Console.WriteLine("\nDeseja receber mais produtos ? (S / N): ");
                var opc = Console.ReadLine();
                if (!opc.Equals("s", StringComparison.InvariantCultureIgnoreCase) && !opc.Equals("sim", StringComparison.InvariantCultureIgnoreCase))
                {
                    loop = false;
                }
            } while (loop);
        }
        public async Task Inventario(Funcionarios funcionarios)
        {
            Console.Clear();

            var produtos = await _context.PRODUTO.Include(p => p.Categoria)
                                   .Include(p => p.Estoque)
                                   .Include(p => p.UnidadeMedida)
                                   .ToListAsync();

            Console.Write("Informe a categoria: ");
            int categoria;
            while (!int.TryParse(Console.ReadLine(), out categoria))
            {
                Console.Write("Valor Inválido!" +
                              "\nInforme a categoria: ");
            }

            var produtosCategoria = produtos.Where(p => p.CATEGORIA_FK == categoria).ToList();

            foreach (var item in produtosCategoria)
            {
                Console.Clear();

                Console.WriteLine($"***Categoria {item.Categoria.CATEGORIA}***");
                Console.WriteLine($"Produto {produtosCategoria.IndexOf(item) + 1} de {produtosCategoria.Count}");
                Console.WriteLine($"Produto: {item.DESCRICAO} - Quantidade Atual: {item.Estoque.QUANTIDADE}{item.UnidadeMedida.UNIDADE_MEDIDA}");

                Console.Write("\n1 - Inventariar" +
                              "\n2 - Proximo Item" +
                              "\n3 - Finalizar Inventário" +
                              "\nEscolha a opção desejada: ");
                int opc;
                while (!int.TryParse(Console.ReadLine(), out opc) || opc < 1 || opc > 3)
                {
                    Console.Write("Valor Inválido!" +
                                  "\nInforme a opção desejada: ");
                }

                switch (opc)
                {
                    case 1:
                        Console.Write("Informe a quantidade: ");
                        double qtd;
                        while (!double.TryParse(Console.ReadLine(), out qtd))
                        {
                            Console.Write("Valor Inválido!" +
                                              "\nInforme a quantidade: ");
                        }
                        item.Estoque.QUANTIDADE = qtd;
                        var itemInventariado = new EstoqueMovimentos()
                        {
                            ESTOQUE_FK = item.Estoque.COD_ESTOQUE,
                            FUNCIONARIO_FK = funcionarios.COD_FUNCIONARIO,
                            QUANTIDADE = qtd,
                            TIPO_MOVIMENTO_FK = 3,
                            DATA_ESTOQUE = DateTime.Now,
                        };
                        await _context.ESTOQUE_MOVIMENTO.AddAsync(itemInventariado);
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Produto inventariado com sucesso!");
                        break;
                    case 2:
                        continue;
                    case 3:
                        Console.WriteLine("Inventário finalizado!");
                        return;
                }
            }
        }
        public async Task SaidaRealEstoque(Pedidos pedido)
        {
            var produtos = await _util.ExibirProdutosDoPedido(pedido);

            foreach (var produto in produtos)
            {
                var saidaEstoque = new EstoqueMovimentos()
                {
                    ESTOQUE_FK = produto.Produto.Estoque.COD_ESTOQUE,
                    FUNCIONARIO_FK = produto.Pedido.FUNCIONARIO_FK,
                    TIPO_MOVIMENTO_FK = 2,
                    QUANTIDADE = produto.QUANTIDADE,
                    DATA_ESTOQUE = DateTime.Now
                };
                produto.Produto.Estoque.QUANTIDADE -= produto.QUANTIDADE;
                await _context.ESTOQUE_MOVIMENTO.AddAsync(saidaEstoque);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<int> SacolaDoPedido(Pedidos novoPedido, Funcionarios funcionario)
        {
            int opc = 0;
            do
            {
                Console.Clear();

                await _util.ExibirProdutosDoPedido(novoPedido);

                Console.Write("\n1 - Adicionar Produto" +
                              "\n2 - Remover Produto" +
                              "\n3 - Atualizar Quantidade do Produto" +
                              "\n4 - Ir para Pagamentos" +
                              "\n5 - Cancelar Pedido" +
                              "\nInforme a opção desejada: ");
                while (!int.TryParse(Console.ReadLine(), out opc) || opc < 1 || opc > 5)
                {
                    Console.Write("Valor Inválido!" +
                                  "\nInforme a opção desejada: ");
                }

                switch (opc)
                {
                    case 1:
                        Console.Clear();
                        await _ppRepository.AdicionarProdutoAoPedido(novoPedido);
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        await _ppRepository.RemoverProdutoDoPedido(novoPedido);
                        Console.ReadKey();
                        break;
                    case 3:
                        Console.Clear();
                        await _ppRepository.AtualizarQuantidadeDoProduto(novoPedido);
                        Console.ReadKey();
                        break;
                    case 5:
                        Console.Clear();
                        Console.Write("Deseja realmente cancelar o pedido ? ( S / N ): ");
                        var decisao = Console.ReadLine();
                        if (decisao.Equals("s", StringComparison.OrdinalIgnoreCase) || decisao.Equals("sim", StringComparison.OrdinalIgnoreCase))
                        {
                            _context.PEDIDO_PRODUTO.RemoveRange(novoPedido.PedidosProdutos);
                            await _context.SaveChangesAsync();
                            Console.WriteLine("Pedido cancelado!");
                            Console.ReadKey();
                            return 1;
                        }
                        else
                        {
                            Console.WriteLine("Pedido não cancelado!");
                            Console.ReadKey();
                            break;
                        }
                }

            } while (opc != 4);
            return 0;
        }
    }
}


