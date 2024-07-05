namespace PadariaProjectAPL.Entities
{
    public class Status_
    {
        public int COD_STATUS { get; set; }
        public string STATUS { get; set; }

        public virtual ICollection<Funcionarios> Funcionarios { get; set; }
    }
}
