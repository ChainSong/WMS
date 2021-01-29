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
    public class DefaultTransData : BaseTransData
    {
        public DefaultTransData(string TransDataType, long CustomerID, long ProjectID, long WareHouseID,
           DataSet Transdata)
            : base(TransDataType, CustomerID, ProjectID, WareHouseID, Transdata)
        {
        }
        //new PreOrderService().ManualAllocationJson(new ManualAllocationRequest() 
        //{ PodRequest = pod, ID = Convert.ToInt64(ID), CustomerId = CustomerId, Creator = base.UserInfo.Name, Criterion = Criterion });
        public override void CustomerDefinedSettledTransData(ref string message)
        {
            try
            {
            if (TransDataType == "PreOrder")
            {
                DataTable dt = Transdata.Tables["预出库单明细信息"].Copy();
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
                DataTable dtpo = Transdata.Tables["预出库单主信息"].Copy();
                DataSet ds = new DataSet();
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
                        dt.Rows[i]["生产日期"]= System.DateTime.Now.ToString();
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