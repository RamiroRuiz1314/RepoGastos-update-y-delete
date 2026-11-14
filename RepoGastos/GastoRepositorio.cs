using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;

namespace RepoGastos
{
    public class GastoRepositorio
    {
        private const string CadenaConexion =
            @"Server=(localdb)\MSSQLLocalDB;" +
            @"Database=GastosDB;" +
            @"Trusted_Connection=True;" +
            @"TrustServerCertificate=True;";

        public void Insertar(Gasto g)
        {
            string sql = "INSERT INTO Gastos (Nombre, Monto, Membresia) " +
                         "VALUES (@nombre, @monto, @membresia)";

            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@nombre", g.Nombre);
                comando.Parameters.AddWithValue("@monto", g.Monto);
                comando.Parameters.AddWithValue("@membresia", g.Membresia);

                conexion.Open();
                int filas = comando.ExecuteNonQuery();
                Console.WriteLine(filas == 1 ? "Socio registrado." : "No se pudo registrar al Socio.");
            }
        }

        public List<Gasto> ListarTodos()
        {
            List<Gasto> lista = new List<Gasto>();
            string sql = "SELECT Id, Nombre, Monto, Membresia " +
                         "FROM Gastos ORDER BY Id";

            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new Gasto
                        {
                            Id = lector.GetInt32(0),
                            Nombre = lector.GetString(1),
                            Monto = lector.GetDecimal(2),
                            Membresia = lector.GetString(3)
                        });
                    }
                }
            }

            return lista;
        }

        public Gasto? BuscarPorId(int id)
        {
            string sql = "SELECT Id, Nombre, Monto, Membresia " +
                         "FROM Gastos WHERE Id = @id";

            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@id", id);
                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        return new Gasto
                        {
                            Id = lector.GetInt32(0),
                            Nombre = lector.GetString(1),
                            Monto = lector.GetDecimal(2),
                            Membresia = lector.GetString(3)
                        };
                    }
                }
            }
            return null;
        }

        public bool Actualizar(Gasto g)
        {
            string sql = "UPDATE Gastos " +
                         "SET Nombre = @Nombre, Monto = @monto, Membresia = @membresia " +
                         "WHERE Id = @id";

            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@nombre", g.Nombre);
                comando.Parameters.AddWithValue("@monto", g.Monto);
                comando.Parameters.AddWithValue("@membresia", g.Membresia);
                comando.Parameters.AddWithValue("@id", g.Id);

                conexion.Open();
                return comando.ExecuteNonQuery() == 1;
            }
        }

        public bool Eliminar(int id)
        {
            string sql = "DELETE FROM Gastos WHERE Id = @id";

            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@id", id);

                conexion.Open();
                return comando.ExecuteNonQuery() == 1;
            }
        }
    }
}