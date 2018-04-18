using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_ED1.Models
{
    public class Usuario
    {
        /// <summary>
        /// Constructor de la clase Usuario
        /// </summary>
        public Usuario()
        {
            Nombre = default(string);
            Apellido = default(string);
            Edad = default(int);
            Username = default(string);
            Password = default(string);
            Administrador = false;
        }

        /// <summary>
        /// Constructor de la clase Usuario para asignar valores a sus atributos
        /// </summary>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="apellido">Apellido del usuario</param>
        /// <param name="edad">Edad del ususario</param>
        /// <param name="username">Username del usuario</param>
        /// <param name="password">Password del usuario</param>
        public Usuario(string nombre,string apellido,int edad,string username,string password)
        {
            Nombre = nombre;
            Apellido = apellido;
            Edad = edad;
            Username = username;
            Password = password;
        }


        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Administrador { get; set; }
        //Arbol con la WatchList

        /// <summary>
        /// Función para validar el ingreso a la plataforma
        /// </summary>
        /// <param name="username">Username del usuario que desea ingresar</param>
        /// <param name="password">Password del usuario que desea ingresar</param>
        /// <param name="Encontrado">Validación de haber encontrado un usuario con dicho Username y Password</param>
        /// <returns>El usuario que cumple con los parametros</returns>
        public Usuario Ingreso(string username,string password, ref bool Encontrado)
        {
            Usuario Utemp_ = new Usuario();
            if(username==Username && password == Password)
            {
                Utemp_ = new Usuario(Nombre, Apellido, Edad, Username, Password);
                Encontrado = true;
                return Utemp_;
            }

            Encontrado = false;
            return Utemp_;
        }
    }
}