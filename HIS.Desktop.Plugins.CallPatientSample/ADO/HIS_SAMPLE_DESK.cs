using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.CallPatientSample.ADO
{
    public class HIS_SAMPLE_DESK
    {
        public string APP_CREATOR { get; set; }
        public string APP_MODIFIER { get; set; }
        public long? CREATE_TIME { get; set; }
        public string CREATOR { get; set; }
        public string GROUP_CODE { get; set; }
        public virtual HIS_SAMPLE_ROOM HIS_SAMPLE_ROOM { get; set; }
        public virtual ICollection<HIS_TREATMENT_SAMPLE_DESK> HIS_TREATMENT_SAMPLE_DESK { get; set; }
        public long ID { get; set; }
        public short? IS_ACTIVE { get; set; }
        public short? IS_DELETE { get; set; }
        public string MODIFIER { get; set; }
        public long? MODIFY_TIME { get; set; }
        public string SAMPLE_DESK_CODE { get; set; }
        public string SAMPLE_DESK_NAME { get; set; }
        public long? SAMPLE_ROOM_ID { get; set; }
    }
}
