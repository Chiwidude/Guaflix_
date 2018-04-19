using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto_ED1.DBContext;
using Proyecto_ED1.Models;
using System.Net;

namespace Proyecto_ED1.Controllers
{
    public class ProyectoController : Controller
    {
        public DefaultConnection db = DefaultConnection.getInstance;
        public Usuario Temp_ = new Usuario();
        /*Usuario prueba = new Usuario("Angel", "Jimenez", 18, "est1032517", "yomero");
        Usuario prueba1 = new Usuario("Diego", "Jimenez", 18, "Alejandro", "hermano");
        Usuario prueba2 = new Usuario("Roberto", "Jimenez", 18, "Angel", "papa");
        Usuario prueba3 = new Usuario("Oscar", "Jimenez", 18, "Castro", "amigo");*/
        // GET: Proyecto
        public ActionResult IndexUsuario()
        {
            return View(db.ArbolPorNombre.ToList());
        }

        public ActionResult IndexAdministrador()
        {
            return View(db.ArbolPorNombre.ToList());
        }

        public ActionResult Login()
        {
            if (db.ArbolUsuarios.ToList().Count == 0)
            {
                return RedirectToAction("Signin");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include ="Username,Password")]Models.Usuario usuario)
        {
            //Solo es de prueba
            /*db.CargarUsuario(prueba);
            db.CargarUsuario(prueba1);
            db.CargarUsuario(prueba2);
            db.CargarUsuario(prueba3);*/

            List<Models.Usuario> Listado = db.ArbolUsuarios.ToList();

            bool Encontrado = false;
            if (usuario.Username != default(string) && usuario.Password != default(string))
            {
                for (int i = 0; i < Listado.Count; i++)
                {
                    Listado[i].Ingreso(usuario.Username, usuario.Password, ref Encontrado);

                    if (Encontrado == true)
                    {
                        usuario = Listado[i];
                        Temp_ = usuario;
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
            if (usuario.Nombre != default(string) && usuario.Apellido != default(string) &&
                usuario.Edad != default(int) && usuario.Username != default(string) && usuario.Password != default(string))
            {
                if (ModelState.IsValid)
                {
                    db.CargarUsuario(usuario);
                    Temp_ = usuario;
                    if (usuario.Administrador)
                        return RedirectToAction("IndexAdministrador");
                    else
                        return RedirectToAction("IndexUsuario");
                }
            }

            return View(usuario);
        }


        public ActionResult WatchList()
        {
            return View(Temp_.WatchList.ToList());
        }

        public ActionResult AgregarWatchList(Producto id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(id);
        }

        [HttpPost,ActionName(nameof(AgregarWatchList))]
        [ValidateAntiForgeryToken]
        public ActionResult AgreWatchList(Producto id)
        {
            Temp_.WatchList.Insertar(id);
            return RedirectToAction("IndexUsuario");
        }
        
        // GET: Proyecto/Create
        public ActionResult CrearProducto()
        {
            return View();
        }

        // POST: Proyecto/Create
        [HttpPost]
        public ActionResult CrearProducto([Bind(Include ="Tipo,Nombre,aLanzamiento,Genero")]Models.Producto producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ArbolPorNombre.Insertar(producto);
                    db.ArbolPorGenero.Insertar(producto);
                    db.ArbolPorALanzamiento.Insertar(producto);
                    Temp_.WatchList.Insertar(producto);

                    return RedirectToAction("IndexAdministrador");
                }

                return RedirectToAction("Index");
            }
            catch(Exception)
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
