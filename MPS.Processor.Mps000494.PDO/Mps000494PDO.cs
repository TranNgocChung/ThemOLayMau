using MOS.EFMODEL.DataModels;
using MPS.ProcessorBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Processor.Mps000494.PDO
{
    public partial class Mps000494PDO : RDOBase
    {
        public SingleKeyValue SingleKeyValue { get; set; }


        public Mps000494PDO(
           HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK _current
           )
        {
            try
            {
                this._currentTreatment = _current;
              
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
