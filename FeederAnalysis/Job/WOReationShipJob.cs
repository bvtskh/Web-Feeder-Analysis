using FeederAnalysis.Business;
using FeederAnalysis.Tokusai;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FeederAnalysis.Job
{
	public class WOReationShipJob : IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			try
			{
				string runApp = @"umcvn:\\vn-file\DX Center\ThanhDX\StartUpECN\.StartUp.exe";
                if (!IsSendMailTime()) return;
                var data = Repository.GetRelationShipWoPCNeedConfirm();
				if(data ==null || data.Count <=0 ) return;
				StringBuilder builder = new StringBuilder();
				string value = "";
				#region Content
				// header
			    value += $@"<!DOCTYPE html>
									<html lang=""en"">
									<head>
										<meta charset=""UTF-8"">
										<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
										
										<title>Cảnh báo nhập WO</title>
										<style>
											table {{
												width: 100%;
												border-collapse: collapse;
											}}

											th, td {{
												border: 1px solid black;
												padding: 8px;
												text-align: left;
											}}

											th {{
												background-color: #646464;
											}}

											tr:nth-child(even) {{
												background-color: #f9f9f9;
											}}

											tr:hover {{
												background-color: #e9e9e9;
											}}
										</style>
									</head>
									<body>
                                        <p>Dear Hạnh san,</p>
							     <p style='margin:0;font-family:arial,,helvetica,sans-serif;line-height:15px;color:Blue;font-size:14px; '>Bạn nhận được email này từ hệ thống thông báo tự động của DX</p>
									<br>
									<a href='{runApp}'>[CLICK HERE TO OPEN SOFTWARE]</a>
										<h3>Danh sách WO cần nhập</h3>
										<table>
											<tr style ='color:#ffffff'>
												<th>ORDER NO</th>
												<th>UPDATE TIME</th>
												<th>ECO NO</th>
												<th>CONFIRM WAITING TIME</th>
											</tr>";
				// data
				foreach (var item in data)
				{

					value += $@"
										<tr>
											<td>{item.ORDER_NO}</td>
											<td>{item.UPD_DATE}</td>
											<td>{item.ECO_NO}</td>
											<td>{(int)(DateTime.Now - item.UPD_DATE).TotalHours}</td>
										</tr>";
				}
				// end
				value += $@"
								</table >																
									<br>
									Admin contact: <a href='mailto:quyetpv@umcvn.com' target='_blank'>quyetpv@umcvn.com</a><br>
									Department: <strong>PI-DX</strong><br>
									Ex: 0936.900.112/ 3143
									</p>
								</ body >

                                </ html > ";

				builder.Append(value);
				#endregion

				if (!string.IsNullOrEmpty(builder.ToString()))
				{
                    EmailHelper.SenMailWORelationship(builder.ToString());
				}
			}
			catch
			{
				// log.Error("Ga Job Error", ex);
			}
			
		}

        private bool IsSendMailTime()
        {
            // Lấy thời gian hiện tại
            DateTime now = DateTime.Now;
			if(now.DayOfWeek == DayOfWeek.Sunday)
			{
				return false;
			}
            // Tạo các đối tượng DateTime cho 8 giờ sáng và 8 giờ tối của ngày hiện tại
            DateTime start = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);  // 8:00 AM
            DateTime end = new DateTime(now.Year, now.Month, now.Day, 20, 0, 0);   // 8:00 PM

            // Kiểm tra xem thời gian hiện tại có nằm trong khoảng từ 8 giờ sáng đến 8 giờ tối hay không
            if (now >= start && now <= end)
            {
                return true;
            }
            return false;
        }
    }
}

