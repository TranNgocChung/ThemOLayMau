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
    public class TreatmentSampleListViewADO : V_HIS_TREATMENT_SAMPLE_DESK
    {
        public TreatmentSampleListViewADO()
        {

        }

        public TreatmentSampleListViewADO(V_HIS_TREATMENT_SAMPLE_DESK data)
        {
            Inventec.Common.Mapper.DataObjectMapper.Map<TreatmentSampleListViewADO>(this, data);
            if (data.IS_BHYT_OR_PAID.HasValue)
            {
                this.IsBhytOrPaid = data.IS_BHYT_OR_PAID.Value == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE ? true : false;
            }
          
        }
        public bool IsChecked { get; set; }
        public bool IsBhytOrPaid { get; set; }
    }
}
