namespace PadariaProjectAPL.Entities
{
    public class Clientes
    {
        public int COD_CLIENTE { get; set; }
        public int ENDERECO_FK { get; set; }
        public int FUNCIONARIO_FK { get; set; }
        public string NOME { get; set; }
        public string CPF { get; set; }
        public string EMAIL { get; set; }
        public string CELULAR { get; set; }
        public DateTime DATA_CADASTRO { get; set; }

        public virtual Enderecos Endereco { get; set; }
        public virtual Fidelidades Fidelidade { get; set; }
        public virtual Funcionarios Funcionario { get; set; }
        public virtual ICollection<Pedidos> Pedidos { get; set; }
    }
}
