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
                    return RedirectToAction(nameof(IndexAdministrador));
                }
                else
                {
                    if (tipo == 0)
                        return RedirectToAction(nameof(IndexUsuario));
                    else
                        return RedirectToAction(nameof(WatchList));
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        public ActionResult Login()
        {
            db.Orden = 0;
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include ="Username,Password")]Models.Usuario usuario)
        {

            var Listado = db.ArbolUsuarios.ToList();

            var Encontrado = false;
            if (usuario.Username != default(string) && usuario.Password != default(string))
            {
                if (usuario.Username == "admin" && usuario.Password == "admin")
                {
                    db.admin.Administrador = true;
                    db.Temp_ = db.admin;
                    return RedirectToAction(nameof(IndexAdministrador));
                } else {
                    for (int i = 0; i < Listado.Count; i++)
                    {
                        Listado[i].Ingreso(usuario.Username, usuario.Password, ref Encontrado);

                        if (Encontrado == true)
                        {
                            usuario = Listado[i];
                            db.Temp_ = usuario;
                            using (var streamit = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + db.Temp_.Username + "_WatchList.json", FileMode.Open))
                            {
                                var Watchlist = db.file.ProductoList(streamit);
                                foreach(Producto P in Watchlist)
                                {
                                    db.Temp_.WatchList.Insertar(P);
                                }
                            }
                            return RedirectToAction(nameof(IndexUsuario));
                        }
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
                    db.file.UserToJson(db.ArbolUsuarios.ToList());
                    db.Temp_ = usuario;
                    return usuario.Administrador ? RedirectToAction(nameof(IndexAdministrador)) : RedirectToAction(nameof(IndexUsuario));
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

            return RedirectToAction(nameof(IndexUsuario));
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
                    db.file.MoviesToJson(db.ArbolPorNombre.ToList());

                    return RedirectToAction(nameof(IndexAdministrador));
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
                    var productos = db.file.ProductoList(file.InputStream);

                    foreach (Producto p in productos)
                    {
                        db.ArbolPorALanzamiento.Insertar(p);
                        db.ArbolPorGenero.Insertar(p);
                        db.ArbolPorNombre.Insertar(p);
                    }

                    return RedirectToAction(nameof(IndexAdministrador));
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
                    var Usuarios = db.file.UsersToList(file.InputStream);

                    foreach (Usuario U in Usuarios)
                    {
                        db.ArbolUsuarios.Insertar(U);
                    }
                     

                    return RedirectToAction("IndexAdministrador");
                }
            }

            return View();
        }
        public ActionResult CerrarSesión()
        {
            db.Temp_ = null;
            
              return RedirectToAction("Index", "Home");
            
        }
        public ActionResult Busqueda()
        {
            var listaopciones = new List<string>() { "Genero", "Año Lanzamiento", "Nombre" };
            ViewBag.filter = new SelectList(listaopciones);
            return View();
        }

        [HttpPost]
        public ActionResult Busqueda(string filter, string SBusqueda)
        {
            var resultado = new List<Producto>();
            var listaopciones = new List<string>() { "Genero", "Año Lanzamiento", "Nombre" };
            ViewBag.filter = new SelectList(listaopciones);

            if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(SBusqueda) && filter.ToUpper() == "GENERO")
            {
                 resultado = db.ArbolPorGenero.ToList().FindAll(x => x.Genero == SBusqueda);
            }
            if(!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(SBusqueda) && filter.ToUpper() == "AÑO LANZAMIENTO")
            {
                 resultado = db.ArbolPorALanzamiento.ToList().FindAll(x => x.aLanzamiento == Convert.ToInt32(SBusqueda));
            }
            if(!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(SBusqueda) && filter.ToUpper() == "NOMBRE")
            {
                 resultado = db.ArbolPorNombre.ToList().FindAll(x => x.Nombre.Contains(SBusqueda));
            }
            return View(resultado);
        }
        public void EscribirJson()
        {
            Session["ArchivoJson"] = string.Empty;

            var elementos = 0;
            foreach (Producto producto in db.Temp_.WatchList.ToList())
            {
                if (elementos == 0)
                    Session["ArchivoJson"] += JsonConvert.SerializeObject(producto);
                else
                    Session["ArchivoJson"] += "," + JsonConvert.SerializeObject(producto);

                elementos++;
            }

            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +@"\" + db.Temp_.Username + "_WatchList.json", "[" + Session["ArchivoJson"].ToString() + "]");

        }

    }
}
