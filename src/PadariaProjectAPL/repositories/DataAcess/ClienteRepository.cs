using Microsoft.EntityFrameworkCore;
using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.Context;
using PadariaProjectAPL.Utils;

namespace PadariaProjectAPL.repositories.DataAcess
{
    public class ClienteRepository
    {
        private readonly PadariaDbContext _context;
        private readonly Util _util;
        public ClienteRepository(PadariaDbContext context, Util util)
        {
            _context = context;
            _util = util;
        }

        public async Task CadastrarCliente(Funcionarios funcionario)
        {
            Console.Write("Informe o nome: ");
            var nome = Console.ReadLine();

            Console.Write("Informe o cpf (123.456.789-10): ");
            var cpf = Console.ReadLine();

            Console.Write("Informe o celular (xx) 12345-6789: ");
            var celular = Console.ReadLine();

            Console.Write("Informe o E-mail (seu.email@gmail.com): ");
            var email = Console.ReadLine();


            Console.Write("Informe o endereço: ");
            var endereco = Console.ReadLine();

            var novoEndereco = new Enderecos()
            {
                ENDERECO = endereco
            };
            await _context.ENDERECO.AddAsync(novoEndereco);
            await _context.SaveChangesAsync();

            var novoCliente = new Clientes()
            {
                NOME = nome,
                CPF = cpf,
                CELULAR = celular,
                EMAIL = email,
                ENDERECO_FK = novoEndereco.COD_ENDERECO,
                FUNCIONARIO_FK = funcionario.COD_FUNCIONARIO,
                DATA_CADASTRO = DateTime.Now,
            };
            await _context.CLIENTE.AddAsync(novoCliente);
            await _context.SaveChangesAsync();

            var novaFidelidade = new Fidelidades()
            {
                CLIENTE_FK = novoCliente.COD_CLIENTE,
                PONTOS = 0,
            };
            await _context.FIDELIDADE.AddAsync(novaFidelidade);

            await _context.SaveChangesAsync();
            Console.WriteLine("Cliente cadastrado com sucesso!");
        }
        public async Task ExibirClientePorCodigo()
        {
            await _util.LocalizarCliente();
        }
        public async Task ExibirTodosClientes()
        {
            var clientes = await _context.CLIENTE.Include(c => c.Fidelidade).Include(c => c.Endereco).ToListAsync();

            foreach (var cliente in clientes)
            {
                Console.WriteLine($"Cliente #{cliente.COD_CLIENTE}" +
                              $"\nNome...........: {cliente.NOME}" +
                              $"\nCPF............: {cliente.CPF}" +
                              $"\nCelular........: {cliente.CELULAR}" +
                              $"\nE-mail.........: {cliente.EMAIL}" +
                              $"\nPontos.........: {cliente.Fidelidade.PONTOS}" +
                              $"\nEndereço.......: {cliente.Endereco.ENDERECO}" +
                              $"\n--------------------------------------------------------------");
            }
        }
        public async Task AtualizarCliente()
        {
            var opc = 0;
            var cliente = await _util.LocalizarCliente();
            if (cliente == null) return;

            Console.WriteLine("1- Nome   2-Celular   3-E-mail   4-endereço   5-Cancelaar Operação");
            while (!int.TryParse(Console.ReadLine(), out opc) || opc < 1 || opc > 5)
            {
                Console.WriteLine("Opção inválida.");
                Console.WriteLine("Digite a opção desejada: ");
            }

            switch (opc)
            {
                case 1:
                    Console.Write("Informe o nome: ");
                    cliente.NOME = Console.ReadLine();
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Cliente atualizado do sucesso!");
                    break;
                case 2:
                    Console.Write("Informe o celular: ");
                    cliente.CELULAR = Console.ReadLine();
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Cliente atualizado do sucesso!");
                    break;
                case 3:
                    Console.Write("Informe o E-mail: ");
                    cliente.EMAIL = Console.ReadLine();
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Cliente atualizado do sucesso!");
                    break;
                case 4:
                    Console.Write("Informe o endereço: ");
                    cliente.Endereco.ENDERECO = Console.ReadLine();
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Cliente atualizado do sucesso!");
                    break;
                case 5:
                    Console.WriteLine("Operação cancelada!");
                    return;
            }
        }
        public async Task DeletarCliente()
        {
            var cliente = await _util.LocalizarCliente();
            if (cliente == null) return;

            if (!_util.ConfirmarExclusao()) return;

            _context.CLIENTE.Remove(cliente);
            await _context.SaveChangesAsync();
            Console.WriteLine("Cliente excluido com sucesso!");
        }
    }
}
