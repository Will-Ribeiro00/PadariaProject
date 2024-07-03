using Microsoft.EntityFrameworkCore;
using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.Context;
using System.Text.Json;

namespace PadariaProjectAPL.Services.Serialize
{
    public class Serializacao
    {
        private readonly PadariaDbContext _context;
        public Serializacao(PadariaDbContext context) => _context = context;


        public async Task FaturamentoAnual(int ano)
        {
            var periodo = await _context.PEDIDO.Include(p => p.Cliente)
                                               .Include(p => p.Pagamentos)
                                               .Include(p => p.Funcionario)
                                               .Where(p => p.DATA_PEDIDO.Year >= ano && p.DATA_PEDIDO.Year <= ano && p.STATUS == "Finalizado")
                                               .Select(p => new
                                               {
                                                   CodPedido = p.COD_PEDIDO,
                                                   ClienteNome = p.Cliente.NOME,
                                                   FuncionarioNome = p.Funcionario.NOME,
                                                   ValorTotal = p.VALOR_TOTAL.ToString("C"),
                                                   Data = p.DATA_PEDIDO.ToString("dd/MM/yyyy HH:mm"),
                                               })
                                               .ToListAsync();

            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var json = System.Text.Json.JsonSerializer.Serialize(periodo, jsonOptions);

            File.WriteAllText("C:\\Users\\willi\\OneDrive\\Ambiente de Trabalho\\Mentoria NET\\PadariaProject\\src\\PadariaProjectAPL\\Json\\Faturamento\\FaturamentoAnual\\FaturamentoAnual_" + ano + ".txt", json);
        }
        public async Task FaturamentoPorPeriodo(DateTime dataInicial, DateTime dataFinal)
        {
            var periodo = await _context.PEDIDO.Include(p => p.Cliente)
                                              .Include(p => p.Pagamentos)
                                              .ThenInclude(pg => pg.TipoPagamento)
                                              .Include(p => p.Funcionario)
                                              .Where(p => p.DATA_PEDIDO >= dataInicial && p.DATA_PEDIDO <= dataFinal && p.STATUS == "Finalizado")
                                              .Select(p => new
                                              {
                                                  CodPedido = p.COD_PEDIDO,
                                                  ClienteNome = p.Cliente.NOME,
                                                  FuncionarioNome = p.Funcionario.NOME,
                                                  ValorTotal = p.VALOR_TOTAL.ToString("C"),
                                                  Data = p.DATA_PEDIDO.ToString("dd/MM/yyyy HH:mm"),
                                              })
                                              .ToListAsync();

            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var json = System.Text.Json.JsonSerializer.Serialize(periodo, jsonOptions);
            File.WriteAllText("C:\\Users\\willi\\OneDrive\\Ambiente de Trabalho\\Mentoria NET\\PadariaProject\\src\\PadariaProjectAPL\\Json\\Faturamento\\FaturamentoPorPeriodo\\FaturamentoPeriodo" + dataInicial.ToString("dd-MM-yyyy") + "_" + dataFinal.ToString("dd-MM-yyyy") + ".txt", json);
        }

