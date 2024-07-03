namespace PadariaProjectAPL.Entities
{
    public class PedidosProdutos
    {
        public int PEDIDO_FK { get; set; }
        public int PRODUTO_FK { get; set; }
        public double QUANTIDADE { get; set; }
        public int INDEX_ { get; set; } = 0;

        public virtual Pedidos Pedido { get; set; }
        public virtual Produtos Produto { get; set; }
    }
}
