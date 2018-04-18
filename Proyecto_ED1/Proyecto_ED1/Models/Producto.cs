using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_ED1.Models
{
    public class Producto:IComparable
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
        public Producto(string tipo,string nombre,int alanzamiento, string genero)
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

        /// <summary>
        /// Compara el objeto tomando como referencia el nombre del producto audiovisual
        /// </summary>
        /// <param name="obj">Objeto con el que se comparará</param>
        /// <returns>Valor para indicar si es mayor o menor</returns>
        public int CompareNombreTo(object obj)
        {
            if (obj == null) return 1;

            var Product = obj as Producto;
            if(Product != null)
            {
                return this.Nombre.CompareTo(Product.Nombre);
            }
            else
            {
                throw new ArgumentException("No esta comparando los atributos correctos");
            }
        }

        /// <summary>
        /// Compara el objeto tomando como referencia el tipo de producto audiovisual
        /// </summary>
        /// <param name="obj">Objeto con el que se comparará</param>
        /// <returns>Valor para indicar si es mayor o menor</returns>
        public int CompareTipoTo(object obj)
        {
            if (obj == null) return 1;

            var Product = obj as Producto;
            if (Product != null)
            {
                int comp = this.Tipo.CompareTo(Product.Tipo);

                if (comp == 0)
                {
                    return CompareNombreTo(Product);
                }
                else
                {
                    return comp;
                }
            }
            else
            {
                throw new ArgumentException("No esta comparando los atributos correctos");
            }
        }

        /// <summary>
        /// Compara el objeto tomando como referencia el año de lanzamiento
        /// </summary>
        /// <param name="obj">Objeto con el que se comparará</param>
        /// <returns>Valor para indicar si es mayor o menor</returns>
        public int CompareALanzamientoTo(object obj)
        {
            if (obj == null) return 1;

            var Product = obj as Producto;
            if (Product != null)
            {
                int comp = this.aLanzamiento.CompareTo(Product.aLanzamiento);

                if (comp == 0)
                {
                    return CompareNombreTo(Product);
                }
                else
                {
                    return comp;
                }
            }
            else
            {
                throw new ArgumentException("No esta comparando los atributos correctos");
            }
        }

        /// <summary>
        /// Compara el objeto tomando como referencia el genero
        /// </summary>
        /// <param name="obj">Objeto con el que se comparará</param>
        /// <returns>Valor para indicar si es mayor o menor</returns>
        public int CompareGeneroTo(object obj)
        {
            if (obj == null) return 1;

            var Product = obj as Producto;
            if (Product != null)
            {
                int comp = this.Genero.CompareTo(Product.Genero);

                if (comp == 0)
                {
                    return CompareNombreTo(Product);
                }
                else
                {
                    return comp;
                }
            }
            else
            {
                throw new ArgumentException("No esta comparando los atributos correctos");
            }
        }



        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}