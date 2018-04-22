using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto_ED1.DBContext;
using Proyecto_ED1.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Data;
using System.Web.UI.WebControls;

namespace Proyecto_ED1.Controllers
{
    public class ProyectoController : Controller
    {
        public DefaultConnection db = DefaultConnection.getInstance;
        
        /*Usuario prueba = new Usuario("Angel", "Jimenez", 18, "est1032517", "yomero");
        Usuario prueba1 = new Usuario("Diego", "Jimenez", 18, "Alejandro", "hermano");
        Usuario prueba2 = new Usuario("Roberto", "Jimenez", 18, "Angel", "papa");
        Usuario prueba3 = new Usuario("Oscar", "Jimenez", 18, "Castro", "amigo");*/
        // GET: Proyecto
        public ActionResult IndexUsuario()
        {
            if (db.Orden == 0)
                return View(db.ArbolPorNombre.ToList());
            else if (db.Orden == 1)
                return View(db.ArbolPorGenero.ToList());
            else
                return View(db.ArbolPorALanzamiento.ToList());
        }

        public ActionResult IndexAdministrador()
        {
            if (db.Orden == 0)
                return View(db.ArbolPorNombre.ToList());
            else if (db.Orden == 1)
                return View(db.ArbolPorGenero.ToList());
            else
                return View(db.ArbolPorALanzamiento.ToList());
        }


        public ActionResult OrdenList(int orden, int tipo)
        {
            try
            {
                db.Orden = orden;
                if (db.Temp_.Administrador)
                {
                    return RedirectToAction("IndexAdministrador");
                }
                else
                {
                    if (tipo == 0)
                        return RedirectToAction("IndexUsuario");
                    else
                        return RedirectToAction("WatchList");
                }
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Login()
        {
            db.Orden = 0;
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
                        db.Temp_ = usuario;
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
            db.Orden = 0;
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
                    db.Temp_ = usuario;
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
            return View(db.Temp_.WatchList.ToList());
        }


        public ActionResult AgregarWatchList(string _nombre, string _tipo, string _alanzamiento, string _genero)
        {
            Producto producto = new Producto(_tipo, _nombre, Convert.ToInt32(_alanzamiento), _genero);

            if (producto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(producto);
        }

        [HttpPost,ActionName(nameof(AgregarWatchList))]
        [ValidateAntiForgeryToken]
        public ActionResult AgreWatchList(string _nombre, string _tipo, string _alanzamiento, string _genero)
        {
            Producto producto = new Producto(_tipo, _nombre, Convert.ToInt32(_alanzamiento), _genero);
            db.Temp_.WatchList.Insertar(producto);

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
                    db.Temp_.WatchList.Insertar(producto);

                    return RedirectToAction("IndexAdministrador");
                }

                return RedirectToAction("Index");
            }
            catch(Exception)
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
            //Falta poder cargar la ruta del archivo, pero es lo unico por que lo probe poniendosela manual y si genero el arbol y todo
            string fileJson = System.IO.File.ReadAllText(@"C: \Users\angel\Desktop\MoviesJsonGuaflix.json");
            List<Producto> Carga = (List<Producto>)JsonConvert.DeserializeObject(fileJson, typeof(List<Producto>));
            for(int i = 0; i < Carga.Count(); i++)
            {
                db.ArbolPorNombre.Insertar(Carga[i]);
                db.ArbolPorGenero.Insertar(Carga[i]);
                db.ArbolPorALanzamiento.Insertar(Carga[i]);
                db.Temp_.WatchList.Insertar(Carga[i]);
            }
            return RedirectToAction("IndexAdministrador");
        }

    }
}
