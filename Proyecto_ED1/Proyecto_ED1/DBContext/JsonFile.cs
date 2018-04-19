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

    }
}