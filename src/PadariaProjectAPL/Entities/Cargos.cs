namespace PadariaProjectAPL.Entities
{
    public class Cargos
    {
        public int COD_CARGO { get; set; }
        public string CARGO { get; set; }

        public virtual ICollection<Funcionarios> Funcionarios { get; set; }
    }
}
