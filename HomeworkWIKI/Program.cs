using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeworkWIKI.Clases;
using System.Configuration;
namespace HomeworkWIKI
{

    class Program
    {

        static void Main(string[] args)
        {

            List<Url> list_url = new List<Url>();
            
            Url url_gene = new Url();


           

            Console.WriteLine("*********************** Welcome to Wiki Data View ***************************************");

            Console.WriteLine("Display Max View for language and domain,press 1 or Display Max Count for page, press 2");
            
            string opci =  Console.ReadLine();
           
            if(opci == "1")
            {
                Console.WriteLine("********************** Generation Max View for language and  domain ************************");
            }else
            {
                Console.WriteLine("********************* Generation Max Count for page ****************************************");
            }

            url_gene.guar_datos(opci);
             
            Console.WriteLine("****************** THANK YOU ************************");
            Console.Read();
        }

       



    }
}