        public async Task RecebimentoMercadoria(DateTime dataInicial, DateTime dataFinal)
        {
            var recebimento = await _context.ESTOQUE_MOVIMENTO.Include(em => em.Estoque)
                                                              .ThenInclude(e => e.Produto)
                                                              .ThenInclude(p => p.UnidadeMedida)
                                                              .Include(em => em.Funcionario)
                                                              .Include(em => em.TipoMovimento)
                                                              .Where(em => em.TIPO_MOVIMENTO_FK == 1 && em.DATA_ESTOQUE >= dataInicial && em.DATA_ESTOQUE <= dataFinal)
                                                              .Select(r => new
                                                              {
                                                                  CodRecebimento = r.COD_MOVIMENTO,
                                                                  Funcionario = r.Funcionario.NOME,
                                                                  ProdutoRecebido = r.Estoque.Produto.DESCRICAO,
                                                                  QuantidadeRecebida = r.QUANTIDADE + r.Estoque.Produto.UnidadeMedida.UNIDADE_MEDIDA,
                                                                  DataRecebimento = r.DATA_ESTOQUE.ToString("dd/MM/yyyy HH:mm"),
                                                              })
                                                              .ToListAsync();

            var jsonOption = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var json = System.Text.Json.JsonSerializer.Serialize(recebimento, jsonOption);

            File.WriteAllText("C:\\Users\\willi\\OneDrive\\Ambiente de Trabalho\\Mentoria NET\\PadariaProject\\src\\PadariaProjectAPL\\Json\\Estoque\\Recebimento\\RecebimentoPeriodo" + dataInicial.ToString("dd-MM-yyyy") + "_" + dataFinal.ToString("dd-MM-yyyy") + ".txt", json);
        }
        public async Task InventarioMercadoria(DateTime dataInicial, DateTime dataFinal)
        {
            var inventarios = await _context.ESTOQUE_MOVIMENTO.Include(em => em.Estoque).ThenInclude(e => e.Produto).ThenInclude(p => p.UnidadeMedida)
                                                              .Include(em => em.Funcionario)
                                                              .Include(em => em.TipoMovimento)
                                                              .Where(em => em.TIPO_MOVIMENTO_FK == 3 && em.DATA_ESTOQUE >= dataInicial && em.DATA_ESTOQUE <= dataFinal)
                                                              .Select(i => new
                                                              {
                                                                  CodInventario = i.COD_MOVIMENTO,
                                                                  Funcionario = i.Funcionario.NOME,
                                                                  ProdutoInventariado = i.Estoque.Produto.DESCRICAO,
                                                                  QuantidadeFinal = i.QUANTIDADE + i.Estoque.Produto.UnidadeMedida.UNIDADE_MEDIDA,
                                                                  DataInventario = i.DATA_ESTOQUE.ToString("dd/MM/yyyy HH:mm"),
                                                              })
                                                              .ToListAsync();

            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            var json = System.Text.Json.JsonSerializer.Serialize(inventarios, jsonOptions);
            File.WriteAllText("C:\\Users\\willi\\OneDrive\\Ambiente de Trabalho\\Mentoria NET\\PadariaProject\\src\\PadariaProjectAPL\\Json\\Estoque\\Inventario\\InventarioPeriodo" + dataInicial.ToString("dd-MM-yyyy") + "_" + dataFinal.ToString("dd-MM-yyyy") + ".txt", json);
        }

        public async Task CadastroCliente(DateTime dataInicial, DateTime dataFinal)
        {
            var cadastros = await _context.CLIENTE.Include(c => c.Funcionario).ThenInclude(f => f.Cargo)
                                                  .Include(c => c.Pedidos)
                                                  .Where(c => c.DATA_CADASTRO >= dataInicial && c.DATA_CADASTRO <= dataFinal)
                                                  .Select(c => new
                                                  {
                                                      CodCliente = c.COD_CLIENTE,
                                                      cliente = c.NOME,
                                                      c.CPF,
                                                      Funcionario = c.Funcionario.NOME,
                                                      Cargo = c.Funcionario.Cargo.CARGO,
                                                      DataCadastro = c.DATA_CADASTRO.ToString("dd/MM/yyyy HH:mm"),

                                                  })
                                                  .ToListAsync();

            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var json = System.Text.Json.JsonSerializer.Serialize(cadastros, jsonOptions);
            File.WriteAllText("C:\\Users\\willi\\OneDrive\\Ambiente de Trabalho\\Mentoria NET\\PadariaProject\\src\\PadariaProjectAPL\\Json\\Cliente\\Cadastro\\CadastroClientesPeriodo" + dataInicial.ToString("dd-MM-yyyy") + "_" + dataFinal.ToString("dd-MM-yyyy") + ".txt", json);

        }
        public async Task PedidoCliente(Clientes cliente, DateTime dataInicial, DateTime dataFinal)
        {

            var pedidos = await _context.PEDIDO.Include(p => p.Funcionario)
                                               .Where(p => p.CLIENTE_FK == cliente.COD_CLIENTE && p.DATA_PEDIDO >= dataInicial && p.DATA_PEDIDO <= dataFinal)
                                               .Select(p => new
                                               {
                                                   CodPedido = p.COD_PEDIDO,
                                                   Cliente = p.Cliente.COD_CLIENTE + " - " + p.Cliente.NOME,
                                                   Funcionario = p.Funcionario.NOME,
                                                   Valor = p.VALOR_TOTAL.ToString("C"),
                                                   StatusPedido = p.STATUS,
                                                   DataPedido = p.DATA_PEDIDO.ToString("dd/MM/yyyy HH:mm")
                                               })
                                               .ToListAsync();

            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var json = System.Text.Json.JsonSerializer.Serialize(pedidos, jsonOptions);
            File.WriteAllText("C:\\Users\\willi\\OneDrive\\Ambiente de Trabalho\\Mentoria NET\\PadariaProject\\src\\PadariaProjectAPL\\Json\\Cliente\\Pedido\\PedidosCliente_" + cliente.COD_CLIENTE + "Periodo" + dataInicial.ToString("dd-MM-yyyy") + "_" + dataFinal.ToString("dd-MM-yyyy") + ".txt", json);

        }
    }
}
