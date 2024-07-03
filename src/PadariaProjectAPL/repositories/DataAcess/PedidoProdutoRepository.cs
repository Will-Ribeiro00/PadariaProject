using Microsoft.EntityFrameworkCore;
using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.Context;
using PadariaProjectAPL.Utils;

namespace PadariaProjectAPL.repositories.DataAcess
{
    public class PedidoProdutoRepository
    {
        private readonly PadariaDbContext _context;
        private readonly Util _util;
        public PedidoProdutoRepository(PadariaDbContext context, Util util)
        {
            _context = context;
            _util = util;
        }

        public async Task AdicionarProdutoAoPedido(Pedidos pedido)
        {
            var pedidoAtual = await _util.ExibirProdutosDoPedido(pedido);

            Console.Write("\nInforme o código do produto: ");
            int codProduto;
            while (!int.TryParse(Console.ReadLine(), out codProduto))
            {
                Console.Write("Valor Inválido" +
                              "\nInforme o código do produto: ");
            }

            var produto = await _context.PRODUTO.Include(p => p.Estoque).FirstOrDefaultAsync(p => p.COD_PRODUTO == codProduto);

            if (produto == null)
            {
                Console.WriteLine("Não foi localizado nenhum produto com o cód informado!");
                return;
            }
            if (pedidoAtual.Exists(pa => pa.PRODUTO_FK == produto.COD_PRODUTO))
            {
                Console.WriteLine($"O produto informado já existe no pedido!");
                return;
            }

            Console.WriteLine($"#{produto.COD_PRODUTO} - {produto.DESCRICAO}");

            Console.Write("Informe a quantidade:");
            double qtd;
            while (!double.TryParse(Console.ReadLine(), out qtd))
            {
                Console.Write("Valor Inválido" +
                              "\nInforme a quantidade: ");
            }
            var estoque = produto.Estoque.QUANTIDADE;
            if (estoque < qtd)
            {
                Console.Write($"Quantidade {qtd} ultrapassa o estoque atual de {produto.Estoque.QUANTIDADE}.");
                Console.WriteLine("Produto não adicionado."); return;
            }
            var novoProduto = new PedidosProdutos()
            {
                PRODUTO_FK = produto.COD_PRODUTO,
                PEDIDO_FK = pedido.COD_PEDIDO,
                QUANTIDADE = qtd
            };
            await _context.PEDIDO_PRODUTO.AddAsync(novoProduto);
            await _context.SaveChangesAsync();

            Console.WriteLine("Produto adicionado com sucesso!");
        }
        public async Task RemoverProdutoDoPedido(Pedidos pedido)
        {
            var pedidoAtual = await _util.ExibirProdutosDoPedido(pedido);
            if (pedidoAtual.Count() == 0)
            {
                Console.WriteLine("O pedido está vazio!"); return;
            }


            Console.Write("\nInforme o Nº do item: ");
            int codItem;
            while (!int.TryParse(Console.ReadLine(), out codItem))
            {
                Console.Write("Valor Inválido" +
                              "\nInforme o código do produto: ");
            }

            var produto = await _context.PEDIDO_PRODUTO.FirstOrDefaultAsync(pp => pp.INDEX_ == codItem && pp.PEDIDO_FK == pedido.COD_PEDIDO);
            if (produto == null)
            {
                Console.WriteLine("Não foi localizado nenhum produto com o Nº do item informado!");
                return;
            }

            _context.Remove(produto);
            await _context.SaveChangesAsync();
            Console.WriteLine("Item removido com sucesso!");
        }
        public async Task AtualizarQuantidadeDoProduto(Pedidos pedido)
        {
            var pedidoAtual = await _util.ExibirProdutosDoPedido(pedido);
            if (pedidoAtual.Count() == 0)
            {
                Console.WriteLine("O pedido está vazio!"); return;
            }


            Console.Write("\nInforme o Nº do item: ");
            int codItem;
            while (!int.TryParse(Console.ReadLine(), out codItem))
            {
                Console.Write("Valor Inválido" +
                              "\nInforme o código do produto: ");
            }

            var produto = await _context.PEDIDO_PRODUTO.Include(pp => pp.Produto).ThenInclude(p => p.Estoque).FirstOrDefaultAsync(pp => pp.INDEX_ == codItem && pp.PEDIDO_FK == pedido.COD_PEDIDO);
            if (produto == null)
            {
                Console.WriteLine("Não foi localizado nenhum produto com o Nº do item informado!");
                return;
            }

            Console.Write("Informe a quantidade: ");
            double qtd;
            while (!double.TryParse(Console.ReadLine(), out qtd))
            {
                Console.Write("Valor Inválido" +
                              "\nInforme o código do produto: ");
            }

            var estoque = produto.Produto.Estoque.QUANTIDADE;
            if (estoque < qtd)
            {
                Console.Write($"Quantidade {qtd} ultrapassa o estoque atual de {produto.Produto.Estoque.QUANTIDADE}.");
                Console.WriteLine("Quantidade não atualizada!"); return;
            }
            produto.QUANTIDADE = qtd;
            await _context.SaveChangesAsync();
            Console.WriteLine("quantidade atualizada!");
        }
    }
}
