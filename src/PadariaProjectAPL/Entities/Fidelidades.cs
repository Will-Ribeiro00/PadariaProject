namespace PadariaProjectAPL.Entities
{
    public class Fidelidades
    {
        public int CLIENTE_FK { get; set; }
        public double PONTOS { get; set; }

        public virtual Clientes Cliente { get; set; }
        public virtual ICollection<FidelidadeMovimentos> FidelidadeMovimentos { get; set; }
    }
}
