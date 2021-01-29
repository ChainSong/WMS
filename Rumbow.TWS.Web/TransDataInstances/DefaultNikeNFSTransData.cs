using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Runbow.TWS.Common;
using UtilConstants = Runbow.TWS.Common.Constants;

namespace Runbow.TWS.Web.TransDataInstances
{
    public class DefaultNikeNFSTransData : BaseTransData
    {
        public DefaultNikeNFSTransData(string TransDataType, long CustomerID, long ProjectID, long WareHouseID,
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

                        #region  建列
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
                        dtpodetail.Columns.Add("自定义字段2");
                        dtpodetail.Columns.Add("自定义字段3");
                        dtpodetail.Columns.Add("自定义字段4");
                        dtpodetail.Columns.Add("自定义字段5");
                        dtpodetail.Columns.Add("自定义字段6");
                        dtpodetail.Columns.Add("自定义字段7");
                        dtpodetail.Columns.Add("单位");
                        dtpodetail.Columns.Add("规格");
                        #endregion

                        List<ProductSearch> productListS = new List<ProductSearch>();
                        IEnumerable<ProductSearch> productList;
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
                            if (dtNike.Rows[i]["客户代码"].ToString() == "3436" || dtNike.Rows[i]["客户代码"].ToString() == "3421")//nso挂架 BE&群光店
                            {
                                productList = ApplicationConfigHelper.GetSearchProduct(CustomerID, productListS, "nso");
                            }
                            else
                            {
                                productList = ApplicationConfigHelper.GetSearchProduct(CustomerID, productListS, "Acticle");
                            }
                            string SKU = drdetail["SKU"].ToString();
                            int skulength = SKU.Split('-').Length;
                            string str10 = SKU.Split('-')[0] + "-" + SKU.Split('-')[1];
                            string str8 = SKU.Substring(SKU.Length - 2, 2);
                            string str9 = SKU.ToString().Substring(str10.Length + 1, SKU.Length - (str10.Length + 4));

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
                            drdetail["自定义字段2"] = productList.Where(c => c.Str10 == str10 && c.Str8 == str8 && c.Str9 == str9).Select(m => m.Price).FirstOrDefault();//价格
                            if (dtNike.Rows[i]["客户代码"].ToString() == "3653" && productList.Where(c => c.Str10 == str10 && c.Str8 == str8 && c.Str9 == str9).Select(m => m.DownCoat).FirstOrDefault() == "Y")
                            {
                                drdetail["自定义字段3"] = "YY";
                            }
                            else if (CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString() && m.UserDef1 == "Y").ToArray().Length > 0)
                            {
                                drdetail["自定义字段3"] = "Y";
                            }
                            else
                            {
                                drdetail["自定义字段3"] = productList.Where(c => c.Str10 == str10 && c.Str8 == str8 && c.Str9 == str9).Select(m => m.SafeLock).FirstOrDefault();//安全扣
                            }
                            drdetail["自定义字段4"] = productList.Where(c => c.Str10 == str10 && c.Str8 == str8 && c.Str9 == str9).Select(m => m.Hanger).FirstOrDefault();//衣架
                            drdetail["自定义字段5"] = dtNike.Rows[i]["存货名称及规格型号"].ToString();
                            drdetail["自定义字段6"] = dtNike.Rows[i]["存货代码"].ToString();
                            drdetail["自定义字段7"] = dtNike.Rows[i]["BU"].ToString();
                            drdetail["单位"] = wmsunit.Unit;
                            dtpodetail.Rows.Add(drdetail);
                        }

                        for (int i = 0; i < dtpodetail.Rows.Count; i++)
                        {
                            productList = ApplicationConfigHelper.GetSearchProduct(CustomerID, productListS, "Acticle");
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
                            dtpodetail.Rows[i]["产品名称"] = productList.Where(c => c.SKU == dtpodetail.Rows[i]["SKU"].ToString()).ToArray().Length > 0 ? productList.Where(c => c.SKU == dtpodetail.Rows[i]["SKU"].ToString()).ToArray()[0].LongMaterial.ToString() : "";
                        }

                        dsResualt.Tables.Add(dtpo);
                        dsResualt.Tables.Add(dtpodetail);

