using Microsoft.EntityFrameworkCore;
using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.Context;

namespace PadariaProjectAPL.repositories.DataAcess
{
    public class FidelidadeRepository
    {
        private readonly PadariaDbContext _context;
        public FidelidadeRepository(PadariaDbContext context)
        {
            _context = context;
        }

        public async Task ReceberPontos(Pedidos pedido)
        {
            var cliente = await _context.PEDIDO.Include(p => p.Cliente)
                                               .ThenInclude(c => c.Fidelidade)
                                               .ThenInclude(f => f.FidelidadeMovimentos).FirstOrDefaultAsync(p => p.COD_PEDIDO == pedido.COD_PEDIDO);

            var pedido_ = await _context.PEDIDO.FirstOrDefaultAsync(p => p.COD_PEDIDO == pedido.COD_PEDIDO && p.CLIENTE_FK == cliente.CLIENTE_FK);

            if (cliente.VALOR_TOTAL >= 25.00M)
            {
                var ganho = (double)cliente.VALOR_TOTAL * 0.05;

                cliente.Cliente.Fidelidade.PONTOS += ganho;
                await _context.SaveChangesAsync();

                var ReceberPontos = new FidelidadeMovimentos()
                {
                    CLIENTE_FK = cliente.CLIENTE_FK,
                    QUANTIDADE = ganho,
                    TIPO_MOVIMENTACAO_FK = 4,
                    PEDIDO_FK = pedido_.COD_PEDIDO,
                    DATA_FIDELIDADE = DateTime.Now
                };

                await _context.FIDELIDADE_MOVIMENTO.AddAsync(ReceberPontos);
                await _context.SaveChangesAsync();
            }
        }
        public async Task ResgatarPontos(Pedidos pedido)
        {
            Console.Clear();
            var cliente = await _context.FIDELIDADE.Include(f => f.Cliente)
                                                   .Include(f => f.FidelidadeMovimentos).FirstOrDefaultAsync(f => f.CLIENTE_FK == pedido.CLIENTE_FK);

            var pedidoAserDebitado = await _context.PEDIDO.FirstOrDefaultAsync(p => p.COD_PEDIDO == pedido.COD_PEDIDO && p.CLIENTE_FK == cliente.CLIENTE_FK);

            if (cliente.CLIENTE_FK != 1 && cliente.PONTOS > 0)
            {

                Console.WriteLine($"#{cliente.CLIENTE_FK} - {cliente.Cliente.NOME}" +
                                  $"\nPontos...............: {cliente.PONTOS:C}");
                Console.Write("Deseja Resgatar seus pontos ? (S / N): ");
                var resposta = Console.ReadLine();
                if (!resposta.Equals("s", StringComparison.OrdinalIgnoreCase) && !resposta.Equals("sim", StringComparison.OrdinalIgnoreCase))
                {
                    Console.ReadKey();
                    return;
                }
                else
                {
                    var pontosResgatados = Math.Min((decimal)cliente.PONTOS, pedidoAserDebitado.VALOR_TOTAL);

                    pedidoAserDebitado.VALOR_TOTAL -= pontosResgatados;
                    cliente.PONTOS -= (double)pontosResgatados;
                    await _context.SaveChangesAsync();

                    var resgate = new FidelidadeMovimentos()
                    {
                        CLIENTE_FK = cliente.CLIENTE_FK,
                        QUANTIDADE = (double)pontosResgatados,
                        TIPO_MOVIMENTACAO_FK = 5,
                        PEDIDO_FK = pedidoAserDebitado.COD_PEDIDO,
                        DATA_FIDELIDADE = DateTime.Now
                    };
                    await _context.FIDELIDADE_MOVIMENTO.AddAsync(resgate);
                    await _context.SaveChangesAsync();

                }
            }
        }
    }
}
