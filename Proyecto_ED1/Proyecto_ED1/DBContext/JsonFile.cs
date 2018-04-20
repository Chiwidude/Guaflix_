using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using Proyecto_ED1.Models;
namespace Proyecto_ED1.DBContext
{
    public class JsonFile
    {
        /// <summary>
        /// Lee un archivo Json, donde se encuentran los productos de la app
        /// </summary>
        /// <param name="path"></param> Dirección donde se encuentra el archivo
        /// <returns></returns> arreglo con los objetos convertidos
        public Producto[] ProductoList(Stream path)
        {
            Producto[] array;
            try
            {
               using (var reader1 = new StreamReader(path))
                {
                    var lecture = reader1.ReadToEnd();
                    array = JsonConvert.DeserializeObject<Producto[]>(lecture);
                    reader1.Close();
                }
                return array;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        /// <summary>
        /// Guarda en un Archivo Json, el catálogo de Películas para su posterior carga
        /// </summary>
        /// <param name="Catálogo"></param>
        public void MoviesToJson(List<Producto> Catálogo)
        {
            try
            {

                using (StreamWriter file = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Catálogo.json"))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(file, Catálogo);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Convierte una lista de usuarios y los escribe en un archivo Json
        /// </summary>
        /// <param name="user"></param> Lista de usuarios registrados
        public void UserToJson(List<Usuario> user)
        {
            try
            {

                using (StreamWriter file = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Usuarios.json"))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(file, user);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Se lee un Json, con un listado de Usuarios Registrados
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns> Lista de Usuarios leidos
        public List<Usuario> UsersToList(Stream Path)
        {
            List<Usuario> array;
            try
            {
                using (var reader1 = new StreamReader(Path))
                {
                    var lecture = reader1.ReadToEnd();
                    array = JsonConvert.DeserializeObject<List<Usuario>>(lecture);
                    reader1.Close();
                }
                return array;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


    }
}