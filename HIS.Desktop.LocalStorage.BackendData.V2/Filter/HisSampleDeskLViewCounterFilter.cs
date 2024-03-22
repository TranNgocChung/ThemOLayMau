
using MOS.Filter;
using System.Collections.Generic;
namespace HIS.Desktop.LocalStorage.BackendData.V2.Filter
{
    public class HisSampleDeskLViewCounterFilter : FilterBase
    {

        public HisSampleDeskLViewCounterFilter()
            : base()
        {
        }
        public long? SAMPLE_ROOM_ID { get; set; }
    }
}
