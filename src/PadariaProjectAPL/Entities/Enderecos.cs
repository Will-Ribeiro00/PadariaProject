namespace PadariaProjectAPL.Entities
{
    public class Enderecos
    {
        public int COD_ENDERECO { get; set; }
        public string ENDERECO { get; set; }

        public virtual ICollection<Funcionarios> Funcionarios { get; set; }
        public virtual ICollection<Clientes> Clientes { get; set; }
    }
}
