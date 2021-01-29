using NikeReturnSFTP.Common;
using Runbow.TWS.Biz;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NikeReturnSFTP
{
    /// <summary>
    /// 生成文件
    /// </summary>
    public class BuildFile
    {
        /// <summary>
        /// 取订单包装信息。一个loadkey下的多个订单组成一个文件
        /// </summary>
        public void BuildPack()
        {
            try
            {
                //查询已出库的包装信息
                GetOrderByConditionResponse response = new GetOrderByConditionResponse();
                response = new OrderManagementService().GetReturnSFTPPackage(0);
                if (response != null && response.OrderCollection != null && response.OrderCollection.Any() && response.packages != null && response.packages.Any() && response.packageDetails != null && response.packageDetails.Any())
                {
                    //List<WMS_NikeReturnSFTP_Log> logs = new List<WMS_NikeReturnSFTP_Log>();

                    //一个loadkey为一个文件
                    List<string> loadkeylist = response.OrderCollection.Select(m => m.str9).Distinct().ToList();
                    foreach (var loadkeyitem in loadkeylist)
                    {
                        WMS_NikeReturnSFTP_Log log = new WMS_NikeReturnSFTP_Log();
                        log.Type = "WMSPACK";
                        log.Str1 = loadkeyitem;
                        string msg = "";
                        try
                        {
                            List<string> wmspacktxtlist = new List<string>();
                            //文档头
                            string WMSPAC = "";
                            WMSPAC += "WMSPAC".TxtPadRightstring(10) + "I".TxtPadRightstring(2) + DateTime.Now.ToString("yyyyMMddHHmmss") + "NIKECN".TxtPadRightstring(20) +
                                "CN".TxtPadRightstring(5) + "Pack Inbound".TxtPadRightstring(30) + "".TxtPadRightstring(20);
                            wmspacktxtlist.Add(WMSPAC);
                            //得到loadkey下的订单
                            IEnumerable<OrderInfo> orders = response.OrderCollection.Where(m => m.str9 == loadkeyitem).ToList();
                            foreach (var orderitem in orders)
                            {
                                string PACHD = "";//订单头
                                PACHD += "PACHDA" + orderitem.str11.TxtPadRightstring(10) + "NIKECN".TxtPadRightstring(20) + "".TxtPadRightstring(10) + orderitem.str9.TxtPadRightstring(10)
                                    + "".TxtPadRightstring(18) + "".TxtPadRightstring(15) + "0" + "".TxtPadRightstring(110);
                                //订单的包装信息
                                IEnumerable<PackageInfo> packages = response.packages.Where(m => m.OID == orderitem.ID).ToList();
                                if (packages == null || !packages.Any())
                                {
                                    msg = "LoadKey：" + loadkeyitem + " 对应的订单：" + orderitem.ExternOrderNumber + " 没有包装信息";
                                    break;
                                }
                                //计算总重量,体积
                                decimal TotCtnWeight = 0;
                                decimal TotCtnCube = 0;
                                packages.ToList().ForEach((m) =>
                                {
                                    TotCtnWeight += m.GrossWeight.ObjectToDecimal();
                                    TotCtnCube += m.NetWeight.ObjectToDecimal();
                                });
                                PACHD += TotCtnWeight.ToString().TxtPadRightstring(16) + TotCtnCube.ToString().TxtPadRightstring(16) + "NIKECN".TxtPadRightstring(10) + "".TxtPadRightstring(290);
                                wmspacktxtlist.Add(PACHD);

                                #region 包装信息
                                int packindex = 0;//包装箱号索引
                                foreach (var packitem in packages)
                                {
                                    //得到明细
                                    IEnumerable<PackageDetailInfo> packageDetails = response.packageDetails.Where(m => m.PID == packitem.ID).ToList();
                                    if (packageDetails == null || !packageDetails.Any())
                                    {
                                        msg = "LoadKey：" + loadkeyitem + " 对应的订单：" + orderitem.ExternOrderNumber + " 没有包装明细信息";
                                        break;
                                    }
                                    packindex++;
                                    //包装头信息
                                    string PACIF = "";
                                    PACIF += "PACIFA" + orderitem.str11.TxtPadRightstring(10) + packindex.ToString().TxtPadRightstring(10) + packitem.GrossWeight.ToString().TxtPadRightstring(16) +
                                        packitem.NetWeight.ToString().TxtPadRightstring(16) + "".TxtPadRightstring(20) + packindex.ToString().TxtPadRightstring(40);
                                    PACIF += packitem.Length.TxtPadRightstring(16) + packitem.Width.TxtPadRightstring(16) + packitem.Height.TxtPadRightstring(16) + "".TxtPadRightstring(170);
                                    wmspacktxtlist.Add(PACIF);
                                    foreach (var packdetailitem in packageDetails)
                                    {
                                        string PACDT = "";//包装明细
                                        PACDT += "PACDTA" + orderitem.str11.TxtPadRightstring(10) + packindex.ToString().TxtPadRightstring(10) + packindex.ToString().TxtPadRightstring(20)
                                            + packindex.ToString().TxtPadLeftstring(5, '0') + "NIKECN".TxtPadRightstring(15);
                                        string sku = packdetailitem.str10.Replace("-", "").ToString() + packdetailitem.str9.ToString();
                                        PACDT += sku.TxtPadRightstring(20) + packdetailitem.Qty.ObjectToNullableInt32().ToString().TxtPadRightstring(10) + "runbow".TxtPadRightstring(20) +
                                            "".TxtPadRightstring(10) + "".TxtPadRightstring(50) + packindex.ToString().TxtPadRightstring(30) + "".TxtPadRightstring(150);
                                        wmspacktxtlist.Add(PACDT);
                                    }

                                }

                                #endregion
                                if (!string.IsNullOrEmpty(msg))
                                {
                                    break;
                                }
                            }
                            if (!string.IsNullOrEmpty(msg))
                            {
                                log.Flag = "N";
                                log.ResultDesc = "生成包装信息反馈文件失败：" + msg;
                                new LogOperationService().AddNikeReturnSFTPLog(log);
                                //下一个loadkey
                                continue;
                            }
                            //loadkey级别生成完毕
                            string PACTR = "";
                            PACTR += "PACTR" + (wmspacktxtlist.Count - 1).ToString().TxtPadLeftstring(10, '0');
                            wmspacktxtlist.Add(PACTR);
                            //生成文件到发送文件夹
                            string filepath = SFTPConstants.SendFilePath + @"\" + "WMSPACK_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                            TextHelper.WriteListToTextFile(wmspacktxtlist, filepath);
                            log.Flag = "Y";
                            log.ResultDesc = "生成包装反馈文件成功";
                            log.ToFileName = filepath;
                            //更新订单反馈状态
                            string ids = "";
                            orders.ToList().ForEach((o) =>
                            {
                                ids += o.ID + ",";
                            });
                            ids = ids.Substring(0, ids.Length - 1);
                            new OrderManagementService().UpdateReturnSFTPOrderFlag(ids, 1);
                            new LogOperationService().AddNikeReturnSFTPLog(log);
                        }
                        catch (Exception el)
                        {
                            //这个loadkey报错了不影响其他loadkey发送
                            log.Flag = "N";
                            log.ResultDesc = "生成包装信息反馈文件失败：" + el.Message.ToString();
                            new LogOperationService().AddNikeReturnSFTPLog(log);
                        }
                    }
                }
                else
                {
                    //没有待回传信息
                }
            }
            catch (Exception ex)
            {
                WMS_NikeReturnSFTP_Log log = new WMS_NikeReturnSFTP_Log()
                {
                    Type = "WMSPACK",
                    Flag = "N",
                    ResultDesc = "生成包装信息反馈文件失败：" + ex.Message.ToString()
                };
                new LogOperationService().AddNikeReturnSFTPLog(log);
            }


        }


        /// <summary>
        /// 取订单信息和包装信息，一个loadkey对应一个文件
        /// </summary>
        public void BuildWMSPAC()
        {
            try
            {
                //查询已出库的包装信息
                GetOrderByConditionResponse response = new GetOrderByConditionResponse();
                response = new OrderManagementService().GetReturnSFTPPackage(0);
                if (response != null && response.OrderCollection != null && response.OrderCollection.Any() && response.packages != null && response.packages.Any() && response.packageDetails != null && response.packageDetails.Any())
                {
                    //循环订单
                    foreach (var orderitem in response.OrderCollection)
                    {
                        WMS_NikeReturnSFTP_Log log = new WMS_NikeReturnSFTP_Log();
                        log.Type = "WMSPACK";
                        log.Str1 = orderitem.str9;
                        string msg = "";
                        string timeStr = "";                       
                        try
                        {
                            if (string.IsNullOrEmpty(orderitem.str9))
                            {
                                log.Flag = "N";
                                log.ResultDesc = "生成包装信息反馈文件失败，订单中的loadkey是空值";
                                new LogOperationService().AddNikeReturnSFTPLog(log);
                                continue;
                            }
                            List<string> wmspacktxtlist = new List<string>();
                            //文档头                            
                            
                            timeStr = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                            string WMSPAC = "";
                            WMSPAC += "WMSPAC".TxtPadRightstring(10) + "I".TxtPadRightstring(2) + timeStr.ToString() + "NIKECN".TxtPadRightstring(20) +
                                "CN".TxtPadRightstring(5) + "Pack Inbound".TxtPadRightstring(30) + "".TxtPadRightstring(20);
                            wmspacktxtlist.Add(WMSPAC);


                            string PACHD = "";//订单头
                            PACHD += "PACHDA" + orderitem.str11.TxtPadRightstring(10) + "NIKECN".TxtPadRightstring(20) + "".TxtPadRightstring(10) + orderitem.str9.TxtPadRightstring(10)
                                + "".TxtPadRightstring(18) + "".TxtPadRightstring(15) + "0" + "".TxtPadRightstring(110);
                            //订单的包装信息
                            IEnumerable<PackageInfo> packages = response.packages.Where(m => m.OID == orderitem.ID).ToList();
                            if (packages == null || !packages.Any())
                            {
                                msg = "LoadKey：" + orderitem.str9 + " 对应的订单：" + orderitem.ExternOrderNumber + " 没有包装信息";
                                continue;
                            }

                            //计算总重量,体积
                            decimal TotCtnWeight = 0;
                            decimal TotCtnCube = 0;
                            packages.ToList().ForEach((m) =>
                            {
                                TotCtnWeight += m.GrossWeight.ObjectToDecimal();
                                TotCtnCube += m.NetWeight.ObjectToDecimal();
                            });
                            PACHD += TotCtnWeight.ToString().TxtPadRightstring(16) + TotCtnCube.ToString().TxtPadRightstring(16) + "NIKECN".TxtPadRightstring(10) + "".TxtPadRightstring(290);
                            wmspacktxtlist.Add(PACHD);

                            #region 包装信息
                            int packindex = 0;//包装箱号索引
                            foreach (var packitem in packages)
                            {
                                //得到明细
                                IEnumerable<PackageDetailInfo> packageDetails = response.packageDetails.Where(m => m.PID == packitem.ID).ToList();
                                if (packageDetails == null || !packageDetails.Any())
                                {
                                    msg = "LoadKey：" + orderitem.str9 + " 对应的订单：" + orderitem.ExternOrderNumber + " 没有包装明细信息";
                                    break;
                                }
                                packindex++;
                                //包装头信息
                                string PACIF = "";
                                PACIF += "PACIFA" + orderitem.str11.TxtPadRightstring(10) + packindex.ToString().TxtPadRightstring(10) + packitem.GrossWeight.ToString().TxtPadRightstring(16) +
                                    packitem.NetWeight.ToString().TxtPadRightstring(16) + "".TxtPadRightstring(20) + packindex.ToString().TxtPadRightstring(40);
                                PACIF += packitem.Length.TxtPadRightstring(16) + packitem.Width.TxtPadRightstring(16) + packitem.Height.TxtPadRightstring(16) + "".TxtPadRightstring(170);
                                wmspacktxtlist.Add(PACIF);
                                int packdetailindex = 0;//箱明细索引
                                foreach (var packdetailitem in packageDetails)
                                {
                                    packdetailindex++;
                                    string PACDT = "";//包装明细
                                    PACDT += "PACDTA" + orderitem.str11.TxtPadRightstring(10) + packindex.ToString().TxtPadRightstring(10) + packindex.ToString().TxtPadRightstring(20)
                                        + packdetailindex.ToString().TxtPadLeftstring(5, '0') + "NIKECN".TxtPadRightstring(15);
                                    string sku = packdetailitem.str10.Replace("-", "").ToString() + packdetailitem.str9.ToString();
                                    PACDT += sku.TxtPadRightstring(20) + packdetailitem.Qty.ObjectToNullableInt32().ToString().TxtPadRightstring(10) + "RBOW".TxtPadRightstring(20) +
                                        "".TxtPadRightstring(10) + "".TxtPadRightstring(50) + packindex.ToString().TxtPadRightstring(30) + "EA".TxtPadRightstring(50) + "".TxtPadRightstring(100);
                                    //"".TxtPadRightstring(150);
                                    wmspacktxtlist.Add(PACDT);
                                }
                            }
                            #endregion
                            if (!string.IsNullOrEmpty(msg))
                            {
                                log.Flag = "N";
                                log.ResultDesc = "生成包装信息反馈文件失败：" + msg;
                                new LogOperationService().AddNikeReturnSFTPLog(log);
                                //下一单
                                continue;
                            }

                            //loadkey级别生成完毕，生成文档尾
                            string PACTR = "";
                            PACTR += "PACTR" + (wmspacktxtlist.Count - 1).ToString().TxtPadLeftstring(10, '0');
                            wmspacktxtlist.Add(PACTR);
                            //生成文件到发送文件夹
                            string filepath = SFTPConstants.SendFilePath + @"\" + "WMSPACK_" + timeStr.ToString() + ".txt";
                            TextHelper.WriteListToTextFile(wmspacktxtlist, filepath);
                            log.Flag = "Y";
                            log.ResultDesc = "生成包装反馈文件成功";
                            log.ToFileName = filepath;
                            //更新订单反馈状态
                            string ids = orderitem.ID.ToString();

                            new OrderManagementService().UpdateReturnSFTPOrderFlag(ids, 1);
                            new LogOperationService().AddNikeReturnSFTPLog(log);

                        }
                        catch (Exception el)
                        {
                            log.ResultDesc = "生成包装信息反馈文件失败：" + el.Message.ToString();
                            new LogOperationService().AddNikeReturnSFTPLog(log);
                            continue;
                        }

                    }


                }
                else
                {
                    //没有待回传信息
                }
            }
            catch (Exception ex)
            {
                WMS_NikeReturnSFTP_Log log = new WMS_NikeReturnSFTP_Log()
                {
                    Type = "WMSPACK",
                    Flag = "N",
                    ResultDesc = "生成包装信息反馈文件失败：" + ex.Message.ToString()
                };
                new LogOperationService().AddNikeReturnSFTPLog(log);
            }

        }

    }
}
