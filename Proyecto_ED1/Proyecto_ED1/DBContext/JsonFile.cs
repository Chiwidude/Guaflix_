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