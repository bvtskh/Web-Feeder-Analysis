using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Bet.Util.Extension;
using FeederAnalysis.DAL.GA;
using FeederAnalysis.DAL.LCA;
using FeederAnalysis.Entities;
using FeederAnalysis.Models;

namespace FeederAnalysis.Business
{
    public class MankichiHelper
    {
        private static List<Tbl_Mankichi> GetAll()
        {
            List<StaffEntity> listStaff = new List<StaffEntity>();
           // var listLiquite = new List<PR_ContractLiquite>();
            using (MankichiContext context = new MankichiContext())
            {
                var sql = @"
                            select distinct PR_Staff.StaffCode,PR_Staff.FullName,PR_DeptHistory.DeptCode,PR_Staff.BirthDate, case PR_Staff.Sex when 1 then 'Nam' when 0 then N'Nữ' end as Sex,PR_Staff.EntryDate,MS_Position.PosName, PR_InputDataToManage.Customer,
	                        PR_Staff.MaritalStatus, PR_Staff.EduCode,PR_ContractLiquite.LiquidationDate,PR_Staff.PermanentAdd, PR_Distance.Distance
		                    from PR_Staff	left join PR_InputDataToManage on PR_Staff.StaffCode = PR_InputDataToManage.Staffcode and  PR_InputDataToManage.KindView = 0 
		                    and PR_InputDataToManage.AppliedDate IN ( select MAX(PR_InputDataToManage.AppliedDate) from PR_InputDataToManage where PR_Staff.StaffCode = PR_InputDataToManage.Staffcode)
		                    left join PR_ContractLiquite on PR_ContractLiquite.StaffCode = PR_Staff.StaffCode
		                    left join PR_StaffDistance ON PR_StaffDistance.StaffCode = PR_Staff.Staffcode AND PR_StaffDistance.ApplyDate IN (SELECT MAX(PR_StaffDistance.ApplyDate) FROM PR_StaffDistance WHERE PR_Staff.StaffCode = PR_StaffDistance.StaffCode)
		                    left join PR_Distance ON PR_Distance.DistanceCode = PR_StaffDistance.DistanceCode,
		                    PR_DeptHistory, PR_PosHistory, MS_Position
		                    where PR_Staff.StaffCode=PR_DeptHistory.StaffCode 
		                    and PR_PosHistory.PosCode=MS_Position.PosCode 
		                    and PR_Staff.StaffCode=PR_PosHistory.StaffCode 
		                    and PR_DeptHistory.OrderHistory = (SELECT MAX(OrderHistory) FROM PR_DeptHistory DepHis WHERE DepHis.StaffCode = PR_DeptHistory.StaffCode)
		                    and PR_PosHistory.OrderHistory = (SELECT MAX(OrderHistory) FROM PR_PosHistory DepPos where DepPos.StaffCode=PR_PosHistory.StaffCode)
                            and PR_Staff.StaffCode not like 'UJ%' ";
                listStaff = context.Database.SqlQuery<StaffEntity>(sql, "").OrderByDescending(r => r.StaffCode).ToList();
                // sql = $"SELECT * FROM [GA_UMC].[dbo].[PR_StaffShift] where mYear = '{DateTime.Now.Year}' AND mMonth = '{DateTime.Now.Month}'";
                //listShift = context.Database.SqlQuery<ShiftEntity>(sql, "").ToList();
            }
            var listMankichi = listStaff.Select(r =>
            {
                int code = r.StaffCode.Replace("U", "").ToInt();
                int code1 = 0;
                bool success = int.TryParse(r.StaffCode, out code1);
                Tbl_Mankichi entity = new Tbl_Mankichi();
                //if (success)
                //{
                entity.AltCode = success ? code1.ToString() : r.StaffCode;
                entity.Code = code;
                entity.Birthday = r.Birthdate;
                entity.EnterDate = r.EntryDate;
                entity.Sex = r.Sex;
                entity.Name = r.FullName;
                entity.MaritalStatus = r.MaritalStatus;
                entity.Customer = r.Customer;
                entity.QuitJob = r.LiquidationDate;
                entity.Dept = r.DeptCode;
                entity.EduLevel = r.EduCode;
                entity.Position = r.PosName;
                entity.Address = r.PermanentAdd;
                // var shiftEntity = listShift.FirstOrDefault(t => t.StaffCode == r.StaffCode);
                // entity.CaLV = shiftEntity != null ? shiftEntity.ShiftCode : string.Empty;
                if (r.Distance != null)
                {
                    entity.Distance = (int)r.Distance;
                }
                //}
                return entity;

            }).OrderByDescending(r => r.Code).ToList();
            //var tmp = listMankichi[listMankichi.Count - 1];
            return listMankichi;
        }
        private static List<StaffEntity> GetAllStaff()
        {
            using (MankichiContext context = new MankichiContext())
            {
                return context.Database.SqlQuery<StaffEntity>("exec sp_Get_All_Staff_2").ToList();
            }
        }
        private static void ClearData()
        {
            using (LCAHumanContext context = new LCAHumanContext())
            {
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE [HumanManagement].[dbo].[Tbl_Mankichi]");
                context.SaveChanges();
            }
        }

