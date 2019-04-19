using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab5ED1.Models;


namespace Lab5ED1.Controllers
{
    public class ListadoController : Controller
    {
        

        static private int Compare(Album x,Album y)
        {
            if (x.No_Est > y.No_Est)
                return 1;
            else if (x.No_Est < y.No_Est)
                return -1;
            else
                return 0;
        }
        private static Dictionary<string, List<Album>> album =
        new Dictionary<string, List<Album>>();
        private static Dictionary<Estampa, bool> estampas =
        new Dictionary<Estampa, bool>();
        private static int contador = 0;
        private static void Escribir_Album()
        {
            
            var tempo = album.Values.ToList();
            
            using (StreamWriter archivo = new StreamWriter("C:/Users/jealb/OneDrive/Escritorio/prueba1.csv", true))//lugar donde sobre escribir el archivo
            {
               
                foreach (List<Album> item in tempo)
                {
                    foreach(Album x in item)
                    {
                        archivo.WriteLine(x.Equipo+","+x.No_Est.ToString() + "," +x.Cantidad.ToString() + "," +x.Disponibilidad);
                    }
                }
            }
        }
        private static void Escribir_Estampas()
        {
            
            var temporal = estampas.Keys.ToList();
            bool[] temp = estampas.Values.ToArray();
            int cont = 0; 
            using (StreamWriter archivo = new StreamWriter("C:/Users/jealb/OneDrive/Escritorio/prueba2.csv", true))//lugar donde sobre escribir el archivo
            {
             
                foreach (Estampa item in temporal)
                {
                    if (temp[cont])
                    {
                        archivo.WriteLine(item.Equipo + "," + item.No_Est.ToString() + "," + "1");
                    }
                    else
                    {
                        archivo.WriteLine(item.Equipo + "," + item.No_Est.ToString() + "," + "0");
                    }
                    cont++;
                }
                
            }
        }
        private static void Datos_Album()
        {
            using (StreamReader archivo = new StreamReader("C:/Users/jealb/OneDrive/Escritorio/Album.csv"))//Direccion del archivo el archivo 
            {


                while (archivo.Peek() > -1)
                {

                    string linea = archivo.ReadLine();
                    string[] temp = linea.Split(',');
                    string a = temp[0];
                    int b = Convert.ToInt16(temp[1]);
                    int c = Convert.ToInt16(temp[2]);
                    string d = temp[3];

                    Album tmp = new Album { Equipo = a, No_Est = b, Cantidad = c, Disponibilidad = d };
                    if (album.Keys.Contains(a))
                    {
                        List<Album> aux = album[a];
                        aux.Add(tmp);
                        album[a] = aux;
                    }
                    else
                    {
                        List<Album> aux = new List<Album>();
                        aux.Add(tmp);
                        album.Add(a, aux);
                    }

                }
            }
        }
        private static void Datos_Estampas()
        {
            using (StreamReader archivo = new StreamReader("C:/Users/jealb/OneDrive/Escritorio/Estampa.csv"))//Direccion del archivo el archivo
            {


                while (archivo.Peek() > -1)
                {

                    string linea = archivo.ReadLine();
                    string[] temp = linea.Split(',');
                    string a = temp[0];
                    int b = Convert.ToInt16(temp[1]);
                    int c = Convert.ToInt16(temp[2]);
                    bool aux;
                    if (c == 0)
                    {
                        aux = false;
                    }
                    else
                        aux = true;
                    Estampa tmp = new Estampa { Equipo = a, No_Est = b };
                    estampas.Add(tmp, aux);


                }
            }

        }
        public ActionResult Busqueda_Equipo()
        {
            return View();
        }
        public ActionResult Busqueda_Equipor()
        {

            try
            {
                var tmp = album[Request.Form["Equipo"]];

                return View(tmp);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error");
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Inicio()
        {
            if (contador == 0)
            {
                Datos_Album();
                Datos_Estampas();
                contador++;

            }
            return View();
        }
        public ActionResult Busqueda_Estampa()
        {
            return View();
        }
        public ActionResult Busqueda_Estampar()
        {

            try
            {
                string linea = "";
                int contador2 = 0;
                Estampa key = new Estampa { Equipo = Request.Form["Equipo"], No_Est = Convert.ToInt16(Request.Form["Numero"]) };
                var tmp = estampas.Keys.ToList();
                foreach (Estampa item in tmp)
                {

                    if (item.No_Est == key.No_Est && key.Equipo == item.Equipo)
                    {
                        bool[] temp = estampas.Values.ToArray();
                        if (temp[contador2])
                        {
                            linea = "La estampilla del equipo " + key.Equipo + " La cual es el numero " + key.No_Est.ToString() + " Se encuentra coleccionada";
                            break;
                        }
                        else
                        {
                            linea = "La estampilla del equipo " + key.Equipo + " La cual es el numero " + key.No_Est.ToString() + " No se encuentra coleccionada";
                            break;
                        }
                    }


                    contador2++;
                }
                if(linea =="")
                {
                    linea = "Busqueda incorrecta";
                }
                ViewData["Texto"] = linea;
               return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error");
            }

        }
        public ActionResult Faltantes()
        {
            List<Album> tmp = new List<Album>();
            var estampas = album.Values.ToList();
            foreach (var item in estampas)
            {
                foreach (Album x in item)
                {
                    if (x.Cantidad == 0)
                    {
                        tmp.Add(x);
                    }
                }
            }
            var y = tmp;
            return View(y);
        }
        public ActionResult Modificar()
        {
            return View();
        }
        public ActionResult Modificar_()
        {
            try
            {
                string tm = "";
                List<Album> aux = new List<Album>(); 
                foreach(Album item in album[Request.Form["Equipo"]])
                {
                    if (Convert.ToInt16(Request.Form["Numero"])!=item.No_Est)
                    {
                        aux.Add(item);
                    }
                }
                if (Convert.ToInt16(Request.Form["Cantidad"]) == 0)
                {
                    tm = "Faltante";
                }
                else if (Convert.ToInt16(Request.Form["Cantidad"]) == 1)
                {
                    tm = "Coleccionada";
                }
                else if (Convert.ToInt16(Request.Form["Cantidad"]) > 1) { tm = "Cambio"; }
                Album tmp = new Album { Equipo = Request.Form["Equipo"], No_Est = Convert.ToInt16(Request.Form["Numero"]), Cantidad = Convert.ToInt16(Request.Form["Cantidad"]), Disponibilidad = tm };
                aux.Add(tmp);
                aux.Sort(Compare);
                album[Request.Form["Equipo"]] = aux;

                int contador2 = 0;
                Estampa key = new Estampa { Equipo = Request.Form["Equipo"], No_Est = Convert.ToInt16(Request.Form["Numero"]) };
                var tmpr = estampas.Keys.ToList();
                bool[] temp = estampas.Values.ToArray(); ;
                foreach (Estampa item in tmpr)
                {

                    if (item.No_Est == key.No_Est && key.Equipo == item.Equipo)
                    {
                        
                        if (Convert.ToInt16(Request.Form["Cantidad"])>0)
                        {
                            temp[contador2] = true;

                            break;
                        }
                        else { temp[contador2] = false; };
                        break;
                        
                    }


                    contador2++;
                }
                contador2 = 0;
                estampas.Clear();
                foreach(Estampa item in tmpr)
                {
                    estampas.Add(item, temp[contador2]);
                    contador2++;
                }
                Escribir_Album();
                Escribir_Estampas();
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error");
            }

        }
        public ActionResult Error()
        {
            return View();
        }


    }

}

