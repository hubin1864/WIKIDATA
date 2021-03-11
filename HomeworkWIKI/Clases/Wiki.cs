using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace HomeworkWIKI.Clases
{
    public class Wiki
    {

        public string Domain { get; set; }
        public string Title { get; set; }
        public int ViewCount { get; set; }
        public string Size { get; set; }
        public string Hora { get; set; }
        public string Idioma { get; set; }
        public Wiki(string domain, string title, int viewcount, string size, string hora, string idioma)
        {
            Domain = domain;
            Title = title;
            ViewCount = viewcount;
            Size = size;
            Hora = hora;
            Idioma = idioma;
        }
          

        public Wiki()
        {

        }
    }

}
