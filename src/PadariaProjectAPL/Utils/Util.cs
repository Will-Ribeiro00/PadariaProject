using Microsoft.EntityFrameworkCore;
using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.Context;

namespace PadariaProjectAPL.Utils
{
    public class Util
    {
        private readonly PadariaDbContext _context;
        public Util(PadariaDbContext context) => _context = context;


        public async Task<Produtos> LocalizarProduto()
        {
            Console.Write("Informe o código do produto: ");
            int codigoProduto;
            while (!int.TryParse(Console.ReadLine(), out codigoProduto))
            {
                Console.WriteLine("Valor Inválido");
                Console.Write("Informe o código do produto: ");
            }

            var produto = await _context.PRODUTO
                                        .Include(p => p.Categoria)
                                        .Include(p => p.UnidadeMedida)
                                        .Include(p => p.Estoque)
                                        .FirstOrDefaultAsync(p => p.COD_PRODUTO == codigoProduto);

            if (produto == null)
            {
                Console.WriteLine("Não foi localizado nenhum produto com o código informado!");
                return null;
            }

            Console.WriteLine($"\nCategoria........: {produto.Categoria.CATEGORIA}" +
                              $"\nCódigo do produto: {produto.COD_PRODUTO}" +
                              $"\nDescrição........: {produto.DESCRICAO}" +
                              $"\nPreço............: R${produto.PRECO} {produto.UnidadeMedida.UNIDADE_MEDIDA}" +
                              $"\nEstoque..........: {produto.Estoque.QUANTIDADE} {produto.UnidadeMedida.UNIDADE_MEDIDA}");

            return produto;
        }

        public async Task<Categorias> LocalizarCategoria()
        {
            Console.Write("Informe o código da categoria: ");
            int codigoCategoria;
            while (!int.TryParse(Console.ReadLine(), out codigoCategoria))
            {
                Console.WriteLine("Valor Inválido");
                Console.Write("Informe o código da categoria: ");
            }

            var categoria = await _context.CATEGORIA
                                          .FirstOrDefaultAsync(c => c.COD_CATEGORIA == codigoCategoria);

            if (categoria == null)
            {
                Console.WriteLine("Não foi possível localizar uma categoria com o código fornecido!");
                return null;
            }

            Console.WriteLine("\nCATEGORIA:");
            Console.WriteLine($"#{categoria.COD_CATEGORIA} - {categoria.CATEGORIA}");

            return categoria;
        }

        public async Task<Funcionarios> LocalizarFuncionario()
        {
            Console.Write("Informe o código do funcionário: ");
            int codigoFuncionario;
            while (!int.TryParse(Console.ReadLine(), out codigoFuncionario))
            {
                Console.WriteLine("Valor Inválido");
                Console.Write("Informe o código do funcionário: ");
            }

            var funcionario = await _context.FUNCIONARIO
                                            .Include(f => f.Endereco)
                                            .Include(f => f.Cargo)
                                            .Include(f => f.Status)
                                            .FirstOrDefaultAsync(f => f.COD_FUNCIONARIO == codigoFuncionario);

            if (funcionario == null)
            {
                Console.WriteLine("Não foi localizado nenhum funcionário com o código informado!");
                return null;
            }

            Console.WriteLine($"Funcionário #{funcionario.COD_FUNCIONARIO}" +
                              $"\nNome...........: {funcionario.NOME}" +
                              $"\nCargo..........: {funcionario.Cargo.CARGO}" +
                              $"\nCPF............: {funcionario.CPF}" +
                              $"\nCelular........: {funcionario.CELULAR}" +
                              $"\nSalário........: {funcionario.SALARIO}" +
                              $"\nStatus.........: {funcionario.Status.STATUS}" +
                              $"\nEndereço.......: {funcionario.Endereco.ENDERECO}");

            return funcionario;
        }

        public async Task<Clientes> LocalizarCliente()
        {
            Console.Write("Informe o código do cliente: ");
            int codigoCliente;
            while (!int.TryParse(Console.ReadLine(), out codigoCliente))
            {
                Console.WriteLine("Valor Inválido");
                Console.Write("Informe o código do cliente: ");
            }

            var cliente = await _context.CLIENTE
                                         .Include(c => c.Endereco)
                                         .Include(c => c.Fidelidade)
                                         .FirstOrDefaultAsync(c => c.COD_CLIENTE == codigoCliente);

            if (cliente == null)
            {
                Console.WriteLine("Não foi localizado nenhum cliente com o código informado!");
                return null;
            }

            Console.WriteLine($"Cliente #{cliente.COD_CLIENTE}" +
                              $"\nNome...........: {cliente.NOME}" +
                              $"\nCPF............: {cliente.CPF}" +
                              $"\nCelular........: {cliente.CELULAR}" +
                              $"\nE-mail.........: {cliente.EMAIL}" +
                              $"\nPontos.........: {cliente.Fidelidade.PONTOS}" +
                              $"\nEndereço.......: {cliente.Endereco.ENDERECO}");

            return cliente;
        }

