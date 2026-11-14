using System;
using System.Collections.Generic;
using System.Text;

namespace RepoGastos
{
    public class Gasto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Monto { get; set; }
        public string Membresia { get; set; }

        public override string ToString()
        {
            return $"{Id}: Nombre Socio: {Nombre}|| Monto: {Monto}|| Membresia: {Membresia}";
        }
    }
}