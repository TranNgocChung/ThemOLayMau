using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.CallPatientSample.ADO
{
    public class SrADO : HIS.Desktop.Plugins.CallPatientSample.ADO.V_HIS_TREATMENT_SAMPLE_DESK
    {
        public bool CallPatientSTT { get; set; }

        public bool IsCalling { get; set; }

        public long? VaccinationId { get; set; }

        public SrADO()
        {

        }
    }
}
