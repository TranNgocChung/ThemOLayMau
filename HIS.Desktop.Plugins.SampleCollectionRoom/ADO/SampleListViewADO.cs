using LIS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Desktop.LocalStorage.BackendData.V2;
using HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL;

namespace HIS.Desktop.Plugins.SampleCollectionRoom.ADO
{
    public class TreatmentSampleListViewADO : HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK
    {
        public TreatmentSampleListViewADO()
        {

        }

        public TreatmentSampleListViewADO(HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK data)
        {
            Inventec.Common.Mapper.DataObjectMapper.Map<TreatmentSampleListViewADO>(this, data);
        }
        public bool IsChecked { get; set; }
        public bool IsBhyt { get; set; }
        public bool IsPaid { get; set; }
    }
}
