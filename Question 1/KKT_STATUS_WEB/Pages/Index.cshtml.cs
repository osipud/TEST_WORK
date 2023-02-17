using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HtmlAgilityPack;
using System.Net;
using System.Net.Mail;
using KKT_STATUS_WEB.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;
using System.Reflection;

namespace KKT_STATUS_WEB.Pages
{
    public class IndexModel : PageModel
    {
        //Создание экземпляра класса модели данных
        public List<Device> devlist = new();

        Device device = new Device();

        #if (DEBUG)
        readonly List<string> IPAdress = new List<string>() {
            "109.73.13.157", 
            "94.72.4.38"
        };
        #elif (RELEASE)
            readonly List<string> IPAdress = new List<string>() {
                "192.168.10.201",
                "192.168.10.202",
                "192.168.10.203",
                "192.168.10.204",
                "192.168.10.205",
                "192.168.10.206",
                "192.168.10.207",
                "192.168.10.208",
                "192.168.10.209",
                "192.168.10.210",
                "192.168.10.211",
                "192.168.10.212",
                "192.168.10.213",
                "192.168.10.214",
                "192.168.10.215",
                "192.168.10.216",
                "192.168.10.217",
                "192.168.10.218"
            };
        #endif

        public void OnGetAsync()
        {
            devlist.AddRange(device.ParseAsync(IPAdress));
        }

    }
}
