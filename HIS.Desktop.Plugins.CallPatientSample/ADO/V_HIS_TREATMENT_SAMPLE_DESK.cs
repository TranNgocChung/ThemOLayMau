using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.CallPatientSample.ADO
{
    public class V_HIS_TREATMENT_SAMPLE_DESK
    {
        public string APP_CREATOR { get; set; }
        public string APP_MODIFIER { get; set; }
        public long? CALL_TIME { get; set; }
        public long? CREATE_TIME { get; set; }
        public string CREATOR { get; set; }
        public string GROUP_CODE { get; set; }
        public long ID { get; set; }
        public short? IS_ACTIVE { get; set; }
        public decimal? IS_BHYT_OR_PAID { get; set; }
        public short? IS_DELETE { get; set; }
        public string MODIFIER { get; set; }
        public long? MODIFY_TIME { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public long? SAMPLE_DESK_ID { get; set; }
        public string SAMPLE_DESK_NAME { get; set; }
        public long SAMPLE_ROOM_ID { get; set; }
        public string SAMPLE_ROOM_NAME { get; set; }
        public short? TDL_IS_PRIORITY { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public long TREATMENT_ID { get; set; }
        public string TREATMENT_TYPE_NAME { get; set; }
        public decimal? VIR_CREATE_DATE { get; set; }
    }
}