using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Net;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using System.Threading;
using System.Text.RegularExpressions;
namespace HomeworkWIKI.Clases
{
   public class Url
    {
        public string Link_gene { get; set; }
        public string hora_link { get; set; }
        public string nomb_arch_link { get; set; }
        public string nomb_arch_secu_link { get; set; }
        public string hora_norm { get; set; }
        public Url() { }
         
        

        //helpt to get the link in order to download the gz.
        public List<Url> link_generado()
        {
            Console.WriteLine("*********************  Generation WIKI LINKS ******************************************  " + DateTime.Now.ToShortTimeString());
            List<Url> lista = new List<Url>();

            string url = ConfigurationSettings.AppSettings["WIKIURL"];
            string horas = ConfigurationSettings.AppSettings["HORAS"];
            double hora_double = Convert.ToDouble(horas);

            for (int i = 0; i < hora_double; i++)
            {
                DateTime fecha = DateTime.Now.AddHours(-i);
                string dia = fecha.Day.ToString("00");
                string mes = fecha.Month.ToString("00");
                int anio = fecha.Year;
                string anio_mes = "" + anio + "-" + mes;
                string hora = fecha.Hour.ToString("00");
                string nomb_arch = "/pageviews-" + anio + "" + mes + "" + dia + "-" + hora + "0000.gz";
                string hora_arch = hora + "0000.gz";
                string comp_url = url + "" + anio + "/" + anio_mes + "/pageviews-" + anio + "" + mes + "" + dia + "-" + hora + "0000.gz";
                string hora_arch_secu = hora + "0000.txt";
                lista.Add(new Url() { Link_gene = comp_url , nomb_arch_link = nomb_arch ,hora_link = hora_arch, nomb_arch_secu_link = hora_arch_secu,hora_norm = hora });
            }
             
            return lista;
        }

