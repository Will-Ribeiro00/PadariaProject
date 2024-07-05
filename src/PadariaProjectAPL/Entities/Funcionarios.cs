namespace PadariaProjectAPL.Entities
{
    public class Funcionarios
    {
        public int COD_FUNCIONARIO { get; set; }
        public int CARGO_FK { get; set; }
        public int ENDERECO_FK { get; set; }
        public int STATUS_FK { get; set; }
        public string NOME { get; set; }
        public string CPF { get; set; }
        public decimal SALARIO { get; set; }
        public string CELULAR { get; set; }
        public string SENHA { get; set; }

        public virtual Cargos Cargo { get; set; }
        public virtual Enderecos Endereco { get; set; }
        public virtual Status_ Status { get; set; }
        public virtual ICollection<EstoqueMovimentos> EstoqueMovimentos { get; set; }
        public virtual ICollection<Pedidos> Pedidos { get; set; }
        public virtual ICollection<Clientes> Clientes { get; set; }
    }
}
