using Microsoft.EntityFrameworkCore;
using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.Context;
using PadariaProjectAPL.Utils;

namespace PadariaProjectAPL.repositories.DataAcess
{
    public class ProdutoRepository
    {
        private readonly PadariaDbContext _context;
        private readonly Util _util;
        public ProdutoRepository(PadariaDbContext context, Util util)
        {
            _context = context;
            _util = util;
        }

        public async Task CriarProduto()
        {
            bool loop = true;
            do
            {
                Console.Clear();
                Console.Write("Informe o codigo da categoria: ");
                int categoria;
                while (!int.TryParse(Console.ReadLine(), out categoria))
                {
                    Console.WriteLine("Valor Inválido!");
                    Console.Write("Informe o codigo da categoria: ");
                }

                Console.Write("Informe o produto a ser adicionado: ");
                var produto = Console.ReadLine();

                Console.Write("Informe a unidade de medida (1-Un  2-Kg  3-Lt): ");
                int unMedida;
                while (!int.TryParse(Console.ReadLine(), out unMedida) || unMedida < 1 || unMedida > 3)
                {
                    Console.WriteLine("Valor Inválido");
                    Console.Write("Informe a unidade de medida (1-Kg  2-Un  3-Lt): ");
                }

                Console.Write("Informe o valor de venda do produto: ");
                decimal valor;
                while (!decimal.TryParse(Console.ReadLine(), out valor))
                {
                    Console.WriteLine("Valor Inválido!");
                    Console.Write("Informe o valor de venda do produto: ");
                }

                var novoProduto = new Produtos()
                {
                    CATEGORIA_FK = categoria,
                    DESCRICAO = produto,
                    PRECO = valor,
                    UNIDADE_MEDIDA_FK = unMedida
                };
                await _context.PRODUTO.AddAsync(novoProduto);
                await _context.SaveChangesAsync();

                var novoEstoque = new Estoques()
                {
                    COD_ESTOQUE = novoProduto.COD_PRODUTO,
                    PRODUTO_FK = novoProduto.COD_PRODUTO,
                    QUANTIDADE = 0
                };
                await _context.ESTOQUE.AddAsync(novoEstoque);
                await _context.SaveChangesAsync();


                Console.WriteLine("Produto adicionado com sucesso!");

                Console.WriteLine("\nDeseja adicionar mais produtos ? (S / N): ");
                var opc = Console.ReadLine();
                if (!opc.Equals("s", StringComparison.InvariantCultureIgnoreCase) && !opc.Equals("sim", StringComparison.InvariantCultureIgnoreCase))
                {
                    loop = false;
                }
            } while (loop);

        }
        public async Task ExibirProdutoPorCodigo()
        {
            await _util.LocalizarProduto();
        }
        public async Task ExibirProdutosPorCategoria()
        {
            var categoria = await _util.LocalizarCategoria();
            var produtos = await _context.PRODUTO.Include(p => p.Categoria)
                                           .Include(p => p.UnidadeMedida)
                                           .Include(p => p.Estoque)
                                           .Where(p => p.Categoria.COD_CATEGORIA == categoria.COD_CATEGORIA)
                                           .ToListAsync();

            foreach (var produto in produtos)
            {
                Console.WriteLine($"\nCódigo do produto: {produto.COD_PRODUTO}" +
                                  $"\nDescrição........: {produto.DESCRICAO}" +
                                  $"\nPreço............: R${produto.PRECO}" +
                                  $"\nEstoque..........: {produto.Estoque.QUANTIDADE} {produto.UnidadeMedida.UNIDADE_MEDIDA}");
            }
        }
        public async Task ExibirTodosProdutos()
        {
            var categorias = await _context.CATEGORIA.Include(c => c.Produtos).ThenInclude(p => p.Estoque)
                                               .Include(c => c.Produtos).ThenInclude(p => p.UnidadeMedida).ToListAsync();

            foreach (var categoria in categorias)
            {
                Console.WriteLine("\n" + categoria.CATEGORIA + ":");

                foreach (var produto in categoria.Produtos)
                {
                    Console.WriteLine($"Código do produto: {produto.COD_PRODUTO}" +
                                  $"\nDescrição........: {produto.DESCRICAO}" +
                                  $"\nPreço............: {produto.PRECO}" +
                                  $"\nEstoque..........: {produto.Estoque.QUANTIDADE} {produto.UnidadeMedida.UNIDADE_MEDIDA}" +
                                  $"\n-----------------------------------------");
                }
            }
        }
        public async Task EditarProduto()
        {
            var produto = await _util.LocalizarProduto();
            if (produto == null) return;

            Console.WriteLine("\n1-Categoria  2-Descrição  3-Preço  4-Unidade de Medida  5-Cancelar Operação");
            Console.Write("Informe o dado que deseja alterar: ");
            int opcao;
            while (!int.TryParse(Console.ReadLine(), out opcao) || opcao < 1 || opcao > 5)
            {
                Console.WriteLine("Valor Inválido!");
                Console.WriteLine("\n1-Categoria  2-Descrição  3-Preço  4-Unidade de Medida  5-Cancelar Operação");
                Console.Write("Informe o dado que deseja alterar: ");
            }

            switch (opcao)
            {
                case 1:
                    Console.Write("Informe o código da categoria: ");
                    int categoria;
                    while (!int.TryParse(Console.ReadLine(), out categoria))
                    {
                        Console.WriteLine("Valor Inválido!");
                        Console.Write("Informe o código da categoria: ");
                    }
                    if (!_context.CATEGORIA.Any(c => c.COD_CATEGORIA == categoria))
                    {
                        Console.WriteLine("Não foi localizada nenhuma categoria para o código informado!");
                        return;
                    }

                    produto.CATEGORIA_FK = categoria;
                    Console.WriteLine($"Produto alterado com sucesso:");
                    await _context.SaveChangesAsync();
                    break;

                case 2:
                    Console.Write("Informe a decrição: ");
                    produto.DESCRICAO = Console.ReadLine();
                    Console.WriteLine($"Produto alterado com sucesso:");
                    await _context.SaveChangesAsync();
                    break;

                case 3:
                    Console.Write("Informe o preço: ");
                    decimal preco;
                    while (!decimal.TryParse(Console.ReadLine(), out preco))
                    {
                        Console.WriteLine("Valor Inválido!");
                        Console.Write("Informe o preço: ");
                    }

                    produto.PRECO = preco;
                    Console.WriteLine($"Produto alterado com sucesso:");
                    await _context.SaveChangesAsync();
                    break;

                case 4:
                    Console.Write("Informe o código da unidade de medida (1-Un  2-Kg  3-Lt): ");
                    int cod;
                    while (!int.TryParse(Console.ReadLine(), out cod) || cod < 1 || cod > 3)
                    {
                        Console.WriteLine("Valor Inválido!");
                        Console.Write("Informe o código da unidade de medida (1-Un  2-Kg  3-Lt): ");
                    }

                    produto.UNIDADE_MEDIDA_FK = cod;
                    Console.WriteLine($"Produto alterado com sucesso:");
                    await _context.SaveChangesAsync();
                    break;

                case 5:
                    Console.WriteLine("Operação Cancelada!");
                    return;
            }
        }
        public async Task DeletarProduto()
        {
            var produto = await _util.LocalizarProduto();
            if (produto == null) return;

            if (!_util.ConfirmarExclusao()) return;

            _context.PRODUTO.Remove(produto);
            await _context.SaveChangesAsync();

            Console.WriteLine("Produto excluido com sucesso!");
        }

    }
}