        //save the archives gz in disk D or depend that you. We can configure the directory in  App.config.
        public void guar_datos(string opci)
        {
            string opcion = opci;

            
            string carp_wiki = ConfigurationSettings.AppSettings["DIRECTORIO"];
            string subc_wiki = ConfigurationSettings.AppSettings["SUBDIRECTORIO"];
            string nomb_arch_temp = "";
            string nomb_arcs_temp = "";
            FileStream fsIn;
            FileStream fsOut;
            try
            {
                if (!Directory.Exists(carp_wiki))
                {
                    Directory.CreateDirectory(carp_wiki);
                }

                if (!Directory.Exists(subc_wiki))
                {
                    Directory.CreateDirectory(subc_wiki);
                }

                List<Url> lista = new List<Url>();
                lista = link_generado();


                for (int i = 0; i < lista.Count; i++)
                {

                    Console.WriteLine(" ******************** Downloading GZ and decompress *********************  " + DateTime.Now.ToShortTimeString());
                    Console.WriteLine(lista[i].Link_gene);

                    nomb_arch_temp = carp_wiki + "" + lista[i].hora_link;
                    nomb_arcs_temp = subc_wiki + "" + lista[i].nomb_arch_secu_link;

                    using (var client = new WebClient())
                    {

                        nomb_arch_temp = carp_wiki + "" + lista[i].hora_link;
                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        client.DownloadFile(lista[i].Link_gene, @nomb_arch_temp);


                    }



                    //Descomprime los archivos
                    fsIn = new FileStream(nomb_arch_temp, FileMode.Open, FileAccess.Read);
                    fsOut = new FileStream(nomb_arcs_temp, FileMode.Create, FileAccess.Write);
                    GZip.Decompress(fsIn, fsOut, true);


                }
                Console.WriteLine(" ******************  Reading TXT *************************** " + DateTime.Now.ToShortTimeString());

                opcion_vista(lista, subc_wiki, opcion);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

      
        }

        //show the option that you choose
        public void opcion_vista(List<Url> lista,string subc_wiki,string opcion)
        {
            string nomb_arcs_temp = "";
            string opci_vist = opcion;
            string deta_arch = "";
            List<Wiki> wiki_list = new List<Wiki>();
            List<Wiki> fina_list = new List<Wiki>();
            if (opci_vist == "1")
            {
                try
                {
                    for (int k = 0; k < lista.Count; k++)
                    {
                        nomb_arcs_temp = subc_wiki + "" + lista[k].nomb_arch_secu_link;



                        var dominio_camb = "";
                        List<string> values = new List<string>();
                        using (FileStream fs = File.Open(nomb_arcs_temp, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (BufferedStream bs = new BufferedStream(fs))
                        using (StreamReader sr = new StreamReader(bs))
                        {
                            string s;
                            while ((s = sr.ReadLine()) != null)
                            {
                                var fields = s.Split(' ');
                                var dominio = fields[0];
                                var pagina = fields[1];
                                var view = Convert.ToInt32(fields[2]);
                                var size = fields[3];


                                var domi_part = dominio.Split('.');

                                if (domi_part.Length < 2)
                                {
                                    dominio_camb = "wikipedia";
                                }
                                else
                                {
                                    if (domi_part[1] == "b")
                                    {
                                        dominio_camb = "wikibooks";
                                    }

                                    if (domi_part[1] == "d")
                                    {
                                        dominio_camb = "wiktionary";
                                    }
                                    if (domi_part[1] == "f")
                                    {
                                        dominio_camb = "wikimediafoundation";
                                    }
                                    if (domi_part[1] == "m")
                                    {
                                        dominio_camb = "wikimedia";
                                    }
                                    if (domi_part[1] == "mw")
                                    {
                                        dominio_camb = "m.${WHITELISTED_PROJECT}";
                                    }
                                    if (domi_part[1] == "n")
                                    {
                                        dominio_camb = "wikinews";
                                    }
                                    if (domi_part[1] == "q")
                                    {
                                        dominio_camb = "wikiquote";
                                    }
                                    if (domi_part[1] == "s")
                                    {
                                        dominio_camb = "wikisource";
                                    }
                                    if (domi_part[1] == "v")
                                    {
                                        dominio_camb = "wikiversity";
                                    }
                                    if (domi_part[1] == "voy")
                                    {
                                        dominio_camb = "wikivoyage";
                                    }
                                    if (domi_part[1] == "w")
                                    {
                                        dominio_camb = "mediawiki";
                                    }
                                    if (domi_part[1] == "wd")
                                    {
                                        dominio_camb = "wikidata";
                                    }
                                }

                                wiki_list.Add(new Wiki { Domain = dominio_camb, Idioma = domi_part[0], Title = pagina, ViewCount = view, Size = size, Hora = lista[k].hora_norm });

                            }

                        }
                        //linq 
                        var grup_data = wiki_list

                           .GroupBy(c => new
                           {
                               c.Hora,
                               c.Idioma,
                               c.Domain
                           })
                           .Select(gcs => new Wiki()
                           {
                               Hora = gcs.Key.Hora,
                               Idioma = gcs.Key.Idioma,
                               Domain = gcs.Key.Domain,
                               ViewCount = gcs.Sum(x => (x.ViewCount))

                           }).ToList();


                        var orde_view = grup_data.AsQueryable().OrderByDescending(d => d.ViewCount).
                            Select(gcsa => new Wiki()
                            {
                                Hora = gcsa.Hora,
                                Idioma = gcsa.Idioma,
                                ViewCount = gcsa.ViewCount,
                                Domain = gcsa.Domain
                            });

                        Console.WriteLine(" ***************** SAVING DATA TXT ************************* "  + DateTime.Now.ToShortTimeString());

                        deta_arch = subc_wiki + "Detalle.txt";
                        FileInfo fi = new FileInfo(deta_arch);

                        if (k != 0)
                        {
                            StreamWriter SW;
                            SW = File.AppendText(deta_arch);
                            foreach (var wiki_data in orde_view)
                            {
                                SW.WriteLine(wiki_data.Hora + " " + wiki_data.Idioma + " " + wiki_data.Domain + " " + wiki_data.ViewCount);

                            }
                            SW.Close();
                        }
                        else
                        {
                            using (StreamWriter sw = fi.CreateText())
                            {
                                foreach (var wiki_data in orde_view)
                                {
                                    sw.WriteLine(wiki_data.Hora + " " + wiki_data.Idioma + " " + wiki_data.Domain + " " + wiki_data.ViewCount);

                                }

                                sw.Close();
                            }
                        }

                        wiki_list.Clear();
                         

                    }

                    Console.WriteLine(" ************************** VIEW **************************** " + DateTime.Now.ToShortTimeString());
                    // tarea final
                    using (FileStream fs = File.Open(deta_arch, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (BufferedStream bs = new BufferedStream(fs))
                    using (StreamReader sr = new StreamReader(bs))
                    {

                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            //final
                            var campos = s.Split(' ');
                            string hora = campos[0];
                            string idioma = campos[1];
                            string dominio = campos[2];
                            int view = Convert.ToInt32(campos[3]);

                            TimeSpan ts = TimeSpan.FromHours(Convert.ToInt16(hora));
                            DateTime dt = DateTime.Today.Add(ts);

                            string hora_form = dt.ToString("hh tt");
                            string hora_form_white = Regex.Replace(hora_form, @"\s", "");

                            fina_list.Add(new Wiki { Domain = dominio, Idioma = idioma, ViewCount = view, Hora = hora_form_white });

 
                        }
                    }

                    
                     
                    // view 1
                    var view_wiki = fina_list.AsQueryable().OrderByDescending(d => d.ViewCount).
                           Select(gcsa => new Wiki()
                           {
                               Hora = gcsa.Hora,
                               Idioma = gcsa.Idioma,
                               ViewCount = gcsa.ViewCount,
                               Domain = gcsa.Domain
                           });
                    Console.Clear();
                    Console.WriteLine( " PERIOD  " + "   " + "     LANGUAGE  " + "   " + "     DOMAIN  " + "    "+ "         VIEW  " );
                    foreach (var wiki_deta in view_wiki)
                    {
                        Console.WriteLine(wiki_deta.Hora + "             " + wiki_deta.Idioma + "            " + wiki_deta.Domain + "            " + wiki_deta.ViewCount);

                    }

                }
                catch (OutOfMemoryException)
                {
                    Console.WriteLine("No hay suficiente memoria");
                }
            }else
            {
                //opcion 2
                try
                {
                    for (int k = 0; k < lista.Count; k++)
                    {
                        nomb_arcs_temp = subc_wiki + "" + lista[k].nomb_arch_secu_link;



                         
                        List<string> values = new List<string>();
                        using (FileStream fs = File.Open(nomb_arcs_temp, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (BufferedStream bs = new BufferedStream(fs))
                        using (StreamReader sr = new StreamReader(bs))
                        {
                            string s;
                            while ((s = sr.ReadLine()) != null)
                            {
                                var fields = s.Split(' ');
                                var dominio = fields[0];
                                var pagina = fields[1];
                                var view = Convert.ToInt32(fields[2]);
                                var size = fields[3];

                                wiki_list.Add(new Wiki {  Title = pagina, ViewCount = view, Hora = lista[k].hora_norm });

                            }

                        }

                        var grup_data = wiki_list

                           .Where(h =>   h.ViewCount > 40)
                           .GroupBy(c => new
                           {
                               c.Hora,
                               c.Title

                           })
                           .Select(gcs => new Wiki()
                           {
                               Hora = gcs.Key.Hora,
                               Title = gcs.Key.Title,
                               ViewCount = gcs.Sum(x => (x.ViewCount))

                           }).ToList();
                         

                        var orde_view = grup_data.AsQueryable().OrderByDescending(d => d.ViewCount).
                            Select(gcsa => new Wiki()
                            {
                                Hora = gcsa.Hora,
                                Title = gcsa.Title,
                                ViewCount = gcsa.ViewCount
                               
                            });

                        Console.WriteLine("*************************** SAVING DATA TXT **************************  " + DateTime.Now.ToShortTimeString());

                        deta_arch = subc_wiki + "Detalle.txt";
                        FileInfo fi = new FileInfo(deta_arch);

                        if (k != 0)
                        {
                            StreamWriter SW;
                            SW = File.AppendText(deta_arch);
                            foreach (var wiki_data in orde_view)
                            {
                                SW.WriteLine(wiki_data.Hora + " " + wiki_data.Title + " " + wiki_data.ViewCount );

                            }
                            SW.Close();
                        }
                        else
                        {
                            using (StreamWriter sw = fi.CreateText())
                            {
                                foreach (var wiki_data in orde_view)
                                {
                                    sw.WriteLine(wiki_data.Hora + " " + wiki_data.Title  + " "   + wiki_data.ViewCount);

                                }

                                sw.Close();
                            }
                        }

                        wiki_list.Clear();
                         

                    }
                    Console.WriteLine("************************** VIEW ****************************  " + DateTime.Now.ToShortTimeString());
                    
                    using (FileStream fs = File.Open(deta_arch, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (BufferedStream bs = new BufferedStream(fs))
                    using (StreamReader sr = new StreamReader(bs))
                    {

                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                             
                            var campos = s.Split(' ');
                            string hora = campos[0];
                            string title = campos[1];
                            int view = Convert.ToInt32(campos[2]);

                            TimeSpan ts = TimeSpan.FromHours(Convert.ToInt16(hora));
                            DateTime dt = DateTime.Today.Add(ts);

                            string hora_form = dt.ToString("hh tt");
                            string hora_form_white = Regex.Replace(hora_form, @"\s", "");

                            fina_list.Add(new Wiki { Hora = hora_form_white , Title = title,  ViewCount = view, });


                        }


                    }
                    // view 2
                    Console.Clear();
                    var view_wiki = fina_list.AsQueryable().OrderByDescending(d => d.ViewCount).
                          Select(gcsa => new Wiki()
                          {
                              Hora = gcsa.Hora,
                              Title = gcsa.Title,
                              ViewCount = gcsa.ViewCount,

                          }).Take(300);
                    Console.WriteLine("PERIOD  " + "           " + "  PAGE  " + "         " + " VIEW  "      );
                    foreach (var wiki_deta in view_wiki)
                    {
                        Console.WriteLine(wiki_deta.Hora + "              " + wiki_deta.Title + "         "   + wiki_deta.ViewCount);

                    }

                }
                catch (OutOfMemoryException)
                {
                    Console.WriteLine("No hay suficiente memoria");
                }
            }

           
        }

    }
}
