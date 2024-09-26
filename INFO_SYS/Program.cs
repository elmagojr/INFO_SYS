using DDigital.Utilidades;
using ProyectoDIGITALPERSONA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace INFO_SYS
{
    internal class Program
    {
       
        static void Main(string[] args)
        {
            ExportarData(trae_info_sistema());


        }
        public class INFO_SISTEMA
        {
            public string NOMBRE_MAQUINA { get; set; }
            public string IP_MAQUINA { get; set; }
            public string SEQUENCIAL_SISTEMA { get; set; }
            public string USUARIO_MAQUINA { get; set; }
            public string VERION_OS_MAQUINA { get; set; }
            public string DOMINIO_MAQUINA { get; set; }
            public string MACHINE_NAME { get; set; }


        }
        static string trae_secuencial(string incrip)
        {
            ODBC_CONN cn = new ODBC_CONN();
           QUERIES sql = new QUERIES();

            try
            {
                Dictionary<string, object> DelPar = new Dictionary<string, object>()
            {
                {"@encriptar",incrip}
            };
                return cn.primeraCol(sql.secuencial, DelPar);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        static INFO_SISTEMA trae_info_sistema()
        {
            // string clientName = Environment.GetEnvironmentVariable("CLIENTNAME"); remoto          
            INFO_SISTEMA iNFO = new INFO_SISTEMA();           
           // iNFO.SEQUENCIAL_SISTEMA = trae_secuencial();
            iNFO.NOMBRE_MAQUINA = Dns.GetHostName();
            IPAddress[] ipAddresses = Dns.GetHostAddresses(iNFO.NOMBRE_MAQUINA);
            string ipAddress = string.Empty;
            foreach (IPAddress ip in ipAddresses)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    iNFO.IP_MAQUINA = ip.ToString();
                    break;
                }
            }

            iNFO.USUARIO_MAQUINA = Environment.UserName;
            iNFO.MACHINE_NAME = Environment.MachineName;
            iNFO.DOMINIO_MAQUINA = Environment.UserDomainName;



            string r = "";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
            {
                ManagementObjectCollection information = searcher.Get();
                if (information != null)
                {
                    foreach (ManagementObject obj in information)
                    {
                        iNFO.VERION_OS_MAQUINA = obj["Caption"].ToString() + " - " + obj["OSArchitecture"].ToString();
                    }
                }
                r = r.Replace("NT 5.1.2600", "XP");
                r = r.Replace("NT 5.2.3790", "Server 2003");

            }
            return iNFO;

        }
        static void ExportarData(INFO_SISTEMA info)
        {
            string path = @"C:\SISC\Addons\info.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(path, false))
                {                   
                    sw.WriteLine("ip:" + info.IP_MAQUINA+",");
                    sw.WriteLine("userpc:" + info.USUARIO_MAQUINA + ",");
                    sw.WriteLine("namepc:" + info.MACHINE_NAME + ",");
                    sw.WriteLine("SOpc:" + info.VERION_OS_MAQUINA);
                }   
                    File.SetAttributes(path, FileAttributes.ReadOnly | FileAttributes.Hidden);            
            }
            catch (Exception ex)
            {
            }
        }
    }
}
