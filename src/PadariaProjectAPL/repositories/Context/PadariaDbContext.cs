using Microsoft.EntityFrameworkCore;
using PadariaProjectAPL.Entities;

namespace PadariaProjectAPL.repositories.Context
{
    public class PadariaDbContext : DbContext
    {
        public PadariaDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Cargos> CARGO { get; set; }
        public DbSet<Categorias> CATEGORIA { get; set; }
        public DbSet<Clientes> CLIENTE { get; set; }
        public DbSet<Enderecos> ENDERECO { get; set; }
        public DbSet<EstoqueMovimentos> ESTOQUE_MOVIMENTO { get; set; }
        public DbSet<Estoques> ESTOQUE { get; set; }
        public DbSet<FidelidadeMovimentos> FIDELIDADE_MOVIMENTO { get; set; }
        public DbSet<Fidelidades> FIDELIDADE { get; set; }
        public DbSet<Funcionarios> FUNCIONARIO { get; set; }
        public DbSet<Pagamentos> PAGAMENTO { get; set; }
        public DbSet<Pedidos> PEDIDO { get; set; }
        public DbSet<PedidosProdutos> PEDIDO_PRODUTO { get; set; }
        public DbSet<Produtos> PRODUTO { get; set; }
        public DbSet<TipoMovimentacoes> TIPO_MOVIMENTACAO { get; set; }
        public DbSet<TipoPagamentos> TIPO_PAGAMENTO { get; set; }
        public DbSet<UnidadeMedidas> UNIDADE_MEDIDA { get; set; }
        public DbSet<Status_> STATUS { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //chaves primarias:
            modelBuilder.Entity<Cargos>().HasKey(c => c.COD_CARGO);
            modelBuilder.Entity<Categorias>().HasKey(c => c.COD_CATEGORIA);
            modelBuilder.Entity<Clientes>().HasKey(c => c.COD_CLIENTE);
            modelBuilder.Entity<Enderecos>().HasKey(e => e.COD_ENDERECO);
            modelBuilder.Entity<EstoqueMovimentos>().HasKey(em => em.COD_MOVIMENTO);
            modelBuilder.Entity<Estoques>().HasKey(e => e.COD_ESTOQUE);
            modelBuilder.Entity<FidelidadeMovimentos>().HasKey(fm => fm.COD_MOVIMENTO);
            modelBuilder.Entity<Fidelidades>().HasKey(f => f.CLIENTE_FK);
            modelBuilder.Entity<Funcionarios>().HasKey(f => f.COD_FUNCIONARIO);
            modelBuilder.Entity<Pagamentos>().HasKey(p => p.COD_PAGAMENTO);
            modelBuilder.Entity<Pedidos>().HasKey(p => p.COD_PEDIDO);
            modelBuilder.Entity<PedidosProdutos>().HasKey(pp => new { pp.PEDIDO_FK, pp.PRODUTO_FK });
            modelBuilder.Entity<Produtos>().HasKey(p => p.COD_PRODUTO);
            modelBuilder.Entity<Status_>().HasKey(s => s.COD_STATUS);
            modelBuilder.Entity<TipoMovimentacoes>().HasKey(tm => tm.COD_TIPO);
            modelBuilder.Entity<TipoPagamentos>().HasKey(tp => tp.COD_TIPO_PAGAMENTO);
            modelBuilder.Entity<UnidadeMedidas>().HasKey(um => um.COD_UNIDADE_MEDIDA);


            //chaves estrangeiras:
            modelBuilder.Entity<Produtos>().HasOne(p => p.Categoria).WithMany(c => c.Produtos).HasForeignKey(p => p.CATEGORIA_FK);
            modelBuilder.Entity<Produtos>().HasOne(p => p.UnidadeMedida).WithMany(um => um.Produtos).HasForeignKey(p => p.UNIDADE_MEDIDA_FK);

            modelBuilder.Entity<Estoques>().HasOne(e => e.Produto).WithOne(p => p.Estoque).HasForeignKey<Estoques>(e => e.PRODUTO_FK);

            modelBuilder.Entity<EstoqueMovimentos>().HasOne(em => em.Estoque).WithMany(e => e.EstoqueMovimentos).HasForeignKey(em => em.ESTOQUE_FK);
            modelBuilder.Entity<EstoqueMovimentos>().HasOne(em => em.Funcionario).WithMany(f => f.EstoqueMovimentos).HasForeignKey(em => em.FUNCIONARIO_FK);
            modelBuilder.Entity<EstoqueMovimentos>().HasOne(em => em.TipoMovimento).WithMany(tm => tm.EstoqueMovimentos).HasForeignKey(em => em.TIPO_MOVIMENTO_FK);

            modelBuilder.Entity<Funcionarios>().HasOne(f => f.Cargo).WithMany(c => c.Funcionarios).HasForeignKey(f => f.CARGO_FK);
            modelBuilder.Entity<Funcionarios>().HasOne(f => f.Endereco).WithMany(e => e.Funcionarios).HasForeignKey(f => f.ENDERECO_FK);
            modelBuilder.Entity<Funcionarios>().HasOne(f => f.Status).WithMany(s => s.Funcionarios).HasForeignKey(f => f.STATUS_FK);

            modelBuilder.Entity<Clientes>().HasOne(c => c.Endereco).WithMany(e => e.Clientes).HasForeignKey(c => c.ENDERECO_FK);
            modelBuilder.Entity<Clientes>().HasOne(c => c.Funcionario).WithMany(f => f.Clientes).HasForeignKey(c => c.FUNCIONARIO_FK);

            modelBuilder.Entity<Fidelidades>().HasOne(f => f.Cliente).WithOne(c => c.Fidelidade).HasForeignKey<Fidelidades>(f => f.CLIENTE_FK);

            modelBuilder.Entity<FidelidadeMovimentos>().HasOne(fm => fm.Fidelidade).WithMany(f => f.FidelidadeMovimentos).HasForeignKey(fm => fm.CLIENTE_FK);
            modelBuilder.Entity<FidelidadeMovimentos>().HasOne(fm => fm.Tipo_movimento).WithMany(tm => tm.FidelidadeMovimentos).HasForeignKey(fm => fm.TIPO_MOVIMENTACAO_FK);
            modelBuilder.Entity<FidelidadeMovimentos>().HasOne(fm => fm.Pedido).WithMany(p => p.FidelidadeMovimentos).HasForeignKey(fm => fm.PEDIDO_FK);

            modelBuilder.Entity<Pagamentos>().HasOne(p => p.TipoPagamento).WithMany(tp => tp.Pagamentos).HasForeignKey(p => p.TIPO_PAGAMENTO_FK);
            modelBuilder.Entity<Pagamentos>().HasOne(p => p.Pedido).WithMany(pe => pe.Pagamentos).HasForeignKey(p => p.PEDIDO_FK);

            modelBuilder.Entity<Pedidos>().HasOne(p => p.Funcionario).WithMany(f => f.Pedidos).HasForeignKey(p => p.FUNCIONARIO_FK);
            modelBuilder.Entity<Pedidos>().HasOne(p => p.Cliente).WithMany(c => c.Pedidos).HasForeignKey(p => p.CLIENTE_FK);

            modelBuilder.Entity<PedidosProdutos>().HasOne(pp => pp.Pedido).WithMany(p => p.PedidosProdutos).HasForeignKey(pp => pp.PEDIDO_FK);
            modelBuilder.Entity<PedidosProdutos>().HasOne(pp => pp.Produto).WithMany(p => p.PedidosProdutos).HasForeignKey(pp => pp.PRODUTO_FK);

            base.OnModelCreating(modelBuilder);
        }
    }
}
