using FeederAnalysis.Cache;
using FeederAnalysis.DAL;
using FeederAnalysis.DAL.COST;
using FeederAnalysis.DAL.UMES;
using FeederAnalysis.DAL.USAP;
using FeederAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace FeederAnalysis.Business
{
    public class Repository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string DB = "SMT";
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
                    var StaffCodeParam = new SqlParameter("@Data", dt)
                    {
                        TypeName = "dbo.udt_Tokusai_LineItem",
                        SqlDbType = SqlDbType.Structured
                    };
                    context.Database
                       .ExecuteSqlCommand("exec Tokusai_LineItem_Update @Data",
                       StaffCodeParam);
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

        public static string MainSub_LineItem_Update(DataTable dt)
        {
            try
            {
                using (DataContext context = new DataContext())
                {
                    var StaffCodeParam = new SqlParameter("@Data", dt)
                    {
                        TypeName = "dbo.udt_MainSub_LineItem",
                        SqlDbType = SqlDbType.Structured
                    };
                    context.Database
                       .ExecuteSqlCommand("exec MainSub_LineItem_Update @Data",
                       StaffCodeParam);
                    //var list = context.Database.SqlQuery<Tokusai_LineItem>("Tokusai_LineItem_Update", new { Data = dt }).ToList();
                    Console.Write("");
                    return "";
                }
            }
            catch (Exception ex)
            {
                log.Error("MainSub_LineItem_Update", ex);
                return ex.Message;
            }

        }

        internal static void UpdateVerifiedOrderItem(VerifyLoadedEntity item)
        {
            using (var context = new DataContext())
            {
                string sql = $@"  UPDATE [{DB}].[dbo].[LoadedOrderItem] 
                                  SET IS_VERIFIED = 1, UPD_VERIFY_TIME = GETDATE()
                                  WHERE LINE_ID = '{item.LINE_ID}' 
                                  AND PART_ID = '{item.PART_ID}' 
                                  AND MACHINE_ID = '{item.MACHINE_ID}' 
                                  AND MACHINE_SLOT = '{item.MACHINE_SLOT}' 
                                  AND PRODUCT_ID = '{item.PRODUCT_ID}'
                                ";
                context.Database.ExecuteSqlCommand(sql, "");
            }
        }

        internal static void CurrentOrderItem_Update(DataTable dt)
        {
            using (var context = new DataContext())
            {
                var StaffCodeParam = new SqlParameter("@Data", dt)
                {
                    TypeName = "dbo.udt_LoadedOrderItem",
                    SqlDbType = SqlDbType.Structured
                };
                context.Database
                   .ExecuteSqlCommand("exec CurrentOrderItem_Update @Data",
                   StaffCodeParam);
            }
        }

        internal static List<VerifyLoadedEntity> FindLoadedOrderItem()
        {
            using (var context = new DataContext())
            {
                var sql = $@"SELECT [LINE_ID]
                                   ,[PART_ID]
                                  ,[MACHINE_ID]
                                  ,[MACHINE_SLOT]
                                  ,[UPD_TIME]
                                  ,[PRODUCT_ID]
                                  ,[IS_VERIFIED]
                                  ,[PRODUCTION_ORDER_ID]
                                  ,[IS_VERIFIED]
                              FROM [{DB}].[dbo].[LoadedOrderItem] where IS_VERIFIED = 0";
                return context.Database.SqlQuery<VerifyLoadedEntity>(sql, "").ToList();
            }
        }

        internal static void VerifiedOrderItem_Update(DataTable dt)
        {
            using (var context = new DataContext())
            {
                var StaffCodeParam = new SqlParameter("@Data", dt)
                {
                    TypeName = "dbo.udt_LoadedOrderItem",
                    SqlDbType = SqlDbType.Structured
                };
                context.Database
                   .ExecuteSqlCommand("exec VerifiedOrderItem_Update @Data",
                   StaffCodeParam);
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
                var res = context.FeederAlarms.ToList();
                return res;
            }

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

        public static List<FindAllMaterialOrderItemChange> FindAllMaterialItemChange()
        {
            using (UmesContext context = new UmesContext())
            {
                var res = context.Database.SqlQuery<FindAllMaterialOrderItemChange>("FindAllMaterialOrderItemChange", "");
                return res.ToList();
            }
        }

        public static List<MaterialOrderItemOP> DX_GetOperationLogByTime(DateTime start, DateTime end)
        {
            using (UmesContext context = new UmesContext())
            {
                var startParam = new SqlParameter("@start", start);
                var endParam = new SqlParameter("@end", end);
                var res = context.Database.SqlQuery<MaterialOrderItemOP>("DX_GetOperationLogByTime @start, @end", startParam, endParam);
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

        internal static void SaveTimeRunning(Guid iD, DateTime time)
        {
            using (var db = new DataContext())
            {
                var sql = $@"DELETE FROM [dbo].[TimeRunning]";
                db.Database.ExecuteSqlCommand(sql);
                sql = $@"INSERT INTO [dbo].[TimeRunning]
                                       ([TIME_RUNNING]
                                       ,[ID])
                                 VALUES
                                       ('{time}'
                                       ,'{iD.ToString()}')";
                db.Database.ExecuteSqlCommand(sql);
            }
        }

        internal static void UpdateTokusaiItemOPLogs(MaterialOrderItemOP item)
        {
            using (var db = new DataContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var itemDB = GetTokusaiItemOPLogs(item.LINE_ID, item.PART_ID, item.PRODUCT_ID, db);
                        if (itemDB != null)
                        {
                            if (!item.IS_DM_ACCEPT && itemDB.IS_DM_ACCEPT)
                            {
                                SaveTokusaiLineHistory(item, TokusaiItemHistoryChangeId.TokusaiDMNotAccept, "Linh kiện Tokusai không được DM cho phép sử dụng", db);
                                UpdateItemOPLogs(item, db);
                            }

                            if (itemDB.IS_TOKUSAI != item.IS_TOKUSAI)
                            {
                                var changeID = itemDB.IS_TOKUSAI ? TokusaiItemHistoryChangeId.HongTrang : TokusaiItemHistoryChangeId.TrangHong;
                                var reason = itemDB.IS_TOKUSAI ? "Tokusai Hồng => Trắng" : "Tokusai Trắng => Hồng";
                                SaveTokusaiLineHistory(item, changeID, reason, db);
                                UpdateItemOPLogs(item, db);
                            }
                        }
                        else
                        {
                            if (!item.IS_DM_ACCEPT)
                            {
                                SaveTokusaiLineHistory(item, TokusaiItemHistoryChangeId.TokusaiDMNotAccept, "Linh kiện Tokusai không được DM cho phép sử dụng", db);
                            }
                            InsertNewItemOPLogs(item, db);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }

        private static void InsertNewItemOPLogs(MaterialOrderItemOP item, DataContext db)
        {
            string tokusai = item.IS_TOKUSAI ? "1" : "0";
            string dm = item.IS_DM_ACCEPT ? "1" : "0";
            var sql = $@"INSERT INTO [dbo].[Tokusai_LineItem_OP_LOGS]
                                       ([LINE_ID]
                                       ,[PART_ID]
                                       ,[PRODUCT_ID]
                                       ,[MATERIAL_ORDER_ID]
                                       ,[UPD_TIME]
                                       ,[IS_TOKUSAI]
                                       ,[WO]
                                       ,[IS_DM_ACCEPT])
                                 VALUES
                                       ('{item.LINE_ID}'
                                       ,'{item.PART_ID}'
                                       ,'{item.PRODUCT_ID}'
                                       ,'{item.MATERIAL_ORDER_ID}'
                                       ,'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'
                                       ,{tokusai}
                                       ,'{item.PRODUCTION_ORDER_ID}'
                                       ,{dm})";
            db.Database.ExecuteSqlCommand(sql);
        }

        private static void UpdateItemOPLogs(MaterialOrderItemOP item, DataContext db)
        {
            var tokusai = item.IS_TOKUSAI ? "1" : "0";
            var dm = item.IS_DM_ACCEPT ? "1" : "0";
            var sql = $@"UPDATE [dbo].[Tokusai_LineItem_OP_LOGS]
                           SET 
                              [UPD_TIME] = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'
                              ,[IS_TOKUSAI] = {tokusai}
                              ,[IS_DM_ACCEPT] = {dm}
                         WHERE LINE_ID = '{item.LINE_ID}' AND PART_ID = '{item.PART_ID}' AND PRODUCT_ID = '{item.PRODUCT_ID}'";
            db.Database.ExecuteSqlCommand(sql);
        }

        private static void SaveTokusaiLineHistory(MaterialOrderItemOP item, TokusaiItemHistoryChangeId changeID, string reason, DataContext db)
        {
            var IsExist = db.Tokusai_LineHistorys.Where(m => m.ID.ToUpper() == item.ID.ToString().ToUpper()).FirstOrDefault();
            string sql = "";
            if (IsExist == null)
            {
                sql = $@"INSERT INTO [dbo].[Tokusai_LineHistory]
                                           ([LINE_ID]
                                           ,[PART_ID]
                                           ,[PRODUCT_ID]
                                           ,[UPD_TIME]
                                           ,[CHANGE_NAME]
                                           ,[CHANGE_ID]
                                           ,[WO]
                                           ,[IS_CONFIRM]
                                           ,[IS_DM_ACCEPT]
                                           ,[ID]
                                           ,[MATERIAL_ORDER_ID]
                                           )
                                     VALUES
                                           ('{item.LINE_ID}'
                                           ,'{item.PART_ID}'
                                           ,'{item.PRODUCT_ID}'
                                           ,'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'
                                           ,N'{reason}'
                                           , {(int)changeID}
                                           ,'{item.PRODUCTION_ORDER_ID}'
                                           ,0
                                           , {item.IS_DM_ACCEPT}
                                           ,'{item.ID}'
                                           ,'{item.MATERIAL_ORDER_ID}')";
                db.Database.ExecuteSqlCommand(sql);
            }
        }

        internal static Tokusai_LineItem GetTokusaiItemOPLogs(string lINE_ID, string pART_ID, string pRODUCT_ID, DataContext db)
        {
            string sql = $@"SELECT [LINE_ID]
                                      ,[PART_ID]
                                      ,[PRODUCT_ID]
                                      ,[MATERIAL_ORDER_ID]
                                      ,[UPD_TIME]
                                      ,[IS_TOKUSAI]
                                      ,[WO]
                                      ,[IS_DM_ACCEPT]
                                  FROM [dbo].[Tokusai_LineItem_OP_LOGS]
                                  WHERE LINE_ID = '{lINE_ID}'
                                  AND PART_ID = '{pART_ID}'
                                  AND PRODUCT_ID = '{pRODUCT_ID}'";
            var res = db.Database.SqlQuery<Tokusai_LineItem>(sql, "").FirstOrDefault();
            return res;
        }

        internal static TimeRunning GetLastIndexRequest()
        {
            using (var db = new DataContext())
            {
                string sql = $@"SELECT [TIME_RUNNING]
                                      ,[ID]
                                  FROM [dbo].[TimeRunning]";
                var res = db.Database.SqlQuery<TimeRunning>(sql, "").FirstOrDefault();
                return res;
            }
        }

        internal static List<MainSub_Model> GetAllPartMainSub()
        {
            using (var db = new DataContext())
            {
                return db.MainSub_Models.ToList();
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
        public static void TokusaiSave(FindAllMaterialOrderItemChange item)
        {
            using (DataContext context = new DataContext())
            {
                var tokusai = new Tokusai_LineHistory()
                {
                    LINE_ID = item.LINE_ID,
                    PART_ID = item.PART_ID,
                    PRODUCT_ID = item.PRODUCT_ID,
                    UPD_TIME = DateTime.Now,
                    CHANGE_NAME = item.CHANGE_ID == 1 ? "Tokusai Trắng => Hồng" : "Tokusai Hồng => Trắng",
                    CHANGE_ID = item.CHANGE_ID,
                    WO = item.PRODUCTION_ORDER_ID,
                    IS_CONFIRM = false,
                    ID = Guid.NewGuid().ToString(),
                    MATERIAL_ORDER_ID = item.MATERIAL_ORDER_ID,
                    MACHINE_SLOT = item.MACHINE_SLOT,
                    MACHINE_ID = item.MACHINE_ID,
                    IS_DM_ACCEPT = true
                };
                context.Tokusai_LineHistorys.Add(tokusai);
                context.SaveChanges();
            }
        }
        public static void MainSubIsTokusaiSave(MaterialOrderItem item)
        {
            using (DataContext context = new DataContext())
            {
                var tokusai = new Tokusai_LineHistory()
                {
                    LINE_ID = item.LINE_ID,
                    PART_ID = item.PART_ID,
                    PRODUCT_ID = item.PRODUCT_ID,
                    UPD_TIME = DateTime.Now,
                    CHANGE_NAME = "Linh kiện sử dụng tem hồng",
                    CHANGE_ID = 4,
                    WO = item.PRODUCTION_ORDER_ID,
                    IS_CONFIRM = false,
                    ID = Guid.NewGuid().ToString(),
                    MATERIAL_ORDER_ID = item.MATERIAL_ORDER_ID,
                    MACHINE_SLOT = item.MACHINE_SLOT,
                    MACHINE_ID = item.MACHINE_ID,
                    IS_DM_ACCEPT = true
                };
                var line = context.Tokusai_LineHistorys.Where(m => m.LINE_ID == tokusai.LINE_ID
                && m.PART_ID == tokusai.PART_ID
                && m.PRODUCT_ID == tokusai.PRODUCT_ID
                && m.MACHINE_ID == tokusai.MACHINE_ID
                && m.MACHINE_SLOT == tokusai.MACHINE_SLOT
                && m.IS_CONFIRM == false).FirstOrDefault();
                if (line == null)
                {
                    context.Tokusai_LineHistorys.Add(tokusai);
                    context.SaveChanges();
                }

            }
        }

        public static DateTime GetMaxTokusaiUpdate()
        {
            try
            {
                using (DataContext db = new DataContext())
                {
                    var maxUpdateTime = db.Tokusai_LineHistorys.Where(m => m.IS_CONFIRM == false).Max(m => m.UPD_TIME);
                    return maxUpdateTime;
                }
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }

        }
        public static List<string> GetAllPartLineItem(string LINE_ID, string PRODUCT_ID)
        {
            string sql = $@"SELECT PART_ID FROM [{DB}].[dbo].[MainSub_LineItem] WHERE LINE_ID = '" + LINE_ID + "' AND PRODUCT_ID = '" + PRODUCT_ID + "'";
            using (DataContext context = new DataContext())
            {
                var res = context.Database.SqlQuery<string>(sql).ToList();
                return res.ConvertAll(m => m.ToUpper().Trim()).ToList();
            }
        }
        public static void MainSubSave(MaterialOrderItem item, string partFrom, string partTo)
        {
            using (DataContext db = new DataContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    var tokusai = new Tokusai_LineHistory()
                    {
                        LINE_ID = item.LINE_ID,
                        PART_ID = item.PART_ID,
                        PRODUCT_ID = item.PRODUCT_ID,
                        UPD_TIME = DateTime.Now,
                        CHANGE_NAME = $"MainSub({partFrom} -> {partTo})",
                        CHANGE_ID = 4,
                        WO = item.PRODUCTION_ORDER_ID,
                        IS_CONFIRM = false,
                        ID = Guid.NewGuid().ToString(),
                        MATERIAL_ORDER_ID = item.MATERIAL_ORDER_ID,
                        MACHINE_SLOT = item.MACHINE_SLOT,
                        MACHINE_ID = item.MACHINE_ID,
                        IS_DM_ACCEPT = true
                    };

                    db.Tokusai_LineHistorys.Add(tokusai);
                    db.SaveChanges();
                    string sql = $@"INSERT INTO [{DB}].[dbo].[MainSub_LineItem_backup] 
                                    SELECT* FROM[{DB}].[dbo].[MainSub_LineItem] WHERE LINE_ID = '{item.LINE_ID}' AND PRODUCT_ID = '{item.PRODUCT_ID}' AND PART_ID = '{partFrom}'";
                    db.Database.ExecuteSqlCommand(sql, "");
                    string sql1 = $@"DELETE FROM [{DB}].[dbo].[MainSub_LineItem] WHERE LINE_ID = '{item.LINE_ID}' AND PRODUCT_ID = '{item.PRODUCT_ID}' AND PART_ID = '{partFrom}'";
                    db.Database.ExecuteSqlCommand(sql1, "");
                    transaction.Commit();
                }

            }
        }

        public static string LoadedOrderItem_Update(DataTable dt)
        {
            try
            {
                using (DataContext context = new DataContext())
                {
                    var StaffCodeParam = new SqlParameter("@Data", dt)
                    {
                        TypeName = "dbo.udt_LoadedOrderItem",
                        SqlDbType = SqlDbType.Structured
                    };
                    context.Database
                       .ExecuteSqlCommand("exec LoadedOrderItem_Update @Data",
                       StaffCodeParam);
                    Console.Write("");
                    return "";
                }
            }
            catch (Exception ex)
            {
                log.Error("LoadedOrderItem_Update", ex);
                return ex.Message;
            }

        }

        public static List<VerifyLoadedEntity> FindVerifiedOrderItem()
        {
            using (UmesContext context = new UmesContext())
            {
                var dayShift = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    dayShift = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0);
                }
                var nightShift = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0);
                var firstTimeCheck = new DateTime();
                if (DateTime.Now > dayShift && DateTime.Now < nightShift)
                {
                    firstTimeCheck = dayShift;
                }
                else if (DateTime.Now > nightShift)
                {
                    firstTimeCheck = nightShift;
                }
                else if (DateTime.Now < dayShift)
                {
                    firstTimeCheck = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1, 20, 0, 0);
                }

                var dateParam = new SqlParameter("@Date", firstTimeCheck);
                var res = context.Database.SqlQuery<VerifyLoadedEntity>("FindVerifiedOrderItem @Date", dateParam);
                return res.ToList();
            }
        }

        public static List<VerifyLoadedEntity> GetAllLoadedOrderItem()
        {
            using (var context = new DataContext())
            {
                string sql = $@"SELECT * FROM LoadedOrderItem";
                return context.Database.SqlQuery<VerifyLoadedEntity>(sql, "").ToList();

            }
        }

        public static List<PartQuantityModel> ShowPartQuantity(DataTable dt)
        {
            try
            {
                using (USAPContext context = new USAPContext())
                {
                    var upnParam = new SqlParameter("@Data", dt)
                    {
                        TypeName = "dbo.udt_PartQuantity",
                        SqlDbType = SqlDbType.Structured
                    };
                    var result = context.Database.SqlQuery<PartQuantityModel>("exec ShowPartQuantity1 @Data",
                       upnParam).ToList();
                    Console.Write("");
                    return result;
                }
            }
            catch (Exception ex)
            {
                log.Error("ShowPartInlineQuantity", ex);
                return null;
            }

        }

        public static void UpdateStock(List<PartQuantityModel> result)
        {
            try
            {
                using (COSTSystemContext context = new COSTSystemContext())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("PO_NO", typeof(string));
                    dt.Columns.Add("PO_LINE", typeof(int));
                    dt.Columns.Add("PART_NO", typeof(string));
                    dt.Columns.Add("RV_YEAR", typeof(int));
                    dt.Columns.Add("RV_MONTH", typeof(int));
                    dt.Columns.Add("OS_QTY", typeof(double));
                    dt.Columns.Add("REC_DATE", typeof(DateTime));
                    dt.Columns.Add("TN_NO", typeof(string));
                    dt.Columns.Add("STD_CODE", typeof(string));

                    var groupByPO = result.GroupBy(m => new { m.PO_NO, m.PO_LINE }).Select(n => new
                    {
                        PO_NO = n.Key.PO_NO,
                        PO_LINE = n.Key.PO_LINE,
                        PART_ID = n.LastOrDefault().PART_ID,
                        QUANTITY = n.Sum(m => m.QUANTITY),
                        LIST = n.ToList()
                    });
                    foreach (var item in groupByPO)
                    {
                        if (item.PO_NO == "7100032927" && item.PO_LINE == 4)
                        {
                            Debug.WriteLine("xxxx:", item.PART_ID);
                        }
                        dt.Rows.Add(new object[]
                        {
                        item.PO_NO,
                        item.PO_LINE,
                        item.PART_ID,
                        DateTime.Now.Year,
                        DateTime.Now.Month,
                        item.QUANTITY,
                        item.LIST.Min(m => m.REC_DATE),
                        "",
                        item.LIST[0].STD_CODE
                        });
                    }
                    var dataParam = new SqlParameter("@Data", dt)
                    {
                        TypeName = "dbo.Udt_PO_STOCK1",
                        SqlDbType = SqlDbType.Structured
                    };
                    context.Database
                       .ExecuteSqlCommand("exec POStock_Update3 @Data",
                       dataParam);
                }
            }
            catch (Exception ex)
            {
                log.Error("UpdateStock", ex);
            }


        }

        public static List<string> ShowPartSMT()
        {
            try
            {
                using (USAPContext context = new USAPContext())
                {
                    var result = context.Database.SqlQuery<string>("exec ShowPartSMT").ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                log.Error("ShowPartSMT", ex);
                return null;
            }

        }
        public static List<UPN_STATUS> GetQuantitySMT(List<string> partNos)
        {
            try
            {
                using (UmesContext context = new UmesContext())
                {
                    string sql = $@"SELECT [CURRENT_QUANTITY]
                                  ,[PART_ID]
                                  ,[UPN_ID]
                              FROM [UMC_MESDB_TEST].[dbo].[UPN_STATUS]
                              WHERE  IS_FINISHED = 0 
                                   AND PART_ID IN(";
                    for (int i = 0; i < partNos.Count; i++)
                    {
                        var partNo = partNos[i];
                        sql += $"'{partNo}'";
                        if (i < partNos.Count - 1)
                        {
                            sql += ",";
                        }
                        else
                        {
                            sql += ")";
                        }
                    }
                    return context.Database.SqlQuery<UPN_STATUS>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("GetQuantitySMT", ex);
                return null;
            }
        }
    }
}
