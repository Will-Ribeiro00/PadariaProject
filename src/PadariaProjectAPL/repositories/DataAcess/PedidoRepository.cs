using Microsoft.EntityFrameworkCore;
using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.Context;
using PadariaProjectAPL.Utils;

namespace PadariaProjectAPL.repositories.DataAcess
{
    public class PedidoRepository
    {
        private readonly PadariaDbContext _context;
        private readonly EstoqueRepository _eRepository;
        private readonly PagamentoRepository _pRepository;
        private readonly FidelidadeRepository _fRepository;
        private readonly Util _util;
        public PedidoRepository(PadariaDbContext context, Util util, EstoqueRepository eRepository, PagamentoRepository pRepository, FidelidadeRepository fRepository)
        {
            _context = context;
            _util = util;
            _eRepository = eRepository;
            _pRepository = pRepository;
            _fRepository = fRepository;
        }

        public async Task NovoPedido(Funcionarios funcionario)
        {
            var cliente = await _util.IdentificarCliente();

            var novoPedido = new Pedidos()
            {
                DATA_PEDIDO = DateTime.Now,
                CLIENTE_FK = cliente.COD_CLIENTE,
                FUNCIONARIO_FK = funcionario.COD_FUNCIONARIO,
                STATUS = "pendente"
            };
            await _context.PEDIDO.AddAsync(novoPedido);
            await _context.SaveChangesAsync();

            Console.Clear();
            var continuar = await _eRepository.SacolaDoPedido(novoPedido, funcionario);
            if (continuar == 1)
            {
                novoPedido.VALOR_TOTAL = 0;
                novoPedido.STATUS = "Cancelado";
                await _context.SaveChangesAsync();
                return;
            }
            await _context.SaveChangesAsync();

            Console.Clear();
            await _pRepository.ConsultarValor(novoPedido);
            await _context.SaveChangesAsync();

            await _fRepository.ResgatarPontos(novoPedido);
            await _context.SaveChangesAsync();

            await _pRepository.RealizarPagamento(novoPedido);
            await _context.SaveChangesAsync();

            await _fRepository.ReceberPontos(novoPedido);
            await _context.SaveChangesAsync();

            await _eRepository.SaidaRealEstoque(novoPedido);
            await _context.SaveChangesAsync();

            novoPedido.STATUS = "Finalizado";
            await _context.SaveChangesAsync();

            await Cupom(novoPedido);
            Console.ReadKey();

        }

        public async Task Cupom(Pedidos pedido)
        {
            Console.Clear();

            var pedidoReferencia = await _context.PEDIDO.Include(p => p.PedidosProdutos).ThenInclude(pp => pp.Produto)
                                             .ThenInclude(p => p.UnidadeMedida)
                                             .Include(p => p.Cliente).ThenInclude(c => c.Fidelidade).ThenInclude(f => f.FidelidadeMovimentos)
                                             .Include(p => p.Pagamentos).ThenInclude(pg => pg.TipoPagamento)
                                             .FirstOrDefaultAsync(p => p.COD_PEDIDO == pedido.COD_PEDIDO);

            Console.WriteLine($"\n**************************CUPOM FISCAL - {pedidoReferencia.COD_PEDIDO}**************************");

            await _util.ExibirProdutosDoPedido(pedidoReferencia);

            if (pedidoReferencia.Cliente.COD_CLIENTE != 1)
            {
                Console.WriteLine($"\nCliente....................................................: {pedidoReferencia.Cliente.NOME}");

                var pontosUtilizados = await _context.FIDELIDADE_MOVIMENTO.FirstOrDefaultAsync(fm => fm.PEDIDO_FK == pedidoReferencia.COD_PEDIDO && fm.TIPO_MOVIMENTACAO_FK == 5);
                if (pontosUtilizados == null || pontosUtilizados.QUANTIDADE == 0)
                {
                    Console.WriteLine($"Pontos Utilizados..........................................: N/A");
                }
                else
                {
                    Console.WriteLine($"Pontos Utilizados..........................................: {pontosUtilizados.QUANTIDADE:C}");
                }

                var pontosGanhos = await _context.FIDELIDADE_MOVIMENTO.FirstOrDefaultAsync(fm => fm.PEDIDO_FK == pedidoReferencia.COD_PEDIDO && fm.TIPO_MOVIMENTACAO_FK == 4);
                if (pontosGanhos == null || pontosGanhos.QUANTIDADE == 0)
                {
                    Console.WriteLine($"Pontos Ganhos..............................................: N/A");
                }
                else
                {
                    Console.WriteLine($"Pontos Ganhos..............................................: {pontosGanhos.QUANTIDADE:C}");
                }

                Console.WriteLine($"Pontos Atuais..............................................: {pedidoReferencia.Cliente.Fidelidade.PONTOS:C}");
            }

            Console.WriteLine($"\nValor Total................................................: {pedidoReferencia.VALOR_TOTAL:C}");

            foreach (var pagamento in pedidoReferencia.Pagamentos)
            {
                var tipoPagamento = pagamento.TipoPagamento.TIPO_PAGAMENTO.ToString().PadRight(8);
                Console.WriteLine($"Forma de Pagamento...........: {tipoPagamento}   Valor............: {pagamento.VALOR:C}");
            }

            Console.WriteLine($"\n***********************DATA - {pedido.DATA_PEDIDO.ToShortDateString()} {pedido.DATA_PEDIDO.ToShortTimeString()}***********************");
        }

    }
}
