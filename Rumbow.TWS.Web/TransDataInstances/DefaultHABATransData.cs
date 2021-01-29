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
    public class DefaultHABATransData : BaseTransData
    {
        public DefaultHABATransData(string TransDataType, long CustomerID, long ProjectID, long WareHouseID,
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

                    //验证一下是否有重复的SKU
                    foreach (DataRow item in dtpo.Rows)
                    {
                        string exterNo = item["外部单号"].ToString().Trim();
                        if (string.IsNullOrEmpty(exterNo))
                        {
                            message = "预出库单主信息中的外部单号不能为空，请检查！";
                            return;
                        }
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
                            List<ASNDetail> asnDetail = new List<ASNDetail>();
                            foreach (DataRow row in dtrow)
                            {
                                ASNDetail detail = new ASNDetail();
                                detail.SKU = row["SKU"].ToString();
                                asnDetail.Add(detail);
                            }
                            var validata = asnDetail.GroupBy(m => new { m.SKU }).Select(m => new { SKU = m.Key.SKU, count = m.Count() });
                            var data = validata.Where(m => m.count > 1);
                            if (data.Count() > 0)
                            {
                                message = "<p><font color='#FF0000'>外部入库单号:" + exterNo + "中的SKU:" + data.FirstOrDefault().SKU + "存在重复值，请检查！</font></p>";
                                return;
                            }
                            this.AfterData = Transdata;

                        }


                    }
                    catch (Exception ex)
                    {
                        this.AfterData = null;
                        message = ex.Message.ToString();
                    }
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