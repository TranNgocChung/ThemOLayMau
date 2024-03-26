using HIS.Desktop.Common;
using Inventec.Common.Adapter;
using Inventec.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.LocalStorage.BackendData.V2.CallPatient
{
    public class CallPtDataWorker
    {
        private static Dictionary<long, List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>> dicCallPatient;

        public static Dictionary<long, List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>> DicCallPatient
        {
            get
            {
                if (dicCallPatient == null)
                {
                    dicCallPatient = new Dictionary<long, List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>>();
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

        public static void UpdateCallTime(List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK> listCall, long roomId, Inventec.Common.WebApiClient.ApiConsumer mosUserConsummer)
        {
            List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK> listUpdateCallTime = new List<LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>();
            foreach (var item in listCall)
            {
                if (item.CALL_TIME != null)
                {
                    listUpdateCallTime.Add(item);
                }
            }
            if (listUpdateCallTime.Count > 0)
            {
                CommonParam param = new CommonParam();
                var result = new BackendAdapter(param).Post<List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>>("api/HisTreatmentSampleDesk/UpdateCallTime", mosUserConsummer, listUpdateCallTime, param);
                if (listUpdateCallTime != null && listUpdateCallTime.Count > 0)
                {
                    if (dicCallPatient != null && dicCallPatient.ContainsKey(roomId) && dicCallPatient[roomId] != null)
                    {
                        dicCallPatient[roomId].AddRange(listUpdateCallTime);
                    }
                    else
                    {
                        if (dicCallPatient == null) dicCallPatient = new Dictionary<long, List<EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>>();
                        dicCallPatient[roomId] = listUpdateCallTime;
                    }
                    dicCallPatient[roomId] = dicCallPatient[roomId].OrderByDescending(o => o.CALL_TIME ?? 0).GroupBy(p => p.ID).Select(q => q.First()).ToList();
                }
            }
        }

        public static bool UpdateSampleDesk(List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK> listSample, long roomId, Inventec.Common.WebApiClient.ApiConsumer mosUserConsummer, CommonParam param)
        {
            bool result = false;
            List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK> listUpdate = new List<LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>();
            foreach (var item in listSample)
            {
                if (item.SAMPLE_DESK_ID != null)
                {
                    listUpdate.Add(item);
                }
            }
            if (listUpdate.Count > 0)
            {
                var resultData = new BackendAdapter(param).Post<List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>>("api/HisTreatmentSampleDesk/UpdateSampleDesk", mosUserConsummer, listUpdate, param);
                if (resultData != null && resultData.Count > 0)
                {
                    result = true;
                    if (dicCallPatient != null && dicCallPatient.ContainsKey(roomId) && dicCallPatient[roomId] != null)
                    {
                        dicCallPatient[roomId].AddRange(resultData);
                    }
                    else
                    {
                        if (dicCallPatient == null) dicCallPatient = new Dictionary<long, List<EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>>();
                        dicCallPatient[roomId] = resultData;
                    }
                    dicCallPatient[roomId] = dicCallPatient[roomId].OrderByDescending(o => o.CALL_TIME ?? 0).OrderByDescending(q=>q.SAMPLE_DESK_ID.HasValue).GroupBy(p => p.ID).Select(q => q.First()).ToList();
                }
            }
            return result;
        }
    }
}
