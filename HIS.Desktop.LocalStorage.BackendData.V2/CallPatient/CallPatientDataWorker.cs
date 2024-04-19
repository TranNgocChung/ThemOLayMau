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
        private static Dictionary<long, List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>> dicDeskPatient;

        public static Dictionary<long, List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>> DicDeskPatient
        {
            get
            {
                if (dicDeskPatient == null)
                {
                    dicDeskPatient = new Dictionary<long, List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>>();
                }
                lock (dicDeskPatient);
                return dicDeskPatient;
            }
            set
            {
                lock (dicDeskPatient) ;
                dicDeskPatient = value;
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
                    dicCallPatient[roomId].ForEach(o => o.IS_CALLING = listUpdateCallTime.Exists(p=>p.ID == o.ID));
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
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData("listUpdate", listUpdate));
                var resultData = new BackendAdapter(param).Post<List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>>("api/HisTreatmentSampleDesk/UpdateSampleDesk", mosUserConsummer, listUpdate, param);
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData("resultData", resultData));
                if (resultData != null && resultData.Count > 0)
                {
                    result = true;
                    if (dicCallPatient != null && dicCallPatient.ContainsKey(roomId) && dicCallPatient[roomId] != null)
                    {
                        dicCallPatient[roomId] = dicCallPatient[roomId].Where(o => !resultData.Exists(p => p.ID == o.ID)).ToList();
                    }

                }
                if (DicDeskPatient == null)
                {
                    DicDeskPatient = new Dictionary<long, List<EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>>();
                }
                if (!DicDeskPatient.ContainsKey(roomId))
                {
                    DicDeskPatient.Add(roomId, new List<EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>());
                }
                if (DicDeskPatient[roomId] == null)
                {
                    DicDeskPatient[roomId] = new List<EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>();
                }
                DicDeskPatient[roomId].AddRange(resultData);
                DicDeskPatient[roomId] = DicDeskPatient[roomId].Where(o=>o.CALL_TIME!=null && o.CALL_TIME > Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Today)).OrderByDescending(o => o.CALL_TIME ?? 0).GroupBy(p => p.ID).Select(q => q.First()).ToList();
            }
            return result;
        }
    }
}
