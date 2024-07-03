using Microsoft.EntityFrameworkCore;
using PadariaProjectAPL.repositories.Context;
using PadariaProjectAPL.Services.Serialize;
using PadariaProjectAPL.Utils;

namespace PadariaProjectAPL.repositories.DataAcess
{
    public class GerenteRepository
    {
        private readonly PadariaDbContext _context;
        private readonly FuncionarioRepository _fRepository;
        private readonly Util _util;
        private readonly Serializacao _serializacao;

        public GerenteRepository(PadariaDbContext context, FuncionarioRepository fRepository, Util util, Serializacao serializacao)
        {
            _context = context;
            _fRepository = fRepository;
            _util = util;
            _serializacao = serializacao;
        }

        public async Task ResetarSenhaFuncionario()
        {
            Console.Clear();
            Console.Write("Informe o código do funcionário: ");
            var funcionario = await _context.FUNCIONARIO.FirstOrDefaultAsync(f => f.COD_FUNCIONARIO == Convert.ToInt32(Console.ReadLine()));
            if (funcionario == null)
            {
                Console.WriteLine("Não foi localizado nenhum funcionário com o código informado!");
            }
            funcionario.SENHA = "123456";
            await _context.SaveChangesAsync();
        }

        public async Task FaturamentoAnual()
        {
            var totalFaturamento = 0M;
            Console.Clear();

            Console.Write("Informe o ano de referência (yyyy): ");
            int ano;
            while (!int.TryParse(Console.ReadLine(), out ano))
            {
                Console.WriteLine("Valor Inválido!");
                Console.Write("Informe o ano de referência (yyyy): ");
            }

            DateTime dataInicial = new DateTime(ano, 1, 1);
            DateTime dataFinal = new DateTime(ano, 12, 31);
            dataFinal.AddDays(1).AddSeconds(-1);

            var periodo = await _context.PEDIDO.Include(p => p.Cliente)
                                               .Include(p => p.Pagamentos)
                                               .Include(p => p.Funcionario)
                                               .Where(p => p.DATA_PEDIDO >= dataInicial && p.DATA_PEDIDO <= dataFinal && p.STATUS == "Finalizado").ToListAsync();

            Console.Clear();
            Console.WriteLine($"Faturamento Anual - {ano}\n");

            foreach (var pedido in periodo)
            {
                var cod = pedido.COD_PEDIDO.ToString().PadRight(5);
                var cliente = pedido.Cliente.NOME.ToString().PadRight(25);
                var funcionario = pedido.Funcionario.NOME.ToString().PadRight(25);
                var vTotal = pedido.VALOR_TOTAL;


                Console.WriteLine($"Pedido: {cod} | Cliente: {cliente} | Funcionário: {funcionario} | Valor Total: {vTotal:C}");
                totalFaturamento += pedido.VALOR_TOTAL;
            }

            Console.WriteLine($"\n\nResumo do Faturamento Anual:");
            Console.WriteLine($"Total de pedidos: {periodo.Count()}");
            Console.WriteLine($"Total faturado: R${totalFaturamento}");

            await _serializacao.FaturamentoAnual(ano);
        }
        public async Task FaturamentoPorPeriodo()
        {
            var totalFaturamento = 0M;
            Console.Clear();

            var dataInicial = _util.DataInicial();

            var dataFinal = _util.DataFinal();

            var periodo = await _context.PEDIDO.Include(p => p.Cliente)
                                              .Include(p => p.Pagamentos)
                                              .ThenInclude(pg => pg.TipoPagamento)
                                              .Include(p => p.Funcionario)
                                              .Where(p => p.DATA_PEDIDO >= dataInicial && p.DATA_PEDIDO <= dataFinal && p.STATUS == "Finalizado").ToListAsync();

            Console.Clear();
            Console.WriteLine($"Data inicial: {dataInicial.ToString("d")}" +
                              $"\nData final: {dataFinal.ToString("d")}");

            foreach (var pedido in periodo)
            {
                totalFaturamento += pedido.VALOR_TOTAL;
                Console.WriteLine($"\nPedido...........: {pedido.COD_PEDIDO}" +
                                  $"\nCliente..........: {pedido.CLIENTE_FK} - {pedido.Cliente.NOME}" +
                                  $"\nFuncionário......: {pedido.FUNCIONARIO_FK} - {pedido.Funcionario.NOME}");

                foreach (var pagamento in pedido.Pagamentos)
                {
                    Console.WriteLine($"\n* * * * * * * * * * * * * * * * * * * * * * * * * " +
                                      $"\nTipo de Pagamento: {pagamento.TipoPagamento.TIPO_PAGAMENTO}" +
                                      $"\nValor............: {pagamento.VALOR}" +
                                      $"\n* * * * * * * * * * * * * * * * * * * * * * * * *  ");
                }
                Console.WriteLine($"\nValor Total......: R${pedido.VALOR_TOTAL}" +
                                  $"\nStatus...........: {pedido.STATUS}" +
                                  $"\nData.............: {pedido.DATA_PEDIDO}" +
                                  $"\n--------------------------------------------------");
            }

            Console.WriteLine($"\n\nResumo do Faturamento:");
            Console.WriteLine($"Total de pedidos: {periodo.Count()}");
            Console.WriteLine($"Total faturado: {totalFaturamento:C}");

            await _serializacao.FaturamentoPorPeriodo(dataInicial, dataFinal);
        }


