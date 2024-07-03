namespace PadariaProjectAPL.Entities
{
    public class EstoqueMovimentos
    {
        public int COD_MOVIMENTO { get; set; }
        public int ESTOQUE_FK { get; set; }
        public int FUNCIONARIO_FK { get; set; }
        public int TIPO_MOVIMENTO_FK { get; set; }
        public double QUANTIDADE { get; set; }
        public DateTime DATA_ESTOQUE { get; set; }

        public virtual Estoques Estoque { get; set; }
        public virtual Funcionarios Funcionario { get; set; }
        public virtual TipoMovimentacoes TipoMovimento { get; set; }
    }
}
