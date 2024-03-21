using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.LocalStorage.BackendData.V2.ADO
{
    public class HIS_TREATMENT_SAMPLE_DESK
    {

        public string APP_CREATOR { get; set; }
        public string APP_MODIFIER { get; set; }
        public long? CALL_TIME { get; set; }
        public long? CREATE_TIME { get; set; }
        public string CREATOR { get; set; }
        public string GROUP_CODE { get; set; }
        public virtual HIS_PATIENT_TYPE HIS_PATIENT_TYPE { get; set; }
        public virtual HIS.Desktop.LocalStorage.BackendData.V2.ADO.HIS_SAMPLE_DESK HIS_SAMPLE_DESK { get; set; }
        public virtual HIS_SAMPLE_ROOM HIS_SAMPLE_ROOM { get; set; }
        public virtual HIS_TREATMENT HIS_TREATMENT { get; set; }
        public virtual HIS_TREATMENT_TYPE HIS_TREATMENT_TYPE { get; set; }
        public long ID { get; set; }
        public short? IS_ACTIVE { get; set; }
        public short? IS_DELETE { get; set; }
        public string MODIFIER { get; set; }
        public long? MODIFY_TIME { get; set; }
        public long? SAMPLE_DESK_ID { get; set; }
        public long SAMPLE_ROOM_ID { get; set; }
        public short? TDL_IS_PRIORITY { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long TREATMENT_ID { get; set; }
        public decimal? VIR_CREATE_DATE { get; set; }
    }
}
