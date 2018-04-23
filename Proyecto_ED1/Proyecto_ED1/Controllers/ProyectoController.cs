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
                    var json = db.File.Value;
                    json.UserToJson(db.ArbolUsuarios.ToList());
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
            EscribirJson();

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
            
            if(file != null)
            {
                
                 if (!file.FileName.EndsWith(".json"))
                {
                    return View();
                }
                 if(file.ContentLength > 0)
                {
                    var jsonfile = db.File.Value;

                    var productos = jsonfile.ProductoList(file.InputStream);

                    foreach (Producto p in productos)
                    {
                        db.ArbolPorALanzamiento.Insertar(p);
                        db.ArbolPorGenero.Insertar(p);
                        db.ArbolPorNombre.Insertar(p);
                    }

                    return RedirectToAction("IndexAdministrador");
                }
            }
           
            return View();
        }


        public ActionResult CargaUsuarios()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CargaUsuarios(HttpPostedFileBase file)
        {

            if (file != null)
            {

                if (!file.FileName.EndsWith(".json"))
                {
                    return View();
                }
                if (file.ContentLength > 0)
                {
                    var jsonfile = db.File.Value;

                    var Usuarios = jsonfile.UsersToList(file.InputStream);

                    foreach (Usuario U in Usuarios)
                    {
                        db.ArbolUsuarios.Insertar(U);
                    }
                     

                    return RedirectToAction("IndexAdministrador");
                }
            }

            return View();
        }


        public void EscribirJson()
        {
            Session["ArchivoJson"] = string.Empty;

            int elementos = 0;
            foreach (Producto producto in db.Temp_.WatchList.ToList())
            {
                if (elementos == 0)
                    Session["ArchivoJson"] += JsonConvert.SerializeObject(producto);
                else
                    Session["ArchivoJson"] += "," + JsonConvert.SerializeObject(producto);

                elementos++;
            }

            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + db.Temp_.Username + "_WatchList.json", "[" + Session["ArchivoJson"].ToString() + "]");
            
        }

    }
}
