using Microsoft.EntityFrameworkCore;
using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.Context;
using PadariaProjectAPL.Utils;

namespace PadariaProjectAPL.repositories.DataAcess
{
    public class CategoriaRepository
    {
        private readonly PadariaDbContext _context;
        private readonly Util _util;
        public CategoriaRepository(PadariaDbContext context, Util util)
        {
            _context = context;
            _util = util;
        }

        public async Task CriarCategoria()
        {
            bool loop = true;
            do
            {
                Console.Clear();
                Console.Write("Informe a nova categoria: ");
                var categoria = Console.ReadLine();

                var novaCategoria = new Categorias()
                {
                    CATEGORIA = categoria
                };

                Console.WriteLine("Categoria criada com sucesso!");

                await _context.CATEGORIA.AddAsync(novaCategoria);
                await _context.SaveChangesAsync();

                Console.WriteLine("\nDeseja adicionar mais categorias ? (S / N): ");
                var opc = Console.ReadLine();
                if (!opc.Equals("s", StringComparison.InvariantCultureIgnoreCase) && !opc.Equals("sim", StringComparison.InvariantCultureIgnoreCase))
                {
                    loop = false;
                }
            } while (loop);

        }

        public async Task ExibirCategorias()
        {
            Console.WriteLine("CÓDIGO - CATEGORIA" +
                            "\n------------------");

            var categorias = await _context.CATEGORIA.ToListAsync();

            foreach (var item in categorias)
            {
                Console.WriteLine($"#{item.COD_CATEGORIA} - {item.CATEGORIA}");
            }
        }

        public async Task AtualizarCategoria()
        {
            var categoria = await _util.LocalizarCategoria();

            if (categoria == null) return;

            Console.WriteLine("Informe a nova descrição:");
            var novaDescricao = Console.ReadLine();

            categoria.CATEGORIA = novaDescricao;
            Console.WriteLine("Categoria atualizada!");

            await _context.SaveChangesAsync();
        }

        public async Task DeletarCategoria()
        {
            var categoria = await _util.LocalizarCategoria();

            if (categoria == null) return;

            if (!_util.ConfirmarExclusao()) return;

            _context.CATEGORIA.Remove(categoria);
            await _context.SaveChangesAsync();

            Console.WriteLine("Categoria removida com sucesso!");
        }

    }
}