                        this.AfterData = dsResualt;
                        #endregion
                    }
                    else if (Transdata.Tables[0].Rows[0]["订单类型"].ToString() == "CSC-门店间互相调拨"||
                        Transdata.Tables[0].Rows[0]["订单类型"].ToString() == "CSC-转货出库"||
                        Transdata.Tables[0].Rows[0]["订单类型"].ToString() == "CSC-退大仓" ||
                         Transdata.Tables[0].Rows[0]["订单类型"].ToString() == "CSC-customer顾客线上支付"||
                         Transdata.Tables[0].Rows[0]["订单类型"].ToString() == "CSC-customer顾客门店支付")
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
                        //获得SKU集合
                        IEnumerable<ProductSearch> productListfs;
                        if (dtNike.Rows[0]["Store Code"].ToString() == "3436" || dtNike.Rows[0]["Store Code"].ToString() == "3421")//nso挂架 BE&群光店
                        {
                            productListfs = ApplicationConfigHelper.GetSearchProduct(CustomerID, searprolistfs, "nso");
                        }
                        else
                        {
                            productListfs = ApplicationConfigHelper.GetSearchProduct(CustomerID, searprolistfs, "Acticle");
                        }
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

                        #region  建列
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
                        dtpodetail.Columns.Add("自定义字段2");
                        dtpodetail.Columns.Add("自定义字段3");
                        dtpodetail.Columns.Add("自定义字段4");
                        dtpodetail.Columns.Add("自定义字段5");
                        dtpodetail.Columns.Add("自定义字段6");
                        dtpodetail.Columns.Add("自定义字段7");
                        dtpodetail.Columns.Add("单位");
                        dtpodetail.Columns.Add("规格");
                        #endregion

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
                                //收货日期+门店代码+code
                                string exterNo = DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + '-' + dtNike.Rows[0]["Store Code"].ToString() + wmslist.Where(m => m.Str1 == gender && m.Str2 == division).FirstOrDefault().Code;
                                if (dtpo.Select("外部单号='" + exterNo + "'").Count() <= 0)
                                {
                                    DataRow dr = dtpo.NewRow();
                                    if (productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length <= 0)
                                    {
                                        message = "第【" + (i + 2) + "】行，" + "【Style Color】列的Article格式不对或Article " + dtNike.Rows[i]["Style Color"].ToString() + "在系统中不存在!";
                                        return;
                                    }
                                    dr["外部单号"] = exterNo;
                                    dr["预出库单类型"] = Transdata.Tables[0].Rows[0]["订单类型"].ToString();
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
                                    dr["自定义字段4"] = dtNike.Rows[i]["Store Code"].ToString();
                                    dr["地址"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].AddressLine1 : "";
                                    dr["自定义字段5"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].Company.ToString() : "";
                                    dr["联系方式"] = "";
                                    dr["自定义字段6"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].GenderAge.ToString() : "";
                                    dr["自定义字段7"] = dtNike.Rows[i]["Store CRD"].ToString();
                                    if (dtNike.Columns.Contains("LF"))
                                    {
                                        dr["自定义字段8"] = dtNike.Rows[i]["LF"].ToString();
                                    }
                                    dtpo.Rows.Add(dr);

                                }

                                DataRow drdetail = dtpodetail.NewRow();
                                drdetail["外部单号"] = exterNo;
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
                                drdetail["自定义字段2"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Price;//价格
                                if (dtNike.Rows[i]["Store Code"].ToString() == "3653" && productListfs.Where(c => c.Str10 == dtNike.Rows[i]["Style Color"].ToString() && c.Str9 == dtNike.Rows[i]["Size"].ToString() && c.Str8 == "01").Select(m => m.DownCoat).FirstOrDefault() == "Y")
                                {
                                    drdetail["自定义字段3"] = "YY";
                                }
                                else if (CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString() && m.UserDef1 == "Y").ToArray().Length > 0)
                                {
                                    drdetail["自定义字段3"] = "Y";
                                }
                                else
                                {
                                    drdetail["自定义字段3"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].SafeLock;//安全扣
                                }
                                drdetail["自定义字段4"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Hanger;//衣架
                                drdetail["自定义字段5"] = "";
                                drdetail["自定义字段6"] = dtNike.Rows[i]["Style Color"].ToString() + "-" + dtNike.Rows[i]["Size"].ToString();
                                drdetail["自定义字段7"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division.ToString() : "";//BU
                                drdetail["单位"] = wmsunit.Unit;
                                dtpodetail.Rows.Add(drdetail);

                            }
                        }
                        else
                        {
                            for (int i = 0; i < dtNike.Rows.Count; i++)
                            {
                                //收货日期+门店代码
                                string exterNo = DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + '-' + dtNike.Rows[i]["Store Code"].ToString();
                                if (dtpo.Select("外部单号='" + exterNo + "'").Count() <= 0)
                                {
                                    DataRow dr = dtpo.NewRow();
                                    if (productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length <= 0)
                                    {
                                        message = "第【" + (i + 2) + "】行，" + "【Style Color】列的Article格式不对或Article " + dtNike.Rows[i]["Style Color"].ToString() + "在系统中不存在!";
                                        return;
                                    }
                                    dr["外部单号"] = exterNo;
                                    dr["预出库单类型"] = Transdata.Tables[0].Rows[0]["订单类型"].ToString();
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
                                    dr["自定义字段4"] = dtNike.Rows[i]["Store Code"].ToString();
                                    dr["地址"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].AddressLine1 : "";
                                    dr["自定义字段5"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].Company.ToString() : "";
                                    dr["联系方式"] = "";
                                    dr["自定义字段6"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].GenderAge.ToString() : "";
                                    dr["自定义字段7"] = dtNike.Rows[i]["Store CRD"].ToString();
                                    if (dtNike.Columns.Contains("LF"))
                                    {
                                        dr["自定义字段8"] = dtNike.Rows[i]["LF"].ToString();
                                    }
                                    dtpo.Rows.Add(dr);
                                }

                                DataRow drdetail = dtpodetail.NewRow();
                                drdetail["外部单号"] = exterNo;
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
                                drdetail["自定义字段2"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Price;//价格
                                if (dtNike.Rows[i]["Store Code"].ToString() == "3653" && productListfs.Where(c => c.Str10 == dtNike.Rows[i]["Style Color"].ToString() && c.Str9 == dtNike.Rows[i]["Size"].ToString() && c.Str8 == "01").Select(m => m.DownCoat).FirstOrDefault() == "Y")
                                {
                                    drdetail["自定义字段3"] = "YY";
                                }
                                else if (CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString() && m.UserDef1 == "Y").ToArray().Length > 0)
                                {
                                    drdetail["自定义字段3"] = "Y";
                                }
                                else
                                {
                                    drdetail["自定义字段3"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].SafeLock;//安全扣
                                }
                                drdetail["自定义字段4"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Hanger;//衣架
                                drdetail["自定义字段5"] = "";
                                drdetail["自定义字段6"] = dtNike.Rows[i]["Style Color"].ToString() + "-" + dtNike.Rows[i]["Size"].ToString();
                                drdetail["自定义字段7"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division.ToString() : "";//BU
                                drdetail["单位"] = wmsunit.Unit;
                                dtpodetail.Rows.Add(drdetail);
                            }
                        }

                        dsResualt.Tables.Add(dtpo);
                        dsResualt.Tables.Add(dtpodetail);

                        ModifyForDataTable MFD = new ModifyForDataTable();
                        dsResualt = MFD.AddSerialNumberForNFS(dsResualt);

                        this.AfterData = dsResualt;
                        #endregion
                    }
                    else if (Transdata.Tables[0].Rows[0]["订单类型"].ToString() == "库内调拨出库")
                    {
                        #region  库内调拨出库 不拆单
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
                        //获得SKU集合
                        IEnumerable<ProductSearch> productListfs;
                        if (dtNike.Rows[0]["Store Code"].ToString() == "3436" || dtNike.Rows[0]["Store Code"].ToString() == "3421")//nso挂架 BE&群光店
                        {
                            productListfs = ApplicationConfigHelper.GetSearchProduct(CustomerID, searprolistfs, "nso");
                        }
                        else
                        {
                            productListfs = ApplicationConfigHelper.GetSearchProduct(CustomerID, searprolistfs, "Acticle");
                        }
                        #region  先验证article存不存在再继续
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
                        #endregion

                        #region  建列
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
                        dtpodetail.Columns.Add("自定义字段2");
                        dtpodetail.Columns.Add("自定义字段3");
                        dtpodetail.Columns.Add("自定义字段4");
                        dtpodetail.Columns.Add("自定义字段5");
                        dtpodetail.Columns.Add("自定义字段6");
                        dtpodetail.Columns.Add("自定义字段7");
                        dtpodetail.Columns.Add("单位");
                        dtpodetail.Columns.Add("规格");
                        #endregion

                        #region  判断是否是殊门店配置
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
                        catch (Exception Ex)
                        {
                        }
                        #endregion

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
                                //收货日期+门店代码+code
                                string exterNo = DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + '-' + dtNike.Rows[0]["Store Code"].ToString() + wmslist.Where(m => m.Str1 == gender && m.Str2 == division).FirstOrDefault().Code;
                                if (dtpo.Select("外部单号='" + exterNo + "'").Count() <= 0)
                                {
                                    DataRow dr = dtpo.NewRow();
                                    if (productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length <= 0)
                                    {
                                        message = "第【" + (i + 2) + "】行，" + "【Style Color】列的Article格式不对或Article " + dtNike.Rows[i]["Style Color"].ToString() + "在系统中不存在!";
                                        return;
                                    }
                                    dr["外部单号"] = exterNo;
                                    dr["预出库单类型"] = dtNike.Rows[i]["订单类型"].ToString();
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
                                    dr["自定义字段4"] = dtNike.Rows[i]["Store Code"].ToString();
                                    dr["地址"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].AddressLine1 : "";
                                    dr["自定义字段5"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].Company.ToString() : "";
                                    dr["联系方式"] = "";
                                    dr["自定义字段6"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].GenderAge.ToString() : "";
                                    dr["自定义字段7"] = dtNike.Rows[i]["Store CRD"].ToString();
                                    if (dtNike.Columns.Contains("LF"))
                                    {
                                        dr["自定义字段8"] = dtNike.Rows[i]["LF"].ToString();
                                    }
                                    dtpo.Rows.Add(dr);

                                }

                                DataRow drdetail = dtpodetail.NewRow();
                                drdetail["外部单号"] = exterNo;
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
                                drdetail["批次号"] = "";
                                drdetail["托号"] = "";
                                drdetail["规格"] = "";
                                drdetail["备注"] = "";
                                drdetail["期望数量"] = dtNike.Rows[i]["Sugg# Rep Qty"].ToString();
                                drdetail["自定义字段2"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Price;//价格
                                if (dtNike.Rows[i]["Store Code"].ToString() == "3653" && productListfs.Where(c => c.Str10 == dtNike.Rows[i]["Style Color"].ToString() && c.Str9 == dtNike.Rows[i]["Size"].ToString() && c.Str8 == "01").Select(m => m.DownCoat).FirstOrDefault() == "Y")
                                {
                                    drdetail["自定义字段3"] = "YY";
                                }
                                else if (CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString() && m.UserDef1 == "Y").ToArray().Length > 0)
                                {
                                    drdetail["自定义字段3"] = "Y";
                                }
                                else
                                {
                                    drdetail["自定义字段3"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].SafeLock;//安全扣
                                }
                                drdetail["自定义字段4"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Hanger;//衣架
                                drdetail["自定义字段5"] = "";
                                drdetail["自定义字段6"] = dtNike.Rows[i]["Style Color"].ToString() + "-" + dtNike.Rows[i]["Size"].ToString();
                                drdetail["自定义字段7"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division.ToString() : "";//BU
                                drdetail["单位"] = wmsunit.Unit;
                                dtpodetail.Rows.Add(drdetail);

                            }
                        }
                        else
                        {
                            for (int i = 0; i < dtNike.Rows.Count; i++)
                            {
                                //不拆单 收货日期+门店代码
                                string exterNo = DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + '-' + dtNike.Rows[i]["Store Code"].ToString();
                                if (dtpo.Select("外部单号='" + exterNo + "'").Count() <= 0)
                                {
                                    DataRow dr = dtpo.NewRow();
                                    if (productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length <= 0)
                                    {
                                        message = "第【" + (i + 2) + "】行，" + "【Style Color】列的Article格式不对或Article " + dtNike.Rows[i]["Style Color"].ToString() + "在系统中不存在!";
                                        return;
                                    }
                                    dr["外部单号"] = exterNo;
                                    dr["预出库单类型"] = dtNike.Rows[i]["订单类型"].ToString();
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

                                    dr["自定义字段4"] = dtNike.Rows[i]["Store Code"].ToString();
                                    dr["地址"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].AddressLine1 : "";
                                    dr["自定义字段5"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].Company.ToString() : "";
                                    dr["联系方式"] = "";
                                    dr["自定义字段6"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].GenderAge.ToString() : "";
                                    dr["自定义字段7"] = dtNike.Rows[i]["Store CRD"].ToString();
                                    if (dtNike.Columns.Contains("LF"))
                                    {
                                        dr["自定义字段8"] = dtNike.Rows[i]["LF"].ToString();
                                    }
                                    dtpo.Rows.Add(dr);
                                }

                                DataRow drdetail = dtpodetail.NewRow();
                                drdetail["外部单号"] = exterNo;
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
                                drdetail["批次号"] = "";
                                drdetail["托号"] = "";
                                drdetail["规格"] = "";
                                drdetail["备注"] = "";
                                drdetail["期望数量"] = dtNike.Rows[i]["Sugg# Rep Qty"].ToString();
                                drdetail["自定义字段2"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Price;//价格
                                if (dtNike.Rows[i]["Store Code"].ToString() == "3653" && productListfs.Where(c => c.Str10 == dtNike.Rows[i]["Style Color"].ToString() && c.Str9 == dtNike.Rows[i]["Size"].ToString() && c.Str8 == "01").Select(m => m.DownCoat).FirstOrDefault() == "Y")
                                {
                                    drdetail["自定义字段3"] = "YY";
                                }
                                else if (CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString() && m.UserDef1 == "Y").ToArray().Length > 0)
                                {
                                    drdetail["自定义字段3"] = "Y";
                                }
                                else
                                {
                                    drdetail["自定义字段3"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].SafeLock;//安全扣
                                }
                                drdetail["自定义字段4"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Hanger;//衣架
                                if (dtNike.Columns.Contains("BIN"))
                                {
                                    drdetail["自定义字段5"] = dtNike.Rows[i]["BIN"].ToString();
                                }
                                else
                                {
                                    drdetail["自定义字段5"] = "";
                                }
                                drdetail["自定义字段6"] = dtNike.Rows[i]["Style Color"].ToString() + "-" + dtNike.Rows[i]["Size"].ToString();
                                drdetail["自定义字段7"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division.ToString() : "";//BU
                                drdetail["单位"] = wmsunit.Unit;
                                dtpodetail.Rows.Add(drdetail);
                            }
                        }
                        dsResualt.Tables.Add(dtpo);
                        dsResualt.Tables.Add(dtpodetail);

                        ModifyForDataTable MFD = new ModifyForDataTable();
                        dsResualt = MFD.AddSerialNumberForNFS(dsResualt);

                        this.AfterData = dsResualt;
                        #endregion
                    }

                    else
                    { }
                }
                else
                {
                    #region  正常出货 拆单
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

                    #region 验证空值
                    List<ProductSearch> searprolistfs = new List<ProductSearch>();
                    for (int i = 0; i < dtNike.Rows.Count; i++)
                    {
                        ProductSearch psfs = new ProductSearch();
                        if (dtNike.Rows[i]["Style Color"].ToString().Equals("") || dtNike.Rows[i]["Style Color"].ToString().Length == 0)
                        {
                            message = "第【" + (i + 2) + "】行，" + "【Style Color】列" + "有空值或格式不正确（应为文本格式）！";
                            return;
                        }
                        if (dtNike.Rows[i]["Size"].ToString().Equals("") || dtNike.Rows[i]["Style Color"].ToString().Length == 0)
                        {
                            message = "第【" + (i + 2) + "】行，" + "【Size】列" + "有空值或格式不正确（应为文本格式）！";
                            return;
                        }
                        if (dtNike.Rows[i]["Sugg# Rep Qty"].ToString().Equals("") || dtNike.Rows[i]["Style Color"].ToString().Length == 0)
                        {
                            message = "第【" + (i + 2) + "】行，" + "【Sugg# Rep Qty】列" + "有空值或格式不正确（应为常规格式）！";
                            return;
                        }
                        psfs.Str10 = dtNike.Rows[i]["Style Color"].ToString();
                        psfs.Str9 = dtNike.Rows[i]["Size"].ToString();
                        psfs.Str8 = "01";
                        searprolistfs.Add(psfs);
                    }
                    #endregion

                    #region 获得SKU集合
                    IEnumerable<ProductSearch> productListnso001;
                    IEnumerable<ProductSearch> productListnso;
                    IEnumerable<ProductSearch> productListfs;
                    //if (dtNike.Rows[0]["Store Code"].ToString() == "3408")//nso 001店
                    //{
                    productListnso001 = ApplicationConfigHelper.GetSearchProduct(CustomerID, searprolistfs, "nso001");
                    //}
                    //else if (dtNike.Rows[0]["Store Code"].ToString() == "3833")//nso挂架 BE&群光店  耐克大中华园区员工店
                    //{
                    productListnso = ApplicationConfigHelper.GetSearchProduct(CustomerID, searprolistfs, "nso");
                    //}
                    //else
                    //{
                    productListfs = ApplicationConfigHelper.GetSearchProduct(CustomerID, searprolistfs, "Acticle");
                    //}
                    #endregion 

                    #region 先验证article存不存在再继续
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
                        dtNike.Rows[i]["BU"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString() && m.Str9 == dtNike.Rows[i]["Size"].ToString()).Select(n => n.Division).FirstOrDefault();
                        dtNike.Rows[i]["TTL Rep Qty for all stores"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString() && m.Str9 == dtNike.Rows[i]["Size"].ToString()).Select(n => n.GenderAge).FirstOrDefault();
                        dtNike.Rows[i]["Balance Qty in the Warehouse"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString() && m.Str9 == dtNike.Rows[i]["Size"].ToString()).Select(n => n.SafeLock).FirstOrDefault();
                        if (dtNike.Rows[i]["Store Code"].ToString().Equals("3408"))
                        {
                            if (dtNike.Rows[i]["TTL Rep Qty for all stores"].ToString().Equals("Mens"))
                            {
                                dtNike.Rows[i]["Store_OnHand"] = "B1";
                            }
                            else
                            {
                                dtNike.Rows[i]["Store_OnHand"] = "2F";
                            }
                        }
                        else if (dtNike.Rows[i]["Store Code"].ToString().Equals("3833"))
                        {
                            if (dtNike.Rows[i]["BU"].ToString().Equals("FTW"))
                            {
                                //if (
                                //    productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString() && m.Str9 == dtNike.Rows[i]["Size"].ToString()).Select(n => n.categoryDes).FirstOrDefault().Equals("BASKETBALL") 
                                //    || productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString() && m.Str9 == dtNike.Rows[i]["Size"].ToString()).Select(n => n.categoryDes).FirstOrDefault().Equals("NIKE SPORTSWEAR")
                                //    || productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString() && m.Str9 == dtNike.Rows[i]["Size"].ToString()).Select(n => n.categoryDes).FirstOrDefault().Equals("RUNNING")
                                //    )
                                //{
                                    dtNike.Rows[i]["Store_OnHand"] = "1F";
                            }
                            else
                            {
                                // dtNike.Rows[i]["Store_OnHand"] = "BM";
                                //    }
                                //}
                                //else
                                //{
                                dtNike.Rows[i]["Store_OnHand"] = "B1";
                            }
                        }
                        else
                        { }
                    }
                    #endregion 

                    #region 排序 收货日期，门店，BU，Gender，VAS，楼层，article，size
                    dtNike.DefaultView.Sort = "Store CRD,Store Code,BU,TTL Rep Qty for all stores,Balance Qty in the Warehouse,Sales Units,Style Color,Size";//按Id Desc倒序和Name Desc倒序
                    dtNike = dtNike.DefaultView.ToTable();//返回一个新的DataTable
                    try
                    {
                        //分单标记
                        int count = 0;
                        int ai = 1;
                        int SplitOrderSize = UtilConstants.SplitOrderSize;
                        for (int i = 0; i < dtNike.Rows.Count; i++)
                        {
                            if (i ==0 )
                            {
                                dtNike.Rows[i]["Sales Units"] = ai.ToString();
                                count += int.Parse( dtNike.Rows[i]["Sugg# Rep Qty"].ToString());
                            }
                            else
                            {
                                int ij = i - 1;
                                if (DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") == DateTime.Parse(dtNike.Rows[ij]["Store CRD"].ToString()).ToString("yyyyMMdd")
                                    && dtNike.Rows[i]["Store Code"].ToString().Equals(dtNike.Rows[ij]["Store Code"].ToString())
                                    && dtNike.Rows[i]["BU"].ToString().Equals(dtNike.Rows[ij]["BU"].ToString())
                                    && dtNike.Rows[i]["TTL Rep Qty for all stores"].ToString().Equals(dtNike.Rows[ij]["TTL Rep Qty for all stores"].ToString())
                                    && dtNike.Rows[i]["Balance Qty in the Warehouse"].ToString().Equals(dtNike.Rows[ij]["Balance Qty in the Warehouse"].ToString())
                                    && dtNike.Rows[i]["Store_OnHand"].ToString().Equals(dtNike.Rows[ij]["Store_OnHand"].ToString())
                                    )
                                {
                                    count += int.Parse(dtNike.Rows[i]["Sugg# Rep Qty"].ToString());
                                    if (count >= SplitOrderSize && count <= (SplitOrderSize+100))
                                    {
                                        dtNike.Rows[i]["Sales Units"] = ai.ToString();
                                        ai++;
                                        count = 0;
                                    }
                                    else
                                    {
                                        dtNike.Rows[i]["Sales Units"] = ai.ToString();
                                    }
                                }
                                else
                                {
                                    ai = 1;
                                    dtNike.Rows[i]["Sales Units"] = ai.ToString();
                                    count = int.Parse(dtNike.Rows[i]["Sugg# Rep Qty"].ToString());
                                }

                            }

                        }

                    }
                    catch (Exception e)
                    {

                        throw;
                    }

                    #endregion 

                    #region  建列
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
                    dtpo.Columns.Add("自定义字段9");//楼层

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
                    dtpodetail.Columns.Add("自定义字段2");
                    dtpodetail.Columns.Add("自定义字段3");
                    dtpodetail.Columns.Add("自定义字段4");
                    dtpodetail.Columns.Add("自定义字段5");
                    dtpodetail.Columns.Add("自定义字段6");
                    dtpodetail.Columns.Add("自定义字段7");
                    dtpodetail.Columns.Add("单位");
                    dtpodetail.Columns.Add("规格");
                    #endregion

                    #region 判断是否是殊门店配置
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
                    catch (Exception Ex)
                    { }
                    #endregion

                    //特殊门店
                    if (wmslist != null && wmslist.Count() > 0)
                    {
                        #region 特殊门店
                        for (int i = 0; i < dtNike.Rows.Count; i++)
                        {
                            string division = "", gender = "";
                            division = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division;
                            gender = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].GenderAge;
                            if (division == "FTW" && gender == "YA")
                            {
                            }
                            string exterNo = DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") + '-' 
                                + dtNike.Rows[0]["Store Code"].ToString() 
                                + wmslist.Where(m => m.Str1 == gender && m.Str2 == division).FirstOrDefault().Code;//年月日+门店代码+code
                            if (dtpo.Select("外部单号='" + exterNo + "'").Count() <= 0)
                            {
                                DataRow dr = dtpo.NewRow();
                                if (productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length <= 0)
                                {
                                    message = "第【" + (i + 2) + "】行，" + "【Style Color】列的Article格式不对或Article " + dtNike.Rows[i]["Style Color"].ToString() + "在系统中不存在!";
                                    return;
                                }
                                dr["外部单号"] = exterNo;
                                dr["预出库单类型"] = "CSC-正常出库门店";
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
                                dr["自定义字段4"] = dtNike.Rows[i]["Store Code"].ToString();
                                dr["地址"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].AddressLine1 : "";
                                dr["自定义字段5"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].Company.ToString() : "";
                                dr["联系方式"] = "";
                                dr["自定义字段6"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].GenderAge.ToString() : "";
                                dr["自定义字段7"] = dtNike.Rows[i]["Store CRD"].ToString();
                                if (dtNike.Columns.Contains("LF"))
                                {
                                    dr["自定义字段8"] = dtNike.Rows[i]["LF"].ToString();
                                }
                                dtpo.Rows.Add(dr);

                            }

                            DataRow drdetail = dtpodetail.NewRow();
                            drdetail["外部单号"] = exterNo;
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
                            drdetail["批次号"] = "";
                            drdetail["托号"] = "";
                            drdetail["规格"] = "";
                            drdetail["备注"] = "";
                            drdetail["期望数量"] = dtNike.Rows[i]["Sugg# Rep Qty"].ToString();
                            drdetail["自定义字段2"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Price;//价格
                            if (dtNike.Rows[i]["Store Code"].ToString() == "3653" && productListfs.Where(c => c.Str10 == dtNike.Rows[i]["Style Color"].ToString() && c.Str9 == dtNike.Rows[i]["Size"].ToString() && c.Str8 == "01").Select(m => m.DownCoat).FirstOrDefault() == "Y")
                            {
                                drdetail["自定义字段3"] = "YY";
                            }
                            else if (CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString() && m.UserDef1 == "Y").ToArray().Length > 0)
                            {
                                drdetail["自定义字段3"] = "Y";
                            }
                            else
                            {
                                drdetail["自定义字段3"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].SafeLock;//安全扣
                            }
                            drdetail["自定义字段4"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Hanger;//衣架
                            drdetail["自定义字段5"] = "";
                            drdetail["自定义字段6"] = dtNike.Rows[i]["Style Color"].ToString() + "-" + dtNike.Rows[i]["Size"].ToString();
                            drdetail["自定义字段7"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division.ToString() : "";//BU
                            drdetail["单位"] = wmsunit.Unit;
                            dtpodetail.Rows.Add(drdetail);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 拆单 收货日期+BU+门店代码+Gender+VAS
                        for (int i = 0; i < dtNike.Rows.Count; i++)
                        {
                            string exterNo = "";
                            if (dtNike.Rows[i]["Store Code"].ToString() == "3408")
                            {
                                exterNo = DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") //预到货日期
                                    + dtNike.Rows[i]["BU"].ToString().Substring(0, 1) + '-' //BU
                                    + dtNike.Rows[i]["Store Code"] //收货门店
                                    + dtNike.Rows[i]["TTL Rep Qty for all stores"].ToString().Substring(0, 1) + "-" //男女童
                                    //+ dtNike.Rows[i]["Balance Qty in the Warehouse"].ToString() //是否打扣
                                    + dtNike.Rows[i]["Sales Units"].ToString()+ "-" //每单数量控制在200~300之间
                                    + dtNike.Rows[i]["Store_OnHand"].ToString(); //楼层
                            }
                            else
                            {
                                exterNo = DateTime.Parse(dtNike.Rows[i]["Store CRD"].ToString()).ToString("yyyyMMdd") //预到货日期
                                    + dtNike.Rows[i]["BU"].ToString().Substring(0, 1) + '-' //BU
                                    + dtNike.Rows[i]["Store Code"] //收货门店
                                    + dtNike.Rows[i]["TTL Rep Qty for all stores"].ToString().Substring(0, 1) + "-" //男女童
                                    //+ dtNike.Rows[i]["Balance Qty in the Warehouse"].ToString() //是否打扣
                                    + dtNike.Rows[i]["Sales Units"].ToString() + "-" //每单数量控制在200~300之间
                                    + dtNike.Rows[i]["Store_OnHand"].ToString(); //楼层
                            }
                            if (dtpo.Select("外部单号='" + exterNo + "'").Count() <= 0)
                            {
                                DataRow dr = dtpo.NewRow();
                                if (productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length <= 0)
                                {
                                    message = "第【" + (i + 2) + "】行，" + "【Style Color】列的Article格式不对或Article " + dtNike.Rows[i]["Style Color"].ToString() + "在系统中不存在!";
                                    return;
                                }
                                dr["外部单号"] = exterNo;
                                dr["预出库单类型"] = "CSC-正常出库门店";
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

                                dr["自定义字段4"] = dtNike.Rows[i]["Store Code"].ToString();
                                dr["地址"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].AddressLine1 : "";
                                dr["自定义字段5"] = CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray().Length > 0 ? CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString()).ToArray()[0].Company.ToString() : "";
                                dr["联系方式"] = "";
                                dr["自定义字段6"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].GenderAge.ToString() : "";
                                dr["自定义字段7"] = dtNike.Rows[i]["Store CRD"].ToString();
                                if (dtNike.Columns.Contains("LF"))
                                {
                                    dr["自定义字段8"] = dtNike.Rows[i]["LF"].ToString();
                                }
                                    dr["自定义字段9"] = dtNike.Rows[i]["Store_OnHand"].ToString();
                                dtpo.Rows.Add(dr);
                            }

                            DataRow drdetail = dtpodetail.NewRow();
                            drdetail["外部单号"] = exterNo;
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
                            drdetail["批次号"] = "";
                            drdetail["托号"] = "";
                            drdetail["规格"] = "";
                            drdetail["备注"] = "";
                            drdetail["期望数量"] = dtNike.Rows[i]["Sugg# Rep Qty"].ToString();
                            drdetail["自定义字段2"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Price;//价格
                            if (dtNike.Rows[i]["Store Code"].ToString() == "3653" && productListfs.Where(c => c.Str10 == dtNike.Rows[i]["Style Color"].ToString() && c.Str9 == dtNike.Rows[i]["Size"].ToString() && c.Str8 == "01").Select(m => m.DownCoat).FirstOrDefault() == "Y")
                            {
                                drdetail["自定义字段3"] = "YY";
                            }
                            else if (CustomerList.Where(m => m.StorerKey == dtNike.Rows[i]["Store Code"].ToString() && m.UserDef1 == "Y").ToArray().Length > 0)
                            {
                                drdetail["自定义字段3"] = "Y";
                            }
                            else
                            {
                                drdetail["自定义字段3"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].SafeLock;//安全扣
                            }

                            if (dtNike.Rows[i]["Store Code"].ToString() == "3408")
                            {
                                drdetail["自定义字段4"] = productListnso001.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Hanger;//衣架
                            }
                            else if(dtNike.Rows[i]["Store Code"].ToString() == "3833")
                            {
                                drdetail["自定义字段4"] = productListnso.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Hanger;//衣架
                            }
                            else
                            {
                                drdetail["自定义字段4"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString().Trim().ToUpper() && m.Str9 == dtNike.Rows[i]["Size"].ToString().Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Hanger;//衣架
                            }

                            if (dtNike.Columns.Contains("BIN"))
                            {
                                drdetail["自定义字段5"] = dtNike.Rows[i]["BIN"].ToString();
                            }
                            else
                            {
                                drdetail["自定义字段5"] = "";
                            }
                            drdetail["自定义字段6"] = dtNike.Rows[i]["Style Color"].ToString() + "-" + dtNike.Rows[i]["Size"].ToString();
                            drdetail["自定义字段7"] = productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].Division.ToString() : "";//BU
                            drdetail["单位"] = wmsunit.Unit;//productListfs.Where(m => m.Str10 == dtNike.Rows[i]["Style Color"].ToString()).ToArray()[0].GenderAge.Substring(0, 1).ToString();
                            dtpodetail.Rows.Add(drdetail);
                        }
                    }
                    #endregion

                    dsResualt.Tables.Add(dtpo);
                    dsResualt.Tables.Add(dtpodetail);

                    ModifyForDataTable MFD = new ModifyForDataTable();
                    dsResualt = MFD.AddSerialNumberForNFS(dsResualt);

                    this.AfterData = dsResualt;
                    #endregion
                }
            }
            else if (TransDataType == "Receiving")
            {
                #region  上架
                DataTable dt = Transdata.Tables[0].Copy();
                foreach (DataRow item in dt.Rows)
                {
                    if (!item["托号"].ToString().Equals(""))
                    {
                        message = "托号列不能有值，请清除后再导入！";
                        return;
                    }
                }
                
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
            else if (TransDataType == "Asn")
            {
                //验证门店是否非法
                IEnumerable<WMS_Customer> CustomerList = ApplicationConfigHelper.GetAllCustomerByID(CustomerID);
                for (int i = 0; i < Transdata.Tables[1].Rows.Count; i++)
                {
                    if (CustomerList.Where(m => m.StorerKey == Transdata.Tables[1].Rows[i]["门店代码"].ToString()).Count() > 0)
                    { }
                    else
                    {
                        message = "第【" + (i + 1) + "】行，" + "门店代码 " + Transdata.Tables[1].Rows[i]["门店代码"].ToString() + "在系统中不存在!";
                        return;
                    }
                    if (CustomerList.Where(m => m.StorerKey == Transdata.Tables[0].Rows[i]["外部入库单号"].ToString().Substring(0, 4)).Count() > 0)
                    { }
                    else
                    {
                        message = "第【" + (i + 1) + "】行，" + "【外部入库单号】列的门店代码 " + Transdata.Tables[0].Rows[i]["外部入库单号"].ToString().Substring(0, 4) + "在系统中不存在!";
                        return;
                    }
                }
                this.AfterData = Transdata;
            }
            else
            {
                this.AfterData = Transdata;
            }
        }
    }
}