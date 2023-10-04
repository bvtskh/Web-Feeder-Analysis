using FeederAnalysis.Cache;
using FeederAnalysis.DAL;
using FeederAnalysis.DAL.UMES;
using FeederAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace FeederAnalysis.Business
{
    public class Repository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static List<string> ListID(string order)
        {
            var sql = $@"SELECT [SystemId]
                      FROM [IOT2021].[dbo].[PDA_ErrorHistory]
                      WHERE WO = '{order}'";
            using (IOT2021Context context = new IOT2021Context())
            {
                return context.Database.SqlQuery<string>(sql, "").ToList();
            }
        }

        public static string Tokusai_LineItem_Update(DataTable dt)
        {
            try
            {
                using (DataContext context = new DataContext())
                {
                    var StaffCodeParam = new SqlParameter("@Data",dt)
                    {
                        TypeName = "dbo.udt_Tokusai_LineItem",
                        SqlDbType = SqlDbType.Structured
                    };
                    context.Database
                       .ExecuteSqlCommand("exec Tokusai_LineItem_Update @Data",
                       StaffCodeParam);
                    //var list = context.Database.SqlQuery<Tokusai_LineItem>("Tokusai_LineItem_Update", new { Data = dt }).ToList();
                    Console.Write("");
                    return "";
                }
            }
            catch (Exception ex)
            {
                log.Error("Ope Job Err", ex);
                return ex.Message;
            }
           
        }
        private static int MapTask(int task)
        {
            int result = 0;
            switch (task)
            {
                case 11: //Load Part
                    result = 1;
                    break;
                case 20: //Veryfied
                    result = 2;
                    break;
                case 15: //Reload
                    result = 3;
                    break;
                case 16: //Replace Feeder
                    result = 4;
                    break;
                default:
                    break;
            }
            return result;
        }
        public static List<FeederAlarm> GetAllFeeder()
        {
            using (DataContext context = new DataContext())
            {
                //var res = context.FeederAlarms.OrderByDescending(r => r.STATE).ThenBy(h => h.EX_DATE).ToList();
                var res = context.FeederAlarms.ToList();
                return res;
            }
            //List<FeederAlarm> feederAlarms = new List<FeederAlarm>();
            //feederAlarms = _baseContext.FeederAlarms.ToList();
            //var result = feederAlarms.OrderByDescending(r => r.STATE).ThenBy(h => h.EX_DATE).ThenBy(t => t.ABOUT).ToList();
            //return result;
        }
        public static void SaveFeeder(List<FeederAlarm> lstFeeder)
        {
            using (DataContext context = new DataContext())
            {
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [SMT].[dbo].[FeederAlarm]");
                context.FeederAlarms.AddRange(lstFeeder);
                context.SaveChanges();
            }
        }

        public static void SaveOpeHistories()
        {
            var lstOpeFail = FindAllOpeFail();
            List<PDA_ErrorHistory> lst = new List<PDA_ErrorHistory>();
            var isConnect = NetworkHelper.CheckConnect("172.28.6.96");
            if (!isConnect)
            {
                log.Error("Connect Server IOT Error!");
            }
            else
            {
                List<string> lstInsert = new List<string>();
                foreach (var item in lstOpeFail.GroupBy(r => r.PRODUCTION_ORDER_ID))
                {
                    var listID = ListID(item.Key);
                    var tmp = item.ToList();
                    var diff = tmp.Select(r => r.ID.ToString()).Except<string>(listID).ToList();
                    lstInsert.AddRange(diff);
                }
                using (IOT2021Context context = new IOT2021Context())
                {
                    foreach (var item in lstInsert)
                    {
                        var entity = lstOpeFail.First(r => r.ID.ToString() == item);
                        context.PDA_ErrorHistory.Add(new PDA_ErrorHistory()
                        {
                            ErrorTime = entity.OPERATE_TIME,
                            Model = entity.PRODUCT_ID,
                            ErrorContent = entity.FAULT_REASON.Length > 199 ? entity.FAULT_REASON.Substring(0, 199) : entity.FAULT_REASON,
                            OperatorCode = entity.OPERATOR_NAME,
                            PartCode = string.IsNullOrEmpty(entity.PART_ID) ? "" : entity.PART_ID,
                            WO = entity.PRODUCTION_ORDER_ID,
                            Line = entity.LINE_ID,
                            Customer = entity.CUSTOMER_ID,
                            Location = GetLocation(entity.LINE_ID),
                            SystemId = entity.ID.ToString(),
                            Slot = entity.MACHINE_SLOT,
                            ProcessID = Repository.MapTask(entity.TASK)
                        });
                        context.SaveChanges();

                    }
                }
            }
        }
        public static string GetLocation(string lineId)
        {
            using (DataContext context = new DataContext())
            {
                var locationEntity = context.Locations.SingleOrDefault(r => r.LineId == lineId);
                return locationEntity != null ? locationEntity.LocationId : "";
            }
        }
        public static List<MaterialOrderItem> FindAllMaterialItem()
        {
            using (UmesContext context = new UmesContext())
            {
                var res = context.Database.SqlQuery<MaterialOrderItem>("FindAllMaterialOrderItem", "");
                return res.ToList();

            }
        }
        public static List<OpeLogEntity> FindAllOpeFail()
        {
            string sql = @"
                            SELECT o.PRODUCT_ID, o.FAULT_REASON, o.OPERATOR_NAME, o.OPERATE_TIME, o.PRODUCTION_ORDER_ID, o.PART_ID, o.LINE_ID, o.CUSTOMER_ID, o.ID, o.TASK, o.MACHINE_SLOT
                            FROM [UMC_MESDB_TEST].[dbo].[OPERATION_LOGS] o
                            INNER JOIN [UMC_MESDB_TEST].[dbo].[MATERIAL_ORDERS] m ON m.ID = o.MATERIAL_ORDER_ID AND o.CUSTOMER_ID = m.CUSTOMER_ID AND o.LINE_ID = m.LINE_ID 
                            WHERE m.IS_STARTED = '1'  AND o.IS_FAILED = '1' AND o.MACHINE_SLOT <> '-1'
                            ";
            using (UmesContext context = new UmesContext())
            {
                var res = context.Database.SqlQuery<OpeLogEntity>(sql, "");
                return res.ToList();
            }
        }

        private static bool TokusaiFn()
        {
            return SingletonHelper.UsapInstance.TokusaiFinish("U005220714-1");
        }


        public static List<DefectEntity> GetDefectWarning()
        {
            List<DefectEntity> lstUmes = new List<DefectEntity>();
            List<DefectEntity> lstInsert = new List<DefectEntity>();
            string sql = $@"SELECT TOP 1000 
                            t2.BOARD_NO,
                            t2.PRODUCT_ID,
                            t2.DEFECT_CODE,
                            t2.LOCATION_CODE,
                            t2.STATION_NO,
                            t2.UPDATE_TIME
                              FROM 
                              (
                              SELECT  DEFECT_CODE, LOCATION_CODE
                              FROM [UMC_MESDB_TEST].[dbo].[DEFECT_LOGS]
                              WHERE STATION_NO = 'ICT_KYD' AND UPDATE_TIME > '2023-1-1'
                              GROUP BY DEFECT_CODE, LOCATION_CODE
                              HAVING COUNT(*) >2
                              ) t1
                              INNER JOIN [UMC_MESDB_TEST].[dbo].[DEFECT_LOGS] t2 ON t1.DEFECT_CODE = t2.DEFECT_CODE AND t1.LOCATION_CODE = t2.LOCATION_CODE
                              WHERE t2.STATION_NO = 'ICT_KYD'
                              ORDER BY t1.DEFECT_CODE, t1.LOCATION_CODE, t2.UPDATE_TIME";
            using (UmesContext context = new UmesContext())
            {
                lstUmes = context.Database.SqlQuery<DefectEntity>(sql).ToList();
                //return result;
            }
            using (DataContext context = new DataContext())
            {
                var lstCurr = context.Database.SqlQuery<string>("SELECT [BOARD_NO] FROM [UMCVN_BASE].[dbo].[PMS_Kyo_ICT]").ToList();
                foreach (var item in lstUmes)
                {
                    if (!lstCurr.Contains(item.BOARD_NO))
                    {
                        lstInsert.Add(item);
                    }
                }
                if (lstInsert.Count > 0)
                {
                    context.Database.ExecuteSqlCommand("TRUNCATE TABLE [UMCVN_BASE].[dbo].[PMS_Kyo_ICT]");
                    foreach (var item in lstUmes)
                    {
                        context.Database.ExecuteSqlCommand($@"INSERT INTO [UMCVN_BASE].[dbo].[PMS_Kyo_ICT] VALUES(
                                                            '{item.BOARD_NO}',
                                                            '{item.PRODUCT_ID}',
                                                            '{item.DEFECT_CODE}',
                                                            '{item.LOCATION_CODE}',
                                                            '{item.STATION_NO}',
                                                            '{item.UPDATE_TIME}'
                                                            )");
                    }
                }
                return lstInsert;
            }
        }
        public static List<CaliEntity> GetListCali()
        {
            var startDate = DateTime.Now;
            var endDate = startDate.AddDays(7);
            string sql = $@" SELECT t.[SERIAL],t.[PART_NO],t.[MODEL],d.CALI_RECOMMEND
                              FROM [DeviceControl].[dbo].[DEVICE] t inner join [DeviceControl].[dbo].[CALIBRATION] d on t.PART_NO=d.PART_NO
                              WHERE d.CALI_RECOMMEND BETWEEN '{startDate.ToString("yyyy-MM-dd")}' AND '{endDate.ToString("yyyy-MM-dd")}'
                              ORDER BY d.CALI_RECOMMEND DESC";
            using (DataContext context = new DataContext())
            {
                var res = context.Database.SqlQuery<CaliEntity>(sql, "").ToList();
                return res;
            }
        }
        public static List<StellMeshEntity> GetStellMesh()
        {
            using (UmesContext context = new UmesContext())
            {
                string sql = @"SELECT t1.MESH_NO,t1.SIDE,t1.MAX_USE_COUNT,t1.LOAD_TIME,t1.MATERIAL_ORDER_ID,t2.PRODUCT_ID,t2.PRODUCTION_ORDER_ID,t2.LINE_ID,t2.QUANTITY,t2.START_TIME,t2.VERSION,t2.CUSTOMER_ID
                                  FROM [UMC_MESDB_TEST].[dbo].[STEEL_MESH_STATUS] t1
                                  INNER JOIN [UMC_MESDB_TEST].[dbo].[MATERIAL_ORDERS] t2 ON t2.ID = t1.MATERIAL_ORDER_ID
                                  where t2.IS_STARTED = '1' ORDER BY t1.LOAD_TIME";
                var res = context.Database.SqlQuery<StellMeshEntity>(sql, "").ToList();
                return res;
            }
        }
        public static List<SqueegeeEntity> GetSqeegee()
        {
            using (UmesContext context = new UmesContext())
            {
                string sql = @"SELECT t1.SQUEEGEE_NO,t1.SIDE,t1.MAX_USE_COUNT,t1.LOAD_TIME,t1.MATERIAL_ORDER_ID,t2.PRODUCT_ID,t2.PRODUCTION_ORDER_ID,t2.LINE_ID,t2.QUANTITY,t2.START_TIME,t2.VERSION,t2.CUSTOMER_ID
                                  FROM [UMC_MESDB_TEST].[dbo].[SQUEEGEE_STATUS] t1
                                  INNER JOIN [UMC_MESDB_TEST].[dbo].[MATERIAL_ORDERS] t2 ON t2.ID = t1.MATERIAL_ORDER_ID
                                  where t2.IS_STARTED = '1' ORDER BY t1.LOAD_TIME";
                var res = context.Database.SqlQuery<SqueegeeEntity>(sql, "").ToList();
                return res;
            }
        }
        private static List<SolderPart> GetSolderPart()
        {
            using (UmesContext context = new UmesContext())
            {
                string sql = @"SELECT t1.[MATERIAL_ORDER_ID],t1.[PART_ID],t1.[UPN_ID],t2.UNFREEZE_TIME,t2.MIX_END_TIME
                               FROM [UMC_MESDB_TEST].[dbo].[MATERIAL_ORDER_ITEMS] t1 WITH (NOLOCK)
							   INNER JOIN [UMC_MESDB_TEST].[dbo].[SPEC_PART_STATUS] t2 ON t1.UPN_ID = t2.UPN_ID
                               WHERE t1.MACHINE_SLOT = '999'";
                return context.Database.SqlQuery<SolderPart>(sql, "").ToList();
            }
        }
        public static void UpdateSolder()
        {
            using (IOT2021Context context = new IOT2021Context())
            {
                foreach (var item in GetSolderPart())
                {
                    string sql = $@"UPDATE [IOT2021].[dbo].[PDAInfo_Ver2]
                                      SET SolderPasteCode = '{item.PART_ID}', UPN = '{item.UPN_ID}', TimeUnfreeze = '{item.UNFREEZE_TIME}',TimeMix = '{item.MIX_END_TIME}'
                                      WHERE MaterialOrderId = '{item.MATERIAL_ORDER_ID}'";
                    var row = context.Database.ExecuteSqlCommand(sql, "");
                }
            }
        }
        public static List<PROFILER_INFO> GetListProfileAlarm()
        {
            var now = DateTime.Now;
            var startTime = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
            var endTime = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0);
            List<PROFILER_INFO> result = new List<PROFILER_INFO>();
            //string sql = $@"SELECT t1.UMC_NO,t1.LOCATION,t1.HOST_NAME,t1.UPD_TIME
            //                  FROM [ERP].[dbo].[PROFILER_INFO] t1
            //                  where UPD_TIME NOT BETWEEN '{startTime}' AND '{endTime}'";
            string sql = $@"SELECT t1.UMC_NO,t1.LOCATION,t1.HOST_NAME,t1.UPD_TIME
                              FROM [ERP].[dbo].[PROFILER_INFO] t1 WHERE t1.LOCATION NOT IN('MAKER','ACC-Asset')
                              AND UPD_TIME NOT BETWEEN '{startTime}' AND '{endTime}'";
            using (DXContext context = new DXContext())
            {
                return context.Database.SqlQuery<PROFILER_INFO>(sql).ToList();
            }
        }
    }
}
