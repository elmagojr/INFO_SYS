using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDigital.Utilidades
{
    public class QUERIES
    {
     
        public readonly string secuencial = "SELECT LEFT((SELECT MAX (OFI_UNI_SEQ) AS SEQ FROM DBA.F_Oficinas where OFI_CODIGO =1), LENGTH((SELECT MAX (OFI_UNI_SEQ) AS SEQ FROM DBA.F_Oficinas where OFI_CODIGO =1)) - 4) AS Resultado";
        public readonly string encriptar = "SELECT LEFT((SELECT MAX (OFI_UNI_SEQ) AS SEQ FROM DBA.F_Oficinas where OFI_CODIGO =1), LENGTH((SELECT MAX (OFI_UNI_SEQ) AS SEQ FROM DBA.F_Oficinas where OFI_CODIGO =1)) - 4) AS Resultado";





    }
}
