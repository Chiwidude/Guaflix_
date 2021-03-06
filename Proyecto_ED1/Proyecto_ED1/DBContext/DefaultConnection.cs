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
        private static Comparador comparer = new Comparador();
        public Btree<Producto> ArbolPorNombre = new Btree<Producto>(5, comparer.CompareByName);
        public Btree<Producto> ArbolPorGenero = new Btree<Producto>(5, comparer.CompareByGenre);
        public Btree<Producto> ArbolPorALanzamiento = new Btree<Producto>(5, comparer.CompareByYear);
        public Btree<Usuario> ArbolUsuarios = new Btree<Usuario>(5, comparer.CompareByName);
        public JsonFile file = new JsonFile();
        public Usuario admin = new Usuario("","",0,"admin","admin");
        public Usuario Temp_ = new Usuario();
        private int nUsuarios = 0;
        public int Orden { get; set; }


        public void CargarUsuario(Usuario uTemp_)
        {
            ArbolUsuarios.Insertar(uTemp_);
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