namespace PadariaProjectAPL.Entities
{
    public class Pagamentos
    {
        public int COD_PAGAMENTO { get; set; }
        public int PEDIDO_FK { get; set; }
        public int TIPO_PAGAMENTO_FK { get; set; }
        public decimal VALOR { get; set; }
        public DateTime DATA_PAGAMENTO { get; set; }

        public virtual Pedidos Pedido { get; set; }
        public virtual TipoPagamentos TipoPagamento { get; set; }
    }
}
