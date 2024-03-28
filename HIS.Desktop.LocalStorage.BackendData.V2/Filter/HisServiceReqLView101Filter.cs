using System.Collections.Generic;

namespace HIS.Desktop.LocalStorage.BackendData.V2.Filter
{
    public class HisServiceReqLView101Filter
    {
        protected static readonly long NEGATIVE_ID = -1;

        public List<long> IDs { get; set; }
        public List<long> TREATMENT_IDs { get; set; }
        public List<long> SAMPLE_ROOM_IDs { get; set; }

        public string SERVICE_REQ_CODE__EXACT { get; set; }
        public string ASSIGN_TURN_CODE__EXACT { get; set; }

        public long? ID { get; set; }
        public long? TREATMENT_ID { get; set; }
        public long? SAMPLE_ROOM_ID { get; set; }

        public string ORDER_FIELD { get; set; }
        public string ORDER_DIRECTION { get; set; }

        public string ORDER_FIELD1 { get; set; }
        public string ORDER_DIRECTION1 { get; set; }
        public string ORDER_FIELD2 { get; set; }
        public string ORDER_DIRECTION2 { get; set; }
        public string ORDER_FIELD3 { get; set; }
        public string ORDER_DIRECTION3 { get; set; }
        public string ORDER_FIELD4 { get; set; }
        public string ORDER_DIRECTION4 { get; set; }

        public HisServiceReqLView101Filter()
            : base()
        {
        }
    }
}
