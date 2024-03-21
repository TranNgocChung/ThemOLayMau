using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Controls.Session;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.ConfigApplication;
using HIS.Desktop.LocalStorage.LocalData;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.LocalStorage.BackendData.V2.ADO;
using HIS.Desktop.Plugins.CallPatientSample.CallPatient;
using HIS.Desktop.Plugins.CallPatientSample.Config;
using HIS.Desktop.Utility;
using Inventec.Common.Adapter;
using Inventec.Common.Logging;
using Inventec.Core;
using LIS.EFMODEL.DataModels;
using LIS.Filter;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.Desktop.Plugins.CallPatientSample
{
    public partial class frmWaitingScreenSample22 : FormBase
    {
        internal HIS.Desktop.LocalStorage.BackendData.V2.ADO.V_HIS_TREATMENT_SAMPLE_DESK lisSample;
        const int STEP_NUMBER_ROW_GRID_SCROLL = 5;
        internal V_HIS_SAMPLE_ROOM room;
        private int scrll { get; set; }
        string organizationName = "";
        List<int> newStatusForceColorCodes = new List<int>();
        internal static string[] FilePath;
        List<int> gridpatientBodyForceColorCodes;
        int index = 0;
        int rowCount = 0;
        bool isSetNum = false; 
        bool? chkIsNotInDebt;

        private Inventec.Common.WebApiClient.ApiConsumer mosUserConsummer;

        public frmWaitingScreenSample22(Inventec.Desktop.Common.Modules.Module module, HIS.Desktop.LocalStorage.BackendData.V2.ADO.V_HIS_TREATMENT_SAMPLE_DESK sample, V_HIS_SAMPLE_ROOM r, bool? _chkIsNotInDebt)
            : base(module)
        {
            InitializeComponent();
            this.lisSample = sample;
            this.room = r;
            this.chkIsNotInDebt = _chkIsNotInDebt;
        }

        private void frmWaitingScreen_QY_Load(object sender, EventArgs e)
        {
            try
            {
                HisConfigCFG.LoadConfig();
                SetDataToRoom(this.room);
                FillDataToDictionaryWaitingPatient();
                UpdateDefaultListPatientSTT();
                SetDataToGridControlWaitingCLSs();
                GetFilePath();
                StartAllTimer();
                SetFromConfigToControl();
                var emp = BackendDataWorker.Get<HIS_EMPLOYEE>().FirstOrDefault(o => o.LOGINNAME == Inventec.UC.Login.Base.ClientTokenManagerStore.ClientTokenManager.GetLoginName());
                lblDoctorName.Text = string.Format("{0} {1}", emp != null ? (emp.TITLE != null ? emp.TITLE + ": " : "") : "", Inventec.UC.Login.Base.ClientTokenManagerStore.ClientTokenManager.GetUserName().ToUpper());
                rowCount = gridViewWaitingCls.RowCount - 1;
                SetFormFrontOfAll();
                timer1.Interval = 2000;
                timer1.Enabled = true;
                timer1.Start();
                //RegisterTimer(ModuleLink, "timer1", 2000, SetDataToLabelMoiBenhNhan);
                //StartTimer(ModuleLink, "timer1");
                SetIcon();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void SetFormFrontOfAll()
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;
                this.BringToFront();
                this.TopMost = true;
                this.Focus();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void UpdateDefaultListPatientSTT()
        {
            try
            {
                if (CallPtDataWorker.DicCallPatient != null && CallPtDataWorker.DicCallPatient.Count > 0 && CallPtDataWorker.DicCallPatient[room.ID] != null && CallPtDataWorker.DicCallPatient[room.ID].Count > 0)
                {
                    foreach (var item in CallPtDataWorker.DicCallPatient[room.ID])
                    {
                        item.CallPatientSTT = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void StartAllTimer()
        {
            try
            {
                //RegisterTimer(ModuleLink, "timerForScrollListPatient", 2000, timerForScrollListPatientProcess);
                //RegisterTimer(ModuleLink, "timerSetDataToGridControl", WaitingScreenCFG.TIMER_FOR_AUTO_LOAD_WAITING_SCREENS * 1000, SetDataToGridControlCLS);
                //RegisterTimer(ModuleLink, "timerAutoLoadDataPatient", WaitingScreenCFG.TIMER_FOR_SET_DATA_TO_GRID_PATIENTS * 1000, LoadWaitingPatientForWaitingScreen);
                //RegisterTimer(ModuleLink, "timerForHightLightCallPatientLayout", WaitingScreenCFG.TIMER_FOR_HIGHT_LIGHT_CALL_PATIENT * 1000, SetDataToCurentCallPatientUsingThread);

                timerForScrollListPatient.Interval = 2000;
                timerForScrollListPatient.Enabled = true;
                timerForScrollListPatient.Start();

                timerSetDataToGridControl.Interval = WaitingScreenCFG.TIMER_FOR_AUTO_LOAD_WAITING_SCREENS * 1000;
                timerSetDataToGridControl.Enabled = true;
                timerSetDataToGridControl.Start();

                timerAutoLoadDataPatient.Interval = WaitingScreenCFG.TIMER_FOR_SET_DATA_TO_GRID_PATIENTS * 1000;
                timerAutoLoadDataPatient.Enabled = true;
                timerAutoLoadDataPatient.Start();

                timerForHightLightCallPatientLayout.Interval = WaitingScreenCFG.TIMER_FOR_HIGHT_LIGHT_CALL_PATIENT * 1000;
                timerForHightLightCallPatientLayout.Enabled = true;
                timerForHightLightCallPatientLayout.Start();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void SetDataToRoom(V_HIS_SAMPLE_ROOM room)
        {
            try
            {
                if (room != null)
                {
                    lblRoomName.Text = (room.SAMPLE_ROOM_NAME + " (" + room.DEPARTMENT_NAME + ")").ToUpper();
                }
                else
                {
                    lblRoomName.Text = "";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void SetIcon()
        {
            try
            {
                this.Icon = Icon.ExtractAssociatedIcon(System.IO.Path.Combine(ApplicationStoreLocation.ApplicationDirectory, ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void timerForScrollListPatientProcess()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate { ScrollListPatientProcess(); }));
                }
                else
                {
                    ScrollListPatientProcess();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private void ScrollListPatientProcess()
        {
            try
            {
                index += 1;
                gridViewWaitingCls.FocusedRowHandle = index;
                if (index == rowCount)
                {
                    index = 0;
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void timerForScrollListPatient_Tick(object sender, EventArgs e)
        {
            try
            {
                Task ts = Task.Factory.StartNew(timerForScrollListPatientProcess);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void timerAutoLoadDataPatient_Tick(object sender, EventArgs e)
        {
            try
            {
                LoadWaitingPatientForWaitingScreen();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        void LoadWaitingPatientForWaitingScreen()
        {
            try
            {
                Task ts = Task.Factory.StartNew(ExecuteThreadWaitingPatientToCall);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void ExecuteThreadWaitingPatientToCall()
        {
            try
            {
                //if (this.InvokeRequired)
                //{
                //    this.Invoke(new MethodInvoker(delegate { StartTheadWaitingPatientToCall(); }));
                //}
                //else
                //{
                StartTheadWaitingPatientToCall();
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        void StartTheadWaitingPatientToCall()
        {
            FillDataToDictionaryWaitingPatient();
        }

        private void SetFromConfigToControl()
        {
            try
            {
                organizationName = WaitingScreenCFG.ORGANIZATION_NAME;
                // mau phong xu ly
                List<int> roomNameColorCodes = WaitingScreenCFG.ROOM_NAME_FORCE_COLOR_CODES;
                if (roomNameColorCodes != null && roomNameColorCodes.Count == 3)
                {
                    lblRoomName.ForeColor = System.Drawing.Color.FromArgb(roomNameColorCodes[0], roomNameColorCodes[1], roomNameColorCodes[2]);
                    lblMoiNguoiBenh.ForeColor = System.Drawing.Color.FromArgb(roomNameColorCodes[0], roomNameColorCodes[1], roomNameColorCodes[2]);
                    lblSo.ForeColor = System.Drawing.Color.FromArgb(roomNameColorCodes[0], roomNameColorCodes[1], roomNameColorCodes[2]);
                }

                // màu tên bác sĩ
                List<int> userNameColorCodes = WaitingScreenCFG.USER_NAME_FORCE_COLOR_CODES;
                if (userNameColorCodes != null && userNameColorCodes.Count == 3)
                {
                    lblDoctorName.ForeColor = System.Drawing.Color.FromArgb(userNameColorCodes[0], userNameColorCodes[1], userNameColorCodes[2]);
                }

                //mau background
                List<int> parentBackColorCodes = WaitingScreenCFG.PARENT_BACK_COLOR_CODES;
                if (parentBackColorCodes != null && parentBackColorCodes.Count == 3)
                {
                    layoutControlGroup1.AppearanceGroup.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    layoutControlGroup3.AppearanceGroup.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    layoutControlGroup4.AppearanceGroup.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    layoutControlGroup5.AppearanceGroup.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    Root.AppearanceGroup.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    layoutControlGroupMoiBenhNhan.AppearanceGroup.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    layoutControlGroupMoiBenhNhanSo.AppearanceGroup.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    lblCoSttNhoHon.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    lblMoiNguoiBenh.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    lblPatientName.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    lblSoThuTuBenhNhan.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    lblSo.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                }

                //màu chữ tên tổ chức
                //List<int> organizationColorCodes = WaitingScreenCFG.ORGANIZATION_FORCE_COLOR_CODES;
                //if (organizationColorCodes != null && organizationColorCodes.Count == 3)
                //{
                //    lblSrollText.ForeColor = System.Drawing.Color.FromArgb(organizationColorCodes[0], organizationColorCodes[1], organizationColorCodes[2]);
                //}
                //gridControlWaitngCls
                //màu nền grid patients
                List<int> gridpatientBackColorCodes = WaitingScreenCFG.GRID_PATIENTS_BACK_COLOR_CODES;
                if (gridpatientBackColorCodes != null && gridpatientBackColorCodes.Count == 3)
                {
                    gridViewWaitingCls.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(gridpatientBackColorCodes[0], gridpatientBackColorCodes[1], gridpatientBackColorCodes[2]);
                }


                //màu nền của header danh sách bệnh nhân
                List<int> gridpatientHeaderBackColorCodes = WaitingScreenCFG.GRID_PATIENTS_HEADER_BACK_COLOR_CODES;
                if (gridpatientHeaderBackColorCodes != null && gridpatientHeaderBackColorCodes.Count == 3)
                {
                    gridColumnAge.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(gridpatientHeaderBackColorCodes[0], gridpatientHeaderBackColorCodes[1], gridpatientHeaderBackColorCodes[2]);
                    gridColumnFirstName.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(gridpatientHeaderBackColorCodes[0], gridpatientHeaderBackColorCodes[1], gridpatientHeaderBackColorCodes[2]);
                    gridColumnInstructionTime.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(gridpatientHeaderBackColorCodes[0], gridpatientHeaderBackColorCodes[1], gridpatientHeaderBackColorCodes[2]);
                    gridColumnLastName.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(gridpatientHeaderBackColorCodes[0], gridpatientHeaderBackColorCodes[1], gridpatientHeaderBackColorCodes[2]);
                    gridColumnServiceReqStt.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(gridpatientHeaderBackColorCodes[0], gridpatientHeaderBackColorCodes[1], gridpatientHeaderBackColorCodes[2]);
                    gridColumnServiceReqType.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(gridpatientHeaderBackColorCodes[0], gridpatientHeaderBackColorCodes[1], gridpatientHeaderBackColorCodes[2]);
                    gridColumnSTT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(gridpatientHeaderBackColorCodes[0], gridpatientHeaderBackColorCodes[1], gridpatientHeaderBackColorCodes[2]);
                    gridColumnAddress.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(gridpatientHeaderBackColorCodes[0], gridpatientHeaderBackColorCodes[1], gridpatientHeaderBackColorCodes[2]);

                }

                //màu chữ của header danh sách bệnh nhân
                List<int> gridpatientHeaderForceColorCodes = WaitingScreenCFG.GRID_PATIENTS_HEADER_FORCE_COLOR_CODES;
                if (gridpatientHeaderForceColorCodes != null && gridpatientHeaderForceColorCodes.Count == 3)
                {
                    gridColumnAge.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                    gridColumnFirstName.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                    gridColumnInstructionTime.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                    gridColumnLastName.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                    gridColumnServiceReqStt.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                    gridColumnServiceReqType.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                    gridColumnSTT.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                    gridColumnAddress.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                }

                //màu chữ của body danh sách bệnh nhân
                gridpatientBodyForceColorCodes = WaitingScreenCFG.GRID_PATIENTS_BODY_FORCE_COLOR_CODES;
                if (gridpatientBodyForceColorCodes != null && gridpatientBodyForceColorCodes.Count == 3)
                {
                    gridColumnAge.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    gridColumnFirstName.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    gridColumnInstructionTime.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb
(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    gridColumnLastName.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    gridColumnServiceReqStt.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb
(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    gridColumnServiceReqType.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    gridColumnSTT.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    gridColumnAddress.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    lblPatientName.ForeColor = System.Drawing.Color.FromArgb(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    lblSoThuTuBenhNhan.ForeColor = System.Drawing.Color.FromArgb(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                }

                //màu chữ của trạng thái yêu cầu là mới
                newStatusForceColorCodes = WaitingScreenCFG.NEW_STATUS_REQUEST_FORCE_COLOR_CODES;

            }
            catch (Exception ex)
            {
            }
        }

        private void gridViewWatingExams_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData && e.Column.UnboundType != UnboundColumnType.Bound)
                {
                    HIS_SERVICE_REQ data = (HIS_SERVICE_REQ)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
                    if (data != null)
                    {
                        DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                        if (e.Column.FieldName == "INSTRUCTION_TIME_STR")
                        {
                            e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(data.INTRUCTION_TIME);
                        }
                        if (e.Column.FieldName == "AGE_DISPLAY")
                        {
                            e.Value = AgeHelper.CalculateAgeFromYear(data.TDL_PATIENT_DOB);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void gridViewWaitingCls_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData && e.Column.UnboundType != UnboundColumnType.Bound)
                {
                    HIS.Desktop.LocalStorage.BackendData.V2.ADO.V_HIS_TREATMENT_SAMPLE_DESK data = (HIS.Desktop.LocalStorage.BackendData.V2.ADO.V_HIS_TREATMENT_SAMPLE_DESK)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
                    if (data != null)
                    {
                        DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                        if (e.Column.FieldName == "AGE_DISPLAY")
                        {
                            e.Value = GetYearOld(data.TDL_PATIENT_DOB);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private string GetYearOld(long dob)
        {
            string yearDob = "";
            try
            {
                if (dob > 0)
                {
                    yearDob = dob.ToString().Substring(0, 4);
                }
            }
            catch (Exception ex)
            {
                yearDob = "";
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return yearDob;
        }

        private void gridViewWaitingCls_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                GridView View = sender as GridView;
                //if (e.RowHandle >= 0)
                //{
                //    bool serviceReqStt = Inventec.Common.TypeConvert.Parse.ToBoolean((View.GetRowCellValue(e.RowHandle, "CallPatientSTT") ?? "").ToString());
                //    if (serviceReqStt)
                //    {
                //        e.Appearance.Font = new System.Drawing.Font("Arial", 29, FontStyle.Bold);
                //        e.HighPriority = true;
                //        e.Appearance.BackColor = Color.Blue;
                //        e.Appearance.ForeColor = Color.Yellow;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        void FillDataToDictionaryWaitingPatient()
        {
            try
            {
                CommonParam param = new CommonParam();
                HIS.Desktop.LocalStorage.BackendData.V2.ADO.HisTreatmentSampleDeskViewFilter filter = new HIS.Desktop.LocalStorage.BackendData.V2.ADO.HisTreatmentSampleDeskViewFilter();

                if (room != null)
                {
                    filter.SAMPLE_ROOM_ID = room.ID;
                }

                List<long> lstServiceReqSTT = new List<long>();
                long startDay = Inventec.Common.TypeConvert.Parse.ToInt64((Inventec.Common.DateTime.Get.StartDay() ?? 0).ToString());//20181212121527
                long endDay = Inventec.Common.TypeConvert.Parse.ToInt64((Inventec.Common.DateTime.Get.EndDay() ?? 0).ToString());
                filter.CREATE_DATE_FROM = startDay;
                filter.CREATE_DATE_TO = endDay;
                filter.ORDER_FIELD = "TDL_IS_PRIORITY";
                filter.ORDER_DIRECTION = "DESC NULLS LAST";
                filter.ORDER_FIELD1 = "CREATE_TIME";
                filter.ORDER_DIRECTION1 = "ASC";
                filter.HASNT_SAMPLE_DESK = true;
                filter.IS_BHYT_OR_PAID = this.chkIsNotInDebt;

                mosUserConsummer = new Inventec.Common.WebApiClient.ApiConsumer(HisConfigCFG.MOS_USER_URI, GlobalVariables.APPLICATION_CODE);
                mosUserConsummer.SetTokenCode(HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer.GetTokenCode());

                LogSystem.Debug(HisConfigCFG.MOS_USER_URI);
                var result = new BackendAdapter(param).Get<List<HIS.Desktop.LocalStorage.BackendData.V2.ADO.V_HIS_TREATMENT_SAMPLE_DESK>>("api/HisTreatmentSampleDesk/GetView", mosUserConsummer, filter, param);
                //Inventec.Common.Logging.LogSystem.Debug("Data Update." + result.Count);
                if (result != null && result.Count > 0)
                {
                    //Inventec.Common.Logging.LogSystem.Debug("Data Update.");
                    //HIS.Desktop.LocalStorage.BackendData.V2.ADO.HIS_TREATMENT_SAMPLE_DESK update = new ADO.HIS_TREATMENT_SAMPLE_DESK();
                    //update.ID = result.First().ID;
                    //update.SAMPLE_ROOM_ID = result.First().SAMPLE_ROOM_ID;
                    //update.TDL_IS_PRIORITY = result.First().TDL_IS_PRIORITY;
                    //update.TDL_PATIENT_TYPE_ID = result.First().TDL_PATIENT_TYPE_ID;
                    //update.TDL_TREATMENT_TYPE_ID = result.First().TDL_TREATMENT_TYPE_ID;
                    //update.TREATMENT_ID = result.First().TREATMENT_ID;
                    //update.CREATE_TIME = result.First().CREATE_TIME;
                    //update.SAMPLE_DESK_ID = 23758235;
                    //Inventec.Common.Logging.LogSystem.Debug("Data Update. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => update), update));
                    //CommonParam param1 = new CommonParam();
                    //var createResult = new BackendAdapter(param1).Post<HIS.Desktop.LocalStorage.BackendData.V2.ADO.HIS_TREATMENT_SAMPLE_DESK>(
                    //                       "/api/HisTreatmentSampleDesk/Update",
                    //                       mosUserConsummer,
                    //                       update,
                    //                       param1);
                    CallPtDataWorker.DicCallPatient[room.ID] = ConnvertListServiceReq1ToADO(result);
                }
                else
                {
                    CallPtDataWorker.DicCallPatient[room.ID] = new List<SrADO>();
                }

                #region Process has exception
                SessionManager.ProcessTokenLost(param);
                #endregion
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private List<SrADO> ConnvertListServiceReq1ToADO(List<HIS.Desktop.LocalStorage.BackendData.V2.ADO.V_HIS_TREATMENT_SAMPLE_DESK> tsd)
        {
            List<SrADO> SrADOs = new List<SrADO>();
            try
            {
                List<SrADO> lisAdos = null;
                if (CallPtDataWorker.DicCallPatient != null && CallPtDataWorker.DicCallPatient.ContainsKey(room.ID))
                {
                    lisAdos = CallPtDataWorker.DicCallPatient[room.ID];
                }
                foreach (var item in tsd)
                {
                    SrADO ado = null;
                    ado = lisAdos != null ? lisAdos.FirstOrDefault(o =>o.ID == item.ID) : null;
                    SrADO SrADO = new SrADO();
                    SrADO.PATIENT_TYPE_NAME = item.PATIENT_TYPE_NAME;
                    SrADO.SAMPLE_DESK_NAME = item.SAMPLE_DESK_NAME;
                    SrADO.TDL_IS_PRIORITY = item.TDL_IS_PRIORITY;
                    SrADO.TDL_PATIENT_DOB = item.TDL_PATIENT_DOB;
                    SrADO.TDL_PATIENT_CODE = item.TDL_PATIENT_CODE;
                    SrADO.TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
                    SrADO.TREATMENT_CODE = item.TREATMENT_CODE;
                    SrADO.VIR_CREATE_DATE = item.VIR_CREATE_DATE;
                    SrADO.TREATMENT_TYPE_NAME = item.TREATMENT_TYPE_NAME;
                    SrADO.TREATMENT_ID = item.TREATMENT_ID;
                    SrADO.SAMPLE_ROOM_ID = item.SAMPLE_ROOM_ID;
                    SrADO.SAMPLE_DESK_ID = item.SAMPLE_DESK_ID;

                    if (ado != null && ado.CallPatientSTT)
                    {
                        SrADO.CallPatientSTT = true;
                    }
                    else
                    {
                        SrADO.CallPatientSTT = false;
                    }

                    SrADOs.Add(SrADO);
                }
                SrADOs = SrADOs.OrderByDescending(o => o.CallPatientSTT).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return SrADOs;
        }

        private void SetDataToCurrentPatientCall(SrADO SrAdo)
        {
            try
            {
                if (SrAdo != null)
                {
                    Inventec.Common.Logging.LogSystem.Debug("PatientIsCall step 7");
                    lblPatientName.Text = SrAdo.TDL_PATIENT_NAME;
                    //lblSoThuTuBenhNhan.Text = SrAdo.NUM_ORDER + "";
                }
                else if (!isSetNum)
                {
                    Inventec.Common.Logging.LogSystem.Debug("PatientIsCall step 8");
                    lblPatientName.Text = "";
                    lblSoThuTuBenhNhan.Text = "";
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void SetDataToLabelMoiBenhNhanChild()
        {
            try
            {
                if (SrAdoWorker.SrAdo != null && SrAdoWorker.SrAdo.CallPatientSTT)
                {
                    SetDataToCurrentPatientCall(SrAdoWorker.SrAdo);
                }
                else if (!isSetNum)
                {
                    SetDataToCurrentPatientCall(null);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void SetDataToCurrentCallPatient()
        {
            try
            {
                if (CallPtDataWorker.DicCallPatient != null && CallPtDataWorker.DicCallPatient.Count > 0 && CallPtDataWorker.DicCallPatient[room.ID] != null && CallPtDataWorker.DicCallPatient[room.ID].Count > 0)
                {
                    SrADO PatientIsCall = CallPtDataWorker.DicCallPatient[room.ID].FirstOrDefault(o => o.CallPatientSTT);
                    Inventec.Common.Logging.LogSystem.Info("SetDataToCurrentCallPatient() tDu lieu PatientIsCall:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => PatientIsCall), PatientIsCall));

                    if (PatientIsCall != null)
                    {
                        isSetNum = false;
                        Inventec.Common.Logging.LogSystem.Debug("PatientIsCall step 1");
                        if (SrAdoWorker.SrAdo == null)
                        {
                            Inventec.Common.Logging.LogSystem.Debug("PatientIsCall step 2");
                            SrAdoWorker.SrAdo = PatientIsCall;
                        }
                        else
                        {
                            if (PatientIsCall.TDL_PATIENT_NAME != SrAdoWorker.SrAdo.TDL_PATIENT_NAME || PatientIsCall.CREATE_TIME != SrAdoWorker.SrAdo.CREATE_TIME)
                            {
                                Inventec.Common.Logging.LogSystem.Debug("PatientIsCall step 3");
                                SrAdoWorker.SrAdo = PatientIsCall;
                            }
                            else
                            {
                                Inventec.Common.Logging.LogSystem.Debug("PatientIsCall step 4");
                            }
                        }
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Info("PatientIsCall step 5");
                        SrAdoWorker.SrAdo = null;
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Info("PatientIsCall step 6");
                    SrAdoWorker.SrAdo = null;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void SetDataToGridControlWaitingCLSs()
        {
            try
            {
                if (CallPtDataWorker.DicCallPatient != null && CallPtDataWorker.DicCallPatient.Count > 0 && CallPtDataWorker.DicCallPatient[room.ID] != null && CallPtDataWorker.DicCallPatient[room.ID].Count > 0)
                {
                    int countPatient = HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<int>(AppConfigKeys.CONFIG_KEY__SO_BENH_NHAN_TREN_DANH_SACH_CHO_KHAM_VA_CLS);
                    if (countPatient == 0)
                        countPatient = 10;

                    // danh sách chờ kết quả cận lâm sàng
                    var ServiceReqFilterSTTs = CallPtDataWorker.DicCallPatient[room.ID];
                    gridControlWaitingCls.Invoke(new MethodInvoker(delegate
                    {
                        gridControlWaitingCls.BeginUpdate();
                        gridControlWaitingCls.DataSource = ServiceReqFilterSTTs;
                        gridControlWaitingCls.EndUpdate();
                    }));
                    Inventec.Common.Logging.LogSystem.Info("Du lieu DicCallPatient:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CallPtDataWorker.DicCallPatient[room.ID].Take(countPatient).ToList()), CallPtDataWorker.DicCallPatient[room.ID].Take(countPatient).ToList()));
                }
                else
                {
                    gridControlWaitingCls.Invoke(new MethodInvoker(delegate
                       {
                           gridControlWaitingCls.BeginUpdate();
                           gridControlWaitingCls.DataSource = null;
                           gridControlWaitingCls.EndUpdate();
                       }));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void GetFilePath()
        {
            try
            {
                FilePath = Directory.GetFiles(ConfigApplicationWorker.Get<string>(AppConfigKeys.CONFIG_KEY__DUONG_DAN_CHAY_FILE_VIDEO));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        // gan du lieu vao gridcontrol
        private void timerSetDataToGridControl_Tick(object sender, EventArgs e)
        {
            try
            {
                SetDataToGridControlCLS();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        void SetDataToGridControlCLS()
        {
            try
            {
                Task ts = Task.Factory.StartNew(executeThreadSetDataToGridControl);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void SetDataToCurentCallPatientUsingThread()
        {
            try
            {
                Task ts = Task.Factory.StartNew(executeThreadSetDataToCurentCallPatient);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void SetDataToLabelMoiBenhNhan()
        {
            try
            {
                Task ts = Task.Factory.StartNew(executeThreadSetDataToLabelMoiBenhNhan);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void StartTheadSetDataToCurentCallPatient()
        {
            SetDataToCurentCallPatientUsingThread();
        }

        void executeThreadSetDataToCurentCallPatient()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate { SetDataToCurrentCallPatient(); }));
                }
                else
                {
                    SetDataToCurrentCallPatient();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        void executeThreadSetDataToLabelMoiBenhNhan()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate { SetDataToLabelMoiBenhNhanChild(); }));
                }
                else
                {
                    SetDataToLabelMoiBenhNhanChild();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        void executeThreadSetDataToGridControl()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate { StartTheadSetDataToGridControl(); }));
                }
                else
                {
                    StartTheadSetDataToGridControl();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        void StartTheadSetDataToGridControl()
        {
            SetDataToGridControlWaitingCLSs();
        }

        private void timerForHightLightCallPatientLayout_Tick(object sender, EventArgs e)
        {
            try
            {

                //SetDataToCurrentCallPatient();
                SetDataToCurentCallPatientUsingThread();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                SetDataToLabelMoiBenhNhan();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        public void CallNumOrder(int min, int max)
        {
            try
            {
                isSetNum = true;
                if (CallPtDataWorker.DicCallPatient != null && CallPtDataWorker.DicCallPatient.ContainsKey(room.ID))
                {
                    CallPtDataWorker.DicCallPatient[room.ID].ForEach(o => o.CallPatientSTT = false);
                }
                if (SrAdoWorker.SrAdo != null)
                {
                    SrAdoWorker.SrAdo.CallPatientSTT = false;
                }
                if (min == max)
                {
                    lblPatientName.Invoke(new MethodInvoker(delegate
                    {
                        lblPatientName.Text = "CÓ SỐ THỨ TỰ";
                    }));
                    lblSoThuTuBenhNhan.Invoke(new MethodInvoker(delegate
                    {
                        lblSoThuTuBenhNhan.Text = min + "";
                    }));
                }
                else
                {
                    lblPatientName.Invoke(new MethodInvoker(delegate
                    {
                        lblPatientName.Text = "CÓ SỐ THỨ TỰ TỪ";
                    }));
                    lblSoThuTuBenhNhan.Invoke(new MethodInvoker(delegate
                    {
                        lblSoThuTuBenhNhan.Text = min + " - " + max;
                    }));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void frmWaitingScreenSample22_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                timerAutoLoadDataPatient.Enabled = false;
                timerForHightLightCallPatientLayout.Enabled = false;
                timerForScrollListPatient.Enabled = false;
                timerSetDataToGridControl.Enabled = false;
                timer1.Enabled = false;

                timerAutoLoadDataPatient.Stop();
                timerForHightLightCallPatientLayout.Stop();
                timerForScrollListPatient.Stop();
                timerSetDataToGridControl.Stop();
                timer1.Stop();

                timerAutoLoadDataPatient.Dispose();
                timerForHightLightCallPatientLayout.Dispose();
                timerForScrollListPatient.Dispose();
                timerSetDataToGridControl.Dispose();
                timer1.Dispose();

                SrAdoWorker.SrAdo = new SrADO();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
