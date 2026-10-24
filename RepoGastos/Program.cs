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
                        RegistrarGasto(repo);
                        break;
                    case "2":
                        ListarGastos(repo);
                        break;
                    case "3":
                        BuscarPorIdGasto(repo);
                        break;
                    case "4":
                        ActualizarMontoGasto(repo);
                        break;
                    case "5":
                        EliminarGasto(repo);
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
            Console.WriteLine("||| REPOSITORIO DE GASTOS |||");
            Console.WriteLine("Opcion #1.Registrar gasto");
            Console.WriteLine("Opcion #2.Listar gastos");
            Console.WriteLine("Opcion #3.Buscar gasto por Id");
            Console.WriteLine("Opcion #4.Actualizar monto");
            Console.WriteLine("Opcion #5.Eliminar gasto");
            Console.WriteLine("Opcion #6.Salir");
            Console.Write("Elige una opción: ");
            return Console.ReadLine();
        }

        private static void RegistrarGasto(GastoRepositorio repo)
        {
            Console.Write("Descripción: ");
            string descripcion = Console.ReadLine().Trim();
            Console.Write("Monto: ");
            bool montoOk = decimal.TryParse(Console.ReadLine(), out decimal monto);
            Console.Write("Categoría: ");
            string categoria = Console.ReadLine().Trim();

            if (string.IsNullOrWhiteSpace(descripcion) || !montoOk || monto <= 0 || string.IsNullOrWhiteSpace(categoria))
            {
                Console.WriteLine("Datos inválidos.");
                return;
            }

            try
            {
                repo.Insertar(new Gasto
                {
                    Descripcion = descripcion,
                    Monto = monto,
                    Categoria = categoria
                });
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al registrar: {ex.Message}");
            }
        }

        private static void ListarGastos(GastoRepositorio repo)
        {
            try
            {
                List<Gasto> lista = repo.ListarTodos();

                if (lista.Count == 0)
                {
                    Console.WriteLine("No hay gastos registrados.");
                    return;
                }

                decimal total = 0;
                foreach (Gasto g in lista)
                {
                    Console.WriteLine(g);
                    total += g.Monto;
                }

                Console.WriteLine($"TOTAL GASTADO: Q {total:N2}");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al listar: {ex.Message}");
            }
        }

        private static void BuscarPorIdGasto(GastoRepositorio repo)
        {
            Console.Write("Id del gasto a buscar: ");
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

        private static void ActualizarMontoGasto(GastoRepositorio repo)
        {
            Console.Write("Id del gasto a actualizar: ");
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

                Console.WriteLine($"Gasto actual: {gasto}");
                Console.Write("Nuevo monto: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal monto) || monto <= 0)
                {
                    Console.WriteLine("Monto inválido.");
                    return;
                }

                gasto.Monto = monto;
                Console.WriteLine(repo.Actualizar(gasto) ? "Gasto actualizado." : "No se actualizó.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al actualizar: {ex.Message}");
            }
        }

        private static void EliminarGasto(GastoRepositorio repo)
        {
            Console.Write("Id del gasto a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            try
            {
                bool ok = repo.Eliminar(id);
                Console.WriteLine(ok ? "Gasto eliminado." : "No existe ese Id.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al eliminar: {ex.Message}");
            }
        }
    }
}