        public async Task RelatorioRecebimentoProdutoPorPeriodo()
        {
            Console.Clear();

            var dataInicial = _util.DataInicial();

            var dataFinal = _util.DataFinal();


            var recebimento = await _context.ESTOQUE_MOVIMENTO.Include(em => em.Estoque)
                                                              .ThenInclude(e => e.Produto)
                                                              .ThenInclude(p => p.UnidadeMedida)
                                                              .Include(em => em.Funcionario)
                                                              .Include(em => em.TipoMovimento)
                                                              .Where(em => em.TIPO_MOVIMENTO_FK == 1 && em.DATA_ESTOQUE >= dataInicial && em.DATA_ESTOQUE <= dataFinal).ToListAsync();

            Console.WriteLine($"Recebimento de mercadorias no período de {dataInicial.ToShortDateString()} a {dataFinal.ToShortDateString()}:\n");
            foreach (var entrada in recebimento)
            {
                Console.WriteLine($"\nCódigo do Recebimento: #{entrada.COD_MOVIMENTO} - Data: {entrada.DATA_ESTOQUE.ToShortDateString()} Hora: {entrada.DATA_ESTOQUE.ToShortTimeString()}" +
                                  $"\nProduto..............: #{entrada.Estoque.Produto.COD_PRODUTO} - {entrada.Estoque.Produto.DESCRICAO}" +
                                  $"\nFuncionario..........: #{entrada.Funcionario.COD_FUNCIONARIO} - {entrada.Funcionario.NOME}" +
                                  $"\nQantidade Recebida...: {entrada.QUANTIDADE} {entrada.Estoque.Produto.UnidadeMedida.UNIDADE_MEDIDA}" +
                                  $"\n--------------------------------------------------------------------------------------------------------");
            }

            await _serializacao.RecebimentoMercadoria(dataInicial, dataFinal);
        }
        public async Task RelatorioInventarioPorPeriodo()
        {
            Console.Clear();
            var dataInicial = _util.DataInicial();
            var dataFinal = _util.DataFinal();
            Console.WriteLine();

            var inventarios = await _context.ESTOQUE_MOVIMENTO.Include(em => em.Estoque).ThenInclude(e => e.Produto).ThenInclude(p => p.UnidadeMedida)
                                                              .Include(em => em.Funcionario)
                                                              .Include(em => em.TipoMovimento)
                                                              .Where(em => em.TIPO_MOVIMENTO_FK == 3 && em.DATA_ESTOQUE >= dataInicial && em.DATA_ESTOQUE <= dataFinal).ToListAsync();

            Console.WriteLine($"Inventários realizados no período de {dataInicial.ToShortDateString()} a {dataFinal.ToShortDateString()}:\n");
            foreach (var inventario in inventarios)
            {
                Console.WriteLine($"Cod Inventário..: {inventario.COD_MOVIMENTO} - Data: {inventario.DATA_ESTOQUE:g}" +
                                  $"\nProduto.........: {inventario.Estoque.Produto.DESCRICAO}" +
                                  $"\nFuncionário.....: {inventario.Funcionario.NOME}" +
                                  $"\nQuantidade......: {inventario.QUANTIDADE} {inventario.Estoque.Produto.UnidadeMedida.UNIDADE_MEDIDA}");
                Console.WriteLine("------------------------------------------------");
            }

            await _serializacao.InventarioMercadoria(dataInicial, dataFinal);
        }


