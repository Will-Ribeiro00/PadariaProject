using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.DataAcess;
using PadariaProjectAPL.Utils;

namespace PadariaProjectAPL.Views.Usuario
{
    public class UsuarioConectadoView
    {
        private readonly FuncionarioRepository _repository;
        private readonly Util _util;
        public UsuarioConectadoView(FuncionarioRepository repository, Util util)
        {
            _repository = repository;
            _util = util;
        }

        public async Task ExibirUsuario(Funcionarios funcionario)
        {
            do
            {
                Console.Clear();
                _util.Cabecalho(funcionario);

                Console.WriteLine($"Funcionário #{funcionario.COD_FUNCIONARIO}" +
                                  $"\nNome...........: {funcionario.NOME}" +
                                  $"\nCargo..........: {funcionario.Cargo.CARGO}" +
                                  $"\nCPF............: {funcionario.CPF}" +
                                  $"\nCelular........: {funcionario.CELULAR}" +
                                  $"\nSalário........: {funcionario.SALARIO}" +
                                  $"\nStatus.........: {funcionario.Status.STATUS}" +
                                  $"\nEndereço.......: {funcionario.Endereco.ENDERECO}");

                Console.WriteLine("\n1 - Atualizar Senha" +
                                  "\n2 - Voltar");
                Console.Write("Informe a opção desejada: ");
                int opc;
                while (!int.TryParse(Console.ReadLine(), out opc) || opc < 1 || opc > 2)
                {
                    Console.WriteLine("Valor Inválido!");
                    Console.Write("Informe a opção desejada: ");
                }

                switch (opc)
                {
                    case 1:
                        Console.Clear();
                        await _repository.AlterarSenha(funcionario);
                        Console.ReadKey();
                        break;
                    case 2:
                        return;
                }
            } while (true);

        }
    }
}
