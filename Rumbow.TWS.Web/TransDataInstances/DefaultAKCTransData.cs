using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.TransDataInstances
{
    public class DefaultAKCTransData : BaseTransData
    {
        public DefaultAKCTransData(string TransDataType, long CustomerID, long ProjectID, long WareHouseID,
              DataSet Transdata)
               : base(TransDataType, CustomerID, ProjectID, WareHouseID, Transdata)
        {
        }

        public override void CustomerDefinedSettledTransData(ref string message)
        {
            try
            {
                if (TransDataType == "PreOrder")
                {
                    DataTable dt = Transdata.Tables["预出库单明细信息$"].Copy();
                    dt.Columns.Add("产品名称");
                    List<ProductSearch> productListS = new List<ProductSearch>();
                    IEnumerable<ProductSearch> productList;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ProductSearch ps = new ProductSearch();
                        ps.SKU = dt.Rows[i]["SKU"].ToString();
                        productListS.Add(ps);
                    }
                    productList = ApplicationConfigHelper.GetSearchProduct(CustomerID, productListS, "SKU");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (productList.Where(c => c.SKU == dt.Rows[i]["SKU"].ToString()).ToArray().Length <= 0)
                        {
                            message = dt.Rows[i]["SKU"].ToString() + "不存在!!";
                            return;
                        }
                        dt.Rows[i]["产品名称"] = productList.Where(c => c.SKU == dt.Rows[i]["SKU"].ToString()).ToArray().Length > 0 ? productList.Where(c => c.SKU == dt.Rows[i]["SKU"].ToString()).ToArray()[0].GoodsName.ToString() : "";

                    }
                    DataTable dtpo = Transdata.Tables["预出库单主信息$"].Copy();
                    DataSet ds = new DataSet();

                    //验证一下快进快出的订单是否填写了其他单号（入库单号）
                    foreach (DataRow item in dtpo.Rows)
                    {
                        string exterNo = item["外部单号"].ToString().Trim();
                        if (string.IsNullOrEmpty(exterNo))
                        {
                            message = "预出库单主信息中的外部单号不能为空，请检查！";
                            return;
                        }
                        string orderType = item["预出库单类型"].ToString().Trim();
                        string otherNo = item["其它单号"].ToString().Trim();
                        if (orderType == "快进快出")
                        {
                            if (string.IsNullOrEmpty(otherNo))
                            {
                                message = "<p><font color='#FF0000'>外部单号：" + exterNo + "属于快进快出订单，其它单号不能为空！</font></p>";
                                return;
                            }
                            //验证一下快进快出订单的其它单号在系统是否存在
                            var asnexter = new ASNManagementService().ExternKeyCheck(otherNo, "1", CustomerID);
                            if (asnexter <= 0)
                            {
                                message = "外部单号：" + exterNo + "中的其它单号在系统不存在，请检查！";
                                return;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(otherNo))
                            {
                                message = "<p><font color='#FF0000'>外部单号：" + exterNo + "不是快进快出类型，请勿填写其它单号！</font></p>";
                                return;
                            }
                        }
                        //验证一下重复sku
                        DataRow[] dtrow = dt.Select("外部单号='" + exterNo + "'");//获取明细
                        if (dtrow.Count() <= 0)
                        {
                            message = "<p><font color='#FF0000'>外部单号：" + exterNo + "没有明细信息，请检查！</font></p>";
                            return;
                        }
                        List<PreOrderDetail> predetail = new List<PreOrderDetail>();
                        foreach (DataRow row in dtrow)
                        {
                            PreOrderDetail detail = new PreOrderDetail();
                            detail.SKU = row["SKU"].ToString();
                            predetail.Add(detail);
                        }
                        var validataSKU = predetail.GroupBy(m => new { m.SKU }).Select(m => new { SKU = m.Key.SKU, count = m.Count() }).ToList();
                        var data = validataSKU.Where(m => m.count > 1);
                        if (data.Count() > 0)
                        {
                            message = "<p><font color='#FF0000'>外部单号：" + exterNo + "中的SKU:" + data.FirstOrDefault().SKU + "存在重复值，请检查！</font></p>";
                            return;
                        }
                    }

                    ds.Tables.Add(dtpo);
                    ds.Tables.Add(dt);

                    this.AfterData = ds;
                }
                else if (TransDataType == "Receiving")
                {
                    DataTable dt = Transdata.Tables[0].Copy();
                    if (!dt.Columns.Contains("生产日期"))
                    {
                        dt.Columns.Add("生产日期");
                    }
                    if (!dt.Columns.Contains("箱内总数"))
                    {
                        dt.Columns.Add("箱内总数");
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        try
                        {
                            Convert.ToDateTime(dt.Rows[i]["生产日期"].ToString());
                        }
                        catch
                        {
                            dt.Rows[i]["生产日期"] = System.DateTime.Now.ToString();
                        }
                    }
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    this.AfterData = ds;
                }
                else if (TransDataType == "Asn")
                {
                    try
                    {

                        foreach (DataRow item in Transdata.Tables["预入库单主信息$"].Rows)
                        {
                            string exterNo = item["外部入库单号"].ToString();
                            string asnType = item["预入库单类型"].ToString();
                            DataRow[] dtrow = Transdata.Tables["预入库单明细信息$"].Select("外部入库单号='" + exterNo + "'");//获取这一单的明细信息
                            if (dtrow.Count() <= 0)
                            {
                                message = "<p><font color='#FF0000'>外部入库单号：" + exterNo + "没有明细信息，请检查！</font></p>";
                                return;
                            }
                            if (asnType == "快进快出")
                            {
                                List<ASNDetail> asnDetail = new List<ASNDetail>();
                                foreach (DataRow row in dtrow)
                                {
                                    ASNDetail detail = new ASNDetail();
                                    //detail.ExternReceiptNumber = row["外部入库单号"].ToString();
                                    detail.SKU = row["SKU"].ToString();
                                    detail.BoxNumber = row["托号"].ToString();
                                    asnDetail.Add(detail);
                                }
                                //验证箱号是不是都有值
                                if (asnDetail.Where(m => m.BoxNumber == "" || m.BoxNumber == null).Count() > 0)
                                {
                                    message = "<p><font color='#FF0000'>外部入库单号:" + exterNo + "类型为快进快出，箱号不能为空，请检查！</font></p>";
                                    return;
                                }

                                var validataBox = asnDetail.GroupBy(m => new { m.SKU, m.BoxNumber }).Select(m => new { SKU = m.Key.SKU, BoxNumber = m.Key.BoxNumber, count = m.Count() });
                                var data = validataBox.Where(m => m.count > 1);
                                if (data.Count() > 0)
                                {
                                    message = "<p><font color='#FF0000'>外部入库单号:" + exterNo + "中的SKU:" + data.FirstOrDefault().SKU + "和箱号:" + data.FirstOrDefault().BoxNumber + "存在重复值，请检查！</font></p>";
                                    return;
                                }
                            }
                            else
                            {
                                List<ASNDetail> asnDetail = new List<ASNDetail>();
                                foreach (DataRow row in dtrow)
                                {
                                    ASNDetail detail = new ASNDetail();
                                    detail.SKU = row["SKU"].ToString();
                                    asnDetail.Add(detail);
                                }
                                var validataBox = asnDetail.GroupBy(m => new { m.SKU }).Select(m => new { SKU = m.Key.SKU, count = m.Count() });
                                var data = validataBox.Where(m => m.count > 1);
                                if (data.Count() > 0)
                                {
                                    message = "<p><font color='#FF0000'>外部入库单号:" + exterNo + "中的SKU:" + data.FirstOrDefault().SKU + "存在重复值，请检查！</font></p>";
                                    return;
                                }

                            }
                        }


                        //    //验证快进快出的箱号是否存在
                        //    for (int i = 0; i < Transdata.Tables["预入库单主信息$"].Rows.Count; i++)
                        //    {
                        //        string exterNo = Transdata.Tables["预入库单主信息$"].Rows[i]["外部入库单号"].ToString();
                        //        string orderType = Transdata.Tables["预入库单主信息$"].Rows[i]["预入库单类型"].ToString();
                        //        DataRow[] dtrow = Transdata.Tables["预入库单明细信息$"].Select("外部入库单号='" + exterNo + "'");//获取这一单的明细信息
                        //        if (dtrow.Count() <= 0)
                        //        {
                        //            message = "<p><font color='#FF0000'>外部入库单号：" + exterNo + "没有明细信息，请检查！</font></p>";
                        //            return;
                        //        }
                        //        //带箱号的订单
                        //        if (orderType == "快进快出")
                        //        {
                        //            var distinctBoxSKU = from s in dtrow.AsEnumerable()
                        //                                 group s by new { s1 = Convert.ToString(s.Field<string>("外部入库单号")), s2 = Convert.ToString(s.Field<string>("SKU")), s3 = Convert.ToString(s.Field<string>("托号")) } into m
                        //                                 select new
                        //                                 {
                        //                                     外部单号 = Convert.ToString(m.Key.s1),
                        //                                     SKU = Convert.ToString(m.Key.s2),
                        //                                     托号 = Convert.ToString(m.Key.s3),
                        //                                     count = m.Count()
                        //                                 };
                        //            var errorBoxSKU = distinctBoxSKU.Where(m => m.count > 1);
                        //            if (errorBoxSKU.Count() > 0)
                        //            {
                        //                message = "<p><font color='#FF0000'>外部入库单号:" + errorBoxSKU.FirstOrDefault().外部单号 + "中的SKU:"
                        //                    + errorBoxSKU.FirstOrDefault().SKU + "和箱号:" + distinctBoxSKU.FirstOrDefault().托号 + "存在重复值，请检查！" + "</font></p>";
                        //                return;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            //普通订单验证是否有相同SKU
                        //            var distinctSKU = from s in dtrow.AsEnumerable()
                        //                              group s by new { s1 = Convert.ToString(s.Field<string>("外部入库单号")), s2 = Convert.ToString(s.Field<string>("SKU")) } into m
                        //                              select new
                        //                              {
                        //                                  外部单号 = Convert.ToString(m.Key.s1),
                        //                                  SKU = Convert.ToString(m.Key.s2),
                        //                                  count = m.Count()
                        //                              };

                        //            var errorSKU = distinctSKU.Where(m => m.count > 1);

                        //            if (errorSKU.Count() > 0)
                        //            {
                        //                message = "<p><font color='#FF0000'>外部入库单号:" + errorSKU.FirstOrDefault().外部单号 + "中的SKU:" + errorSKU.FirstOrDefault().SKU + "存在重复值，请检查！" + "</font></p>";
                        //                return;
                        //            }
                        //        }

                        //    }
                        this.AfterData = Transdata;
                    }
                    catch (Exception ex)
                    {
                        this.AfterData = null;
                        message = ex.Message.ToString();
                    }

                }
                else
                {
                    this.AfterData = Transdata;
                }
            }
            catch
            {
                this.AfterData = null;
            }
        }



    }
}