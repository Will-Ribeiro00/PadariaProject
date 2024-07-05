using Microsoft.EntityFrameworkCore;
using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.Context;
using PadariaProjectAPL.Utils;

namespace PadariaProjectAPL.repositories.DataAcess
{
    public class FuncionarioRepository
    {
        private readonly PadariaDbContext _dbContext;
        private readonly Util _util;
        public FuncionarioRepository(PadariaDbContext dbContext, Util util)
        {
            _dbContext = dbContext;
            _util = util;
        }

        public async Task CadastrarFuncionario()
        {
            Console.Write("Informe o nome: ");
            var nome = Console.ReadLine();

            Console.Write("Informe o CPF (123.456.789-10); ");
            var cpf = Console.ReadLine();

            Console.Write("Informe o celular (xx) 12345-6789: ");
            var celular = Console.ReadLine();

            Console.Write("informe o código do cargo (1-Gerente 2-Caixa 3-Estoquista): ");
            int cargo;
            while (!int.TryParse(Console.ReadLine(), out cargo))
            {
                Console.WriteLine("Valor Inválido");
                Console.Write("informe o código do cargo (1-Gerente 2-Caixa 3-Estoquista): ");
            }

            Console.Write("Informe o endereço: ");
            var endereco = Console.ReadLine();

            Console.Write("Informe o sálario: ");
            decimal salario;
            while (!decimal.TryParse(Console.ReadLine(), out salario))
            {
                Console.WriteLine("Valor Inválido");
                Console.Write("informe o salário: ");
            }

            var novoEndereco = new Enderecos()
            {
                ENDERECO = endereco
            };
            await _dbContext.ENDERECO.AddAsync(novoEndereco);
            await _dbContext.SaveChangesAsync();

            var novoFuncionario = new Funcionarios()
            {
                NOME = nome,
                CPF = cpf,
                CELULAR = celular,
                CARGO_FK = cargo,
                SALARIO = salario,
                SENHA = "123456",
                ENDERECO_FK = novoEndereco.COD_ENDERECO,
            };
            await _dbContext.FUNCIONARIO.AddAsync(novoFuncionario);
            await _dbContext.SaveChangesAsync();
            Console.WriteLine("Funcionário cadastrado com sucesso!");
        }
        public async Task ExibirFuncionarioPorCodigo()
        {
            await _util.LocalizarFuncionario();
        }
        public async Task ExibirFuncionariosPorCargo()
        {
            int cod;
            Console.Write("Informe o código do cargo: ");
            while (!int.TryParse(Console.ReadLine(), out cod))
            {
                Console.WriteLine("Valor Inválido");
                Console.Write("Informe o código do cargo: ");
            }

            var cargo = await _dbContext.CARGO.FirstOrDefaultAsync(c => c.COD_CARGO == cod);
            if (cargo == null)
            {
                Console.WriteLine("Não foi possível lovalizar um cargo com o código informado!");
                return;
            }
            var funcionarios = await _dbContext.FUNCIONARIO.Include(f => f.Cargo)
                                                           .Include(f => f.Endereco)
                                                           .Include(f => f.Cargo)
                                                           .Include(f => f.Status)
                                                           .Where(f => f.CARGO_FK == cargo.COD_CARGO).ToListAsync();
            Console.WriteLine($"\n{cargo.CARGO.ToUpper()}:\n");
            foreach (var funcionario in funcionarios)
            {
                Console.WriteLine($"Funcionário #{funcionario.COD_FUNCIONARIO}" +
                              $"\nNome...........: {funcionario.NOME}" +
                              $"\nCargo..........: {funcionario.Cargo.CARGO}" +
                              $"\nCPF............: {funcionario.CPF}" +
                              $"\nCelular........: {funcionario.CELULAR}" +
                              $"\nSalário........: {funcionario.SALARIO}" +
                              $"\nStatus.........: {funcionario.Status.STATUS}" +
                              $"\nEndereço.......: {funcionario.Endereco.ENDERECO}" +
                              $"\n---------------------------------------------------------------------");
            }

        }
        public async Task ExibirTodosFuncionarios()
        {
            var cargos = await _dbContext.CARGO.Include(c => c.Funcionarios).ThenInclude(f => f.Endereco)
                                               .Include(c => c.Funcionarios).ThenInclude(f => f.Status).ToListAsync();

            foreach (var cargo in cargos)
            {
                Console.WriteLine($"\n{cargo.CARGO.ToUpper()}:");
                foreach (var funcionario in cargo.Funcionarios)
                {
                    Console.WriteLine($"Funcionário #{funcionario.COD_FUNCIONARIO}" +
                              $"\nNome...........: {funcionario.NOME}" +
                              $"\nCargo..........: {funcionario.Cargo.CARGO}" +
                              $"\nCPF............: {funcionario.CPF}" +
                              $"\nCelular........: {funcionario.CELULAR}" +
                              $"\nSalário........: {funcionario.SALARIO}" +
                              $"\nStatus.........: {funcionario.Status.STATUS}" +
                              $"\nEndereço.......: {funcionario.Endereco.ENDERECO}" +
                              $"\n---------------------------------------------------------------");
                }
            }
        }
        public async Task AtualizarFuncionario()
        {
            int opc;
            var funcionario = await _util.LocalizarFuncionario();
            if (funcionario == null) return;

            Console.WriteLine("1-Nome | 2-Cargo | 3-Celular | 4-Salário | 5-Endereço | 6-Status | 7-Cancelar Operação");
            Console.Write("Informe o dado que deseja alterar: ");
            while (!int.TryParse(Console.ReadLine(), out opc) || opc < 1 || opc > 7)
            {
                Console.WriteLine("Valor Inválido!");
                Console.Write("Informe o dado que deseja alterar: ");
            }

            switch (opc)
            {
                case 1:
                    Console.Write("Informe o nome: ");
                    funcionario.NOME = Console.ReadLine();
                    Console.WriteLine("Funcionário alterado com sucesso!");
                    await _dbContext.SaveChangesAsync();
                    break;

                case 2:
                    Console.Write("Informe o código do cargo (1-Gerente  2-Caixa  3-Estoquista): ");
                    int cargo;
                    while (!int.TryParse(Console.ReadLine(), out cargo) || cargo < 1 || cargo > 3)
                    {
                        Console.WriteLine("Valor Inválido");
                        Console.Write("Informe o código do cargo (1-Gerente 2-Caixa 3-Estoquista): ");
                    }
                    funcionario.CARGO_FK = cargo;
                    Console.WriteLine("Funcionário alterado com sucesso!");
                    await _dbContext.SaveChangesAsync();
                    break;

                case 3:
                    Console.Write("Informe o celular: ");
                    funcionario.CELULAR = Console.ReadLine();
                    Console.WriteLine("Funcionário alterado com sucesso!");
                    await _dbContext.SaveChangesAsync();
                    break;

                case 4:
                    Console.Write("Informe o salário: ");
                    decimal salario;
                    while (!decimal.TryParse(Console.ReadLine(), out salario))
                    {
                        Console.WriteLine("Valor Inválido");
                        Console.Write("informe o salário: ");
                    }
                    funcionario.SALARIO = salario;
                    Console.WriteLine("Funcionário alterado com sucesso!");
                    await _dbContext.SaveChangesAsync();
                    break;

                case 5:
                    Console.Write("Informe o endereço: ");
                    funcionario.Endereco.ENDERECO = Console.ReadLine();
                    Console.WriteLine("Funcionário alterado com sucesso!");
                    await _dbContext.SaveChangesAsync();
                    break;

                case 6:
                    Console.Write("Informe o código do status (1-Ativo  2-Atestado  3-Férias  4-Desligado): ");
                    int status;
                    while (!int.TryParse(Console.ReadLine(), out status) || status < 1 || status > 4)
                    {
                        Console.WriteLine("Valor Inválido");
                        Console.Write("Informe o código do status (1-Ativo  2-Atestado  3-Férias  4-Desligado): ");
                    }
                    funcionario.STATUS_FK = status;
                    Console.WriteLine("Funcionário alterado com sucesso!");
                    await _dbContext.SaveChangesAsync();
                    break;

                case 7:
                    Console.WriteLine("Operação cancelada!");
                    return;
            }
        }
        public async Task DeletarFuncionario()
        {
            var funcionario = await _util.LocalizarFuncionario();
            if (!_util.ConfirmarExclusao()) return;

            _dbContext.FUNCIONARIO.Remove(funcionario);
            await _dbContext.SaveChangesAsync();

            Console.WriteLine("Funcionário excluido com sucesso!");
        }

        public async Task AlterarSenha(Funcionarios funcionario)
        {
            Console.Write("\nDigite a senha atual: ");
            var senhaAtual = Console.ReadLine();
            if (senhaAtual == funcionario.SENHA)
            {
                Console.Write("Digite a nova senha: ");
                var novaSenha = Console.ReadLine();
                Console.Write("Confirme a nova senha: ");
                var confirmacao = Console.ReadLine();

                if (novaSenha == confirmacao)
                {
                    funcionario.SENHA = novaSenha;
                    await _dbContext.SaveChangesAsync();
                    Console.WriteLine("Senha alterada com sucesso!");
                }
                else
                {
                    Console.WriteLine("Erro - As senhas não coincidem!");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Senha atual inválida");
                return;
            }
        }
    }
}
