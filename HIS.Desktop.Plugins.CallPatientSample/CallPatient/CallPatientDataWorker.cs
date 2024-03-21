using HIS.Desktop.Common;
using HIS.Desktop.LocalStorage.BackendData.V2.ADO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.CallPatientSample.CallPatient
{
    public class CallPtDataWorker
    {
        private static Dictionary<long, List<SrADO>> dicCallPatient;

        public static Dictionary<long, List<SrADO>> DicCallPatient
        {
            get
            {
                if (dicCallPatient == null)
                {
                    dicCallPatient = new Dictionary<long, List<SrADO>>();
                }
                lock (dicCallPatient) ;
                return dicCallPatient;
            }
            set
            {
                lock (dicCallPatient) ;
                dicCallPatient = value;
            }
        }

        private static Dictionary<long, DelegateSelectData> dicDelegateCallingPatient;

        public static Dictionary<long, DelegateSelectData> DicDelegateCallingPatient
        {
            get
            {
                if (dicDelegateCallingPatient == null)
                {
                    dicDelegateCallingPatient = new Dictionary<long, DelegateSelectData>();
                }
                lock (dicDelegateCallingPatient) ;
                return dicDelegateCallingPatient;
            }
            set
            {
                lock (dicDelegateCallingPatient) ;
                dicDelegateCallingPatient = value;
            }
        }
    }
}
