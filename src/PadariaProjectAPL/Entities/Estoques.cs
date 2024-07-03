namespace PadariaProjectAPL.Entities
{
    public class Estoques
    {
        public int COD_ESTOQUE { get; set; }
        public int PRODUTO_FK { get; set; }
        public double QUANTIDADE { get; set; }

        public virtual Produtos Produto { get; set; }
        public virtual ICollection<EstoqueMovimentos> EstoqueMovimentos { get; set; }
    }
}
