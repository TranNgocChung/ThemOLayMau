using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MPS.Processor.Mps000494.PDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Processor.Mps000494
{
    public class DataRawProcess
    {
        public static TReatmentADO TreatmentRawToADO(HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK treatment)
        {
            TReatmentADO treatmentADO = new TReatmentADO();
            try
            {
                if (treatment != null)
                {
                    AutoMapper.Mapper.CreateMap<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK, TReatmentADO>();
                    treatmentADO = AutoMapper.Mapper.Map<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK, TReatmentADO>(treatment);
                    treatmentADO.VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                    treatmentADO.VIR_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                    treatmentADO.DOB = treatment.TDL_PATIENT_DOB;
                    treatmentADO.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                    treatmentADO.AGE = AgeUtil.CalculateFullAge(treatmentADO.DOB);
                    if (treatment.TDL_PATIENT_DOB > 0)
                    {
                        treatmentADO.DOB_YEAR = treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                treatmentADO = null;
            }
            return treatmentADO;
        }

        public static PatyAlterBhytADO PatyAlterBHYTRawToADO(V_HIS_PATIENT_TYPE_ALTER patyAlter)
        {
            PatyAlterBhytADO patyAlterBhytADO = null;
            try
            {
                if (patyAlter != null)
                {
                    AutoMapper.Mapper.CreateMap<V_HIS_PATIENT_TYPE_ALTER, PatyAlterBhytADO>();
                    patyAlterBhytADO = AutoMapper.Mapper.Map<MOS.EFMODEL.DataModels.V_HIS_PATIENT_TYPE_ALTER, PatyAlterBhytADO>(patyAlter);

                    patyAlterBhytADO.HEIN_CARD_NUMBER_SEPARATE = SetHeinCardNumberDisplayByNumber(patyAlter.HEIN_CARD_NUMBER);
                    patyAlterBhytADO.HEIN_MEDI_ORG_CODE = patyAlter.HEIN_MEDI_ORG_CODE;
                    patyAlterBhytADO.HEIN_MEDI_ORG_NAME = patyAlter.HEIN_MEDI_ORG_NAME;
                    patyAlterBhytADO.IS_HEIN = "X";
                    patyAlterBhytADO.IS_VIENPHI = "";
                    if (!String.IsNullOrEmpty(patyAlter.HEIN_CARD_NUMBER))
                    {
                        patyAlterBhytADO.HEIN_CARD_NUMBER_1 = patyAlter.HEIN_CARD_NUMBER.Substring(0, 2);
                        patyAlterBhytADO.HEIN_CARD_NUMBER_2 = patyAlter.HEIN_CARD_NUMBER.Substring(2, 1);
                        patyAlterBhytADO.HEIN_CARD_NUMBER_3 = patyAlter.HEIN_CARD_NUMBER.Substring(3, 2);
                        patyAlterBhytADO.HEIN_CARD_NUMBER_4 = patyAlter.HEIN_CARD_NUMBER.Substring(5, 2);
                        patyAlterBhytADO.HEIN_CARD_NUMBER_5 = patyAlter.HEIN_CARD_NUMBER.Substring(7, 3);
                        patyAlterBhytADO.HEIN_CARD_NUMBER_6 = patyAlter.HEIN_CARD_NUMBER.Substring(10, 5);
                    }
                   
                    if (patyAlter.HEIN_CARD_FROM_TIME.HasValue)
                    {
                        patyAlterBhytADO.STR_HEIN_CARD_FROM_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString((patyAlter.HEIN_CARD_FROM_TIME.Value));
                    }
                    if (patyAlter.HEIN_CARD_TO_TIME.HasValue)
                    {
                        patyAlterBhytADO.STR_HEIN_CARD_TO_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString((patyAlter.HEIN_CARD_TO_TIME.Value));
                    }

                }
                else
                {
                    patyAlterBhytADO.HEIN_CARD_NUMBER_SEPARATE = "";
                    patyAlterBhytADO.HEIN_MEDI_ORG_CODE = "";
                    patyAlterBhytADO.HEIN_MEDI_ORG_NAME = "";
                    patyAlterBhytADO.IS_HEIN = "";
                    patyAlterBhytADO.IS_VIENPHI = "X";
                    patyAlterBhytADO.HEIN_CARD_NUMBER_1 = "";
                    patyAlterBhytADO.HEIN_CARD_NUMBER_2 = "";
                    patyAlterBhytADO.HEIN_CARD_NUMBER_3 = "";
                    patyAlterBhytADO.HEIN_CARD_NUMBER_4 = "";
                    patyAlterBhytADO.HEIN_CARD_NUMBER_5 = "";
                    patyAlterBhytADO.HEIN_CARD_NUMBER_6 = "";
                    patyAlterBhytADO.STR_HEIN_CARD_FROM_TIME = "";
                    patyAlterBhytADO.STR_HEIN_CARD_TO_TIME = "";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                patyAlterBhytADO = null;
            }
            return patyAlterBhytADO;
        }

        public static string SetHeinCardNumberDisplayByNumber(string heinCardNumber)
        {
            string result = "";
            try
            {
                if (!String.IsNullOrWhiteSpace(heinCardNumber) && heinCardNumber.Length == 15)
                {
                    string separateSymbol = "-";
                    result = new StringBuilder().Append(heinCardNumber.Substring(0, 2)).Append(separateSymbol).Append(heinCardNumber.Substring(2, 1)).Append(separateSymbol).Append(heinCardNumber.Substring(3, 2)).Append(separateSymbol).Append(heinCardNumber.Substring(5, 2)).Append(separateSymbol).Append(heinCardNumber.Substring(7, 3)).Append(separateSymbol).Append(heinCardNumber.Substring(10, 5)).ToString();
                }
                else
                {
                    result = heinCardNumber;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = heinCardNumber;
            }
            return result;
        }
    }
}