        public async Task RelatorioCadastroClientePorPeriodo()
        {
            Console.Clear();

            var dataInicial = _util.DataInicial();

            var dataFinal = _util.DataFinal();

            var cadastros = await _context.CLIENTE.Include(c => c.Funcionario).ThenInclude(f => f.Cargo)
                                                  .Include(c => c.Pedidos)
                                                  .Where(c => c.DATA_CADASTRO >= dataInicial && c.DATA_CADASTRO <= dataFinal).ToListAsync();


            Console.WriteLine($"Clientes cadastrados no período de {dataInicial.ToShortDateString()} a {dataFinal.ToShortDateString()}:\n");
            foreach (var cliente in cadastros)
            {
                Console.WriteLine($"\nDados Cliente....: #{cliente.COD_CLIENTE} Nome: {cliente.NOME} CPF: {cliente.CPF}" +
                                  $"\nDados Funcionario: #{cliente.Funcionario.COD_FUNCIONARIO} Nome: {cliente.Funcionario.NOME} - Cargo: {cliente.Funcionario.Cargo.CARGO}" +
                                  $"\nCadastrado Em....: DATA: {cliente.DATA_CADASTRO.ToShortDateString()} Hora: {cliente.DATA_CADASTRO.ToShortTimeString()}" +
                                  $"\n---------------------------------------------------------------------------------------------");
            }

            await _serializacao.CadastroCliente(dataInicial, dataFinal);
        }
        public async Task RelatiorioPedidosClientePorPeriodo()
        {
            Console.Clear();
            decimal total = 0;
            var cliente = await _util.LocalizarCliente();

            Console.WriteLine();
            var dataInicial = _util.DataInicial();

            var dataFinal = _util.DataFinal();
            Console.WriteLine();

            var pedidos = await _context.PEDIDO.Include(p => p.Funcionario).Where(p => p.CLIENTE_FK == cliente.COD_CLIENTE && p.DATA_PEDIDO >= dataInicial && p.DATA_PEDIDO <= dataFinal).ToListAsync();

            Console.WriteLine($"Pedidos do cliente: #{cliente.COD_CLIENTE + " " + cliente.NOME} no período de {dataInicial.ToShortDateString()} a {dataFinal.ToShortDateString()}");
            foreach (var pedido in pedidos)
            {
                Console.WriteLine($"Pedido: {pedido.COD_PEDIDO} - Pedido do Cliente: {pedidos.IndexOf(pedido)}");
                Console.WriteLine($"Data.....................: {pedido.DATA_PEDIDO:g}");
                Console.WriteLine($"Funcionário..............: {pedido.Funcionario.NOME}");
                Console.WriteLine($"Valor....................: {pedido.VALOR_TOTAL:C}");
                Console.WriteLine($"Status...................: {pedido.STATUS}");
                if (pedido.STATUS == "Finalizado")
                {
                    total += pedido.VALOR_TOTAL;
                }
                Console.WriteLine("-----------------------------------------------------------------------------");
            }
            Console.WriteLine($"\nValor total gasto do período: {total:C}");

            await _serializacao.PedidoCliente(cliente, dataInicial, dataFinal);
        }

    }
}
