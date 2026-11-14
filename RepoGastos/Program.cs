using Microsoft.Data.SqlClient;

namespace RepoGastos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GastoRepositorio repo = new GastoRepositorio();
            string opcion;

            do
            {
                opcion = MostrarMenu();

                switch (opcion)
                {
                    case "1":
                        RegistrarSocio(repo);
                        break;
                    case "2":
                        ListarSocios(repo);
                        break;
                    case "3":
                        BuscarPorIdSocio(repo);
                        break;
                    case "4":
                        ActualizarMontoSocio(repo);
                        break;
                    case "5":
                        EliminarSocio(repo);
                        break;
                    case "6":
                        Console.WriteLine("Saliendo del programa...");
                        break;
                    default:
                        Console.WriteLine("Opción inválida. Intente nuevamente.");
                        break;
                }
            }
            while (opcion != "6");
        }

        private static string MostrarMenu()
        {
            Console.WriteLine();
            Console.WriteLine("||| SISTEMA DE REGISTRO DE SOCIOS GIMNASIO |||");
            Console.WriteLine("Opcion #1.Registrar Socio");
            Console.WriteLine("Opcion #2.Listar Socios");
            Console.WriteLine("Opcion #3.Buscar socio por Id");
            Console.WriteLine("Opcion #4.Actualizar Socio");
            Console.WriteLine("Opcion #5.Eliminar socio");
            Console.WriteLine("Opcion #6.Salir");
            Console.Write("Elige una opción: ");
            return Console.ReadLine();
        }

        private static void RegistrarSocio(GastoRepositorio repo)
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine().Trim();
            Console.Write("Monto: ");
            bool montoOk = decimal.TryParse(Console.ReadLine(), out decimal monto);
            Console.Write("Membresia: ");
            string membresia = Console.ReadLine().Trim();

            if (string.IsNullOrWhiteSpace(nombre) || !montoOk || monto <= 0 || string.IsNullOrWhiteSpace(membresia))
            {
                Console.WriteLine("Datos inválidos.");
                return;
            }

            try
            {
                repo.Insertar(new Gasto
                {
                    Nombre = nombre,
                    Monto = monto,
                    Membresia = membresia
                });
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al registrar: {ex.Message}");
            }
        }

        private static void ListarSocios(GastoRepositorio repo)
        {
            try
            {
                List<Gasto> lista = repo.ListarTodos();

                if (lista.Count == 0)
                {
                    Console.WriteLine("No hay Socios registrados.");
                    return;
                }

                decimal total = 0;
                foreach (Gasto g in lista)
                {
                    Console.WriteLine(g);
                    total += g.Monto;
                }

                Console.WriteLine($"TOTAL SOCIO: Q {total:N2}");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al listar: {ex.Message}");
            }
        }

        private static void BuscarPorIdSocio(GastoRepositorio repo)
        {
            Console.Write("Id del Socio a buscar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            try
            {
                Gasto? gasto = repo.BuscarPorId(id);
                Console.WriteLine(gasto == null ? "No existe ese Id." : gasto.ToString());
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al buscar: {ex.Message}");
            }
        }

        private static void ActualizarMontoSocio(GastoRepositorio repo)
        {
            Console.Write("Id del Socio a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            try
            {
                Gasto? gasto = repo.BuscarPorId(id);
                if (gasto == null)
                {
                    Console.WriteLine("No existe ese Id.");
                    return;
                }

                Console.WriteLine($"Socio Membresia: {gasto}");
                Console.Write("Nueva Membresia: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal monto) || monto <= 0)
                {
                    Console.WriteLine("Membresia inválida.");
                    return;
                }

                gasto.Monto = monto;
                Console.WriteLine(repo.Actualizar(gasto) ? "Membresia actualizada." : "No se actualizó la membresia.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al actualizar la membresia: {ex.Message}");
            }
        }

        private static void EliminarSocio(GastoRepositorio repo)
        {
            Console.Write("Id del Socio a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            try
            {
                bool ok = repo.Eliminar(id);
                Console.WriteLine(ok ? "Socio eliminado." : "No existe ese Id.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al eliminar: {ex.Message}");
            }
        }
    }
}