        public bool ConfirmarExclusao()
        {
            Console.Write("Deseja prosseguir com a exclusão ? ( S / N ): ");
            var decisao = Console.ReadLine();
            if (!decisao.Equals("s", StringComparison.OrdinalIgnoreCase) && !decisao.Equals("sim", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Operação cancelada!");
                return false;
            }
            return true;
        }

        public async Task<Clientes> IdentificarCliente()
        {
            Console.WriteLine("Cliente deseja se identificar ( S / N ) : ");
            var resposta = Console.ReadLine();

            if (resposta.Equals("s", StringComparison.OrdinalIgnoreCase) || resposta.Equals("sim", StringComparison.OrdinalIgnoreCase))
            {
                Clientes cliente = null;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Informe o código do cliente: ");
                    int codigoCliente;
                    while (!int.TryParse(Console.ReadLine(), out codigoCliente))
                    {
                        Console.WriteLine("Valor Inválido!");
                        Console.Write("Informe o código do cliente: ");
                    }

                    cliente = await _context.CLIENTE.FirstOrDefaultAsync(c => c.COD_CLIENTE == codigoCliente);
                    if (cliente == null)
                    {
                        Console.WriteLine("Não foi possível localizar nenhum cliente com esse código");
                        Console.WriteLine("Deseja tentar novamente ( S / N )?");
                        resposta = Console.ReadLine();
                        if (!resposta.Equals("s", StringComparison.OrdinalIgnoreCase) && !resposta.Equals("sim", StringComparison.OrdinalIgnoreCase))
                        {
                            return await _context.CLIENTE.FirstOrDefaultAsync(c => c.COD_CLIENTE == 1);
                        }
                    }
                } while (cliente == null);

                Console.WriteLine($"Cliente: {cliente.COD_CLIENTE} - {cliente.NOME}");
                Console.ReadKey();

                return cliente;
            }

            var clientePadrao = await _context.CLIENTE.FirstOrDefaultAsync(c => c.COD_CLIENTE == 1);
            return clientePadrao;
        }

        public async Task<List<PedidosProdutos>> ExibirProdutosDoPedido(Pedidos pedido)
        {
            int index = 1;

            var pedidoAtual = await _context.PEDIDO.Include(p => p.PedidosProdutos).ThenInclude(pp => pp.Produto).ThenInclude(p => p.UnidadeMedida)
                                                   .Include(p => p.PedidosProdutos).ThenInclude(pp => pp.Produto).ThenInclude(p => p.Estoque)
                                                   .FirstOrDefaultAsync(p => p.COD_PEDIDO == pedido.COD_PEDIDO);

            if (pedidoAtual.PedidosProdutos.Count == 0)
            {
                Console.WriteLine("Nenhum produto adicionado até o momento!");
            }
            else
            {
                decimal subtotal = 0;
                Console.WriteLine("Item   Cód    Produto                 V.Unitário   Qtd       Total");

                foreach (var item in pedido.PedidosProdutos.ToList())
                {
                    var cod = item.PRODUTO_FK.ToString().PadRight(4);
                    var descricao = item.Produto.DESCRICAO.PadRight(21);
                    var vUnitario = item.Produto.PRECO;
                    var qtd = item.QUANTIDADE;
                    var totalProduto = (decimal)item.QUANTIDADE * item.Produto.PRECO;
                    var uMedida = item.Produto.UnidadeMedida.UNIDADE_MEDIDA;

                    var nItem = item.INDEX_ = index;
                    nItem.ToString().PadRight(4);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"{nItem}      {cod}   {descricao}   {vUnitario:C}{uMedida}   {qtd:F3}{uMedida}   {totalProduto:C}");

                    subtotal += totalProduto;
                    index++;
                }
                Console.WriteLine($"Subtotal...................................................: {subtotal:C}");
            }

            return pedido.PedidosProdutos.ToList();
        }

        public void Cabecalho(Funcionarios usuarioConectado)
        {
            Console.WriteLine("*********MERCADINHO DA VILA*********" +
                              "\n---------------------------------------" +
                             $"\n#{usuarioConectado.COD_FUNCIONARIO} - {usuarioConectado.Cargo.CARGO} - {usuarioConectado.NOME}" +
                             $"\n---------------------------------------");
        }

        public DateTime DataInicial()
        {
            Console.Write("Informe a data inicial (dd/mm/yyyy): ");
            DateTime dataInicial;
            while (!DateTime.TryParse(Console.ReadLine(), out dataInicial))
            {
                Console.WriteLine("Valor Inválido");
                Console.Write("Informe a data inicial (dd/mm/yyyy): ");
            }
            return dataInicial;
        }

        public DateTime DataFinal()
        {
            Console.Write("Informe a data final (dd/mm/yyyy): ");
            DateTime dataFinal;
            while (!DateTime.TryParse(Console.ReadLine(), out dataFinal))
            {
                Console.WriteLine("Valor Inválido");

            }
            return dataFinal.AddDays(1).AddSeconds(-1);
        }
    }
}
