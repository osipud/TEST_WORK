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
    public class DeviceModel : PageModel
    {
            //�������� ���������� ������ ������ ������
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
                "192.168.10.203"
            };
            #endif

            public void OnGet()
            {
                devlist.AddRange(device.ParseAsync(IPAdress));
            }




            //private static async Task SendEmailAsync(int index)
            //{
            //    //MailAddress from = new MailAddress("����� ��� ��������", "KKT INFO");
            //    //MailAddress to1 = new MailAddress("������� 1");
            //    //MailAddress to2 = new MailAddress("������� 2");
            //    //MailMessage m1 = new MailMessage(from, to1);
            //    //MailMessage m2 = new MailMessage(from, to2);
            //    //m1.Subject = "���������� ���";
            //    //m1.Body = $"<b>��������!</b><br>��������� ��������!<br>��� �{index} �� �������� �� ������� �������!";
            //    //m1.IsBodyHtml = true;
            //    //m2.Subject = "���������� ���";
            //    //m2.Body = $"<b>��������!</b><br>��������� ��������!<br>��� �{index} �� �������� �� ������� �������!";
            //    //m2.IsBodyHtml = true;
            //    //SmtpClient smtp = new("SMTP ������ �����", ���� SMTP �������);
            //    //smtp.Credentials = new NetworkCredential("����� ��� ��������", "������ ��� ��������");
            //    //smtp.EnableSsl = true;
            //    //await smtp.SendMailAsync(m1);
            //    //await smtp.SendMailAsync(m2);
            //    //Console.WriteLine($"{DateTime.Now}: ������ ����������");
            //}
        }
    }










