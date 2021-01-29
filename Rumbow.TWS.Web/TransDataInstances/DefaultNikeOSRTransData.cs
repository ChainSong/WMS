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
    public class DefaultNikeOSRTransData : BaseTransData
    {
        public DefaultNikeOSRTransData(string TransDataType, long CustomerID, long ProjectID, long WareHouseID,
           DataSet Transdata)
            : base(TransDataType, CustomerID, ProjectID, WareHouseID, Transdata)
        {
        }
        //new PreOrderService().ManualAllocationJson(new ManualAllocationRequest() 
        //{ PodRequest = pod, ID = Convert.ToInt64(ID), CustomerId = CustomerId, Creator = base.UserInfo.Name, Criterion = Criterion });
          public override void CustomerDefinedSettledTransData(ref string message)
        {
            if (TransDataType == "PreOrder")
            {
                List<ProductSearch> productListS = new List<ProductSearch>();
                IEnumerable<ProductSearch> productList;
                if (WareHouseID == 0)
                {
                    message = "用户没有分配仓库!!";
                    return;
                } 
                WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseList().Where(m => m.ID == WareHouseID).FirstOrDefault();//customerid获取仓库
                IEnumerable<WMS_Customer> CustomerList = ApplicationConfigHelper.GetAllCustomerByID(CustomerID);
                WMS_UnitAndSpecifications_Config wmsunit = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, warehouse.ID).ToArray()[0];
                DataSet dsResualt=new DataSet();
                DataTable dtpo=new DataTable();
                dtpo.TableName = "预出库单主信息$";
                DataTable dtpodetail=new DataTable();
                dtpodetail.TableName = "预出库单明细信息$";
                DataTable dtNike = Transdata.Tables[0];
                dtpo.Columns.Add("外部单号");
                dtpo.Columns.Add("预出库单类型");
                dtpo.Columns.Add("仓库名称");
                dtpo.Columns.Add("订单日期");
                dtpo.Columns.Add("省");
                dtpo.Columns.Add("市");
                dtpo.Columns.Add("联系人");
                dtpo.Columns.Add("快递公司");
                dtpo.Columns.Add("备注");
                dtpo.Columns.Add("自定义字段2");
                dtpo.Columns.Add("自定义字段3");
                dtpo.Columns.Add("自定义字段4");
                dtpo.Columns.Add("地址");
                dtpo.Columns.Add("联系方式");
                dtpo.Columns.Add("自定义字段5");
                dtpo.Columns.Add("自定义字段6");
                dtpo.Columns.Add("自定义字段7");
                dtpo.Columns.Add("自定义字段12");

                dtpodetail.Columns.Add("外部单号");
                dtpodetail.Columns.Add("仓库");
                dtpodetail.Columns.Add("SKU");
                dtpodetail.Columns.Add("UPC");
                dtpodetail.Columns.Add("产品名称");
                dtpodetail.Columns.Add("产品等级");
                dtpodetail.Columns.Add("批次号");
                dtpodetail.Columns.Add("托号");
                dtpodetail.Columns.Add("备注");
                dtpodetail.Columns.Add("期望数量");
                dtpodetail.Columns.Add("自定义字段4");
                dtpodetail.Columns.Add("自定义字段5");
                dtpodetail.Columns.Add("自定义字段6");
                dtpodetail.Columns.Add("自定义字段7");
                dtpodetail.Columns.Add("单位");
                dtpodetail.Columns.Add("规格");
                for (int i = 0; i < dtNike.Rows.Count; i++)
                {
                    if (dtpo.Select("外部单号='" + dtNike.Rows[i]["订单号"].ToString() + "'").Count() <= 0)
                    {
                        DataRow dr = dtpo.NewRow();
                        dr["外部单号"] = dtNike.Rows[i]["订单号"].ToString();
                        dr["预出库单类型"] = dtNike.Rows[i]["订单类型"].ToString();
                        dr["仓库名称"] = warehouse.WarehouseName;
                        dr["市"] = "";
                        dr["省"] = "";
                        dr["联系人"] = "";
                        dr["订单日期"] = dtNike.Rows[i]["日期"].ToString();
                        dr["备注"] = dtNike.Rows[i]["备注1"].ToString();
                        dr["自定义字段2"] = dtNike.Rows[i]["备注2"].ToString();
                        dr["自定义字段3"] = dtNike.Rows[i]["备注3"].ToString();
                        dr["自定义字段4"] = dtNike.Rows[i]["客户代码"].ToString();
                        //客户代码不存在
                        if (CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["客户代码"].ToString()).ToArray().Length <= 0)
                        {
                            message = dtNike.Rows[i]["客户代码"].ToString() + "客户代码不存在!";
                            return;
                        }
                        dr["地址"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["客户代码"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["客户代码"].ToString()).ToArray()[0].AddressLine1 : "";
                        dr["自定义字段5"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["客户代码"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["客户代码"].ToString()).ToArray()[0].Company : "";
                        dr["联系方式"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["客户代码"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["客户代码"].ToString()).ToArray()[0].PhoneNum1 : "";
                        dr["自定义字段6"] = dtNike.Rows[i]["备注4"].ToString();
                        dr["自定义字段7"] = dtNike.Rows[i]["备注5"].ToString();
                        if (dtNike.Rows[i]["订单类型"].ToString().Equals("退仓出货"))
                        {
                            string querykey = "R" + System.DateTime.Now.Year.ToString().Substring(3, 1).ToString() + System.DateTime.Now.ToString("MMdd");
                            string Carrerkey = ApplicationConfigHelper.GetQueryDetail("pro_wms_getCarrierKey_select", querykey).ToString();
                            if (Carrerkey == "")
                            {
                                Carrerkey = "R" + System.DateTime.Now.Year.ToString().Substring(3, 1).ToString() + System.DateTime.Now.ToString("MMdd") + "01";
                            }
                            else
                            {
                                Carrerkey = "R" + (Convert.ToInt32(Carrerkey.Substring(1, Carrerkey.Length - 1)) + 1).ToString();
                            }
                            dr["自定义字段12"] = Carrerkey;
                        }
                        else
                        {
                            dr["自定义字段12"] = "";
                        }
                        dtpo.Rows.Add(dr);
                    }

                    DataRow drdetail = dtpodetail.NewRow();
                    drdetail["外部单号"] = dtNike.Rows[i]["订单号"].ToString();
                    drdetail["仓库"] = warehouse.WarehouseName;
                    drdetail["SKU"] = dtNike.Rows[i]["存货代码"].ToString();
                    ProductSearch ps = new ProductSearch();
                    //存货代码格式不正确
                    //for (int j = 0; j < drdetail["SKU"].ToString().Split('-').Length; j++)
                    //{
                    //    if (drdetail["SKU"].ToString().Split('-')[j].Equals(""))
                    //    {
                    //        message = drdetail["SKU"].ToString() + "存货代码格式不正确!";
                    //        return;
                    //    }
                    //}
                    if (drdetail["SKU"].ToString().Split('-').Length < 3)
                    {
                        message = drdetail["SKU"].ToString() + "存货代码格式不正确!";
                                return;
                    }
                    try
                    {
                        ps.Str10 = drdetail["SKU"].ToString().Split('-')[0] + "-" + drdetail["SKU"].ToString().Split('-')[1];
                        ps.Str8 = drdetail["SKU"].ToString().Substring(drdetail["SKU"].ToString().Length - 2, 2);
                        ps.Str9 = drdetail["SKU"].ToString().Substring(ps.Str10.Length + 1, drdetail["SKU"].ToString().Length - (ps.Str10.Length + 4));
                    }
                    catch
                    {
                        message = drdetail["SKU"].ToString() + "存货代码格式不正确!";
                        return;
                    }
                        //if (drdetail["SKU"].ToString().Split('-').Length <= 4)
                    //{
                    //    ps.Str9 = drdetail["SKU"].ToString().Split('-')[2];
                    //    ps.Str8 = drdetail["SKU"].ToString().Split('-')[3];
                    //}
                    //else if (drdetail["SKU"].ToString().Split('-').Length == 5)
                    //{
                    //    ps.Str9 = drdetail["SKU"].ToString().Split('-')[2] + "-" + drdetail["SKU"].ToString().Split('-')[3];
                    //    ps.Str8 = drdetail["SKU"].ToString().Split('-')[4];
                    //}
                    //else if (drdetail["SKU"].ToString().Split('-').Length == 6)
                    //{
                    //    ps.Str9 = drdetail["SKU"].ToString().Split('-')[2] + "-" + drdetail["SKU"].ToString().Split('-')[3] + drdetail["SKU"].ToString().Split('-')[4];
                    //    ps.Str8 = drdetail["SKU"].ToString().Split('-')[5];
                    //}
                    //获得SKU集合
                    productListS.Add(ps);
                   
                    drdetail["UPC"] = "";
                    try
                    {
                        drdetail["产品等级"] = dtNike.Rows[i]["产品等级"].ToString();
                    }
                    catch
                    {
                        drdetail["产品等级"] = "A品";
                    }
                    drdetail["批次号"] = "";
                    drdetail["托号"] = "";
                    drdetail["规格"] = "";
                    drdetail["备注"] = "";
                    drdetail["期望数量"] = dtNike.Rows[i]["数量"].ToString();
                    drdetail["自定义字段4"] = dtNike.Rows[i]["单位"].ToString();
                    drdetail["自定义字段5"] = dtNike.Rows[i]["存货名称及规格型号"].ToString();
                    drdetail["自定义字段6"] = dtNike.Rows[i]["存货代码"].ToString();
                    drdetail["自定义字段7"] = dtNike.Rows[i]["BU"].ToString();
                    drdetail["单位"] = wmsunit.Unit;
                    dtpodetail.Rows.Add(drdetail);
                }
                
                productList = ApplicationConfigHelper.GetSearchProduct(CustomerID, productListS, "Acticle");
                for (int i = 0; i < dtpodetail.Rows.Count; i++)
                {

                    string SKU = dtpodetail.Rows[i]["SKU"].ToString();
                    int skulength = SKU.Split('-').Length;
                    string str10 = SKU.Split('-')[0]+"-"+ SKU.Split('-')[1];
                    //string str8 = SKU.Split('-')[skulength - 1];
                    //string str9 = "";
                    string str8 = SKU.Substring(SKU.Length - 2, 2);
                    string str9 = SKU.ToString().Substring(str10.Length + 1, SKU.Length - (str10.Length + 4));
                    //for (int m = 2; m < skulength - 1; m++)
                    //{
                    //    str9 += SKU.Split('-')[m]+"-";
                    //}
                    //str9 = str9.Substring(0,str9.Length - 1);
                    //存货代码不存在
                    if (productList.Where(c => c.Str10 == str10 && c.Str8 == str8 && c.Str9 == str9).Select(m => m.SKU).ToArray().Length <= 0)
                    {
                        message = dtpodetail.Rows[i]["SKU"].ToString() + "存货代码不存在!";
                        return;
                    }
                    //Article不存在
                    if (productList.Where(c => c.Str10 == str10 && c.Str8 == str8 && c.Str9 == str9).Select(m => m.LongMaterial).ToArray().Length <= 0)
                    {
                        message = str10 + "不存在!";
                        return;
                    }
                    dtpodetail.Rows[i]["SKU"] = productList.Where(c => c.Str10 == str10 && c.Str8 == str8 && c.Str9 == str9).Select(m => m.SKU).FirstOrDefault();
                    dtpodetail.Rows[i]["产品名称"] = productList.Where(c => c.SKU == dtpodetail.Rows[i]["SKU"]).ToArray().Length > 0 ? productList.Where(c => c.SKU == dtpodetail.Rows[i]["SKU"]).ToArray()[0].LongMaterial.ToString() : "";
                }
                dsResualt.Tables.Add(dtpo);
                dsResualt.Tables.Add(dtpodetail);

                this.AfterData = dsResualt;
            }
            else if (TransDataType == "Receiving")
            {
                DataTable dt = Transdata.Tables[0].Copy();
                if (!dt.Columns.Contains("生产日期"))
                {
                    dt.Columns.Add("生产日期");
                }
                if (!dt.Columns.Contains("UPC"))
                {
                    dt.Columns.Add("UPC");
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
    }
}