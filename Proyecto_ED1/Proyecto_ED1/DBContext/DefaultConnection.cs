using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proyecto_ED1.Models;

namespace Proyecto_ED1.DBContext
{
    public class DefaultConnection
    {
        private static volatile DefaultConnection instance;
        private static object sync = new Object();

        public List<Producto> Catalogo = new List<Producto>();
        public List<Usuario> Usuarios = new List<Usuario>();
        private int nUsuarios = 0;

        public void CargarUsuario(Usuario uTemp_)
        {
            if (nUsuarios < 1)
            {
                uTemp_.Administrador = true;
            }

            Usuarios.Add(uTemp_);
            nUsuarios++;
        }

        public static DefaultConnection getInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new DefaultConnection();
                        }
                    }
                }
                return instance;
            }
        }
    }
}