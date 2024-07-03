namespace PadariaProjectAPL.Entities
{
    public class UnidadeMedidas
    {
        public int COD_UNIDADE_MEDIDA { get; set; }
        public string UNIDADE_MEDIDA { get; set; }

        public virtual ICollection<Produtos> Produtos { get; set; }
    }
}
