using Proyecto_ED1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_ED1.DBContext
{
    public class Comparador
    {
        public int CompareByName(Producto pelicula1, Producto pelicula2)
        {
            return pelicula1 == null || pelicula2 == null ? 1 : pelicula1.Nombre.CompareTo(pelicula2.Nombre);
        }
        public int CompareByGenre(Producto pelicula1, Producto pelicula2)
        {
            if (pelicula1 == null || pelicula2 == null)
            {
                return 1;
            }
            else
            {
                var result = pelicula1.Genero.CompareTo(pelicula2.Genero);
                return result == 0 ? CompareByName(pelicula1, pelicula2) : result;
            }
        }
        public int CompareByYear(Producto pelicula1, Producto pelicula2)
        {
            if (pelicula1 == null || pelicula2 == null)
            {
                return 1;
            }
            else
            {
                var comparison = pelicula1.aLanzamiento.CompareTo(pelicula2.aLanzamiento);
                return comparison == 0 ? CompareByName(pelicula1, pelicula2) : comparison;
            }
        }

        public int CompareByName(Usuario user1, Usuario user2)
        {
            return user1 == null || user2 == null ? 1 : user1.Username.CompareTo(user2.Username);
        }
    }
}