        public static void SaveLCA()
        {
            InitSqlServer();
            var listOfMankichi = GetAll();
            var dtCloned = listOfMankichi.ToDataTable();
            ClearData();
            SQLHelper.ExecProcedureNonData("sp_Tbl_Mankichi", new { Data = dtCloned, windowUser = "System" });
        }
        public static void SaveStaffShift()
        {
            List<ShiftEntity> listShift = new List<ShiftEntity>();
            using (MankichiContext context = new MankichiContext())
            {
                var sql = $"SELECT * FROM [GA_UMC].[dbo].[PR_StaffShift] where mYear = '{DateTime.Now.Year}' AND mMonth = '{DateTime.Now.Month}'";
                listShift = context.Database.SqlQuery<ShiftEntity>(sql, "").ToList();
            }
            using (LCAHumanContext context = new LCAHumanContext())
            {
                foreach (var item in listShift)
                {
                    var sql = $@"IF EXISTS (SELECT * FROM [OverTime].[dbo].[Tbl_ShiftWorking] WHERE Code = '{item.StaffCode}' AND Year = '{item.mYear}' AND Month = '{item.mMonth}')
                                UPDATE [OverTime].[dbo].[Tbl_ShiftWorking]
                                SET
                                 Day1 = '{item.D1}',Day2 = '{item.D2}',Day3 = '{item.D3}',Day4 = '{item.D4}',Day5 = '{item.D5}',Day6 = '{item.D6}',Day7 = '{item.D7}',Day8 = '{item.D8}',Day9 = '{item.D9}',Day10 = '{item.D10}'
                                ,Day11 = '{item.D11}',Day12 = '{item.D12}',Day13 = '{item.D13}',Day14 = '{item.D14}',Day15 = '{item.D15}',Day16 = '{item.D16}',Day17 = '{item.D17}',Day18 = '{item.D18}',Day19 = '{item.D19}',Day20 = '{item.D20}'
                                ,Day21 = '{item.D21}',Day22 = '{item.D22}',Day23 = '{item.D23}',Day24 = '{item.D24}',Day25 = '{item.D25}',Day26 = '{item.D26}',Day27 = '{item.D27}',Day28 = '{item.D28}',Day29 = '{item.D29}',Day30 = '{item.D30}'
                                ,Day31 = '{item.D31}'
                                WHERE Code = '{item.StaffCode}' AND Year = '{item.mYear}' AND Month = '{item.mMonth}'
                                ELSE
                                INSERT INTO [OverTime].[dbo].[Tbl_ShiftWorking] VALUES('{item.StaffCode}','{item.mYear}','{item.mMonth}'
                                ,'{item.D1}','{item.D2}','{item.D3}','{item.D4}','{item.D5}','{item.D6}','{item.D7}','{item.D8}','{item.D9}','{item.D10}'
                                ,'{item.D11}','{item.D12}','{item.D13}','{item.D14}','{item.D15}','{item.D16}','{item.D17}','{item.D18}','{item.D19}','{item.D20}'
                                ,'{item.D21}','{item.D22}','{item.D23}','{item.D24}','{item.D25}','{item.D26}','{item.D27}','{item.D28}','{item.D29}','{item.D30}'
                                ,'{item.D31}','')";
                    context.Database.ExecuteSqlCommand(sql);
                    context.SaveChanges();
                }
            }
        }
        public static void SavePIBase()
        {
            var lstStaff = new List<StaffEntity>();
            using (MankichiContext context = new MankichiContext())
            {
                lstStaff = context.Database.SqlQuery<StaffEntity>("sp_Get_All_Staff_2").ToList();
            }
            using (PIContext context = new PIContext())
            {
                foreach (var staff in lstStaff.Where(r => !r.PosName.Contains("Operator")))
                {
                    string sql = $@"IF EXISTS(SELECT *  FROM [PI_BASE].[dbo].[Users] where Code = '{staff.StaffCode}')
                              UPDATE[PI_BASE].[dbo].[Users] SET FullName = N'{staff.FullName}', Dept = '{staff.DeptCode}' WHERE Code = '{staff.StaffCode}'
                              ELSE
                              INSERT INTO[PI_BASE].[dbo].[Users](Code, FullName, Dept, Password) VALUES('{staff.StaffCode}', N'{staff.FullName}', '{staff.DeptCode}', 'umcvn')";

                    context.Database.ExecuteSqlCommand(sql);
                }
            }
        }
        public static void SaveUserForm()
        {
            using (DataContext context = new DataContext())
            {
                foreach (var item in GetAllStaff().Where(r => r.PosName != "Operator" && !r.StaffCode.StartsWith("U")))
                {
                    string sql = $"UPDATE [UMC_FORM].[dbo].[Form_User] SET NAME = N'{item.FullName}' WHERE STAFFCODE = '{item.StaffCode}'";
                    context.Database.ExecuteSqlCommand(sql);
                    context.SaveChanges();
                }

            }
        }
        private static void InitSqlServer()
        {
            SQLHelper.SERVER_NAME = "172.28.6.96";
            SQLHelper.USERNAME_DB = "sa";
            SQLHelper.PASSWORD_DB = "umc@123";
            SQLHelper.DATABASE = "HumanManagement";
            SQLHelper.ConnectString();
        }
    }
}
