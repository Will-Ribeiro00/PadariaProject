namespace PadariaProjectAPL.Entities
{
    public class Produtos
    {
        public int COD_PRODUTO { get; set; }
        public int CATEGORIA_FK { get; set; }
        public int UNIDADE_MEDIDA_FK { get; set; }
        public string DESCRICAO { get; set; }
        public decimal PRECO { get; set; }


        public virtual Categorias Categoria { get; set; }
        public virtual Estoques Estoque { get; set; }
        public virtual UnidadeMedidas UnidadeMedida { get; set; }
        public virtual ICollection<PedidosProdutos> PedidosProdutos { get; set; }
    }
}
