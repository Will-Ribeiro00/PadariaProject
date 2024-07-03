namespace PadariaProjectAPL.Entities
{
    public class Pedidos
    {
        public int COD_PEDIDO { get; set; }
        public int FUNCIONARIO_FK { get; set; }
        public int CLIENTE_FK { get; set; }
        public DateTime DATA_PEDIDO { get; set; }
        public string STATUS { get; set; }
        public decimal VALOR_TOTAL { get; set; }

        public virtual Clientes Cliente { get; set; }
        public virtual Funcionarios Funcionario { get; set; }
        public virtual ICollection<Pagamentos> Pagamentos { get; set; }
        public virtual ICollection<PedidosProdutos> PedidosProdutos { get; set; }
        public virtual ICollection<FidelidadeMovimentos> FidelidadeMovimentos { get; set; }
    }
}
