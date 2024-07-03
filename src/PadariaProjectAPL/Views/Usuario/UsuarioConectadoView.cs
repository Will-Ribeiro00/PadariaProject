using PadariaProjectAPL.Entities;
using PadariaProjectAPL.repositories.DataAcess;

namespace PadariaProjectAPL.Views.Usuario
{
    public class UsuarioConectadoView
    {
        private readonly FuncionarioRepository _repository;
        public UsuarioConectadoView(FuncionarioRepository repository)
        {
            _repository = repository;
        }

        public async Task ExibirUsuario(Funcionarios funcionario)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("*********PADOKA DA VILA*********" +
                              "\n---------------------------------------" +
                             $"\n#{funcionario.COD_FUNCIONARIO} - {funcionario.Cargo.CARGO} - {funcionario.NOME}" +
                             $"\n---------------------------------------" +
                             $"\n\n***********MEUS DADOS***********");

                Console.WriteLine($"Funcionário #{funcionario.COD_FUNCIONARIO}" +
                                  $"\nNome...........: {funcionario.NOME}" +
                                  $"\nCargo..........: {funcionario.Cargo.CARGO}" +
                                  $"\nCPF............: {funcionario.CPF}" +
                                  $"\nCelular........: {funcionario.CELULAR}" +
                                  $"\nSalário........: {funcionario.SALARIO}" +
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
