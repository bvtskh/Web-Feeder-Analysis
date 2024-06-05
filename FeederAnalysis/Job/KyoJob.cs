using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using Common.Logging;
using FeederAnalysis.DAL.LCA;
using Quartz;

namespace FeederAnalysis.Business
{
    public class KyoJob : IJob
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var data = Repository.GetDefectWarning();
                StringBuilder builder = new StringBuilder();
                #region Content
                foreach (var item in data)
                {
                    var value = $@"<tr style='border-collapse:collapse'>
										<td align='left' style='padding:0;margin:0;width:120px'>
											<p style='margin:0;font-family:arial,helvetica,sans-serif;line-height:18px;color:#333333;font-size:12px'> 
												<strong>{item.BOARD_NO}</strong>		                                                                            
											</p>
										</td>
										<td align='left' style='padding:0;margin:0;width:120px'>
											<p style='margin:0;font-family:arial,helvetica,sans-serif;line-height:18px;color:#333333;font-size:12px'> 
												<strong>{item.PRODUCT_ID}</strong>		
											</p>
										</td>
										<td align='left' style='padding:0;margin:0;width:120px'>
											<span style='margin:0;font-family:arial,helvetica,sans-serif;line-height:18px;color:#696969;font-size:12px'>
												<strong>{item.DEFECT_CODE}</strong>
											</span>
										</td>
										<td align='left' style='padding:0;margin:0;width:80px'>
											<span style='margin:0;font-family:arial,helvetica,sans-serif;line-height:18px;color:#696969;font-size:12px'>
												<strong>{item.LOCATION_CODE}</strong>
											</span>
										</td>
										<td align='left' style='padding:0;margin:0;width:190px'>
											<span style='margin:0;font-family:arial,helvetica,sans-serif;line-height:18px;color:#696969;font-size:12px'>
												<strong>{item.UPDATE_TIME}</strong>
											</span>
										</td>
							    </tr>";
                    builder.Append(value);
                }
                #endregion
                #region Body mail
                string body = $@"<div style='background-color:#f6f6f6'>
<table width='100%' cellspacing='0' cellpadding='0' style='border-collapse:collapse;border-spacing:0px;padding:0;margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top'>
	<tbody>
		<tr style='border-collapse:collapse'>
			<td valign='top' style='padding:0;margin:0'>
				<table cellspacing='0' cellpadding='0' align='center' style='border-collapse:collapse;border-spacing:0px;table-layout:fixed!important;width:100%'>
					<tbody>
						<tr style='border-collapse:collapse'>
							<td align='center' style='padding:0;margin:0'>
								<table cellspacing='0' cellpadding='0' bgcolor='#ffffff' align='center' style='border-collapse:collapse;border-spacing:0px;background-color:#ffffff;width:600px'>
									<tbody>
										<tr style='border-collapse:collapse'>
											<td align='left' bgcolor='#f6f6f6' style='padding:0;margin:0;padding-top:20px;padding-left:20px;background-color:#f6f6f6'>                           
											</td>
										</tr>
										<tr style='border-collapse:collapse'>
											<td align='left' style='padding:0;margin:0;padding-top:10px;padding-left:20px;padding-right:20px'>
												<table cellpadding='0' cellspacing='0' width='100%' role='presentation' style='border-collapse:collapse;border-spacing:0px'>
													<tbody>
														<tr style='border-collapse:collapse'>
															<td align='left' style='padding:0;margin:0;padding-bottom:5px'>
																<h3 style='margin:0;line-height:21px;font-family:arial,,helvetica,sans-serif;font-size:14px;font-style:normal;font-weight:normal;color:#d34127'><strong>Dear Kyocera Team,</strong>
																</h3>
															</td>
														</tr>
														<tr style='border-collapse:collapse'>
															<td align='center' bgcolor='#fff1f0' style='padding:10px;margin:0'>
																<h3 style='margin:0;line-height:21px;font-family:arial,,helvetica,sans-serif;font-size:14px;font-style:normal;font-weight:normal;color:#333333;text-align:left'><strong>Nội dung thông báo:</strong>
																</h3>
																<p style='margin:0;font-family:arial,helvetica,sans-serif;line-height:21px;color:#333333;font-size:13px'>
																	<strong>“Dữ liệu cảnh báo lỗi.”<br></strong>
																	<em>-&nbsp;IT System&nbsp;-</em>
																</p>
															</td>
														</tr>
														<tr>
																<td align='left' style='margin:0;padding-top:10px;padding-bottom:10px;'>
																	<h1 style='margin:0;line-height:auto;font-family:arial,helvetica,sans-serif;font-size:24px;font-style:normal;font-weight:normal;text-align:center;background:#d34127;color:#ffffff; padding-top: 10px;padding-bottom: 10px;'>
                                                                        <strong>Cảnh báo lỗi ICT(Umes) quá 3 lần tại 1 vị trí</strong>
																	</h1>
																</td>
															</tr>
														<tr style='border-collapse:collapse'>
															<td align='center' style='padding:0;margin:0;padding-bottom:10px;font-size:0'>
																<table border='0' width='100%' height='100%' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px'>
																	<tbody>
																		<tr style='border-collapse:collapse'>
																			<td style='padding:0;border-bottom:1px solid #cccccc;background:none;height:1px;width:100%;margin:0'></td>
																		</tr>
																	</tbody>
																</table>
															</td>
														</tr>
														<tr style='border-collapse:collapse'>
															<td align='left' style='padding:0;margin:0;padding-bottom:5px;padding-top:10px'>
																<h3 style='margin:0;line-height:21px;font-family:arial,helvetica,sans-serif;font-size:14px;font-style:normal;font-weight:normal;color:#333333'><strong>Danh sách lỗi mới phát sinh:</strong>
																</h3>
															</td>
														</tr>
													</tbody>
												</table>
											</td>
										</tr>
										<tr>
											<td align='left' style='margin:0;padding-top:5px;padding-bottom:5px;padding-left:20px;padding-right:20px'>
												<table cellpadding='0' cellspacing='0' style='border-collapse:collapse;border-spacing:0px;width:560px;background-color:#efefef'>
													<tbody>
														<tr>
															<td align='left' bgcolor='#efefef' style='margin:0;padding-top:5px;padding-bottom:5px;padding-left:10px;padding-right:30px;background-color:#efefef'>
																<strong style='margin:0;font-family:arial,helvetica,sans-serif;line-height:21px;color:#d34127;font-size:14px'>Serial</strong>
															</td>
															<td>
																<p style='padding-left:0;padding-right:20px;margin:0;font-family:arial,,helvetica,sans-serif;line-height:21px;color:#d34127;font-size:14px'>
																	<strong>Model</strong>                                                         
																</p>
															</td>
															<td>
																<p style='padding-left:0;padding-right:5px;margin:0;font-family:arial,,helvetica,sans-serif;line-height:21px;color:#d34127;font-size:14px'>
																	<strong>Defect Code</strong>                                                         
																</p>
															</td>
															<td>
																<p style='padding-left:0;padding-right:20px;margin:0;font-family:arial,,helvetica,sans-serif;line-height:21px;color:#d34127;font-size:14px'>
																	<strong>Location</strong>                                                         
																</p>
															</td>
															<td>
																<p style='padding-left:0;padding-right:20px;margin:0;font-family:arial,,helvetica,sans-serif;line-height:21px;color:#d34127;font-size:14px'>
																	<strong>Time</strong>                                                         
																</p>
															</td>
														</tr>{builder.ToString()}
													</tbody>
												</table>
												<hr style='border-top:1px solid #cccccc;background:#cccccc;width:100%;margin-top:10px'>											
											</tr>
											<tr style='border-collapse:collapse'>
												<td bgcolor='#f6f6f6' align='left' style='margin:0;padding-top:5px;padding-bottom:15px;padding-left:20px;padding-right:20px'>
													<table cellpadding='0' cellspacing='0' style='border-collapse:collapse;border-spacing:0px;width:560px'>
														<tbody>
															<tr>
																<td align='left' style='padding:0;margin:0;width:174px'>
																	<p style='margin:0;font-family:arial,,helvetica,sans-serif;line-height:15px;color:#333333;font-size:10px'>   
																		<strong>UMC Electronic Viet Nam Ltd.</strong><br>
																		<a href='http://umc.com.vn'>https://umc.com.vn</a><br>
																		Tan Truong IZ, Cam Giang, Hai Duong.<br>
																		Telephone: +84 2203570001 - Email: <a href='mailto:quyetpv@umcvn.vn' target='_blank'>quyetpv@umcvn.com</a> 
																	 </p>
																	</td>
																	<td align='center' style='padding:0;margin:0;width:173px'>
																		<table>
																			<tbody>
																				<tr>
																					<td colspan='2' align='right'>
																						<p style='margin:0;font-family:arial,,helvetica,sans-serif;line-height:15px;color:#333333;font-size:10px'>Bạn nhận được email này từ hệ thống thông báo tự động.<br>
																							Admin contact: <a href='mailto:quyetpv@umcvn.com' target='_blank'>quyetpv@umcvn.com</a><br>
																							Department: <strong>PI-IT</strong><br>
																							Ex: 0972.089.889/ 3143
																						</p>
																						</td>
																					</tr>
																				</tbody>
																			</table>
																		</td>
																	</tr>
																</tbody>
															</table>
														</td>
													</tr>
												</tbody>
											</table>
										</td>
									</tr>
								</tbody>
							</table>
						</td>
					</tr>
				</tbody>
			</table>
		</div>
	";
                #endregion
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    EmailHelper.SenMailOutlook(body);
                }
            }
            catch
            {
                // log.Error("Ga Job Error", ex);
            }
        }
    }
}
