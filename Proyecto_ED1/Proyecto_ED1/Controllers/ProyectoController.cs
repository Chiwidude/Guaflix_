using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto_ED1.DBContext;
using Proyecto_ED1.Models;

namespace Proyecto_ED1.Controllers
{
    public class ProyectoController : Controller
    {
        public DefaultConnection db = DefaultConnection.getInstance;
        Usuario prueba = new Usuario("Angel", "Jimenez", 18, "est1032517", "yomero");
        Usuario prueba1 = new Usuario("Diego", "Jimenez", 18, "Alejandro", "hermano");
        Usuario prueba2 = new Usuario("Roberto", "Jimenez", 18, "Angel", "papa");
        Usuario prueba3 = new Usuario("Oscar", "Jimenez", 18, "Castro", "amigo");
        // GET: Proyecto
        public ActionResult IndexUsuario()
        {
            return View(db.Catalogo);
        }

        public ActionResult IndexAdministrador()
        {
            return View(db.Catalogo);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include ="Username,Password")]Models.Usuario usuario)
        {
            //Solo es de prueba
            db.CargarUsuario(prueba);
            db.CargarUsuario(prueba1);
            db.CargarUsuario(prueba2);
            db.CargarUsuario(prueba3);

            bool Encontrado = false;
            if (usuario.Username != default(string) && usuario.Password != default(string))
            {
                for (int i = 0; i < db.Usuarios.Count; i++)
                {
                    db.Usuarios[i].Ingreso(usuario.Username, usuario.Password, ref Encontrado);

                    if (Encontrado == true)
                    {
                        usuario = db.Usuarios[i];
                        if (usuario.Administrador)
                            return RedirectToAction("IndexAdministrador");
                        else
                            return RedirectToAction("IndexUsuario");
                    }
                }

                return View();
            }
            return View();
        }

        public ActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signin([Bind(Include = "Nombre,Apellido,Edad,Username,Password")]Models.Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.CargarUsuario(usuario);
                if (usuario.Administrador)
                    return RedirectToAction("IndexAdministrador");
                else
                    return RedirectToAction("IndexUsuario");
            }

            return View(usuario);
        }


        public ActionResult WatchList()
        {
            return RedirectToAction("IndexUsuario");
        }


        // GET: Proyecto/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Proyecto/Create
        public ActionResult CrearProducto()
        {
            return View();
        }

        // POST: Proyecto/Create
        [HttpPost]
        public ActionResult CrearProducto(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proyecto/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proyecto/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proyecto/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CargaCatalogo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CargaCatalogo(HttpPostedFileBase file)
        {
            return RedirectToAction("IndexUsuario");
        }

    }
}
