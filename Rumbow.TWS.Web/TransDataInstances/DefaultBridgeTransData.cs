using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Runbow.TWS.Common;

namespace Runbow.TWS.Web.TransDataInstances
{
    public class DefaultBridgeTransData : BaseTransData
    {
        public DefaultBridgeTransData(string TransDataType, long CustomerID, long ProjectID, long WareHouseID,
           DataSet Transdata)
            : base(TransDataType, CustomerID, ProjectID, WareHouseID, Transdata)
        {
        }
        public override void CustomerDefinedSettledTransData(ref string message)
        {
            if (TransDataType == "PreOrder")
            {
                if (Transdata.Tables[0].Columns.Contains("订单类型"))
                {
                    if (Transdata.Tables[0].Rows[0]["订单类型"].ToString() == "退仓出货")
                    {
                        #region 退仓出货 不拆单
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
                        DataSet dsResualt = new DataSet();
                        DataTable dtpo = new DataTable();
                        dtpo.TableName = "预出库单主信息$";
                        DataTable dtpodetail = new DataTable();
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
                                    message = "第【" + (i + 2) + "】行，" + "【客户代码】列的客户代码格式不对或 " + dtNike.Rows[i]["客户代码"].ToString() + "客户代码不存在!";
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
                            if (dtNike.Columns.Contains("批次号"))
                            {
                                drdetail["批次号"] = dtNike.Rows[i]["批次号"].ToString();
                            }
                            else
                            {
                                drdetail["批次号"] = "";
                            }
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
                            string str10 = SKU.Split('-')[0] + "-" + SKU.Split('-')[1];
                            string str8 = SKU.Substring(SKU.Length - 2, 2);
                            string str9 = SKU.ToString().Substring(str10.Length + 1, SKU.Length - (str10.Length + 4));
                            //存货代码不存在
                            if (productList.Where(c => c.Str10 == str10 && c.Str8 == str8 && c.Str9 == str9).Select(m => m.SKU).ToArray().Length <= 0)
                            {
                                message = "第【" + (i + 2) + "】行，" + "【SKU】列的存货代码格式不对或 " + dtpodetail.Rows[i]["SKU"].ToString() + "存货代码不存在!";
                                return;
                            }
                            //Article不存在
                            if (productList.Where(c => c.Str10 == str10 && c.Str8 == str8 && c.Str9 == str9).Select(m => m.LongMaterial).ToArray().Length <= 0)
                            {
                                message = str10 + "不存在!";
                                return;
                            }
                            dtpodetail.Rows[i]["SKU"] = productList.Where(c => c.Str10 == str10.Trim().ToUpper() && c.Str8 == str8 && c.Str9 == str9.Trim().ToUpper()).Select(m => m.SKU).FirstOrDefault();
                            dtpodetail.Rows[i]["产品名称"] = productList.Where(c => c.SKU == dtpodetail.Rows[i]["SKU"]).ToArray().Length > 0 ? productList.Where(c => c.SKU == dtpodetail.Rows[i]["SKU"]).ToArray()[0].LongMaterial.ToString() : "";
                        }
                        dsResualt.Tables.Add(dtpo);
                        dsResualt.Tables.Add(dtpodetail);

                        this.AfterData = dsResualt;
                        #endregion
                    }
                    else if (Transdata.Tables[0].Rows[0]["订单类型"].ToString() == "CRW退货")
                    {
                        #region CRW退货 不拆单
                        if (ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == WareHouseID).ToArray().Count() <= 0)
                        {
                            message = "用户没有分配仓库!!";
                            return;
                        }
                        WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == WareHouseID).ToArray()[0];//Bob
                        IEnumerable<WMS_Customer> CustomerList = ApplicationConfigHelper.GetAllCustomerByID(CustomerID);
                        WMS_UnitAndSpecifications_Config wmsunit = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, warehouse.ID).ToArray()[0];
                        DataSet dsResualt = new DataSet();
                        DataTable dtpo = new DataTable();
                        dtpo.TableName = "预出库单主信息$";
                        DataTable dtpodetail = new DataTable();
                        dtpodetail.TableName = "预出库单明细信息$";

