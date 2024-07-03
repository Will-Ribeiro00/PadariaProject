using Microsoft.EntityFrameworkCore;
using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.Context;

namespace PadariaProjectAPL.repositories.DataAcess
{
    public class PagamentoRepository
    {
        private readonly PadariaDbContext _context;
        private readonly FidelidadeRepository _fRepository;
        public PagamentoRepository(PadariaDbContext context, FidelidadeRepository fRepository)
        {
            _context = context;
            _fRepository = fRepository;
        }

        public async Task ConsultarValor(Pedidos pedido)
        {

            decimal subtotalCompra = 0;

            var produtos = await _context.PEDIDO_PRODUTO.Include(pp => pp.Produto)
                                                        .Include(pp => pp.Pedido)
                                                        .Where(pp => pp.PEDIDO_FK == pedido.COD_PEDIDO)
                                                        .ToListAsync();

            foreach (var produto in produtos)
            {
                var valorUnitario = produto.Produto.PRECO;
                var subtotalProduto = (decimal)produto.QUANTIDADE * valorUnitario;

                subtotalCompra += subtotalProduto;
            }

            pedido.VALOR_TOTAL = Math.Round(subtotalCompra, 2);
            await _context.SaveChangesAsync();
        }
        public async Task RealizarPagamento(Pedidos pedido)
        {
            var pedidoASerPago = await _context.PEDIDO.Include(p => p.Pagamentos).FirstOrDefaultAsync(p => p.COD_PEDIDO == pedido.COD_PEDIDO);
            var valorAReceber = pedidoASerPago.VALOR_TOTAL;

            do
            {
                Console.Clear();
                Console.WriteLine(valorAReceber.ToString("c"));
                Console.WriteLine("1- Dinheiro    2-Débito   3-Crédito   4-PIX");
                Console.Write("Informe a forma de pagamento: ");
                int tipoPagamento;
                while (!int.TryParse(Console.ReadLine(), out tipoPagamento))
                {
                    Console.WriteLine("Valor Inválido!");
                    Console.Write("Informe a forma de pagamento: ");
                }

                Console.Write("Informe a valor a ser pago: ");
                decimal pago;
                while (!decimal.TryParse(Console.ReadLine(), out pago))
                {
                    Console.WriteLine("Valor Inválido!");
                    Console.Write("Informe a valor a ser pago: ");
                }

                var novoPagamento = new Pagamentos()
                {
                    PEDIDO_FK = pedido.COD_PEDIDO,
                    TIPO_PAGAMENTO_FK = tipoPagamento,
                    VALOR = pago,
                    DATA_PAGAMENTO = DateTime.Now,
                };
                await _context.PAGAMENTO.AddAsync(novoPagamento);
                await _context.SaveChangesAsync();

                valorAReceber -= pago;

            } while (valorAReceber > 0.00M);
        }
    }
}
