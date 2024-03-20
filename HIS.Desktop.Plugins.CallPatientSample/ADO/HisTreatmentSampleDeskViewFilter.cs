using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.CallPatientSample.ADO
{
    public class HisTreatmentSampleDeskViewFilter : FilterBase
    {
        public long? CREATE_DATE_FROM { get; set; }
        public long? CREATE_DATE_TO { get; set; }
        public bool? IS_BHYT_OR_PAID { get; set; }
        public bool? HASNT_SAMPLE_DESK { get; set; }
        public string ORDER_DIRECTION1 { get; set; }
        public string ORDER_DIRECTION2 { get; set; }
        public string ORDER_DIRECTION3 { get; set; }
        public string ORDER_DIRECTION4 { get; set; }
        public string ORDER_FIELD1 { get; set; }
        public string ORDER_FIELD2 { get; set; }
        public string ORDER_FIELD3 { get; set; }
        public string ORDER_FIELD4 { get; set; }
        public string PATIENT_CODE { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public long? SAMPLE_ROOM_ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public long? TREATMENT_ID { get; set; }
        public List<long> TREATMENT_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
    }
}