                        DataTable dtNike = Transdata.Tables[0];
                        List<ProductSearch> searprolistfs = new List<ProductSearch>();
                        for (int i = 0; i < dtNike.Rows.Count; i++)
                        {
                            ProductSearch psfs = new ProductSearch();
                            //客户代码不存在
                            if (dtNike.Rows[i]["Style Color"].ToString().Equals("") || dtNike.Rows[i]["Style Color"].ToString().Length == 0)
                            {
                                message = "第【" + (i + 2) + "】行，" + "【Style Color】列" + "有空值或格式不正确!";
                                return;
                            }
                            if (dtNike.Rows[i]["Size"].ToString().Equals("") || dtNike.Rows[i]["Style Color"].ToString().Length == 0)
                            {
                                message = "第【" + (i + 2) + "】行，" + "【Size】列" + "有空值或格式不正确!";
                                return;
                            }
                            psfs.Str10 = dtNike.Rows[i]["Style Color"].ToString();
                            psfs.Str9 = dtNike.Rows[i]["Size"].ToString();
                            psfs.Str8 = "01";
                            searprolistfs.Add(psfs);
                        }
                        IEnumerable<ProductSearch> productListfs = ApplicationConfigHelper.GetSearchProduct(CustomerID, searprolistfs, "Acticle");
                        //先验证article存不存在再继续
                        for (int i = 0; i < dtNike.Rows.Count; i++)
                        {
                            if (productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length <= 0)
                            {
                                message = "第【" + (i + 2) + "】行，" + "【Style Color】列的Article格式不对或Article " + dtNike.Rows[i]["Style Color"].ToString() + " 在系统中不存在!";
                                return;
                            }
                            else
                            {
                                if (productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString() && m.Str9 == dtNike.Rows[i]["Size"].ToString()).ToArray().Length <= 0)
                                {
                                    message = "第【" + (i + 2) + "】行，" + "【Size】列的尺寸格式不对或 " + dtNike.Rows[i]["Style Color"].ToString() + "对应的尺寸" + dtNike.Rows[i]["Size"].ToString() + " 在系统中不存在!";
                                    return;
                                }
                            }
                        }

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
                        dtpo.Columns.Add("自定义字段8");

                        dtpodetail.Columns.Add("外部单号");
                        dtpodetail.Columns.Add("仓库");
                        dtpodetail.Columns.Add("SKU");
                        dtpodetail.Columns.Add("UPC");
                        dtpodetail.Columns.Add("产品等级");
                        dtpodetail.Columns.Add("产品名称");
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

                        //判断是否是殊门店配置                    
                        IEnumerable<WMSConfig> wmslist = null;//NFS分单配置
                        string userdef3 = "";
                        if (CustomerList.Where(m => m.StorerKey == dtNike.Rows[0]["Store Code"].ToString()).Count() > 0)
                        {
                            userdef3 = CustomerList.Where(m => m.StorerKey == dtNike.Rows[0]["Store Code"].ToString()).FirstOrDefault().UserDef3;
                        }
                        else
                        {
                            message = "第【1】行，" + "【Store Code】列的门店代码格式不对或门店代码 " + dtNike.Rows[0][0].ToString() + "在系统中不存在!";
                            return;
                        }
                        try
                        {
                            wmslist = ApplicationConfigHelper.GetWMS_Config(userdef3);
                        }
                        catch (Exception)
                        {
                        }

