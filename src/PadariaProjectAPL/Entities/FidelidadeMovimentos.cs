namespace PadariaProjectAPL.Entities
{
    public class FidelidadeMovimentos
    {
        public int COD_MOVIMENTO { get; set; }
        public int CLIENTE_FK { get; set; }
        public int TIPO_MOVIMENTACAO_FK { get; set; }
        public double QUANTIDADE { get; set; }
        public DateTime DATA_FIDELIDADE { get; set; }
        public int PEDIDO_FK { get; set; }

        public virtual TipoMovimentacoes Tipo_movimento { get; set; }
        public virtual Fidelidades Fidelidade { get; set; }
        public virtual Pedidos Pedido { get; set; }
    }
}
