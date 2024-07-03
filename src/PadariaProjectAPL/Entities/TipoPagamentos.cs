namespace PadariaProjectAPL.Entities
{
    public class TipoPagamentos
    {
        public int COD_TIPO_PAGAMENTO { get; set; }
        public string TIPO_PAGAMENTO { get; set; }

        public virtual ICollection<Pagamentos> Pagamentos { get; set; }
    }
}
