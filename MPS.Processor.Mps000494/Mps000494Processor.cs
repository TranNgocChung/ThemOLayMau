using SAR.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.ProcessorBase.Core;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MPS.Processor.Mps000494.PDO;
using FlexCel.Report;
using MPS.ProcessorBase;
using MPS.Processor.Mps000494.ADO;

namespace MPS.Processor.Mps000494
{
    public partial class Mps000494Processor : AbstractProcessor
    {
        private TReatmentADO patientADO { get; set; }
        private PatyAlterBhytADO patyAlter { get; set; }

        Mps000494PDO rdo;
        public Mps000494Processor(CommonParam param, PrintData printData)
            : base(param, printData)
        {
            rdo = (Mps000494PDO)rdoBase;
        }

        public override bool ProcessData()
        {
            bool result = false;
            try
            {
                Inventec.Common.FlexCellExport.ProcessSingleTag singleTag = new Inventec.Common.FlexCellExport.ProcessSingleTag();
                Inventec.Common.FlexCellExport.ProcessBarCodeTag barCodeTag = new Inventec.Common.FlexCellExport.ProcessBarCodeTag();
                Inventec.Common.FlexCellExport.ProcessObjectTag objectTag = new Inventec.Common.FlexCellExport.ProcessObjectTag();

                store.ReadTemplate(System.IO.Path.GetFullPath(fileName));
                ProcessSingleKey();

                singleTag.ProcessData(store, singleValueDictionary);
                barCodeTag.ProcessData(store, dicImage);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

            return result;
        }

        void ProcessSingleKey()
        {
            try
            {

                AddObjectKeyIntoListkey<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>(rdo._currentTreatment, false);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
