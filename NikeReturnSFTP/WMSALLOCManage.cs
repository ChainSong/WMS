using NikeReturnSFTP.Common;
using Runbow.TWS.Biz;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.MessageContracts.WMS.PreOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NikeReturnSFTP
{
    public class WMSALLOCManage
    {
        /// <summary>
        /// 解析出库单，一个文件一单 根据loadkey区分
        /// </summary>
        /// <param name="txtlists"></param>
        /// <param name="externumber"></param>
        /// <returns></returns>
        public string LFOrderImportByLoadKey(List<string> txtlists, out string externumber)
        {
            externumber = "";
            try
            {
                PreOrderRequest requestPo = new PreOrderRequest();
                List<PreOrder> _preorderLists = new List<PreOrder>();
                List<PreOrderDetail> _preorderDetailLists = new List<PreOrderDetail>();

                if (txtlists[0].TxtSubstring(0, 10) == "WMSSHP" && txtlists[0].TxtSubstring(10, 2) == "O")//LF出库单
                {
                    int linenumber = 1;
                    for (int i = 0; i < txtlists.Count; i++)
                    {
                        if (txtlists[i].TxtSubstring(0, 10) == "WMSSHP")//文档头
                        {
                            continue;
                        }
                        if (txtlists[i].TxtSubstring(0, 5) == "SHPHD" && txtlists[i].TxtSubstring(5, 1) == "A")//订单头
                        {
                            string loadkey = txtlists[i].TxtSubstring(4166, 10);
                            if (string.IsNullOrEmpty(loadkey))
                            {
                                return ReturnTxtError("文档中存在LoadKey为空的订单");
                            }

                            if (_preorderLists.Count() <= 0)//订单头只有一行
                            {
                                #region 订单头
                                PreOrder preorder = new PreOrder();
                                preorder.str9 = loadkey;
                                externumber = loadkey;
                                string plantime = string.IsNullOrEmpty(txtlists[i].TxtSubstring(65, 14)) ? "" :
                                 DateTime.ParseExact(txtlists[i].TxtSubstring(65, 14), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");//计划发货时间 
                                string shiptype = txtlists[i].TxtSubstring(209, 30);//运输方式

                                if (string.IsNullOrEmpty(plantime))
                                {
                                    return ReturnTxtError("文档中计划发货时间为空，无法生成外部单号");
                                }

                                preorder.str7 = plantime;//计划发货时间
                                preorder.str3 = shiptype;//运输方式                               

                                //外部单号
                                preorder.ExternOrderNumber = "LF" + preorder.str7.Replace("-", "") + "-" + preorder.str9;
                                if (!string.IsNullOrEmpty(shiptype))//加上运输方式
                                {
                                    preorder.ExternOrderNumber += "-" + preorder.str3;
                                }

                                preorder.str17 = txtlists[i].TxtSubstring(2093, 10);//LF出库单号,这个在数据库是存在明细str8里面的，插入时清空这个值
                                //preorder.str16 = txtlists[i].TxtSubstring(21, 20);//NIKE单据编码,这个在数据库是存在明细里面的，插入时清空这个值
                                preorder.str20 = txtlists[i].TxtSubstring(21, 20);//NIKE单据编码,这个在数据库是存在明细里面的，插入时清空这个值

                                preorder.OrderTime = DateTime.Now;
                                preorder.str4 = txtlists[i].TxtSubstring(79, 15);//NFS店铺编码
                                string DivisionCode = txtlists[i].TxtSubstring(349, 10);//Division Code
                                if (DivisionCode == "10")
                                {
                                    preorder.str13 = "APP";
                                }
                                else if (DivisionCode == "20" || DivisionCode == "40")
                                {
                                    preorder.str13 = "FTW";
                                }
                                else
                                {
                                    preorder.str13 = "EQP";
                                }
                                preorder.str10 = txtlists[i].TxtSubstring(703, 20);//NIKE PO
                                preorder.str2 = txtlists[i].TxtSubstring(724, 20);//运输时效
                                preorder.str8 = txtlists[i].TxtSubstring(804, 20);//VAS CODE
                                preorder.str11 = txtlists[i].TxtSubstring(907, 30);//PACK SLIP NO
                                preorder.str5 = txtlists[i].TxtSubstring(1705, 45);//公司名
                                preorder.Address = txtlists[i].TxtSubstring(1750, 45) + "&" + txtlists[i].TxtSubstring(1795, 45) + "&" + txtlists[i].TxtSubstring(1840, 45) + "&" + txtlists[i].TxtSubstring(1885, 45);//地址1，2，3，4
                                preorder.City = txtlists[i].TxtSubstring(1930, 45);//城市
                                preorder.Province = txtlists[i].TxtSubstring(1975, 2);//省
                                preorder.str12 = "否";//是否单仓LoadKey

                                preorder.str6 = "";//好像是男女童鞋服配之类的
                                preorder.str14 = "";//RP LI,先判断NIKE PO的第九位，是L时，值为LI,是S时，值为RP,否则取VAS Code
                                if (preorder.str10.Length >= 9)
                                {
                                    if (preorder.str10.Substring(8, 1) == "L")
                                    {
                                        preorder.str14 = "LI";
                                    }
                                    else if (preorder.str10.Substring(8, 1) == "S")
                                    {
                                        preorder.str14 = "RP";
                                    }
                                    else
                                    {
                                        if (preorder.str8.Equals("NIKECN"))//VAS Code
                                        {
                                            preorder.str14 = "";
                                        }
                                        else
                                        {
                                            preorder.str14 = preorder.str8;
                                        }
                                    }
                                }
                                else
                                {
                                    if (preorder.str8.Equals("NIKECN"))//VAS Code
                                    {
                                        preorder.str14 = "";
                                    }
                                    else
                                    {
                                        preorder.str14 = preorder.str8;
                                    }
                                }

                                //CRD逻辑 计划发货时间+运输时效
                                preorder.str15 = (DateTime.Parse(preorder.str7).AddDays(int.Parse(preorder.str2))).ToString("yyyy-MM-dd");
                                try
                                {
                                    if (DateTime.Parse(preorder.str7).AddDays(int.Parse(preorder.str2)) > DateTime.ParseExact(preorder.str10.Substring(0, 8), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture))
                                    {
                                        preorder.str15 = "";//CRD
                                    }
                                    else
                                    {
                                        preorder.str15 = DateTime.ParseExact(preorder.str10.Substring(0, 8), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                                    }
                                }
                                catch (Exception ea)
                                {

                                }

                                //预计卸货时间
                                preorder.str16 = (DateTime.Parse(preorder.str7).AddDays(int.Parse(preorder.str2))).ToString("yyyy-MM-dd");

                                try
                                {
                                    DateTime time = DateTime.Parse(preorder.str10.Substring(0, 8).Insert(4, "-").Insert(7, "-"));
                                    preorder.str16 = preorder.str10.Substring(0, 8).Insert(4, "-").Insert(7, "-");
                                }
                                catch (Exception)
                                { }

                                preorder.str18 = "LF";//标识利丰订单
                                preorder.CustomerID = 103;
                                preorder.CustomerName = "NIKE-Return";
                                preorder.Warehouse = "NIKE-退货仓";
                                preorder.OrderType = "正常出库";
                                preorder.Status = 1;
                                preorder.Creator = "SFTPUser";
                                _preorderLists.Add(preorder);

                                #endregion  
                            }
                            else
                            {
                                if (_preorderLists.Where(m => m.str9 == loadkey).Count() <= 0)
                                {
                                    return ReturnTxtError("文档中存在多个LoadKey的情况");
                                }
                                continue;
                            }
                        }
                        else if (txtlists[i].TxtSubstring(0, 5) == "SHPDT" && txtlists[i].TxtSubstring(5, 1) == "A")//订单明细SKU
                        {
                            string ArticleColorSize = txtlists[i].TxtSubstring(36, 20);//lf传过来的sku，里面包含款色码
                            if (string.IsNullOrEmpty(ArticleColorSize))
                            {
                                return ReturnTxtError("文档信息中SKU（articlesize）为空");
                            }
                            //相同SKU
                            if (_preorderDetailLists.Where(m => m.str16 == ArticleColorSize).Count() > 0)
                            {
                                //同SKU数量累加
                                _preorderDetailLists.Where(m => m.str16 == ArticleColorSize).FirstOrDefault().OriginalQty += txtlists[i].TxtSubstring(538, 10).ObjectToInt32();//数量
                                continue;
                            }
                            else
                            {
                                #region 订单明细

                                PreOrderDetail predetail = new PreOrderDetail();
                                predetail.str16 = ArticleColorSize;//先存着和vas行去比较
                                predetail.str17 = ArticleColorSize.Substring(0, 6).Trim() + '-' + ArticleColorSize.Substring(6, 3).Trim();//article
                                predetail.str18 = ArticleColorSize.Substring(9).Trim();//size
                                predetail.str6 = predetail.str17 + '-' + predetail.str18;//Article+size

                                predetail.str7 = "";//取主表的Division Code
                                predetail.str8 = txtlists[i].TxtSubstring(460, 10);//LF出库单号
                                predetail.str9 = "";//NIKE单据编码
                                predetail.OriginalQty = txtlists[i].TxtSubstring(538, 10).ObjectToInt32();//数量
                                //predetail.LineNumber = txtlists[i].TxtSubstring(548, 5);//LF出库单行号
                                predetail.LineNumber = linenumber.ToString().TxtPadLeftstring(5, '0');


                                predetail.CustomerID = 103;
                                predetail.CustomerName = "NIKE-Return";
                                predetail.Warehouse = "NIKE-退货仓";
                                predetail.WarehouseId = 58;
                                predetail.GoodsType = "A品";
                                _preorderDetailLists.Add(predetail);
                                linenumber++;
                                #endregion
                            }

                        }
                        else if (txtlists[i].TxtSubstring(0, 5) == "ORDRF" && txtlists[i].TxtSubstring(5, 1) == "A")//SKU的VAS信息
                        {
                            continue;
                        }
                    }

                    if (!_preorderLists.Any() || !_preorderDetailLists.Any())
                    {
                        return ReturnTxtError("文档中不存在订单信息");
                    }
                    //验证loadkey在系统是否存在
                    IEnumerable<PreOrder> getpreorders = new PreOrderService().GetWMSPreOrderlistByLoadKey(_preorderLists);
                    if (getpreorders != null && getpreorders.Any())
                    {
                        return ReturnTxtError("文档中的LoadKey在系统中已经存在");
                    }

                    //验证SKU是否存在
                    List<ProductSearch> searprolistfs = new List<ProductSearch>();
                    foreach (var item in _preorderDetailLists)
                    {
                        ProductSearch psfs = new ProductSearch();
                        psfs.Str10 = item.str17;
                        psfs.Str9 = item.str18;
                        psfs.Str8 = "01";
                        searprolistfs.Add(psfs);
                    }
                    IEnumerable<ProductSearch> productListfs = new ProductService().GetSearchProduct(103, searprolistfs, "Acticle");

                    foreach (var item in _preorderDetailLists)
                    {
                        //判断如果通过article+size是否存在信息
                        ProductSearch search = productListfs.Where(m => m.Str10 == item.str17 && m.Str9 == item.str18).FirstOrDefault();
                        if (search == null)
                        {
                            return ReturnTxtError("文档中的article:" + item.str17 + "Size:" + item.str18 + "在系统不存在");
                        }
                        item.SKU = search.SKU;
                        item.GoodsName = search.GoodsName;
                        item.str2 = search.Price;
                        item.str3 = search.SafeLock;
                        item.str4 = search.Hanger;
                        item.str10 = search.GenderAge;

                        item.ExternOrderNumber = _preorderLists.FirstOrDefault().ExternOrderNumber;
                        item.str7 = _preorderLists.FirstOrDefault().str13; //Division Code
                        //item.str9 = _preorderLists.FirstOrDefault().str16;//NIKE单据编码
                        item.str9 = _preorderLists.FirstOrDefault().str20;//NIKE单据编码
                        _preorderLists.FirstOrDefault().str6 = item.str10;////GenderAge
                    }

                    //foreach (var detailitem in _preorderDetailLists)
                    //{
                    //    detailitem.ExternOrderNumber = _preorderLists.FirstOrDefault().ExternOrderNumber;
                    //    detailitem.str7 = _preorderLists.FirstOrDefault().str13; //Division Code
                    //    detailitem.str9 = _preorderLists.FirstOrDefault().str16;//NIKE单据编码
                    //    _preorderLists.FirstOrDefault().str6 = detailitem.str10;////GenderAge

                    //}
                    //把主表不需要的清空，明细不需要的清空
                    _preorderLists.ForEach((item) =>
                    {
                        item.str20 = null;// item.str16 = null;
                        item.str17 = null;
                    });
                    _preorderDetailLists.ForEach((item) =>
                    {
                        item.str10 = null;
                        item.str16 = null;
                        item.str17 = null;
                        item.str18 = null;
                    });
                    requestPo.PreOrderList = _preorderLists;
                    requestPo.PreOd = _preorderDetailLists;
                    var response = new PreOrderService().AddPreOrderAndPreOrderDetail(requestPo, "SFTPService");
                    if (response.IsSuccess)
                    {
                        if (response.Result == null || response.Result.PreOrderList == null || response.Result.PreOrderList.Count() <= 0)
                        {
                            return "订单数据库插入失败，不移动文件！";
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else
                    {
                        return "数据库层报错：" + response.ErrorCode.ToString();
                    }

                }
                else
                {
                    return ReturnTxtError();
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }




        /// <summary>
        /// 格式有误的
        /// </summary>
        /// <returns></returns>
        public string ReturnTxtError(string msg = "")
        {
            return string.IsNullOrEmpty(msg) ? "文档内部格式错误" : "文档内部格式错误:" + msg;
        }



    }
}