                        //特殊门店
                        if (wmslist != null && wmslist.Count() > 0)
                        {
                            for (int i = 0; i < dtNike.Rows.Count; i++)
                            {
                                string division = "", gender = "";
                                division = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division;
                                gender = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].GenderAge;
                                if (division == "FTW" && gender == "YA")
                                {

                                }
                                string exterNo = DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + '-' + dtNike.Rows[0]["Store Code"].ToString() + wmslist.Where(m => m.Str1 == gender && m.Str2 == division).FirstOrDefault().Code;//年月日+门店代码+code
                                //if (dtpo.Select("外部单号='" + DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + dtNike.Rows[i]["Store Code"].ToString() + wmslist.Where(m => m.Str1 == gender && m.Str2 == division).FirstOrDefault().Code + "'").Count() <= 0)
                                if (dtpo.Select("外部单号='" + exterNo + "'").Count() <= 0)
                                {
                                    DataRow dr = dtpo.NewRow();
                                    if (productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length <= 0)
                                    {
                                        message = "第【" + (i + 2) + "】行，" + "【Style Color】列的Article格式不对或Article " + dtNike.Rows[i]["Style Color"].ToString() + "在系统中不存在!";
                                        return;
                                    }
                                    dr["外部单号"] = DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + '-' + dtNike.Rows[0]["Store Code"].ToString() + wmslist.Where(m => m.Str1 == gender && m.Str2 == division).FirstOrDefault().Code;//年月日+门店代码+code
                                    dr["预出库单类型"] = "门店调拨出库";
                                    dr["仓库名称"] = warehouse.WarehouseName;
                                    dr["市"] = "";
                                    dr["省"] = "";
                                    dr["联系人"] = "";
                                    dr["订单日期"] = DateTime.Now.ToString();
                                    dr["备注"] = "";
                                    dr["自定义字段2"] = dtNike.Rows[i]["Contract Code"].ToString();
                                    dr["自定义字段3"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division.ToString() : "";
                                    if (CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length <= 0)
                                    {
                                        message = "第【" + (i + 2) + "】行，" + "【Store Code】列的门店代码格式不对或门店代码 " + dtNike.Rows[i]["Store Code"].ToString() + "在系统中不存在!";//dtNike.Rows[i]["Store Code"].ToString() + "不存在!";
                                        return;
                                    }
                                    if (dtNike.Columns.Contains("LF"))
                                    {
                                        dr["自定义字段8"] = dtNike.Rows[i]["LF"].ToString();
                                    }
                                    dr["自定义字段4"] = dtNike.Rows[i]["Store Code"].ToString();
                                    dr["地址"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].AddressLine1 : "";
                                    dr["自定义字段5"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].Company.ToString() : "";
                                    dr["联系方式"] = "";
                                    dr["自定义字段6"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].GenderAge.ToString() : "";
                                    dr["自定义字段7"] = dtNike.Rows[i]["Store CRD"].ToString();
                                    dtpo.Rows.Add(dr);

                                }

                                DataRow drdetail = dtpodetail.NewRow();
                                drdetail["外部单号"] = DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + '-' + dtNike.Rows[0]["Store Code"].ToString() + wmslist.Where(m => m.Str1 == gender && m.Str2 == division).FirstOrDefault().Code;//年月日+门店代码+code
                                drdetail["仓库"] = warehouse.WarehouseName;
                                drdetail["SKU"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].SKU : "";
                                drdetail["UPC"] = "";
                                try
                                {
                                    drdetail["产品等级"] = dtNike.Rows[i]["产品等级"].ToString();
                                }
                                catch
                                {
                                    drdetail["产品等级"] = "A品";
                                }
                                drdetail["产品名称"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].LongMaterial.ToString() : "";//产品名称
                                if (dtNike.Columns.Contains("批次号"))
                                {
                                    drdetail["批次号"] = dtNike.Rows[i]["批次号"].ToString();
                                }
                                else
                                {
                                    drdetail["批次号"] = "";
                                }
                                drdetail["托号"] = "";
                                drdetail["规格"] = "";
                                drdetail["备注"] = "";
                                drdetail["期望数量"] = dtNike.Rows[i]["Sugg# Rep Qty"].ToString();
                                drdetail["自定义字段4"] = "";
                                drdetail["自定义字段5"] = "";
                                drdetail["自定义字段6"] = dtNike.Rows[i]["Style Color"].ToString() + "-" + dtNike.Rows[i]["Size"].ToString();
                                drdetail["自定义字段7"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division.ToString() : "";//BU
                                drdetail["单位"] = wmsunit.Unit;
                                dtpodetail.Rows.Add(drdetail);

                            }
                        }
                        else
                        {
                            //string time = DateTime.Now.ToString("HHmmssfff");
                            //dtNike.DefaultView.Sort = "Store CRD ASC,BU ASC,Store Code ASC,Style Color ASC";
                            //dtNike = dtNike.DefaultView.ToTable();
                            for (int i = 0; i < dtNike.Rows.Count; i++)
                            {
                                //if (dtpo.Select("外部单号='" + DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division.Substring(0, 1).ToString() + '-' + dtNike.Rows[i]["Store Code"] + productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].GenderAge.Substring(0, 1).ToString() + "'").Count() <= 0)
                                //不拆单
                                if (dtpo.Select("外部单号='" + DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + '-' + dtNike.Rows[i]["Store Code"] + "'").Count() <= 0)
                                {
                                    //time = DateTime.Now.ToString("HHmmssfff");
                                    DataRow dr = dtpo.NewRow();
                                    if (productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length <= 0)
                                    {
                                        message = "第【" + (i + 2) + "】行，" + "【Style Color】列的Article格式不对或Article " + dtNike.Rows[i]["Style Color"].ToString() + "在系统中不存在!";
                                        return;
                                    }
                                    //dr["外部单号"] = DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division.Substring(0, 1).ToString() + '-' + dtNike.Rows[i]["Store Code"] + productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].GenderAge.Substring(0, 1).ToString();
                                    //不拆单
                                    dr["外部单号"] = DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + '-' + dtNike.Rows[i]["Store Code"].ToString();
                                    dr["预出库单类型"] = "门店调拨出库";
                                    dr["仓库名称"] = warehouse.WarehouseName;
                                    dr["市"] = "";
                                    dr["省"] = "";
                                    dr["联系人"] = "";
                                    dr["订单日期"] = DateTime.Now.ToString();
                                    dr["备注"] = "";
                                    dr["自定义字段2"] = dtNike.Rows[i]["Contract Code"].ToString();
                                    dr["自定义字段3"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division.ToString() : "";
                                    if (CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length <= 0)
                                    {
                                        message = "第【" + (i + 2) + "】行，" + "【Store Code】列的门店代码格式不对或门店代码 " + dtNike.Rows[i]["Store Code"].ToString() + "在系统中不存在!";//dtNike.Rows[i]["Store Code"].ToString() + "不存在!";
                                        return;
                                    }
                                    if (dtNike.Columns.Contains("LF"))
                                    {
                                        dr["自定义字段8"] = dtNike.Rows[i]["LF"].ToString();
                                    }
                                    dr["自定义字段4"] = dtNike.Rows[i]["Store Code"].ToString();
                                    dr["地址"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].AddressLine1 : "";
                                    dr["自定义字段5"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].Company.ToString() : "";
                                    dr["联系方式"] = "";
                                    dr["自定义字段6"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].GenderAge.ToString() : "";
                                    dr["自定义字段7"] = dtNike.Rows[i]["Store CRD"].ToString();
                                    dtpo.Rows.Add(dr);
                                }

                                DataRow drdetail = dtpodetail.NewRow();
                                //drdetail["外部单号"] = DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division.Substring(0, 1).ToString() + '-' + dtNike.Rows[i]["Store Code"] + productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].GenderAge.Substring(0, 1).ToString();
                                //不拆单
                                drdetail["外部单号"] = DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + '-' + dtNike.Rows[i]["Store Code"].ToString().ToString();
                                drdetail["仓库"] = warehouse.WarehouseName;
                                drdetail["SKU"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].SKU : "";
                                drdetail["UPC"] = "";
                                try
                                {
                                    drdetail["产品等级"] = dtNike.Rows[i]["产品等级"].ToString();
                                }
                                catch
                                {
                                    drdetail["产品等级"] = "A品";
                                }
                                drdetail["产品名称"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].LongMaterial.ToString() : "";//产品名称
                                if (dtNike.Columns.Contains("批次号"))
                                {
                                    drdetail["批次号"] = dtNike.Rows[i]["批次号"].ToString();
                                }
                                else
                                {
                                    drdetail["批次号"] = "";
                                }
                                drdetail["托号"] = "";
                                drdetail["规格"] = "";
                                drdetail["备注"] = "";
                                drdetail["期望数量"] = dtNike.Rows[i]["Sugg# Rep Qty"].ToString();
                                drdetail["自定义字段4"] = "";
                                drdetail["自定义字段5"] = "";
                                drdetail["自定义字段6"] = dtNike.Rows[i]["Style Color"].ToString() + "-" + dtNike.Rows[i]["Size"].ToString();
                                drdetail["自定义字段7"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division.ToString() : "";//BU
                                drdetail["单位"] = wmsunit.Unit;
                                dtpodetail.Rows.Add(drdetail);
                            }
                        }
                        //datatable 外部单号升序排序，并依次添加序号
                        //ModifyForDataTable MFD = new ModifyForDataTable();
                        //dtpo = MFD.AddSerialNumberForNFS(dtpo);
                        //dtpodetail = MFD.AddSerialNumberForNFS(dtpodetail);

                        dsResualt.Tables.Add(dtpo);
                        dsResualt.Tables.Add(dtpodetail);
                        //datatable 外部单号升序排序，并依次添加序号
                        ModifyForDataTable MFD = new ModifyForDataTable();
                        dsResualt = MFD.AddSerialNumberForNFS(dsResualt);
                        this.AfterData = dsResualt;
                        #endregion
                    }
                }
                else
                {
                    #region 正常出货 拆单
                    if (ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == WareHouseID).ToArray().Count() <= 0)
                    {
                        message = "用户没有分配仓库!!";
                        return;
                    }
                    WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == WareHouseID).ToArray()[0];
                    IEnumerable<ProjectCustomer> Customer = ApplicationConfigHelper.GetProjectCustomers(ProjectID).ToArray();
                    IEnumerable<WMS_UnitAndSpecifications_Config> wmsunit = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, warehouse.ID).ToArray();
                    #region 产品等级列表
                    IEnumerable<WMSConfig> ProductLevelList = null;
                    try
                    {
                        ProductLevelList = ApplicationConfigHelper.GetWMS_Config("ProductLevel_" + Customer.Select(s=>s.Name).FirstOrDefault().ToString()).ToArray();
                    }
                    catch (Exception)
                    {
                        ProductLevelList = ApplicationConfigHelper.GetWMS_Config("ProductLevel").ToArray();
                    }
                    #endregion
                    #region 预出库单类型列表
                    IEnumerable<WMSConfig> ordertypes = null;
                    try
                    {
                        ordertypes = ApplicationConfigHelper.GetWMS_Config("OrderType_" + Customer.Select(s => s.Name).FirstOrDefault().ToString()).ToArray();
                    }
                    catch (Exception)
                    {
                        ordertypes = ApplicationConfigHelper.GetWMS_Config("OrderType").ToArray();
                    }
                    #endregion

                    DataSet dsResualt = new DataSet();
                    DataTable dtpo = new DataTable();
                    dtpo.TableName = "预出库单主信息$";
                    DataTable dtpodetail = new DataTable();
                    dtpodetail.TableName = "预出库单明细信息$";

                    DataTable dtNike = Transdata.Tables[0];
                    List<ProductSearch> searprolistfs = new List<ProductSearch>();
                    for (int i = 0; i < dtNike.Rows.Count; i++)
                    {
                        ProductSearch psfs = new ProductSearch();
                        #region 验证物料 验证包装单位 验证产品等级 验证预出库单类型 数量是否为空
                        if (dtNike.Rows[i]["物料"].ToString().Equals("") || dtNike.Rows[i]["物料"].ToString().Length == 0)
                        {
                            message = "第【" + (i + 1) + "】行，" + "【物料】列" + "有空值或格式不正确!";
                            return;
                        }
                        if (dtNike.Rows[i]["包装单位"].ToString().Equals("") || dtNike.Rows[i]["包装单位"].ToString().Length == 0)
                        {
                            message = "第【" + (i + 1) + "】行，" + "【包装单位】列" + "有空值或格式不正确!";
                            return;
                        }
                        if (dtNike.Columns.Contains("产品等级"))
                        {
                            if (dtNike.Rows[i]["产品等级"].ToString().Equals("") || dtNike.Rows[i]["产品等级"].ToString().Length == 0)
                            {
                                message = "第【" + (i + 1) + "】行，" + "【产品等级】列" + "有空值或格式不正确!";
                                return;
                            }
                        }
                        if (dtNike.Columns.Contains("预出库单类型"))
                        {
                            if (dtNike.Rows[i]["预出库单类型"].ToString().Equals("") || dtNike.Rows[i]["预出库单类型"].ToString().Length == 0)
                            {
                                message = "第【" + (i + 1) + "】行，" + "【预出库单类型】列" + "有空值或格式不正确!";
                                return;
                            }
                        }
                        if (dtNike.Rows[i]["出货数量"].ToString().Equals("") || dtNike.Rows[i]["出货数量"].ToString().Length == 0)
                        {
                            message = "第【" + (i + 1) + "】行，" + "【出货数量】列" + "有空值或格式不正确!";
                            return;
                        }
                        #endregion
                        psfs.SKU = dtNike.Rows[i]["物料"].ToString();
                        searprolistfs.Add(psfs);
                    }
                    IEnumerable<ProductSearch> productListfs = ApplicationConfigHelper.GetSearchProduct(CustomerID, searprolistfs, "SKU");
                    for (int i = 0; i < dtNike.Rows.Count; i++)
                    {
                        #region 验证物料 验证单位 验证产品等级 验证出库单类型
                        if (productListfs.Where(m => m.SKU == dtNike.Rows[i]["物料"].ToString()).ToArray().Length <= 0)
                        {
                            message = "第【" + (i + 1) + "】行，" + "【物料】列的格式不对或 " + dtNike.Rows[i]["物料"].ToString() + " 在系统中不存在!";
                            return;
                        }
                        if (wmsunit.Where(m => m.Unit == dtNike.Rows[i]["包装单位"].ToString()).ToArray().Length <= 0)
                        {
                            message = "第【" + (i + 1) + "】行，" + "【包装单位】列的格式不对或 " + dtNike.Rows[i]["包装单位"].ToString() + " 在系统中不存在!";
                            return;
                        }
                        if (dtNike.Columns.Contains("产品等级"))
                        {
                            if (ProductLevelList.Where(m => m.Name == dtNike.Rows[i]["产品等级"].ToString()).ToArray().Length <= 0)
                            {
                                message = "第【" + (i + 1) + "】行，" + "【产品等级】列的格式不对或 " + dtNike.Rows[i]["产品等级"].ToString() + " 在系统中不存在!";
                                return;
                            }
                        }
                        if (dtNike.Columns.Contains("预出库单类型"))
                        {
                            if (ordertypes.Where(m => m.Name == dtNike.Rows[i]["预出库单类型"].ToString()).ToArray().Length <= 0)
                            {
                                message = "第【" + (i + 1) + "】行，" + "【预出库单类型】列的格式不对或 " + dtNike.Rows[i]["预出库单类型"].ToString() + " 在系统中不存在!";
                                return;
                            }
                        }
                        #endregion
                    }
                    #region
                    dtpo.Columns.Add("外部单号");
                    dtpo.Columns.Add("预出库单类型");
                    dtpo.Columns.Add("仓库名称");
                    dtpo.Columns.Add("订单日期");
                    dtpo.Columns.Add("省");
                    dtpo.Columns.Add("市");
                    dtpo.Columns.Add("地址");
                    dtpo.Columns.Add("联系人");
                    dtpo.Columns.Add("联系方式");
                    dtpo.Columns.Add("快递公司");
                    dtpo.Columns.Add("备注");
                    dtpo.Columns.Add("自定义字段4");
                    dtpo.Columns.Add("自定义字段5");
                    #endregion
                    #region
                    dtpodetail.Columns.Add("外部单号");
                    dtpodetail.Columns.Add("仓库");
                    dtpodetail.Columns.Add("SKU");
                    dtpodetail.Columns.Add("UPC");
                    dtpodetail.Columns.Add("产品等级");
                    dtpodetail.Columns.Add("产品名称");
                    dtpodetail.Columns.Add("批次号");
                    dtpodetail.Columns.Add("托号");
                    dtpodetail.Columns.Add("单位");
                    dtpodetail.Columns.Add("规格");
                    dtpodetail.Columns.Add("备注");
                    dtpodetail.Columns.Add("期望数量");
                    #endregion
                    for (int i = 0; i < dtNike.Rows.Count; i++)
                    {
                        if (dtpo.Select("外部单号='" + dtNike.Rows[i]["运单号"].ToString() + "'").Count() <= 0)
                        {
                            #region
                            DataRow dr = dtpo.NewRow();
                            dr["外部单号"] = dtNike.Rows[i]["运单号"].ToString();
                            try
                            {
                                dr["预出库单类型"] = dtNike.Rows[i]["预出库单类型"].ToString();
                            }
                            catch
                            {
                                dr["预出库单类型"] = "市内配送出库";
                            }
                            dr["仓库名称"] = warehouse.WarehouseName;
                            dr["订单日期"] = DateTime.Now.ToString();
                            dr["省"] = "";
                            dr["市"] = dtNike.Rows[i]["市"].ToString();
                            dr["地址"] = dtNike.Rows[i]["客户地址"].ToString();
                            dr["联系人"] = dtNike.Rows[i]["联系人"].ToString();
                            dr["联系方式"] = dtNike.Rows[i]["电话"].ToString();
                            dr["快递公司"] = dtNike.Rows[i]["承运商"].ToString();
                            dr["备注"] = "";
                            dr["自定义字段4"] = dtNike.Rows[i]["客户编号"].ToString();
                            dr["自定义字段5"] = dtNike.Rows[i]["客户名称"].ToString();
                            dtpo.Rows.Add(dr);
                            #endregion
                        }
                        #region
                        DataRow drdetail = dtpodetail.NewRow();
                        drdetail["外部单号"] = dtNike.Rows[i]["运单号"].ToString();
                        drdetail["仓库"] = warehouse.WarehouseName;
                        drdetail["SKU"] = dtNike.Rows[i]["物料"].ToString().Trim().ToUpper();
                        drdetail["UPC"] = "";
                        try
                        {
                            drdetail["产品等级"] = dtNike.Rows[i]["产品等级"].ToString();
                        }
                        catch
                        {
                            drdetail["产品等级"] = "A品";
                        }
                        drdetail["产品名称"] = dtNike.Rows[i]["物料描述"].ToString();
                        drdetail["批次号"] = DateTime.Now.ToString("yyyyMMdd");
                        drdetail["托号"] = "";
                        drdetail["单位"] = dtNike.Rows[i]["包装单位"].ToString();
                        drdetail["规格"] = "";
                        drdetail["备注"] = dtNike.Rows[i]["发货备注"].ToString();
                        drdetail["期望数量"] = dtNike.Rows[i]["出货数量"].ToString();
                        dtpodetail.Rows.Add(drdetail);
                        #endregion
                    }
                    dsResualt.Tables.Add(dtpo);
                    dsResualt.Tables.Add(dtpodetail);
                    //datatable 外部单号升序排序，并依次添加序号
                    //ModifyForDataTable MFD = new ModifyForDataTable();
                    //dsResualt = MFD.AddSerialNumberForNFS(dsResualt);
                    this.AfterData = dsResualt;
                    #endregion
                }
            }
            else if (TransDataType == "Receiving")
            {
                #region 上架
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
                #endregion
            }
            else
            {
                this.AfterData = Transdata;
            }
        }



    }
}