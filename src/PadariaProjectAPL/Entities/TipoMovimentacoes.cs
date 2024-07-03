namespace PadariaProjectAPL.Entities
{
    public class TipoMovimentacoes
    {
        public int COD_TIPO { get; set; }
        public string TIPO_MOVIMENTACAO { get; set; }

        public virtual ICollection<EstoqueMovimentos> EstoqueMovimentos { get; set; }
        public virtual ICollection<FidelidadeMovimentos> FidelidadeMovimentos { get; set; }
    }
}
