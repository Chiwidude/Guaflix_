using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_ED1.Models
{
    public class Producto
    {

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public Producto()
        {

        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="tipo">Tipo de producto audiovisual</param>
        /// <param name="nombre">Nombre del producto audiovisual</param>
        /// <param name="alanzamiento">Año de lanzamiento del producto audiovisual</param>
        /// <param name="genero">Género del producto audiovisual</param>
        public Producto(string tipo, string nombre, int alanzamiento, string genero)
        {
            Tipo = tipo;
            Nombre = nombre;
            aLanzamiento = alanzamiento;
            Genero = genero;
        }

        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public int aLanzamiento { get; set; }
        public string Genero { get; set; }
    }

}