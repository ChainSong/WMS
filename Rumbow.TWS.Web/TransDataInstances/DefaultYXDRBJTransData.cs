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
    public class DefaultYXDRBJTransData : BaseTransData
    {
        //由BaseTransData 传来的参数
        public DefaultYXDRBJTransData(string TransDataType, long CustomerID, long ProjectID, long WareHouseID,
           DataSet Transdata)
            : base(TransDataType, CustomerID, ProjectID, WareHouseID, Transdata)
        {

        }
        public override void CustomerDefinedSettledTransData(ref string message)
        {
            if (this.TransDataType == "PreOrder")
            {
                List<ProductSearch> productListS = new List<ProductSearch>();//产品
                IEnumerable<ProductSearch> productList;


                if (WareHouseID == 0)
                {
                    message = "用户没有分配仓库!!";
                    return;
                } 
                WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseList().Where(m => m.ID == WareHouseID).FirstOrDefault();//customerid获取仓库
                 
                IEnumerable<WMS_Customer> CustomerList = ApplicationConfigHelper.GetAllCustomerByID(CustomerID);
                WMS_UnitAndSpecifications_Config wmsunit = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, warehouse.ID).ToArray()[0];
                DataSet dsResualt = new DataSet();
                DataTable dtpo = new DataTable();
                DataTable dtpodetail = new DataTable();
                dtpo.TableName = "预出库单主信息$";
                dtpodetail.TableName = "预出库单明细信息$";
                DataTable dtYXDRBJ = Transdata.Tables[0];//获取转换之后的excel数据

                //主表
                dtpo.Columns.Add("外部单号");
                dtpo.Columns.Add("预出库单类型");
                dtpo.Columns.Add("仓库名称");
                dtpo.Columns.Add("订单日期");
                dtpo.Columns.Add("收货公司");
                dtpo.Columns.Add("省");
                dtpo.Columns.Add("市");
                dtpo.Columns.Add("联系人");
                dtpo.Columns.Add("备注");
                dtpo.Columns.Add("地址");
                dtpo.Columns.Add("联系方式");
                dtpo.Columns.Add("自定义字段4");
                dtpo.Columns.Add("自定义字段2");
                dtpo.Columns.Add("自定义字段3");


                //子表
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
                dtpodetail.Columns.Add("单位");
                dtpodetail.Columns.Add("规格");
                dtpodetail.Columns.Add("自定义字段4");
                dtpodetail.Columns.Add("自定义字段5");
                dtpodetail.Columns.Add("自定义字段6");


                for (int i = 0; i < dtYXDRBJ.Rows.Count; i++)
                {
                    if (dtpo.Select("外部单号='" + dtYXDRBJ.Rows[i]["订单号"].ToString() + "'").Count() <= 0)//判断excel中是否有相同的外部单号
                    {
                        DataRow dr = dtpo.NewRow();
                        dr["外部单号"] = dtYXDRBJ.Rows[i]["订单号"].ToString();
                        dr["预出库单类型"] = dtYXDRBJ.Rows[i]["订单类型"].ToString();
                        dr["仓库名称"] = warehouse.WarehouseName;
                        dr["市"] = CustomerList.Where(m => m.StorerKey == dtYXDRBJ.Rows[i]["货主"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtYXDRBJ.Rows[i]["货主"].ToString()).ToArray()[0].City : "";
                        dr["省"] = "";
                        dr["联系人"] = CustomerList.Where(m => m.StorerKey == dtYXDRBJ.Rows[i]["货主"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtYXDRBJ.Rows[i]["货主"].ToString()).ToArray()[0].Contact1 : "";


                        dr["订单日期"] = dtYXDRBJ.Rows[i]["订单日期"].ToString();
                        dr["备注"] = dtYXDRBJ.Rows[i]["备注1"].ToString();
                        dr["自定义字段4"] = dtYXDRBJ.Rows[i]["货主"].ToString();
                        dr["自定义字段2"] = dtYXDRBJ.Rows[i]["备注2"].ToString();
                        dr["自定义字段3"] = dtYXDRBJ.Rows[i]["备注3"].ToString();
                        if (CustomerList.Where(a => a.StorerKey == dtYXDRBJ.Rows[i]["货主"].ToString()).ToArray().Length <= 0)
                        {
                            message = dtYXDRBJ.Rows[i]["货主"].ToString() + "货主不存在！！";
                            return;
                        }
                        dr["地址"] = CustomerList.Where(m => m.StorerKey == dtYXDRBJ.Rows[i]["货主"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtYXDRBJ.Rows[i]["货主"].ToString()).ToArray()[0].AddressLine1 : "";//地址从wms_customer获取
                        dr["联系方式"] = CustomerList.Where(m => m.StorerKey == dtYXDRBJ.Rows[i]["货主"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtYXDRBJ.Rows[i]["货主"].ToString()).ToArray()[0].PhoneNum1 : "";//联系方式从wms_customer获取
                        dr["收货公司"] = CustomerList.Where(m => m.StorerKey == dtYXDRBJ.Rows[i]["货主"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtYXDRBJ.Rows[i]["货主"].ToString()).ToArray()[0].Company : "";//门店的名称从wms_customer获取


                        dtpo.Rows.Add(dr);//主表添加一行
                    }

                    DataRow drdetail = dtpodetail.NewRow();
                    drdetail["外部单号"] = dtYXDRBJ.Rows[i]["订单号"].ToString();
                    drdetail["仓库"] = warehouse.WarehouseName;

                    ProductSearch ps = new ProductSearch();
                    ps.Str10 = dtYXDRBJ.Rows[i]["存货Article"].ToString();
                    ps.Str9 = dtYXDRBJ.Rows[i]["存货Size"].ToString();
                    productListS.Add(ps);//先获取Article+size集合,然后一起校验sku

                    drdetail["UPC"] = "";
                    try
                    {
                        drdetail["产品等级"] = dtYXDRBJ.Rows[i]["产品等级"].ToString();
                    }
                    catch
                    {
                        drdetail["产品等级"] = "A品";
                    }
                    drdetail["批次号"] = "";
                    drdetail["托号"] = "";
                    drdetail["规格"] = "";
                    drdetail["备注"] = "";
                    drdetail["期望数量"] = dtYXDRBJ.Rows[i]["数量"].ToString();
                    drdetail["自定义字段4"] = dtYXDRBJ.Rows[i]["备注4"].ToString();
                    drdetail["自定义字段5"] = dtYXDRBJ.Rows[i]["备注5"].ToString();
                    drdetail["自定义字段6"] = dtYXDRBJ.Rows[i]["备注6"].ToString();
                    drdetail["单位"] = dtYXDRBJ.Rows[i]["单位"].ToString();
                    dtpodetail.Rows.Add(drdetail);
                }
                productList = ApplicationConfigHelper.GetSearchProductYXDR(CustomerID, productListS, "ArticleSize");
                for (int i = 0; i < dtpodetail.Rows.Count; i++)
                {
                    if (productList.ToArray().Length <= 0)
                    {
                        message = "excel中的Article+Size未匹配到任何SKU,请检查excel数据！";
                        return;
                    }
                    string sku = productList.Where(p => p.Str10 == productListS[i].Str10 && p.Str9 == productListS[i].Str9).ToArray().Length > 0 ? productList.Where(p => p.Str10 == productListS[i].Str10 && p.Str9 == productListS[i].Str9).ToArray()[0].SKU : "";
                    if (string.IsNullOrEmpty(sku))
                    {
                        int x = i + 1;
                        message = "excel中第" + x + "行的Article=" + productListS[i].Str10 + "和Size=" + productListS[i].Str9 + "未匹配到SKU";
                        return;
                    }
                    dtpodetail.Rows[i]["SKU"] = sku;
                    dtpodetail.Rows[i]["产品名称"] = productList.Where(p => p.Str10 == productListS[i].Str10 && p.Str9 == productListS[i].Str9).ToArray().Length > 0 ? productList.Where(p => p.Str10 == productListS[i].Str10 && p.Str9 == productListS[i].Str9).ToArray()[0].GoodsName : "";

                }
                //验证同一外部单号是否存在相同的SKU
                //var dd = from t in dtpodetail.AsEnumerable()
                //         group t by new { t1 = t.Field<string>("外部单号"), t2 = t.Field<string>("SKU") } into m
                //         select new
                //         {
                //             外部单号 = m.Select(p => p.Field<string>("外部单号")).First(),
                //             SKU = m.Select(p => p.Field<string>("SKU")).First(),
                //             count = m.Count(),
                //         };
                //var dr1 = dd.Where(c => c.count > 1);
                //if (dr1.Count() > 0)
                //{
                //    foreach (var item in dr1)
                //    {
                //        message += "<p><font color='#FF0000'>外部单号" + item.外部单号 + "中的SKU" + item.SKU + "存在重复值！" + "</font></p>";
                //        return;
                //    }
                //}

                dsResualt.Tables.Add(dtpo);
                dsResualt.Tables.Add(dtpodetail);

                this.AfterData = dsResualt;

            }
            else if (TransDataType == "Asn")//入库单
            {

                if (ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == WareHouseID).ToArray().Count() <= 0)
                {
                    message = "用户没有分配仓库!!";
                    return;
                }

                List<ProductSearch> productListS = new List<ProductSearch>();//产品
                IEnumerable<ProductSearch> productList;

                WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == WareHouseID).ToArray()[0];//查询仓库
                IEnumerable<WMS_Customer> CustomerList = ApplicationConfigHelper.GetAllCustomerByID(CustomerID);//查询wms_customer
                WMS_UnitAndSpecifications_Config wmsunit = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, warehouse.ID).ToArray()[0];//查询单位等信息

                DataSet dsresult = new DataSet();
                DataTable dtas = new DataTable();
                DataTable dtasdetail = new DataTable();
                DataTable dtYXDRBJas = Transdata.Tables[0];

                dtas.TableName = "预入库单主信息$";
                dtasdetail.TableName = "预入库单明细信息$";

                //主表
                dtas.Columns.Add("外部入库单号");
                dtas.Columns.Add("预入库单类型");
                dtas.Columns.Add("预入库日期");
                dtas.Columns.Add("仓库名称");
                dtas.Columns.Add("客户");
                dtas.Columns.Add("备注");
                dtas.Columns.Add("自定义字段1");
                dtas.Columns.Add("自定义字段2");
                dtas.Columns.Add("自定义字段3");

                //子表
                dtasdetail.Columns.Add("外部入库单号");
                dtasdetail.Columns.Add("仓库");
                dtasdetail.Columns.Add("SKU");
                dtasdetail.Columns.Add("产品名称");
                dtasdetail.Columns.Add("UPC");
                dtasdetail.Columns.Add("规格");
                dtasdetail.Columns.Add("预收数量");
                dtasdetail.Columns.Add("单位");
                dtasdetail.Columns.Add("产品等级");
                dtasdetail.Columns.Add("箱号");
                dtasdetail.Columns.Add("备注");
                dtasdetail.Columns.Add("自定义字段4");
                dtasdetail.Columns.Add("自定义字段5");
                dtasdetail.Columns.Add("自定义字段6");

                for (int i = 0; i < dtYXDRBJas.Rows.Count; i++)
                {
                    if (dtas.Select("外部入库单号='" + dtYXDRBJas.Rows[i]["订单号"].ToString() + "'").Count() <= 0)
                    {
                        DataRow dr = dtas.NewRow();
                        dr["外部入库单号"] = dtYXDRBJas.Rows[i]["订单号"].ToString();
                        dr["预入库单类型"] = dtYXDRBJas.Rows[i]["预入库单类型"].ToString();
                        dr["预入库日期"] = dtYXDRBJas.Rows[i]["预入库日期"].ToString();
                        dr["仓库名称"] = warehouse.WarehouseName;
                        dr["客户"] = dtYXDRBJas.Rows[i]["客户"].ToString();
                        dr["备注"] = dtYXDRBJas.Rows[i]["备注1"].ToString();
                        dr["自定义字段1"] = dtYXDRBJas.Rows[i]["备注1"].ToString();
                        dr["自定义字段2"] = dtYXDRBJas.Rows[i]["备注2"].ToString();
                        dr["自定义字段3"] = dtYXDRBJas.Rows[i]["备注3"].ToString();
                        dtas.Rows.Add(dr);
                    }

                    //子表
                    DataRow drdetail = dtasdetail.NewRow();
                    drdetail["外部入库单号"] = dtYXDRBJas.Rows[i]["订单号"].ToString();
                    drdetail["仓库"] = warehouse.WarehouseName;

                    ProductSearch ps = new ProductSearch();
                    ps.SKU = dtYXDRBJas.Rows[i]["SKU"].ToString();
                    productListS.Add(ps);

                    drdetail["UPC"] = "";
                    drdetail["规格"] = "";
                    drdetail["预收数量"] = dtYXDRBJas.Rows[i]["预收数量"].ToString();
                    drdetail["箱号"] = dtYXDRBJas.Rows[i]["箱号"].ToString();
                    drdetail["单位"] = dtYXDRBJas.Rows[i]["单位"].ToString();
                    try
                    {
                        drdetail["产品等级"] = dtYXDRBJas.Rows[i]["产品等级"].ToString();
                    }
                    catch (Exception)
                    {
                        drdetail["产品等级"] = "A品";//默认A
                    }
                    drdetail["备注"] = dtYXDRBJas.Rows[i]["备注4"].ToString();
                    drdetail["自定义字段4"] = dtYXDRBJas.Rows[i]["备注4"].ToString();
                    drdetail["自定义字段5"] = dtYXDRBJas.Rows[i]["备注5"].ToString();
                    drdetail["自定义字段6"] = dtYXDRBJas.Rows[i]["备注6"].ToString();
                    dtasdetail.Rows.Add(drdetail);
                }

                productList = ApplicationConfigHelper.GetSearchProductYXDR(CustomerID, productListS, "SKU");

                for (int i = 0; i < dtYXDRBJas.Rows.Count; i++)
                {

                    if (productList.ToArray().Length <= 0)
                    {
                        message = "excel中的SKU在系统中不存在,请检查excel数据！";
                        return;
                    }
                    string sku = productList.Where(p => p.SKU == productListS[i].SKU).ToArray().Length > 0 ? productList.Where(p => p.SKU == productListS[i].SKU).ToArray()[0].SKU : "";
                    if (string.IsNullOrEmpty(sku))
                    {
                        message = "excel中的SKU=" + dtYXDRBJas.Rows[i]["SKU"].ToString() + "在系统中不存在，请检查数据！";
                        return;
                    }
                    dtasdetail.Rows[i]["SKU"] = sku;
                    dtasdetail.Rows[i]["产品名称"] = productList.Where(p => p.SKU == sku).Select(p => p.GoodsName).FirstOrDefault();
                }

                //验证同一外部单号是否存在相同的SKU
                //var dd = from t in dtasdetail.AsEnumerable()
                //         group t by new { t1 = t.Field<string>("外部入库单号"), t2 = t.Field<string>("SKU") } into m
                //         select new
                //         {
                //             外部单号 = m.Select(p => p.Field<string>("外部入库单号")).First(),
                //             SKU = m.Select(p => p.Field<string>("SKU")).First(),
                //             count = m.Count(),
                //         };
                //var dr1 = dd.Where(c => c.count > 1);
                //if (dr1.Count() > 0)
                //{
                //    foreach (var item in dr1)
                //    {
                //        message += "<p><font color='#FF0000'>外部入库单号" + item.外部单号 + "中的SKU" + item.SKU + "存在重复值！" + "</font></p>";
                //        return;
                //    }
                //}


                dsresult.Tables.Add(dtas);
                dsresult.Tables.Add(dtasdetail);
                this.AfterData = dsresult;

            }
            else if (TransDataType == "Receiving")//上架单
            {
                DataTable dt = new DataTable();
                dt = this.Transdata.Tables[0].Copy();
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