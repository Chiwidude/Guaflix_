﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proyecto_ED1.Models;
using BTree;


namespace Proyecto_ED1.DBContext
{
    public class DefaultConnection
    {
        private static volatile DefaultConnection instance;
        private static object sync = new Object();

        public List<Producto> Catalogo = new List<Producto>();
        public List<Usuario> Usuarios = new List<Usuario>();
        private static Comparador comparer = new Comparador();
        public Btree<Producto> ArbolPorNombre = new Btree<Producto>(5, comparer.CompareByName);
        public Btree<Producto> ArbolPorGenero = new Btree<Producto>(5, comparer.CompareByGenre);
        public Btree<Producto> ArbolPorALanzamiento = new Btree<Producto>(5, comparer.CompareByYear);
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