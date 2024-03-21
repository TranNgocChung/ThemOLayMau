using HIS.Desktop.LocalStorage.BackendData.V2.ADO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.CallPatientSample
{
    public class SrAdoWorker
    {
        private static SrADO srAdo;

        public static SrADO SrAdo
        {
            get
            {
                if (srAdo == null)
                {
                    srAdo = new SrADO();
                }
                lock (srAdo) ;
                return srAdo;
            }
            set
            {
                lock (srAdo) ;
                srAdo = value;
            }
        }
    }
}
