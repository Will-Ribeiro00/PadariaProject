using Microsoft.EntityFrameworkCore;
using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.Context;

namespace PadariaProjectAPL.Services.Authentication
{
    public class Autenticacao
    {
        private readonly PadariaDbContext _context;
        public Autenticacao(PadariaDbContext context) => _context = context;

        public async Task<Funcionarios> Login()
        {
            Funcionarios confirmacao;
            do
            {
                Console.Clear();
                Console.Write("Informe o ID do funcionário: ");
                int id;
                while (!int.TryParse(Console.ReadLine(), out id))
                {
                    Console.WriteLine("Valor Inválido");
                    Console.Write("Informe o ID do funcionário: ");
                }

                Console.Write("Informe a Senha do funcionário: ");
                var senha = Console.ReadLine();

                confirmacao = await _context.FUNCIONARIO.Include(f => f.Cargo)
                                            .Include(f => f.Endereco)
                                            .Include(f => f.Pedidos)
                                            .Include(f => f.Endereco)
                                            .Include(f => f.EstoqueMovimentos)
                                            .FirstOrDefaultAsync(f => f.COD_FUNCIONARIO == id && f.SENHA == senha);
                if (confirmacao == null)
                {
                    Console.WriteLine("Credencias inválidas");
                    Console.ReadKey();
                }
            } while (confirmacao == null);

            return confirmacao;
        }

        public bool ValidarPermissaoGerente(Funcionarios funcionario)
        {
            if (funcionario == null) return false;
            else
            {
                if (funcionario.CARGO_FK != 1)
                {
                    Console.WriteLine("Acesso Negado!");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool ValidarPermissaoCaixa(Funcionarios funcionario)
        {
            if (funcionario == null) return false;
            else
            {
                if (funcionario.CARGO_FK == 2 || funcionario.CARGO_FK == 1)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Acesso Negado!");
                    return false;
                }
            }
        }

        public bool ValidarPermissaoEstoque(Funcionarios funcionario)
        {
            if (funcionario == null) return false;
            else
            {
                if (funcionario.CARGO_FK == 3 || funcionario.CARGO_FK == 1)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Acesso Negado!");
                    return false;
                }
            }
        }
    }
}
