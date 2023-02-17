using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KKT_STATUS_WEB.Models
{
    public class Device
    {
        /// <summary>
        /// DeviceId - инкрементный номер устройства
        /// </summary>
        public int DeviceId { get; set; } = 0;

        /// <summary>
        /// FiscalStoreNumber - номер фискального накопителя
        /// </summary>
        public string? FiscalStoreNumber { get; set; } = "-";

        /// <summary>
        /// SpentResource - потраченный ресурс фискального накопителя
        /// </summary>
        public int? SpentResource { get; set; } = 0;

        /// <summary>
        /// LastDocumentDate - дата последнего отфискализированного документа
        /// </summary>
        public DateTime LastDocumentDate { get; set; } = Convert.ToDateTime(Convert.ToString("01.01.2000"));

        /// <summary>
        /// LastDocumentNumber - номер последнего отфискализированного документа
        /// </summary>
        public int LastDocumentNumber { get; set; } = 0;

        /// <summary>
        /// ResidualResource - остаток ресурсов фискального накопителя
        /// </summary>
        public int ResidualResource { get; set; } = 0;

        /// <summary>
        /// ResidualProcent - процент остатка фискального ресурса
        /// </summary>
        public double ResidualProcent { get; set; } = 0;

        /// <summary>
        /// SoftwareVersion - версия программного обеспечения
        /// </summary>
        public string SoftwareVersion { get; set; } = "-";

        /// <summary>
        /// LastDocumentDateTimeSpan - диапозон разницы между текущим временем и датой последнего документа
        /// </summary>
        public double LastDocumentDateTimeSpan { get; set; } = 0;

        /// <summary>
        /// FiscalStoreStatus - значение присутствия фискального накопителя в устройстве
        /// </summary>
        public bool FiscalStoreStatus { get; set; } = false;

        /// <summary>
        /// MessagesPendingToSend - количество сообщений для передачи в ОФД
        /// </summary>
        public int MessagesPendingToSend { get; set; } = 0;

        /// <summary>
        /// LifePhase - фаза жизни фискального накопителя
        /// </summary>
        public int LifePhase { get; set; } = 100;


        public List<Device> ParseAsync(List<string> IPAdress)
        {
            //Создание экземпляра класса библиотеки
            HtmlWeb web = new();

            List<Device> devlist = new();
            int index = 1;

                foreach (string ip in IPAdress)
                {
                    try
                    {
                        web.OverrideEncoding = Encoding.UTF8;
                        var htmlDoc = web.Load($"http://{ip}/index.shtm");
                        web.PreRequest = delegate (HttpWebRequest webRequest)
                        {
                            webRequest.Timeout = 5000;
                            return true;
                        };
                        devlist.Add(new Device()
                        {
                            DeviceId = index++,
                            SpentResource = GetSpentResource(htmlDoc),
                            LastDocumentDate = GetLastDocumentDate(htmlDoc),
                            LastDocumentNumber = GetLastDocumentNumber(htmlDoc),
                            ResidualResource = GetResidualResource(htmlDoc),
                            ResidualProcent = GetResidualProcent(htmlDoc),
                            SoftwareVersion = GetSoftwareVersion(htmlDoc),
                            FiscalStoreNumber = CheckCorrectFiscalNumber(GetSoftwareVersion(htmlDoc), htmlDoc),
                            LastDocumentDateTimeSpan = LastDocumentDateTimeSpanCalculate(GetLastDocumentDate(htmlDoc)),
                            FiscalStoreStatus = FiscalStoreExist(GetSoftwareVersion(htmlDoc), GetLastDocumentNumber(htmlDoc)),
                            MessagesPendingToSend = GetCountMessagesPendingToSend(htmlDoc),
                            LifePhase = GetLifePhaseFN(htmlDoc)
                        });
                    }
                    catch (Exception ex)
                    {
                        devlist.Add(new Device()
                        {
                            DeviceId = index

                        });
                        Debug.WriteLine(ex);
                        index++;
                    }
                
            }
            return devlist;
        }
        private int GetLifePhaseFN(HtmlDocument htmlDoc)
        {
            int LifePhaseFN = 99;

            try
            {
                if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Фаза ФН']/td[1]/following::td[1]") != null)
                {
                    LifePhaseFN = int.Parse(htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Фаза ФН']/td[1]/following::td[1]").InnerText);
                }
                else if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Фаза ФН']/td[1]/following::td[1]") == null)
                {
                    LifePhaseFN = 99;
                }
            }
            catch
            {
                LifePhaseFN = 99;
            }
            return LifePhaseFN;
        }
        private int GetResidualResource(HtmlDocument htmlDoc)
        {
            int ResidualResource = 0;

            try
            {
                if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер последнего ФД']/td[1]/following::td[1]") != null)
                {
                    ResidualResource = 249000 - int.Parse(htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер последнего ФД']/td[1]/following::td[1]").InnerText);
                }
                else if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер последнего ФД']/td[1]/following::td[1]") == null)
                {
                    ResidualResource = 249000 - int.Parse(htmlDoc.DocumentNode.SelectSingleNode("//tr/td//tr[9]/td[2]").InnerText);
                }
            }
            catch
            {
                ResidualResource = 0;
            }
            return ResidualResource;
        }
        private double GetResidualProcent(HtmlDocument htmlDoc)
        {
            double ResidualProcent = 0;
            try
            {
                if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер последнего ФД']/td[1]/following::td[1]") != null)
                {
                    ResidualProcent = 100 - (249000 - int.Parse(htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер последнего ФД']/td[1]/following::td[1]").InnerText)) / (249000 / 100);
                }
                else if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер последнего ФД']/td[1]/following::td[1]") == null)
                {
                    ResidualProcent = 100 - (249000 - int.Parse(htmlDoc.DocumentNode.SelectSingleNode("//tr/td//tr[9]/td[2]").InnerText)) / (249000 / 100);
                }

            }
            catch
            {
                ResidualProcent = 0;
            }

            return ResidualProcent;
        }
        private int GetCountMessagesPendingToSend(HtmlDocument htmlDoc)
        {
            int CountMessagesPendingToSend = 0;

            try
            {

                if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Количество сообщений']/td[1]/following::td[1]") != null)
                {
                    CountMessagesPendingToSend = int.Parse(htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Количество сообщений']/td[1]/following::td[1]").InnerText);
                }
                else if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Колличество сообщений']/td[1]/following::td[1]") != null)
                {
                    CountMessagesPendingToSend = int.Parse(htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Колличество сообщений']/td[1]/following::td[1]").InnerText);
                }
                else
                {
                    CountMessagesPendingToSend = 0;
                }
            }
            catch
            {
                CountMessagesPendingToSend = 0;
            }
            return CountMessagesPendingToSend;
        }

        private double LastDocumentDateTimeSpanCalculate(DateTime LastDocumentDate)
        {

            TimeSpan LastDocumentDateTimeSpan = DateTime.Now.Subtract(Convert.ToDateTime(LastDocumentDate));
            return LastDocumentDateTimeSpan.TotalMinutes;
        }
        private DateTime GetLastDocumentDate(HtmlDocument htmlDoc)
        {
            DateTime LastDocumentDate = Convert.ToDateTime(Convert.ToString("01.01.2000"));

            try
            {
                if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер последнего ФД']/td[1]/following::td[1]") != null)
                {
                    LastDocumentDate = Convert.ToDateTime(Convert.ToString(htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Дата и время']/td[1]/following::td[1]").InnerText));
                }
                else if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер последнего ФД']/td[1]/following::td[1]") == null)
                {
                    LastDocumentDate = Convert.ToDateTime(Convert.ToString(htmlDoc.DocumentNode.SelectSingleNode("//tr[1]/td[2]//tr[7]/td[2]").InnerText));
                }
            }
            catch
            {
                LastDocumentDate = Convert.ToDateTime(Convert.ToString("01.01.2000"));
            }

            return LastDocumentDate;
        }
        private int GetSpentResource(HtmlDocument htmlDoc)
        {
            int SpentResource = 0;
                try
                {
                    if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер последнего ФД']/td[1]/following::td[1]") != null)
                    {
                        SpentResource = int.Parse(htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер последнего ФД']/td[1]/following::td[1]").InnerText);
                    }
                    else if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер последнего ФД']/td[1]/following::td[1]") == null)
                    {
                        SpentResource = int.Parse(htmlDoc.DocumentNode.SelectSingleNode("//tr/td//tr[9]/td[2]").InnerText);
                    }

                }
                catch
                {
                    SpentResource = 0;
                }


            return SpentResource;
        }
        private string CheckCorrectFiscalNumber(string SoftwareVersion, HtmlDocument htmlDoc)
        {
            string FiscalStoreNumber = "FiscalStoreNumber";
            try
            {
                if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер ФН']/td[1]/following::td[1]") != null)
                {
                    FiscalStoreNumber = Convert.ToString(htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер ФН']/td[1]/following::td[1]").InnerText);
                }
                else if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Версия прошивки']/td[1]/following::td[1]") == null)
                {
                    FiscalStoreNumber = Convert.ToString(htmlDoc.DocumentNode.SelectSingleNode("//html/body/div/div[2]/table//table[2]//tr[8]/td[2]").InnerText);
                }
            }
            catch
            {
                FiscalStoreNumber = "Error";
            }
            return FiscalStoreNumber;
        }

        public bool FiscalStoreExist(string SoftwareVersion, int LastDocumentNumber)
        {
            bool ExistFlag = true;
            if (SoftwareVersion != "error" && LastDocumentNumber > 0)
            {
                ExistFlag = true;
            }
            else
            {
                ExistFlag = false;
            }
            return ExistFlag;
        }

        public string GetSoftwareVersion(HtmlDocument htmlDoc)
        {
            string SoftwareVersion = "SoftwareVersion";

            try
            {

                if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Версия сборки']/td[1]/following::td[1]") != null)
                {
                    SoftwareVersion = Convert.ToString(htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Версия сборки']/td[1]/following::td[1]").InnerText);
                }
                else if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Версия прошивки']/td[1]/following::td[1]") != null)
                {
                    SoftwareVersion = Convert.ToString(htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Версия прошивки']/td[1]/following::td[1]").InnerText);
                }
                else
                {
                    SoftwareVersion = Convert.ToString(htmlDoc.DocumentNode.SelectSingleNode("//td[1]/table[1]//tr[4]/td[2]").InnerText);
                }
            }
            catch
            {
                SoftwareVersion = "Error";
            }
            return SoftwareVersion;
        }

        public int GetLastDocumentNumber(HtmlDocument htmlDoc)
        {
            int lastDocumentNumber;
            try
            {
                if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер последнего ФД']/td[1]/following::td[1]") != null)
                {
                    lastDocumentNumber = int.Parse(htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер последнего ФД']/td[1]/following::td[1]").InnerText);
                }
                else if (htmlDoc.DocumentNode.SelectSingleNode("//table//*[td='Номер последнего ФД']/td[1]/following::td[1]") == null)
                {
                    lastDocumentNumber = int.Parse(htmlDoc.DocumentNode.SelectSingleNode("//tr/td//tr[9]/td[2]").InnerText);
                }
                else
                {
                    lastDocumentNumber = int.Parse(htmlDoc.DocumentNode.SelectSingleNode("/html/body/div/div[2]/table/tbody/tr/td[2]/table[2]/tbody/tr[9]/td[2]").InnerText);
                }
            }
            catch
            {
                lastDocumentNumber = 0;
            }

            return lastDocumentNumber;
        }
    }

}
