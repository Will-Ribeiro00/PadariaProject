namespace PadariaProjectAPL.Entities
{
    public class Categorias
    {
        public int COD_CATEGORIA { get; set; }
        public string CATEGORIA { get; set; }

        public virtual ICollection<Produtos> Produtos { get; set; }
    }
}
